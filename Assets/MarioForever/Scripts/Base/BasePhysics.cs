using System;
using System.Linq;
using SweetMoleHouse.MarioForever.Scripts.Constants;
using SweetMoleHouse.MarioForever.Scripts.Facility;
using SweetMoleHouse.MarioForever.Scripts.Util;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Base
{
    /// <summary>
    /// 移动的东西
    /// 需要自身有<see cref="Rigidbody2D"/>方可生效
    /// </summary>
    public class BasePhysics : MonoBehaviour, IAppearable
    {
        protected const float AntiTrapEpsilon = Consts.OnePixel / 4;
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
        protected float maxXSpeed = float.PositiveInfinity;
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

        [Header("显示设置")] 
        [SerializeField, RenameInInspector("渲染用物体")]
        protected Transform display;
        
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

        protected XFacingWallStatus IsFacingWallX;
        private int lastXDir;
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
        private Vector2 displayLocalPos;
        
        // 属性
        
        public bool IsOnGround { get; private set; }

        public float XSpeed { get => vel.x; set => vel.x = value; }
        public float YSpeed { get => vel.y; set => vel.y = value; }
        public virtual float Gravity { get => gravity; set => gravity = value; }
        public Rigidbody2D R2d { get; private set; }

        public bool IgnoreCollision
        {
            get => ignoreCollision;
            set => ignoreCollision = value;
        }

        public BaseSlope CurSlopeObj
        {
            set { curSlopeObj = value; }
            get { return curSlopeObj; }
        }

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
        
        private static void InitClass()
        {
            if (Inited) return;
            Filter.SetLayerMask(~LayerMask.GetMask(LayerNames.AllMovable, LayerNames.DmgDetector));
            Filter.useTriggers = false;
            Inited = true;
        }

        protected virtual void Start()
        {
            InitClass();
            if (display == null)
            {
                display = this.BfsComponentInChildren<SpriteRenderer>().transform;
            }

            if (display != null)
            {
                displayLocalPos = display.position - transform.position;
            }
            R2d = this.BfsComponentInChildren<Rigidbody2D>();
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
            if (vel.sqrMagnitude <= 1e-6f)
            {
                return;
            }

            if (lastFUpdateTime >= 0.0001)
            {
                transform.position = tickEndPos;
            }

            ClampSpeed();
            if (!ignoreCollision)
            {
                CheckSurroundings();
            }
            MoveAndRecordPos();
            if (CompareTag(Tags.Player))
            {
                Global.DebugText = $"SlopeState:{slopeState},FacingWallX:{IsFacingWallX},LastXDir:{lastXDir}";
            }
        }

        private void CheckSurroundings()
        {
            slopeState = SlopeState.FLAT;
            CheckSlope(GetDirX(), true);
            CheckSlope(Vector2.down, false);
            
            StopTowardsWall(GetDirXWithSlope(XSpeed), () => vel.x = 0);
            StopTowardsWall(GetDirY(), () => vel.y = 0);
        }

        /// <summary>
        /// 运动并记录坐标运动前后的坐标
        /// 这两个坐标会在Update时用于坐标插值
        /// </summary>
        private void MoveAndRecordPos()
        {
            lastFUpdateTime = Time.time;
            tickStartPos = transform.position;
            var dirX = Math.Sign(XSpeed);
            if (dirX != 0)
            {
                lastXDir = dirX;
            }
            MoveX(XSpeed * Time.fixedDeltaTime, true);
            MoveY(YSpeed * Time.fixedDeltaTime, true);
            RecordPos();
        }

        private void RecordPos()
        {
            tickEndPos = transform.position;
        }

        /// <summary>
        /// 对马里奥的位置进行平滑插值
        /// </summary>
        protected virtual void Update()
        {
            if (lastFUpdateTime < 0.0001) return;
            
            float prog = (Time.time - lastFUpdateTime) / Time.fixedDeltaTime;
            display.position = displayLocalPos + Vector2.Lerp(tickStartPos, tickEndPos, prog);
        }

        /// <summary>
        /// 如果碰到不是斜坡的物体则速度归零
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="whenHit"></param>
        protected virtual void StopTowardsWall(in Vector2 dir, in Action whenHit)
        {
            int hits = Cast(dir, Filter, RCastTempArray, AntiTrapEpsilon);
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
            whenHit();
        }
        /// <summary>
        /// 应用速度上下限
        /// </summary>
        protected virtual void ClampSpeed()
        {
            XSpeed = Mathf.Clamp(XSpeed, -maxXSpeed, maxXSpeed);
            ClampYSpeed();
        }

        protected void ClampYSpeed()
        {
            YSpeed = Mathf.Clamp(YSpeed, -minYSpeed, maxYSpeed);
        }

        /// <summary>
        /// 检测斜坡状态
        /// </summary>
        /// <param name="dir">检测的方向矢量</param>
        /// <param name="isUp">这次检测是上坡(true)还是下坡(false)</param>
        private void CheckSlope(in Vector2 dir, in bool isUp)
        {
            int results = Cast(dir, Filter, RCastTempArray, 0.125f);
            for (int i = 0; i < results; i++)
            {
                var result = RCastTempArray[i];
                if (result.collider == null) continue;
                if (!result.collider.TryGetComponent(out BaseSlope slope))
                {
                    continue;
                }
                
                int xDir = dir.x == 0 ? lastXDir : Math.Sign(dir.x);
                if (isUp && xDir == slope.Dir)
                {
                    slopeState = SlopeState.UP;
                    SetSlope(slope);
                    return;
                }
                //判断是不是朝着斜坡上坡的反方向走

                if (!isUp && xDir + slope.Dir == 0)
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

        public void MoveX(in float distance) => MoveX(distance, false);
        /// <summary>
        /// 横向移动一段距离
        /// 会计算上下坡
        /// </summary>
        public virtual void MoveX(float distance, bool updateStatus)
        {
            if (IgnoreCollision)
            {
                transform.Translate(distance, 0, 0);
                RecordPos();
                return;
            }
            float actualDist;
            var dir = new Vector2(Mathf.Sign(distance), 0);
            int amount = Cast(dir, Filter, RCastTempArray, Math.Abs(distance) + AntiTrapEpsilon);
            if (amount == 0)
            {
                actualDist = Math.Abs(distance);
                transform.Translate(distance, 0, 0);
                if (updateStatus)
                {
                    Vector2 dir2 = GetDirXWithSlope(distance);
                    if (Cast(dir2, Filter, RCastTempArray, 3 * AntiTrapEpsilon) == 0)
                    {
                        IsFacingWallX = XFacingWallStatus.NONE;
                    }
                }
            }
            else if (slopeState == SlopeState.UP)
            {
                distance *= slopeFactor;
                actualDist = Math.Abs(distance);
                int hitAmount = Move(new Vector2(distance, actualDist * curSlopeObj.Degree));
                if (hitAmount > 0)
                {
                    HitWallX(TakeColliders(hitAmount));
                    UpdateWallStatusX();
                }
                else if (updateStatus)
                {
                    IsFacingWallX = XFacingWallStatus.NONE;
                }
            }
            else
            {
                actualDist = MinHitDistance(amount) - AntiTrapEpsilon;
                transform.Translate(Math.Sign(distance) * actualDist, 0, 0);
                HitWallX(TakeColliders(amount));
                UpdateWallStatusX();
            }
            
            if (slopeState == SlopeState.DOWN)
            {
                Move(new Vector2(0, -actualDist * curSlopeObj.Degree));
            }
            RecordPos();

            void UpdateWallStatusX()
            {
                if (!updateStatus || distance == 0F) return;
                IsFacingWallX = XFacingWallStatusFactory.FromSpeed(distance);
            }
        }

        public void PushX(float distance)
        {
            CheckSlope(new Vector2(distance, 0), true);
            if (slopeState == SlopeState.UP)
            {
                distance *= curSlopeObj.Degree;
            }
            MoveX(distance, false);
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
        public void MoveY(float distance) => MoveY(distance, false);
        /// <summary>
        /// 纵向移动一段距离
        /// </summary>
        public virtual void MoveY(float distance, bool updateStatus)
        {
            if (IgnoreCollision)
            {
                transform.Translate(0, distance, 0);
                RecordPos();
                return;
            }

            var dir = new Vector2(0, Mathf.Sign(distance));
            int amount = Cast(dir, Filter, RCastTempArray, Math.Abs(distance) + AntiTrapEpsilon);
            if (amount == 0)
            {
                transform.Translate(0, distance, 0);
                if (updateStatus)
                {
                    if (Cast(Vector2.down, Filter, RCastTempArray, 2 * AntiTrapEpsilon) == 0)
                    {
                        IsOnGround = false;
                    }
                }
            }
            else
            {
                float actualDist = MinHitDistance(amount) - AntiTrapEpsilon;
                transform.Translate(0, Math.Sign(distance) * actualDist, 0);

                Collider2D[] colliders = RCastTempArray.Take(amount).Select(rr => rr.collider).ToArray();
                HitWallY(colliders);

                if (updateStatus && distance <= 0)
                {
                    IsOnGround = true;
                }
                YSpeed = 0;
            }
            RecordPos();
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

        public void TeleportTo(Vector2 pos)
        {
            transform.position = tickStartPos = tickEndPos = pos;
        }

        public void TeleportBy(Vector2 offset)
        {
            var pos = (Vector2) transform.position + offset;
            TeleportTo(pos);
        }

        private static float MinHitDistance(int amount)
        {
            return RCastTempArray
                .Take(amount)
                .Select(hit2D => hit2D.distance)
                .Where(dist => dist > 0)
                .DefaultIfEmpty(0F)
                .Min();
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
        public int Move(in Vector2 offset)
        {
            float length = offset.magnitude;
            int amount = Cast(offset.normalized, Filter, RCastTempArray, length + AntiTrapEpsilon);
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
                float actualDist = MinHitDistance(amount) - AntiTrapEpsilon;
                float actualScale = actualDist / length;
                transform.Translate(offset * actualScale);
            }
            return amount;
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
        protected Vector2 GetDirXWithSlope(float xSpeed)
        {
            float x = xSpeed == 0 ? lastXDir : Math.Sign(XSpeed);
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

        protected int Cast(Vector2 direction,
            ContactFilter2D contactFilter,
            RaycastHit2D[] results,
            float distance = Mathf.Infinity)
        {
            int count = R2d.Cast(direction, contactFilter, results, distance);
            bool considerPlatform = direction.y < 0 && -direction.y > Mathf.Abs(direction.x);

            int bias = 0;
            for (var i = 0; i < count; i++)
            {
                var result = results[i];
                if (bias > 0)
                {
                    results[i - bias] = result;
                }
                // 如果不是平台，不做处理
                var rig = result.rigidbody != null ? (Component) result.rigidbody : result.collider;
                if (!rig.CompareTag(Tags.Platform)) continue;
                // normal.y >= 0 && normal.y > abs(normal.x)
                // 不考虑平台，或者法线不超上的情况，此时把平台视作空心的
                if (!considerPlatform 
                    || !this.IsStandingOnPlatform(result.point)
                    || result.normal.y < Mathf.Abs(result.normal.x))
                {
                    // 说明这个实心是平台且需要剔除，那么把它从 Cast 结果中剔除
                    CullPlatform();
                }
                
                void CullPlatform()
                {
                    bias++;
                }
            }

            return count - bias;

        }
    }
}