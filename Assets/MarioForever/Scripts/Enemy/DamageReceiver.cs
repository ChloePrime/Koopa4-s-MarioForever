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


    public event Func<DamageEvent, ActionResult> OnDeath;
    public event Func<DamageEvent, ActionResult> OnGenerateCorpse;

    /// <summary>
    /// 返回null时意味着使用默认音效。
    /// </summary>
    public event Func<EnumDamageType, AudioClip> OnGetDeathSound;
    
    public void Damage(DamageEvent damage) {
        if ((acceptedDamageTypes & damage.Type) > 0) {
            SetDead(damage);
        }
    }

    public void SetDead(DamageEvent damage) {
        // 防止死亡代码重复执行
        if (isDead) return;
        isDead = true;

        if (OnDeath?.Invoke(damage).IsCanceled() == true) {
            // 死亡被取消后支持再次死亡，以方便乌龟切换。
            ResetDeathFlagAtNextFrame().Forget();
            return;
        }

        PlayDeathSound(damage.Type);
        SummonScore(damage);
        GenerateCorpse(damage);

        if (TryGetComponent(out DamageSource damageSource)) {
            damageSource.enabled = false;
        }

        Destroy(host.gameObject);
    }

    public void PlayDeathSound(EnumDamageType damageType) => Global.PlaySound(GetDeathSound(damageType));

    public void SummonScore(DamageEvent damage) {
        Action scorer = damage.CreateScoreOverride;
        if (scorer == null) {
            score.Summon(host.transform);
        } else {
            scorer();
        }
    }

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

    private void GenerateCorpse(DamageEvent damage) {
        bool overriden = OnGenerateCorpse?.Invoke(damage).IsCanceled() ?? false;
        if (overriden) {
            Destroy(corpse);
            return;
        }

        corpse.SetActive(true);
        corpse.transform.parent = host.transform.parent;
        corpseBehavior.InitCorpse(damage.Source, Renderer);
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
            mario.Jumper.Jump(GetStompPower(mario));

            DamageEvent damage = mario.StompDamageSource.CreateDamageEvent();
            // 使用马里奥的连踩 Combo 替代默认生成的分数
            damage.CreateScoreOverride += () => mario.ComboInfo.Hit(mario.transform);
            mario.StompDamageSource.DoDamageTo(this, damage);
        }
    }

    private static float GetStompPower(in Mario mario) {
        return mario.Jumper.IsHoldingJumpKey ? 20 : 13;
    }
}
}