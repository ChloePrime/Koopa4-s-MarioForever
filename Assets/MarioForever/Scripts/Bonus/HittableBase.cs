using System;
using Cysharp.Threading.Tasks;
using SweetMoleHouse.MarioForever.Scripts.Base.Physics;
using SweetMoleHouse.MarioForever.Scripts.Base.Rpg;
using SweetMoleHouse.MarioForever.Scripts.Enemy;
using SweetMoleHouse.MarioForever.Scripts.Util;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Bonus {
/// <summary>
/// 可以顶，有顶起动画，且可以顶死自身上方敌人的东西
/// </summary>
public abstract class HittableBase : MonoBehaviour, IHittable {
    private const float HitRange = 0.2f;
    private static readonly RaycastHit2D[] RaycastPool = new RaycastHit2D[64];
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
        int c = _collider.Cast(Vector2.up, ColliderFilter, RaycastPool, HitRange);
        for (var i = 0; i < c; i++) {
            RaycastHit2D enemy = RaycastPool[i];
            if (enemy.collider.TryGetComponent(out IDamageReceiver dr)) {
                _bumpDamageSource.DoDamageTo(dr);
            }
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
