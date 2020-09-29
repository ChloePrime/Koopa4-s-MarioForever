using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

namespace SweetMoleHouse.MarioForever
{
    /// <summary>
    /// 移动的东西
    /// 需要自身有<see cref="Rigidbody2D"/>方可生效
    /// </summary>
    public class MovingThing : MonoBehaviour
    {
        protected static readonly float ANTI_TRAP_EPSILON = Consts.PIXEL_EPSLION;
        protected static ContactFilter2D FILTER = new ContactFilter2D().NoFilter();
        protected static bool inited = false;

        [SerializeField]
        private float gravity = 0f;

        /// <summary>
        /// 速度矢量（px/frame）
        /// </summary>
        protected Vector2 vel = Vector2.zero;
        public bool IsOnGround { get; private set; }

        public float XSpeed { get => vel.x; set => vel.x = value; }
        public float YSpeed { get => vel.y; set => vel.y = value; }
        public virtual float Gravity { get => gravity; set => gravity = value; }

        protected int antiStuckCd;
        protected Rigidbody2D r2d;
        protected virtual void Start()
        {
            InitClass();
            r2d = GetComponent<Rigidbody2D>();
            antiStuckCd = gameObject.GetInstanceID() % 8;
        }

        private void InitClass()
        {
            if (inited) return;
            FILTER.SetLayerMask(~LayerMask.GetMask(Consts.LAYER_ALL_MOVEABLE));
            FILTER.useTriggers = false;
            inited = true;
        }

        /// <summary>
        /// 最小速度的平方
        /// 若速度的平方小于这个值，那么被视为静止
        /// </summary>
        private static readonly float SQR_MIN_SPEED = 1e-8f;
        protected virtual void FixedUpdate()
        {
            //应用重力
            YSpeed -= Gravity;
            transform.Translate(vel);
            /*
            //跳过静止的物体
            if (vel.sqrMagnitude < SQR_MIN_SPEED) return;
            //移动
            if (IsOnGround && YSpeed <= 1e-5)
            {
                MoveOnGround(vel * Consts.ONE_PIXEL);
                YSpeed = 0;
            }
            else
            {
                //检测是否对着墙运动
                //并在检测到对着墙运动的时候取消速度
                CheckWallSticking(true, ref vel.x);
                CheckWallSticking(false, ref vel.y);
                Move(vel * Consts.ONE_PIXEL);
            }
            //防止卡墙
            antiStuckCd++;
            if (antiStuckCd >= 8)
            {
                antiStuckCd -= 8;
                AntiStuck();
            }
            //判断是否踩空
            bool onGround = MFUtil.YBottomCast(r2d);
            HandleFallFromCliff(ref onGround);
            //更新状态信息（是否落地等）
            UpdateStatus(onGround);
            */
        }

        /// <summary>
        /// 判断是否踩空
        /// </summary>
        private void HandleFallFromCliff(ref bool onGround)
        {
            if (!onGround) return;
            var origin = new Vector2(MFUtil.XCenter(r2d), MFUtil.YBottom(r2d));
            var size = new Vector2(MFUtil.Width(r2d) - 2 * Consts.ONE_PIXEL, 8 * Consts.ONE_PIXEL);
            var shouldFall = Physics2D.BoxCast(origin, size, 0f, Vector2.zero, FILTER, CAST_RESULT_ARRAY, 0) == 0;
            if (!shouldFall) return;

            onGround = false;
            bool leftSideOnGround = Physics2D.BoxCast(origin, size, 0f,
                Vector2.left, FILTER, CAST_RESULT_ARRAY, 2 * Consts.ONE_PIXEL) > 0;
            transform.Translate(Consts.ONE_PIXEL * (leftSideOnGround ? Vector2.right : Vector2.left));
        }

        protected virtual void UpdateStatus(in bool onGround)
        {
            IsOnGround = onGround;
        }

        protected static readonly List<RaycastHit2D> CAST_RESULT_ARRAY = new List<RaycastHit2D>();
        /// <summary>
        /// 检测这个物体是否贴着墙运动
        /// 如果是的话将对应轴的速度设置为0
        /// </summary>
        private void CheckWallSticking(bool xOrY, ref float fieldToReset)
        {
            var dir = new Vector2(xOrY ? vel.x : 0, xOrY ? 0 : vel.y);
            if (dir == Vector2.zero)
            {
                return;
            }
            int impactNum = r2d.Cast(dir, FILTER, CAST_RESULT_ARRAY, Consts.ONE_PIXEL);

            if (impactNum > 0)
            {
                //AntiStuck(-0.1f * dir);
                fieldToReset = 0;
            }
        }

        private static readonly float SLOPE_THRESHOLD = 1 / 3f;
        private static readonly float SLOPE_MOVE_STEP = 8 * Consts.ONE_PIXEL;
        /// <summary>
        /// 会计算斜面的运动
        /// </summary>
        /// <param name="vel">输入速度，只会考虑x轴</param>
        private void MoveOnGround(in Vector2 vel)
        {
            var xMovement = vel.x;
            if (Math.Abs(xMovement) <= 1e-4)
            {
                return;
            }
            int sign = Math.Sign(xMovement);
            xMovement = Math.Abs(xMovement);
            float stepVel = SLOPE_MOVE_STEP * sign;
            while (xMovement > SLOPE_MOVE_STEP)
            {
                bool fails = MoveSlopeStep(stepVel);
                if (fails)
                {
                    return;
                }
                xMovement -= SLOPE_MOVE_STEP;
            }
            MoveSlopeStep(xMovement * sign);
        }

