using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Base
{
    /// <summary>
    /// 移动的东西
    /// 需要自身有<see cref="Rigidbody2D"/>方可生效
    /// </summary>
    public class BasePhysics : MonoBehaviour
    {
        protected static readonly float ANTI_TRAP_EPSILON = Consts.ONE_PIXEL / 8;
        protected static ContactFilter2D FILTER = new ContactFilter2D().NoFilter();
        protected static RaycastHit2D[] RCAST_TEMP_ARRAY = new RaycastHit2D[64];
        protected static bool inited = false;

        [Header("基础物理设置")]
        [SerializeField, RenameInInspector("重力")]
        private float gravity = 0f;
        [SerializeField, RenameInInspector("最大X速度")]
        private float maxXSpeed = float.PositiveInfinity;
        [SerializeField, RenameInInspector("最大Y速度（向上）")]
        private float maxYSpeed = float.PositiveInfinity;
        [SerializeField, RenameInInspector("最小Y速度（向下）")]
        private float minYSpeed = float.PositiveInfinity;

        /// <summary>
        /// 速度矢量（px/frame）
        /// </summary>
        [SerializeField, RenameInInspector("初速度")]
        protected Vector2 vel = Vector2.zero;
        public bool IsOnGround { get; private set; }

        public float XSpeed { get => vel.x; set => vel.x = value; }
        public float YSpeed { get => vel.y; set => vel.y = value; }
        public virtual float Gravity { get => gravity; set => gravity = value; }

        protected int antiStuckCd;
        public Rigidbody2D R2d { get; protected set; }
        protected virtual void Start()
        {
            InitClass();
            R2d = GetComponent<Rigidbody2D>();
            antiStuckCd = gameObject.GetInstanceID() % 8;
        }

        public enum SlopeState
        {
            FLAT = 0,
            UP = 1,
            DOWN = -1
        }
        private SlopeState slopeState = SlopeState.FLAT;
        private BaseSlope curSlopeObj;
        /// <summary>
        /// 斜坡坡度对速度的衰减
        /// 真实x速度 = x速度 * 这个值
        /// </summary>
        private float slopeFactor;

        protected virtual void Update()
        {
            ClampSpeed();

            slopeState = SlopeState.FLAT;
            CheckSlope(GetDirX(), true);
            CheckSlope(Vector2.down, false);
            StopTowardsWall(GetDirXWithSlope(), ref vel.x);
            StopTowardsWall(GetDirY(), ref vel.y);

            YSpeed -= Gravity * Time.deltaTime;
            MoveX(XSpeed * Time.deltaTime);
            MoveY(YSpeed * Time.deltaTime);
        }

        /// <summary>
        /// 如果碰到不是斜坡的物体则速度归零
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="fieldToSet"></param>
        private void StopTowardsWall(Vector2 dir, ref float fieldToSet)
        {
            int hits = R2d.Cast(dir, FILTER, RCAST_TEMP_ARRAY, ANTI_TRAP_EPSILON);
            //不撞到东西或者撞到任意斜坡时不停止速度
            if (hits == 0)
            {
                return;
            }
            for (int i = 0; i < hits; i++)
            {
                var result = RCAST_TEMP_ARRAY[i];
                if (result.collider.GetComponent<BaseSlope>() != null)
                {
                    return;
                }
            }
            fieldToSet = 0;
        }
        private void ClampSpeed()
        {
            XSpeed = Mathf.Clamp(XSpeed, -maxXSpeed, maxXSpeed);
            YSpeed = Mathf.Clamp(YSpeed, -minYSpeed, maxYSpeed);
        }

        private void CheckSlope(in Vector2 dir, in bool isUp)
        {
            int results = R2d.Cast(dir, FILTER, RCAST_TEMP_ARRAY, 0.125f);
            for (int i = 0; i < results; i++)
            {
                var result = RCAST_TEMP_ARRAY[i];
                if (result.collider == null) continue;
                var slope = result.collider.GetComponent<BaseSlope>();
                if (slope != null)
                {
                    int xDir = Math.Sign(XSpeed);
                    if (isUp && (xDir == slope.Dir))
                    {
                        slopeState = SlopeState.UP;
                        SetSlope(slope);
                        return;
                    }
                    //判断是不是朝着斜坡上坡的反方向走
                    else if (!isUp && (xDir + slope.Dir == 0))
                    {
                        slopeState = SlopeState.DOWN;
                        SetSlope(slope);
                        return;
                    }
                }
            }
        }
        private void SetSlope(in BaseSlope slope)
        {
            if (slope == curSlopeObj)
            {
                return;
            }
            curSlopeObj = slope;
            slopeFactor = 1 / Mathf.Sqrt(1 + slope.Degree * slope.Degree);
        }

        public void MoveX(float distance)
        {
            float actualDist;
            int amount = R2d.Cast(GetDirX(), FILTER, RCAST_TEMP_ARRAY, Math.Abs(distance) + ANTI_TRAP_EPSILON);
            if (amount == 0)
            {
                actualDist = Math.Abs(distance);
                transform.Translate(distance, 0, 0);
            }
            else if (slopeState == SlopeState.UP)
            {
                distance *= slopeFactor;
                actualDist = Math.Abs(distance);
                int hitAmount = Move(new Vector2(distance, actualDist * curSlopeObj.Degree));
                if (hitAmount > 0)
                {
                    OnHitWallX(TakeColliders(hitAmount));
                }
            }
            else
            {
                actualDist = RCAST_TEMP_ARRAY[0].distance - ANTI_TRAP_EPSILON;
                transform.Translate(Math.Sign(distance) * actualDist, 0, 0);
                OnHitWallX(TakeColliders(amount));
            }
            if (slopeState == SlopeState.DOWN)
            {
                Move(new Vector2(0, -actualDist * curSlopeObj.Degree));
            }
        }

        /// <summary>
        /// 当物体横向撞墙时出发
        /// 此时速度尚未归0
        /// </summary>
        /// <param name="colliders">碰撞结果</param>
        protected virtual void OnHitWallX(in Collider2D[] colliders) { }
        public void MoveY(float distance)
        {
            int amount = R2d.Cast(GetDirY(), FILTER, RCAST_TEMP_ARRAY, Math.Abs(distance) + ANTI_TRAP_EPSILON);
            if (amount == 0)
            {
                transform.Translate(0, distance, 0);
                if (R2d.Cast(Vector2.down, FILTER, RCAST_TEMP_ARRAY, 2 * ANTI_TRAP_EPSILON) == 0)
                {
                    IsOnGround = false;
                }
            }
            else
            {
                float actualDist = RCAST_TEMP_ARRAY[0].distance - ANTI_TRAP_EPSILON;
                transform.Translate(0, Math.Sign(distance) * actualDist, 0);

                Collider2D[] colliders = RCAST_TEMP_ARRAY.Take(amount).Select(rr => rr.collider).ToArray();
                OnHitWallY(colliders);

                if (distance <= 0)
                {
                    IsOnGround = true;
                }
                YSpeed = 0;
            }
        }
        protected virtual void OnHitWallY(in Collider2D[] colliders) 
        { 
            if (YSpeed > 0)
            {
                bool defaultSound = true;
                foreach (var col in colliders)
                {
                    IHitable hitable;
                    if ((hitable = col.GetComponent<IHitable>()) != null)
                    {
                        defaultSound = hitable.OnHit(transform) && defaultSound;
                    }
                }
                if (defaultSound)
                {

                }
            }
            YSpeed = 0;
        }
        private static Collider2D[] TakeColliders(int amount)
        {
            return RCAST_TEMP_ARRAY.Take(amount).Select(rr => rr.collider).ToArray();
        }

        protected int Move(in Vector2 vel)
        {
            float length = vel.magnitude;
            int amount = R2d.Cast(vel.normalized, FILTER, RCAST_TEMP_ARRAY, length + ANTI_TRAP_EPSILON);
            if (amount == 0)
            {
                transform.Translate(vel);
            }
            else
            {
                if (length <= ANTI_TRAP_EPSILON)
                {
                    return amount;
                }
                float actualDist = RCAST_TEMP_ARRAY[0].distance - ANTI_TRAP_EPSILON;
                float actualScale = actualDist / length;
                transform.Translate(vel * actualScale);
            }
            return amount;
        }

        private void InitClass()
        {
            if (inited) return;
            FILTER.SetLayerMask(~LayerMask.GetMask(Consts.LAYER_ALL_MOVEABLE));
            FILTER.useTriggers = false;
            inited = true;
        }

        protected Vector2 GetDirX()
        {
            if (XSpeed == 0)
            {
                return Vector2.zero;
            }
            else
            {
                return Math.Sign(XSpeed) > 0 ? Vector2.right : Vector2.left;
            }
        }

        protected Vector2 GetDirXWithSlope()
        {
            if (XSpeed == 0)
            {
                return Vector2.zero;
            }
            else
            {
                float x = Math.Sign(XSpeed);
                if (slopeState != SlopeState.UP)
                {
                    return new Vector2(x, 0);
                }
                x *= slopeFactor;
                float y = Mathf.Abs(x) * (int)slopeState * curSlopeObj.Degree;
                return new Vector2(x, y);
            }
        }
        protected Vector2 GetDirY()
        {
            if (YSpeed == 0)
            {
                return Vector2.zero;
            }
            else
            {
                return Math.Sign(YSpeed) > 0 ? Vector2.up: Vector2.down;
            }
        }
    }
}