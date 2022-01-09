using System;
using Cysharp.Threading.Tasks;
using SweetMoleHouse.MarioForever.Scripts.Util;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Base.Physics {
/// <summary>
/// 行走的东西
/// 类似于RE的组1
/// </summary>
public class Walk : BasePhysics {
    [Header("物品行走设置")] [SerializeField, RenameInInspector("行走速度")]
    protected float walkSpeed = 1.5f;

    [SerializeField, RenameInInspector("与其他行走物品相撞")]
    private bool collideWithOthers = true;

    public float WalkSpeed {
        get => walkSpeed;
        set {
            walkSpeed = value;
            int dir = MathF.Sign(RealWalkSpeed);
            // 防止设置 WalkSpeed 时由于 RealWalkSpeed 为 0 而导致怪物不动 
            dir = dir == 0 ? 1 : dir;
            RealWalkSpeed = value * dir;
        }
    }

    /// <summary>
    /// 带符号的行走速度
    /// </summary>
    private float realWalkSpeed;

    /// <summary>
    /// 带符号的行走速度
    /// </summary>
    protected float RealWalkSpeed {
        get => realWalkSpeed;
        set {
            bool dirty = MathF.Sign(realWalkSpeed) != MathF.Sign(value);
            realWalkSpeed = value;
            if (dirty) {
                ResetDisplayDirection();
            }
        }
    }

    /// <summary>
    /// 转向
    /// </summary>
    public void TurnRound() {
        RealWalkSpeed *= -1;
        TeleportBy(RealWalkSpeed * Time.fixedDeltaTime, 0);
    }

    public override void SetDirection(float dir) {
        base.SetDirection(dir);
        RealWalkSpeed = MathF.Abs(walkSpeed) * MathF.Sign(dir);
    }

    protected override void Awake() {
        base.Awake();
        RealWalkSpeed = walkSpeed;
    }

    protected override void FixedUpdate() {
        XSpeed = RealWalkSpeed;
        base.FixedUpdate();
    }

    public override void HitWallX(in Collider2D[] colliders) {
        base.HitWallX(colliders);
        TurnRound();
    }

    private void ResetDisplayDirection() {
        if (display == transform) return;
        Vector3 animScale = display.transform.localScale;
        animScale.x = Mathf.Sign(RealWalkSpeed);
        display.transform.localScale = animScale;
    }

    protected async void OnCollisionEnter2D(Collision2D collision) {
        var hisCollider = collision.collider;
        await UniTask.Yield(PlayerLoopTiming.LastFixedUpdate);
        bool isInvalid = this == null || hisCollider == null;

        if (isInvalid || !hisCollider.GetHost().TryGetComponent(out Walk that)) {
            return;
        }

        //如果任意一方设置了不与其他物体碰撞则忽略
        if (!(collideWithOthers && that.collideWithOthers)) {
            return;
        }

        if (R2d.position.x < that.R2d.position.x) {
            RealWalkSpeed = -walkSpeed;
        } else {
            RealWalkSpeed = walkSpeed;
        }
    }
}
}