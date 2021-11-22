using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SweetMoleHouse.MarioForever.Scripts.Base.Rpg;
using SweetMoleHouse.MarioForever.Scripts.Constants;
using SweetMoleHouse.MarioForever.Scripts.Enemy;
using SweetMoleHouse.MarioForever.Scripts.Facility;
using SweetMoleHouse.MarioForever.Scripts.Util;
using UnityEngine;
using static UnityEngine.Mathf;

namespace SweetMoleHouse.MarioForever.Scripts.Base.Physics {
[SuppressMessage("ReSharper", "Unity.InefficientPropertyAccess")]
public class MovingObstacle : BasePhysics {
    private const float CatchEpsilon = 2 * AntiTrapEpsilon;

    private readonly struct CaughtObjectInfo {
        public readonly BasePhysics Physics;
        public readonly RaycastHit2D HitResult;

        public CaughtObjectInfo(BasePhysics physics, RaycastHit2D hitResult) {
            Physics = physics;
            HitResult = hitResult;
        }

        public bool Equals(CaughtObjectInfo other) {
            return Equals(Physics, other.Physics);
        }

        public override bool Equals(object obj) {
            return obj is CaughtObjectInfo other && Equals(other);
        }

        public override int GetHashCode() {
            return (Physics != null ? Physics.GetHashCode() : 0);
        }

        public static bool operator ==(CaughtObjectInfo left, CaughtObjectInfo right) {
            return left.Equals(right);
        }

        public static bool operator !=(CaughtObjectInfo left, CaughtObjectInfo right) {
            return !left.Equals(right);
        }
    }

    private static ContactFilter2D catcherFilter = new ContactFilter2D().NoFilter();
    private static bool classInited;

    private Collider2D[] colliders;
    private bool isPlatform;

    [Header("高级设置")] [SerializeField] private DamageSource damageSource;

    protected override void Awake() {
        base.Awake();
        colliders = this.DfsComponentsInChildren<Collider2D>().ToArray();
        isPlatform = CompareTag(Tags.Platform);
    }

    protected override void Start() {
        base.Start();
        InitClass();
    }

    private static void InitClass() {
        if (classInited) return;
        catcherFilter.layerMask = LayerMask.GetMask(LayerNames.AllMovable);
        classInited = true;
    }

    public override void MoveX(float distance, bool updateStatus) {
        var dx = transform.position.x;
        base.MoveX(distance, updateStatus);
        dx = transform.position.x - dx;

        var dir = new Vector2(Sign(distance), 0);
        // 推动前方物体
        CatchPushDrag(dir, 0, dx, ActionMoveX, false);

        static void ActionMoveX(BasePhysics physics, float dist)
            => physics.PushX(dist);
    }

    public override void MoveY(float distance, bool updateStatus) {
        var dy = transform.position.y;
        base.MoveY(distance, updateStatus);
        dy = transform.position.y - dy;

        var dir = new Vector2(0, Sign(distance));
        // 推动前方物体
        CatchPushDrag(dir, 0, dy, ActionMoveY, true);

        static void ActionMoveY(BasePhysics physics, float dist)
            => physics.MoveY(dist);
    }

    private void CatchPushDrag(in Vector2 castDir, in float castDist, in float offset,
        in Action<BasePhysics, float> action, in bool isY) {
        bool isMovingUp = isY && offset > 0;
        bool isMovingDown = isY && offset < 0;
        // 推动前方物体
        if (!isPlatform) {
            CatchAndMove(castDir, castDist, offset, action, true);
            if (isMovingUp) return;
        }

        // 拖拽站在平台上的物体
        var dragCastLength = (isMovingDown ? -offset : 0) + CatchEpsilon;
        // 防止向上运动的实心多次推动上方物体
        CatchAndMove(Vector2.up, dragCastLength, offset, action, false);
    }

    private void CatchAndMove(in Vector2 castDir, in float castDist, in float offset,
        in Action<BasePhysics, float> action, in bool isPush) {
        if (Abs(offset) < 1e-4) return;

        var colDisabled = false;
        foreach (var caught in CatchObjs(castDir, castDist)) {
            if (isPlatform && !caught.Physics.IsStandingOnPlatform(caught.HitResult.point)) {
                continue;
            }

            if (!colDisabled) {
                SetCollisionAvailability(false);
                colDisabled = true;
            }

            float distToMove = isPush
                ? Sign(offset) * (Abs(offset) - caught.HitResult.distance)
                : offset;

            // 如果实际位移小于被推动的位移，
            // 那么说明被推动的物体被阻挡，
            // 此时kill掉被推懂地物体
            var actualOffset = caught.Physics.transform.position;
            action(caught.Physics, distToMove);

            if (!isPush) continue;
            actualOffset = caught.Physics.transform.position - actualOffset;

            bool actionBlocked = actualOffset.sqrMagnitude <= distToMove * distToMove - 1e-4;
            if (actionBlocked && !ReferenceEquals(damageSource, null) 
                              && caught.Physics.TryBfsComponentInChildren(out IDamageReceiver receiver)) {
                damageSource.DoDamageTo(receiver);
            }
        }

        if (colDisabled) {
            SetCollisionAvailability(true);
        }
    }

    private IEnumerable<CaughtObjectInfo> CatchObjs(in Vector2 dir, in float distance) {
        var caughtObjCache = new List<CaughtObjectInfo>();

        var amount = R2d.Cast(dir, catcherFilter, RCastTempArray, distance);
        for (var i = 0; i < amount; i++) {
            var rig = (Component)RCastTempArray[i].rigidbody;
            if (rig == null) {
                rig = RCastTempArray[i].collider;
            }

            if (!rig.TryGetComponent(out BasePhysics physics)) {
                continue;
            }

            caughtObjCache.Add(new CaughtObjectInfo(
                physics, RCastTempArray[i]
            ));
        }

        return caughtObjCache.Distinct();
    }

    private void SetCollisionAvailability(bool available) {
        List<Collider2D> invalidColliders = null;
        foreach (var c2d in colliders) {
            if (c2d == null) {
                invalidColliders ??= new List<Collider2D>();
                invalidColliders.Add(c2d);
                continue;
            }

            c2d.enabled = available;
        }

        if (invalidColliders != null) {
            RemoveEmptyCollider(invalidColliders);
        }
    }

    private void RemoveEmptyCollider(IEnumerable<Collider2D> blacklist) {
        colliders = colliders.Where(campaign => !blacklist.Contains(campaign)).ToArray();
    }

    internal void SetDisplay(Transform displayIn) {
        display = displayIn;
    }
}
}