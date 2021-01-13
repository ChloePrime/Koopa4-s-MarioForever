using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SweetMoleHouse.MarioForever.Scripts.Base.Rpg;
using SweetMoleHouse.MarioForever.Scripts.Constants;
using SweetMoleHouse.MarioForever.Scripts.Enemy;
using SweetMoleHouse.MarioForever.Scripts.Util;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Base
{
    [SuppressMessage("ReSharper", "Unity.InefficientPropertyAccess")]
    public class MovingObstacle : BasePhysics
    {
        private const float CatchEpsilon = 2 * AntiTrapEpsilon;

        private struct CaughtObjectInfo
        {
            public BasePhysics physics;
            public RaycastHit2D hitResult;

            public CaughtObjectInfo(BasePhysics physics, RaycastHit2D hitResult)
            {
                this.physics = physics;
                this.hitResult = hitResult;
            }
        }

        private static readonly List<CaughtObjectInfo> CAUGHT_OBJ_CACHE = new List<CaughtObjectInfo>();
        private static ContactFilter2D catcherFilter = new ContactFilter2D().NoFilter();
        private static bool classInited;

        protected override void Start()
        {
            base.Start();
            InitClass();
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
            transform.Translate(-dx, 0, 0);
            // 推动前方物体
            CatchAndMove(GetDirX(), dx, ActionMoveX, true);
            transform.Translate(dx, 0, 0);

            static void ActionMoveX(BasePhysics physics, float dist) => physics.MoveX(dist);
        }

        public override void MoveY(float distance)
        {
            var dy = transform.position.y;
            base.MoveY(distance);
            dy = transform.position.y - dy;
            transform.Translate(0, -dy, 0);
            
            // 推动前方物体
            Vector2 dirY = GetDirY();
            CatchAndMove(dirY, dy, ActionMoveY, true);
            
            // 下降时抓住上方物体
            if (distance < 0)
            {
                CatchAndMove(-dirY, CatchEpsilon, dy, ActionMoveY, false);
            }
            
            transform.Translate(0, dy, 0);
            
            static void ActionMoveY(BasePhysics physics, float dist) => physics.MoveY(dist);
        }

        private void CatchAndMove(in Vector2 dir, in float offset, in Action<BasePhysics, float> action, in bool kills)
        {
            CatchAndMove(dir, Mathf.Abs(offset) + CatchEpsilon, offset, action, kills);
        }

        private void CatchAndMove(in Vector2 dir, in float castDist,in float offset, in Action<BasePhysics, float> action, in bool kills)
        {
            foreach (var caught in CatchObjs(dir, castDist))
            {
                var actualDist = offset;// - Mathf.Sign(distance) * caught.hitResult.distance;

                var actualOffset = caught.physics.transform.position;
                action(caught.physics, actualDist);
                actualOffset = caught.physics.transform.position - actualOffset;

                if (!kills) continue;

                bool actionBlocked = actualOffset.sqrMagnitude <= actualDist * actualDist - 1e-4;
                if (actionBlocked && caught.physics.TryBfsComponentInChildren(out IDamageReceiver receiver))
                {
                    //receiver.SetDead(EnumDamageType.ENVIRONMENT);
                }
            }
        }


        /// <summary>
        /// 此方法不会产生垃圾
        /// </summary>
        /// <returns></returns>
        private IEnumerable<CaughtObjectInfo> CatchObjs(in Vector2 dir, in float distance)
        {
            CAUGHT_OBJ_CACHE.Clear();
            transform.Translate(0, CatchEpsilon, 0);

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

            transform.Translate(0, -CatchEpsilon, 0);
            return CAUGHT_OBJ_CACHE;
        }

    }
}