using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SweetMoleHouse.MarioForever.Scripts.Base.Rpg;
using SweetMoleHouse.MarioForever.Scripts.Constants;
using SweetMoleHouse.MarioForever.Scripts.Enemy;
using SweetMoleHouse.MarioForever.Scripts.Util;
using UnityEngine;
using static UnityEngine.Mathf;

namespace SweetMoleHouse.MarioForever.Scripts.Base
{
    [SuppressMessage("ReSharper", "Unity.InefficientPropertyAccess")]
    public class MovingObstacle : BasePhysics
    {
        private const float CatchEpsilon = 2 * AntiTrapEpsilon;

        private readonly struct CaughtObjectInfo
        {
            public readonly BasePhysics Physics;
            public readonly RaycastHit2D HitResult;

            public CaughtObjectInfo(BasePhysics physics, RaycastHit2D hitResult)
            {
                Physics = physics;
                HitResult = hitResult;
            }
        }

        private static readonly List<CaughtObjectInfo> CAUGHT_OBJ_CACHE = new List<CaughtObjectInfo>();
        private static ContactFilter2D catcherFilter = new ContactFilter2D().NoFilter();
        private static bool classInited;

        private Collider2D[] colliders;
        
        protected override void Start()
        {
            base.Start();
            InitClass();
            colliders = this.DfsComponentsInChildren<Collider2D>().ToArray();
        }

        private static void InitClass()
        {
            if (classInited) return;
            catcherFilter.layerMask = LayerMask.GetMask(LayerNames.AllMovable);
            classInited = true;
        }

        public override void MoveX(float distance)
        {
            var dx = transform.position.x;
            base.MoveX(distance);
            dx = transform.position.x - dx;
            
            // 推动前方物体
            CatchPushDrag(GetDirX(), 0, dx, ActionMoveX, false);
            
            static void ActionMoveX(BasePhysics physics, float dist)
                => physics.MoveX(dist);
        }

        public override void MoveY(float distance, bool updateOnGroundStatus)
        {
            var dy = R2d.position.y;
            base.MoveY(distance, updateOnGroundStatus);
            dy = R2d.position.y - dy;

            // 推动前方物体
            CatchPushDrag(GetDirY(), CatchEpsilon, dy, ActionMoveY, true);

            static void ActionMoveY(BasePhysics physics, float dist)
                => physics.MoveY(dist);
        }

        private void CatchPushDrag(in Vector2 castDir, in float castDist, in float offset,
            in Action<BasePhysics, float> action, in bool isY)
        {
            // 推动前方物体
            CatchAndMove(castDir, castDist, offset, action, true);

            // 拖拽站在平台上的物体
            var dragCastLength = Max(0, (isY ? -offset : 0) + CatchEpsilon); 
            CatchAndMove(Vector2.up, dragCastLength, offset, action, false);
        }

        private void CatchAndMove(in Vector2 castDir, in float castDist, in float offset,
            in Action<BasePhysics, float> action, in bool isPush)
        {
            if (Abs(offset) < 1e-4) return;

            var colDisabled = false;
            foreach (var caught in CatchObjs(castDir, castDist))
            {
                if (!colDisabled)
                {
                    SetCollisionAvailability(false);
                    colDisabled = true;
                }

                var hitDistance = Max(0, caught.HitResult.distance);
                const float pushProtection = AntiTrapEpsilon;
                var distToMove = isPush 
                    ? Sign(offset) * (Abs(offset) - hitDistance + pushProtection)
                    : offset;

                // 如果实际位移小于被推动的位移，
                // 那么说明被推动的物体被阻挡，
                // 此时kill掉被推懂地物体
                var actualOffset = caught.Physics.transform.position;
                action(caught.Physics, distToMove);
                
                if (!isPush) continue;
                actualOffset = caught.Physics.transform.position - actualOffset;
                
                bool actionBlocked = actualOffset.sqrMagnitude <= distToMove * distToMove - 1e-4;
                if (actionBlocked && caught.Physics.TryBfsComponentInChildren(out IDamageReceiver receiver))
                {
                    receiver.SetDead(EnumDamageType.ENVIRONMENT);
                }
            }
            
            if (colDisabled)
            {
                SetCollisionAvailability(true);
            }
        }

        /// <summary>
        /// 此方法不会产生垃圾
        /// </summary>
        /// <returns></returns>
        private IEnumerable<CaughtObjectInfo> CatchObjs(in Vector2 dir, in float distance)
        {
            CAUGHT_OBJ_CACHE.Clear();

            var amount = R2d.Cast(dir, catcherFilter, RCastTempArray, distance);
            for (var i = 0; i < amount; i++)
            {
                var rig = (Component) RCastTempArray[i].rigidbody;
                if (rig == null)
                {
                    rig = RCastTempArray[i].collider;
                }

                if (!rig.TryGetComponent(out BasePhysics physics))
                {
                    continue;
                }

                CAUGHT_OBJ_CACHE.Add(new CaughtObjectInfo(
                    physics, RCastTempArray[i]
                ));
            }

            return CAUGHT_OBJ_CACHE;
        }

        private void SetCollisionAvailability(bool available)
        {
            foreach (var c2d in colliders)
            {
                c2d.enabled = available;
            }
        }
    }
}