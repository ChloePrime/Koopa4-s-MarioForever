using SweetMoleHouse.MarioForever.Constants;
using SweetMoleHouse.MarioForever.Util;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SweetMoleHouse.MarioForever.Base
{
    /// <summary>
    /// 移动的东西
    /// 需要自身有<see cref="Rigidbody2D"/>方可生效
    /// </summary>
    public class BasePhysics : MonoBehaviour, IAppearable
    {
        protected static readonly float AntiTrapEpsilon = Consts.ONE_PIXEL / 4;
        protected static ContactFilter2D Filter = new ContactFilter2D().NoFilter();
        public static ContactFilter2D GlobalFilter => Filter;

        /// <summary>
        /// 射线追踪的结果缓存
        /// 请不要保留对这个对象的引用
        /// 因为内部数据随时会被覆写
        /// </summary>
        protected static readonly RaycastHit2D[] RCastTempArray = new RaycastHit2D[64];
        protected static readonly Collider2D[] OverlapTempArray = new Collider2D[64];
        protected static bool Inited;

        [Header("基础物理设置")]
        [SerializeField, RenameInInspector("重力")]
        private float gravity;
        [SerializeField, RenameInInspector("最大X速度")]
        private float maxXSpeed = float.PositiveInfinity;
        [SerializeField, RenameInInspector("最大Y速度（向上）")]
        private float maxYSpeed = float.PositiveInfinity;
        [SerializeField, RenameInInspector("最小Y速度（向下）")]
        private float minYSpeed = float.PositiveInfinity;

        /// <summary>
        /// 速度矢量（block/s）
        /// </summary>
        [SerializeField, RenameInInspector("初速度")]
        protected Vector2 vel = Vector2.zero;

        [Space, Space, Space]
        [SerializeField, RenameInInspector("无碰撞")]
        private bool ignoreCollision;
        
        [Header("音效设置")]
        [SerializeField, RenameInInspector("顶头音效")]
        private AudioClip hitHeadSfx;
        
        [Header("出水管设置")]
        [SerializeField, RenameInInspector("出现速度")]
        private float appearSpeed = 1.5f;
        
        // 运行时字段

        private enum SlopeState
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
        /// (真实y速度 = x速度 * <see cref="BaseSlope.Degree"/>)
        /// 只对上坡有效
        /// </summary>
        private float slopeFactor;
        private float lastFUpdateTime;
        private Vector2 tickStartPos;
        private Vector2 tickEndPos;
        
        // 属性
        
        public bool IsOnGround { get; private set; }

        public float XSpeed { get => vel.x; set => vel.x = value; }
        public float YSpeed { get => vel.y; set => vel.y = value; }
        public virtual float Gravity { get => gravity; set => gravity = value; }
        public Rigidbody2D R2d { get; private set; }

        public bool IgnoreCollision => ignoreCollision;

        #region 从水管出现
        
        private bool appeared = true;
        private Vector2 appearDir;
        private float appearProgress;
        private bool appearingInSolidBefore;
        public virtual void Appear(in Vector2 direction, in Vector2 size)
        {
            appeared = false;
            appearDir = MathHelper.GetAxis(direction);
            if (appearDir == Vector2.left || appearDir == Vector2.right)
            {
                appearProgress = size.x;
            }
            else
            {
                appearProgress = size.y;
            }
            appearProgress += AntiTrapEpsilon;
        }

        private void AppearUpdate()
        {
            float distance = appearSpeed * Time.fixedDeltaTime;
            if (distance > appearProgress)
            {
                distance = appearProgress;
            }
            appearProgress -= distance;
            transform.Translate(appearDir * distance);

            TryEndAppearInAdvance();
            if (appearProgress <= 0)
            {
                appeared = true;
            }
        }

        private void TryEndAppearInAdvance()
        {
            bool inSolid = R2d.GetContacts(Filter, OverlapTempArray) != 0;
            if (inSolid && !appearingInSolidBefore)
            {
                appearingInSolidBefore = true;
            }
            if (appearingInSolidBefore && !inSolid)
            {
                appearProgress = 0;
                print("Appearing Stopped!");
            }
        }

        #endregion

        public virtual void SetDirection(float dir)
        {
            XSpeed = Mathf.Abs(XSpeed) * Mathf.Sign(dir);
        }

        protected virtual void Start()
        {
            InitClass();
            R2d = GetComponent<Rigidbody2D>();
            // 防止卡在墙里
            if (appeared && R2d.Cast(Vector2.zero, RCastTempArray) > 0)
            {
                transform.Translate(0, 0.01F, 0);
            }
        }

        protected virtual void FixedUpdate()
        {
            if (!appeared)
            {
                AppearUpdate();
                return;
            }

            YSpeed -= Gravity * Time.fixedDeltaTime;
            //如果物体的速度接近静止则停止计算运动
            if (vel.sqrMagnitude <= 1e-8f)
            {
                return;
            }

            if (lastFUpdateTime >= 0.0001)
            {
                transform.position = tickEndPos;
            }

            ClampSpeed();
            if (!IgnoreCollision)
            {
                CheckSurroundings();
            }
            MoveAndRecordPos();
        }

        private void CheckSurroundings()
        {
            slopeState = SlopeState.FLAT;
            CheckSlope(GetDirX(), true);
            CheckSlope(Vector2.down, false);
            StopTowardsWall(GetDirXWithSlope(), ref vel.x);
            StopTowardsWall(GetDirY(), ref vel.y);
        }

        /// <summary>
        /// 运动并记录坐标运动前后的坐标
        /// 这两个坐标会在Update时用于坐标插值
        /// </summary>
        private void MoveAndRecordPos()
        {
            tickStartPos = transform.position;
            MoveX(XSpeed * Time.fixedDeltaTime);
            MoveY(YSpeed * Time.fixedDeltaTime);
            tickEndPos = transform.position;
            lastFUpdateTime = Time.time;
        }

        /// <summary>
        /// 对马里奥的位置进行平滑插值
        /// </summary>
        protected virtual void Update()
        {
            if (lastFUpdateTime >= 0.0001)
            {
#pragma warning disable UNT0004 // Time.fixedDeltaTime used with Update
                float prog = (Time.time - lastFUpdateTime) / Time.fixedDeltaTime;
#pragma warning restore UNT0004 // Time.fixedDeltaTime used with Update
                transform.position = Vector2.Lerp(tickStartPos, tickEndPos, prog);
            }
        }

        /// <summary>
        /// 如果碰到不是斜坡的物体则速度归零
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="fieldToSet"></param>
        protected virtual void StopTowardsWall(in Vector2 dir, ref float fieldToSet)
        {
            int hits = R2d.Cast(dir, Filter, RCastTempArray, AntiTrapEpsilon);
            //不撞到东西或者撞到任意斜坡时不停止速度
            if (hits == 0)
            {
                return;
            }
            for (int i = 0; i < hits; i++)
            {
                var result = RCastTempArray[i];
                if (result.collider.GetComponent<BaseSlope>() != null)
                {
                    return;
                }
            }
            fieldToSet = 0;
        }
        /// <summary>
        /// 应用速度上下限
        /// </summary>
        private void ClampSpeed()
        {
            XSpeed = Mathf.Clamp(XSpeed, -maxXSpeed, maxXSpeed);
            YSpeed = Mathf.Clamp(YSpeed, -minYSpeed, maxYSpeed);
        }

        /// <summary>
        /// 检测斜坡状态
        /// </summary>
        /// <param name="dir">检测的方向矢量</param>
        /// <param name="isUp">这次检测是上坡(true)还是下坡(false)</param>
        private void CheckSlope(in Vector2 dir, in bool isUp)
        {
            int results = R2d.Cast(dir, Filter, RCastTempArray, 0.125f);
            for (int i = 0; i < results; i++)
            {
                var result = RCastTempArray[i];
                if (result.collider == null) continue;
                if (!result.collider.TryGetComponent(out BaseSlope slope))
                {
                    continue;
                }
                
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
        /// <summary>
        /// 设置马里奥当前踩的是哪个斜坡
        /// 同时计算坡度
        /// </summary>
        private void SetSlope(in BaseSlope slope)
        {
            if (slope == curSlopeObj)
            {
                return;
            }
            curSlopeObj = slope;
            slopeFactor = 1 / Mathf.Sqrt(1 + slope.Degree * slope.Degree);
        }

        public bool OverlappingAnything() => R2d.OverlapCollider(Filter, OverlapTempArray) > 0;

        /// <summary>
        /// 横向移动一段距离
        /// 会计算上下坡
        /// </summary>
        public void MoveX(float distance)
        {
            if (IgnoreCollision)
            {
                transform.Translate(distance, 0, 0);
                return;
            }
            float actualDist;
            int amount = R2d.Cast(GetDirX(), Filter, RCastTempArray, Math.Abs(distance) + AntiTrapEpsilon);
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
                    HitWallX(TakeColliders(hitAmount));
                }
            }
            else
            {
                actualDist = RCastTempArray[0].distance - AntiTrapEpsilon;
                transform.Translate(Math.Sign(distance) * actualDist, 0, 0);
                HitWallX(TakeColliders(amount));
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
        protected virtual void HitWallX(in Collider2D[] colliders)
        {
            OnHitWallX?.Invoke(colliders);
        }

        public event Action<Collider2D[]> OnHitWallX;
        public event Action<Collider2D[]> OnHitWallY;
        /// <summary>
        /// 纵向移动一段距离
        /// </summary>
        public void MoveY(float distance)
        {
            if (IgnoreCollision)
            {
                transform.Translate(0, distance, 0);
                return;
            }
            int amount = R2d.Cast(GetDirY(), Filter, RCastTempArray, Math.Abs(distance) + AntiTrapEpsilon);
            if (amount == 0)
            {
                transform.Translate(0, distance, 0);
                if (R2d.Cast(Vector2.down, Filter, RCastTempArray, 2 * AntiTrapEpsilon) == 0)
                {
                    IsOnGround = false;
                }
            }
            else
            {
                float actualDist = RCastTempArray[0].distance - AntiTrapEpsilon;
                transform.Translate(0, Math.Sign(distance) * actualDist, 0);

                Collider2D[] colliders = RCastTempArray.Take(amount).Select(rr => rr.collider).ToArray();
                HitWallY(colliders);

                if (distance <= 0)
                {
                    IsOnGround = true;
                }
                YSpeed = 0;
            }
        }
        /// <summary>
        /// 当物体纵向撞墙时出发
        /// 此时速度尚未归0
        /// </summary>
        /// <param name="colliders">碰撞结果</param>
        protected virtual void HitWallY(in Collider2D[] colliders) 
        {
            OnHitWallY?.Invoke(colliders);
            
            if (YSpeed > 0)
            {
                bool defaultSound = true;
                foreach (var col in colliders)
                {
                    IHitable hittable;
                    if ((hittable = col.GetComponent<IHitable>()) != null)
                    {
                        defaultSound = !hittable.OnHit(transform) && defaultSound;
                    }
                }
                if (defaultSound && hitHeadSfx != null)
                {
                    Global.PlaySound(hitHeadSfx);
                }
            }
        }

        public void Teleport(Vector3 pos)
        {
            transform.position = pos;
            tickStartPos = tickEndPos = pos;
        }

        private static Collider2D[] TakeColliders(int amount)
        {
            return RCastTempArray.Take(amount).Select(rr => rr.collider).ToArray();
        }

        /// <summary>
        /// 移动一段距离
        /// </summary>
        /// <param name="offset">要移动的位移矢量</param>
        /// <returns>
        /// 撞到的实心对象个数
        /// 实心对象存放在<see cref="RCastTempArray"/>内
        /// </returns>
        protected int Move(in Vector2 offset)
        {
            float length = offset.magnitude;
            int amount = R2d.Cast(offset.normalized, Filter, RCastTempArray, length + AntiTrapEpsilon);
            if (amount == 0)
            {
                transform.Translate(offset);
            }
            else
            {
                if (length <= AntiTrapEpsilon)
                {
                    return amount;
                }
                float actualDist = RCastTempArray[0].distance - AntiTrapEpsilon;
                float actualScale = actualDist / length;
                transform.Translate(offset * actualScale);
            }
            return amount;
        }

        private void InitClass()
        {
            if (Inited) return;
            Filter.SetLayerMask(~LayerMask.GetMask(LayerNames.ALL_MOVEABLE, LayerNames.DMG_DETECTOR));
            Filter.useTriggers = false;
            Inited = true;
        }

        /// <summary>
        /// 获取X方向的方向矢量
        /// </summary>
        /// <returns>X方向的方向矢量</returns>
        protected Vector2 GetDirX()
        {
            if (XSpeed == 0)
            {
                return Vector2.zero;
            }

            return Math.Sign(XSpeed) > 0 ? Vector2.right : Vector2.left;
        }

        /// <summary>
        /// 获取X方向的方向矢量
        /// 受上坡影响，不受下坡影响
        /// </summary>
        /// <returns>X方向的方向矢量</returns>
        protected Vector2 GetDirXWithSlope()
        {
            if (XSpeed == 0)
            {
                return Vector2.zero;
            }

            float x = Math.Sign(XSpeed);
            if (slopeState != SlopeState.UP)
            {
                return new Vector2(x, 0);
            }
            x *= slopeFactor;
            float y = Mathf.Abs(x) * (int)slopeState * curSlopeObj.Degree;
            return new Vector2(x, y);
        }

        /// <summary>
        /// 获取Y方向的方向矢量
        /// </summary>
        /// <returns>Y方向的方向矢量</returns>
        protected Vector2 GetDirY()
        {
            if (YSpeed == 0)
            {
                return Vector2.zero;
            }

            return Math.Sign(YSpeed) > 0 ? Vector2.up: Vector2.down;
        }
    }
}