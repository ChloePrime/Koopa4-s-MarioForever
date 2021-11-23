using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
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
    private const float CatchEpsilon = 2 * Consts.OnePixel;

    private readonly struct CaughtObjectInfo: IEquatable<CaughtObjectInfo> {
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

    private bool isPlatform;

    [Header("高级设置")] [SerializeField] private DamageSource damageSource;

    protected override void Awake() {
        base.Awake();
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
        float dx = transform.position.x;
        base.MoveX(distance, updateStatus);
        dx = transform.position.x - dx;

        var dir = new Vector2(Sign(distance), 0);
        // 推动前方物体
        if (!isPlatform) {
            CatchAndMove(dir, 0, dx, ActionMoveX, true);
        }

        // 载动上方物体
        CatchAndMove(Vector2.up, CatchEpsilon, dx, ActionMoveX, false);

        static void ActionMoveX(BasePhysics physics, float dist, RaycastHit2D hitInfo)
            => physics.PushX(dist);
    }

    public override void MoveY(float distance, bool updateStatus) {
        float y = transform.position.y;
        base.MoveY(distance, updateStatus);
        float dy = transform.position.y - y;

        Vector2 dir = new(0, Sign(distance));
        float castDistance = Abs(dy) + CatchEpsilon;
        // 推动前方物体
        CatchAndMove(dir, castDistance, dy, ActionMoveY, true);
        // 拉动上方物体
        if (dy < 0) {
            CatchAndMove(Vector2.up, castDistance, dy, ActionMoveY, false);
        }

        static void ActionMoveY(BasePhysics physics, float dist, RaycastHit2D hitInfo)
            => physics.MoveY(dist);
    }

    private delegate void OnDragCall(BasePhysics physics, float offset, RaycastHit2D hitInfo);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void CatchAndMove(
        in Vector2 castDir, in float castDist, in float offset,
        in OnDragCall action, in bool isPush) {
        
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
            Vector3 actualOffset = caught.Physics.transform.position;
            action(caught.Physics, distToMove, caught.HitResult);

            if (!isPush) continue;
            actualOffset = caught.Physics.transform.position - actualOffset;

            bool actionBlocked = actualOffset.sqrMagnitude < distToMove * distToMove - 1e-8;
            if (actionBlocked && !ReferenceEquals(damageSource, null)
                              && caught.Physics.TryBfsComponentInChildren(out IDamageReceiver receiver)) {
                damageSource.Kill(receiver);
            }
        }

        if (colDisabled) {
            SetCollisionAvailability(true);
        }
    }

    private IEnumerable<CaughtObjectInfo> CatchObjs(in Vector2 dir, in float distance) {
        List<CaughtObjectInfo> caughtObjCache = new();

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
        R2d.simulated = available;
    }

    internal void SetDisplay(Transform displayIn) {
        display = displayIn;
    }
}
}