        /// <summary>
        /// 分块计算斜面的每一块
        /// </summary>
        /// <param name="xMovement">x位移</param>
        /// <returns>是否撞墙</returns>
        private bool MoveSlopeStep(float xMovement)
        {

            transform.Translate(xMovement, 0, 0);
            bool onGroundAfter = MFUtil.YBottomCast(r2d);
            float acceptableSlopeDeltaY = MFUtil.Width(r2d) * SLOPE_THRESHOLD;
            if (onGroundAfter)
            {
                //移动后还在地里，说明是上坡
                transform.Translate(0, acceptableSlopeDeltaY, 0);
                //如果撞到墙，那么使用普通的移动代码
                bool hitsWall = xMovement > 0 ? MFUtil.XRightCast(r2d, false) : MFUtil.XLeftCast(r2d, false);
                if (hitsWall)
                {
                    transform.Translate(-xMovement, -acceptableSlopeDeltaY, 0);
                    XSpeed = 0;
                    return true;
                }
                Move(new Vector2(0, -acceptableSlopeDeltaY));
            }
            else
            {
                //下坡
                transform.Translate(0, -acceptableSlopeDeltaY, 0);
                if (MFUtil.YBottomCast(r2d, false))
                {
                    Global.DebugText = "下坡 " + UnityEngine.Random.Range(1, 100);
                    //AntiStuck(new Vector2(0, acceptableSlopeDeltaY));
                }
                else
                {
                    //踩空的情况
                    transform.Translate(0, acceptableSlopeDeltaY, 0);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 移动一定距离
        /// </summary>
        /// <param name="vel">要移动的位移矢量</param>
        public void Move(in Vector2 vel) => Move(vel, out _);

        /// <summary>
        /// 移动一定距离，返回是否撞到东西
        /// </summary>
        /// <param name="vel">要移动的位移矢量</param>
        /// <param name="hasCollidered">是否撞墙</param>
        public void Move(in Vector2 vel, out bool hasCollidered)
        {
            float near = 0f, far = vel.magnitude, distance = far;
            int impactNum = r2d.Cast(vel, FILTER, CAST_RESULT_ARRAY, distance);
            if (impactNum != 0)
            {
                //对分查找查找正确的移动距离
                //⚪----------->|//////////////|
                //⚪------------------> far
                //⚪----------> near
                //⚪--------------> mid
                do
                {
                    distance = (far + near) / 2;
                    impactNum = r2d.Cast(vel, FILTER, CAST_RESULT_ARRAY, distance);
                    if (impactNum > 0)
                    {
                        far = distance;
                    }
                    else
                    {
                        near = distance;
                    }
                }
                while ((far - near) > Consts.ONE_PIXEL);
                //distance到底在墙内还是墙外是未知的
                //所以取near作为实际距离
                distance = near;
                transform.Translate(vel.normalized * distance);
                hasCollidered = true;
            }
            else
            {
                transform.Translate(vel);
                hasCollidered = false;
            }
        }

        protected static readonly Collider2D[] CONTACT_RESULT_ARRAY = new Collider2D[1];
        /// <summary>
        /// 把这个物体挤出墙体
        /// 根据第一个接触点判断方向
        /// </summary>
        public void AntiStuck()
        {
            if (!r2d.IsTouching(FILTER))
            {
                return;
            }

            bool left = MFUtil.XLeftCast(r2d, false),
                right = MFUtil.XRightCast(r2d, false),
                top = MFUtil.YTopCast(r2d, false),
                bottom = MFUtil.YBottomCast(r2d, false);

            if (right && !left)
            {
                AntiStuck(0.2f * Vector2.left);
            }
            else if (left && !right)
            {
                AntiStuck(0.2f * Vector2.right);
            }
            if(bottom && !top)
            {
                AntiStuck(0.2f * Vector2.up);
            }
            else if (top && !bottom)
            {
                AntiStuck(0.2f * Vector2.down);
            }
        }

        /// <summary>
        /// 防卡墙步长（2像素）
        /// </summary>
        private static readonly float ANTI_STUCK_MOVE_STEP = 2 * Consts.ONE_PIXEL;
        /// <summary>
        /// 将该对象弹出一定距离
        /// </summary>
        /// <param name="vec">法线方向，朝外，不要填入太大的值</param>
        public void AntiStuck(in Vector2 vec)
        {
            Vector2 stepVec = vec.normalized * ANTI_STUCK_MOVE_STEP;
            float targetProgress = vec.sqrMagnitude;
            for (float prg = 0f; prg * prg < targetProgress; prg += ANTI_STUCK_MOVE_STEP)
            {
                transform.Translate(stepVec);
                if (r2d.IsTouching(FILTER)) continue;
                Move(-vec);
                break;
            }
        }
    }
}