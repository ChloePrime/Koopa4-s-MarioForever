using System;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using SweetMoleHouse.MarioForever.Scripts.Base.Rpg;
using SweetMoleHouse.MarioForever.Scripts.Constants;
using SweetMoleHouse.MarioForever.Scripts.Player;
using SweetMoleHouse.MarioForever.Scripts.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace SweetMoleHouse.MarioForever.Scripts.Enemy {
/// <summary>
/// 敌人的攻击判定
/// 该脚本使用这个物品的父物体作为敌人本体
/// </summary>
public class DamageReceiver : Stompable, IDamageReceiver {
    [SerializeField, RenameInInspector("可被攻击的方式")]
    private EnumDamageType acceptedDamageTypes = 0;

    [SerializeField, RenameInInspector("尸体")]
    private GameObject corpse;

    [SerializeField, RenameInInspector("踩踏音效")]
    private AudioClip stompSound;

    [SerializeField, RenameInInspector("死亡音效")]
    private AudioClip defeatSound;

    [SerializeField, RenameInInspector("奖分")]
    private ScoreType score;

    [SerializeField] private Faction faction;

    [Header("高级设置")]
    [SerializeField]
    [FormerlySerializedAs("self")]
    private Transform host;

    [SerializeField] private DamageSource myDamageSource;

    private bool isDead;
    private Corpse corpseBehavior;
    public Transform Host => host;
    public SpriteRenderer Renderer { get; private set; }

    public Faction Faction => faction;
    public DamageSource MyDamageSource => myDamageSource;


    public event Func<DamageSource, EnumDamageType, ActionResult> OnDeath;
    public event Func<DamageSource, EnumDamageType, ActionResult> OnGenerateCorpse;

    /// <summary>
    /// 返回null时意味着使用默认音效。
    /// </summary>
    public event Func<EnumDamageType, AudioClip> OnGetDeathSound;
    
    public void Damage(DamageSource damager, EnumDamageType damageType) {
        if ((acceptedDamageTypes & damageType) > 0) {
            SetDead(damager, damageType);
        }
    }

    public void SetDead(DamageSource damager, EnumDamageType damageType) {
        // 防止死亡代码重复执行
        if (isDead) return;
        isDead = true;

        if (OnDeath?.Invoke(damager, damageType).IsCanceled() == true) {
            // 死亡被取消后支持再次死亡，以方便乌龟切换。
            ResetDeathFlagAtNextFrame().Forget();
            return;
        }

        PlayDeathSound(damageType);
        if (!damageType.Contains(EnumDamageType.STOMP)) {
            score.Summon(host.transform);
        }

        GenerateCorpse(damager, damageType);

        if (TryGetComponent(out DamageSource damageSource)) {
            damageSource.enabled = false;
        }

        Destroy(host.gameObject);
    }

    public void PlayDeathSound(EnumDamageType damageType) => Global.PlaySound(GetDeathSound(damageType));

    protected override void Awake() {
        base.Awake();
        Renderer = host.GetComponentInChildren<SpriteRenderer>();
        if (host == null) {
            host = GetComponent<Collider2D>().attachedRigidbody.transform;
        }

        corpse = Instantiate(corpse, host);
        corpseBehavior = corpse.GetComponent<Corpse>();
        corpse.SetActive(false);
    }

    private AudioClip GetDeathSound(EnumDamageType damageType) {
        AudioClip result;
        if (OnGetDeathSound != null && (result = OnGetDeathSound(damageType)) != null) {
            return result;
        }

        return damageType.Contains(EnumDamageType.STOMP) ? stompSound : defeatSound;
    }

    private async UniTaskVoid ResetDeathFlagAtNextFrame() {
        await UniTask.NextFrame(PlayerLoopTiming.FixedUpdate);
        isDead = false;
    }

    private void GenerateCorpse(DamageSource damager, EnumDamageType damageType) {
        bool overriden = OnGenerateCorpse?.Invoke(damager, damageType).IsCanceled() ?? false;
        if (overriden) {
            Destroy(corpse);
            return;
        }

        corpse.SetActive(true);
        corpse.transform.parent = host.transform.parent;
        corpseBehavior.InitCorpse(damager, Renderer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnCollisionEnter2D(Collision2D collision) {
        OnCollisionStay2D(collision);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnCollisionStay2D(Collision2D collision) {
        // 这个地方会优先使用 collision.rigidbody.transform
        TryGetStomped(collision.transform);
    }

    public void TryGetStomped(Transform other) {
        if (other.TryGetComponent(out Mario mario) && IsStomp(other, mario)) {
            mario.OnStomp(GetStompPower(mario), true);
            Damage(mario.StompDamageSource, EnumDamageType.STOMP);
        }
    }

    private static float GetStompPower(in Mario mario) {
        return mario.Jumper.IsHoldingJumpKey ? 20 : 13;
    }
}
}