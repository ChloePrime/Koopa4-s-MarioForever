using System;
using System.Collections.Generic;
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
    public Transform Host => host;
    public SpriteRenderer Renderer { get; private set; }

    public Faction Faction => faction;
    public DamageSource MyDamageSource => myDamageSource;


    public event Func<DamageEvent, ActionResult> OnDeath;
    public event Func<DamageEvent, ActionResult> OnGenerateCorpse;

    /// <summary>
    /// 返回null时意味着使用默认音效。
    /// </summary>
    public event Func<DamageEvent, AudioClip> OnGetDeathSound;
    
    public void Damage(DamageEvent damage) {
        if ((acceptedDamageTypes & damage.Type) > 0) {
            SetDead(damage);
        }
    }

    public void SetDead(DamageEvent damage) {
        // 防止死亡代码重复执行
        if (_isDead) return;
        _isDead = true;

        if (OnDeath?.Invoke(damage).IsCanceled() == true) {
            // 死亡被取消后支持再次死亡，以方便乌龟切换。
            ResetDeathFlagAtNextFrame().Forget();
            return;
        }

        PlayDeathSound(damage);
        SummonScore(damage);
        GenerateCorpse(damage);

        if (TryGetComponent(out DamageSource damageSource)) {
            damageSource.enabled = false;
        }

        Destroy(host.gameObject);
    }

    public void PlayDeathSound(DamageEvent damage) {
        AudioClip sound = GetDeathSound(damage);
        if (!ReferenceEquals(sound, null)) {
            Global.PlaySound(sound);
        }
    }

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
        _corpseBehavior = corpse.GetComponent<Corpse>();
        corpse.SetActive(false);
    }

    private AudioClip GetDeathSound(DamageEvent damage) {
        AudioClip result;

        if (damage.PlayDeathSoundOverride != null) {
            damage.PlayDeathSoundOverride();
            return null;
        }
        
        if (OnGetDeathSound != null && (result = OnGetDeathSound(damage)) != null) {
            return result;
        }

        return damage.Type.Contains(EnumDamageType.STOMP) ? stompSound : defeatSound;
    }

    private async UniTaskVoid ResetDeathFlagAtNextFrame() {
        await UniTask.NextFrame(PlayerLoopTiming.FixedUpdate);
        _isDead = false;
    }

    private void GenerateCorpse(DamageEvent damage) {
        bool overriden = OnGenerateCorpse?.Invoke(damage).IsCanceled() ?? false;
        if (overriden) {
            Destroy(corpse);
            return;
        }

        corpse.SetActive(true);
        corpse.transform.parent = host.transform.parent;
        _corpseBehavior.InitCorpse(damage.Source, Renderer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnCollisionEnter2D(Collision2D collision) {
        (_inCollision ??= new HashSet<Transform>()).Add(collision.transform);
    }

    private void FixedUpdate() {
        if (ReferenceEquals(_inCollision, null)) {
            return;
        }
        _inCollision.RemoveWhere(t => t == null);
        foreach (Transform collision in _inCollision) {
            // 这个地方会优先使用 collision.rigidbody.transform
            TryGetStomped(collision);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnCollisionExit2D(Collision2D other) {
        (_inCollision ??= new HashSet<Transform>()).Remove(other.transform);
    }

    public void TryGetStomped(Transform other) {
        if (other.TryGetComponent(out Mario mario) && IsStomp(other, mario)) {
            mario.Jumper.Jump(GetStompPower(mario));

            DamageEvent damage = mario.StompDamageSource.CreateDamageEvent(this);
            // 使用马里奥的连踩 Combo 替代默认生成的分数
            damage.CreateScoreOverride += () => mario.ComboInfo.Hit(mario.transform);
            mario.StompDamageSource.DoDamageTo(this, damage);
        }
    }

    private static float GetStompPower(in Mario mario) {
        return mario.Jumper.IsHoldingJumpKey ? 20 : 13;
    }
    
    private bool _isDead;
    private Corpse _corpseBehavior;
    private HashSet<Transform> _inCollision;
}
}