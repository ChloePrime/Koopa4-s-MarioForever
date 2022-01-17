using System;
using Cysharp.Threading.Tasks;
using SweetMoleHouse.MarioForever.Scripts.Base.Physics;
using SweetMoleHouse.MarioForever.Scripts.Base.Rpg;
using SweetMoleHouse.MarioForever.Scripts.Enemy;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Bonus {
/// <summary>
/// 可以顶，有顶起动画，且可以顶死自身上方敌人的东西
/// </summary>
public abstract class HittableBase : MonoBehaviour, IHittable {
    private static readonly Vector2 HitRange = new(0, 0.2f);
    private static readonly Collider2D[] ColliderPool = new Collider2D[64];
    private static readonly ContactFilter2D ColliderFilter = new ContactFilter2D().NoFilter();

    private static readonly int BlockHitAnim = Animator.StringToHash("顶起");
    protected static readonly TimeSpan AnimLen = TimeSpan.FromSeconds(0.3f);

    protected virtual void Awake() {
        _collider = GetComponent<Collider2D>();
        _animator = GetComponentInChildren<Animator>();
        _bumpDamageSource = GetComponent<DamageSource>();
    }

    public virtual bool OnHit(Transform hitter) {
        if (!Ready) {
            return true;
        }

        _animator.SetTrigger(BlockHitAnim);
        DamageEnemiesAbove();
        StartCooldown();
        return true;
    }

    protected void DamageEnemiesAbove() {
        Vector2 physicsPos = _collider.offset;
        _collider.offset = physicsPos + HitRange;
        try {
            int c = _collider.OverlapCollider(ColliderFilter, ColliderPool);
            for (var i = 0; i < c; i++) {
                Collider2D enemy = ColliderPool[i];
                if (enemy.TryGetComponent(out IDamageReceiver dr)) {
                    _bumpDamageSource.DoDamageTo(dr);
                }
            }
        } finally {
            _collider.offset = physicsPos;
        }
    }

    private async void StartCooldown() {
        Ready = false;
        await UniTask.Delay(AnimLen);
        Ready = true;
    }

    private Collider2D _collider;
    private DamageSource _bumpDamageSource;
    private Animator _animator;

    protected bool Ready { get; private set; } = true;
}
}
