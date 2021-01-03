using System;
using UnityEngine;
using SweetMoleHouse.MarioForever.Base;
using SweetMoleHouse.MarioForever.Level;
using SweetMoleHouse.MarioForever.Util;
using static UnityEngine.InputSystem.InputAction;
using static UnityEngine.Mathf;


namespace SweetMoleHouse.MarioForever.Player
{
    /// <summary>
    /// 马里奥横向移动
    /// </summary>
    public class MarioMove : BasePhysics
    {
        #region 可配置属性

        [Header("马里奥物理属性")]
        [SerializeField]
        private AccProfile walking = new AccProfile(35f / 8f, 0.125f, 0.5f);

        [SerializeField]
        private AccProfile running = new AccProfile(8f, 0.125f, 0.5f);
        public AccProfile RunProfile => running;

        [SerializeField, RenameInInspector("初速度")]
        private float minSpeed = 1f;
        [Serializable]
        public class AccProfile
        {
            [RenameInInspector("最大速度")] public float maxSpeed;
            [RenameInInspector("加速度（正向）")]  public float runAcc;
            [RenameInInspector("加速度（转向）")]  public float turnAcc;

            public AccProfile(float maxSpeed, float runAcc, float turnAcc)
            {
                this.maxSpeed = maxSpeed;
                this.runAcc = runAcc;
                this.turnAcc = turnAcc;
            }
        }

        #endregion

        /// <summary>
        /// 加速度方向，-1，0或1
        /// </summary>
        public int AccDirection { get; private set; }
        private bool IsHoldingRunKey { get; set; }
        private bool IsTowardsWall { get; set; }
        public bool IsRunning { get => IsHoldingRunKey && !IsTowardsWall; }
        public AccProfile CurProfile { get => IsRunning ? running : walking; }
        private bool IsTurning { get => (Math.Sign(XSpeed) != AccDirection); }
        private Mario mario;
        private MarioJump jumper;

        protected override void Start()
        {
            base.Start();
            mario = GetComponent<Mario>();
            jumper = GetComponent<MarioJump>();
            var input = Global.Inputs.Mario;
            input.HorizontalMove.performed += OnMoveX;
            input.FireOrRun.performed += OnRunPressed;
            input.FireOrRun.canceled += OnRunReleased;
        }
        private void OnDisable()
        {
            var input = Global.Inputs.Mario;
            input.HorizontalMove.performed -= OnMoveX;
            input.FireOrRun.performed -= OnRunPressed;
            input.FireOrRun.canceled -= OnRunReleased;
        }
        private void OnMoveX(CallbackContext ctx) => AccDirection = Math.Sign(ctx.ReadValue<float>());
        private void OnRunPressed(CallbackContext ctx) => IsHoldingRunKey = true;
        private void OnRunReleased(CallbackContext ctx) => IsHoldingRunKey = false;

        public override float Gravity { get => base.Gravity * jumper.GetGravityScale(); }

        protected override void FixedUpdate()
        {
            var accDir = mario.ControlDisabled ? 0 : AccDirection;
            if (accDir != 0)
            {
                AddSpeed();
            }
            else if (XSpeed != 0f)
            {
                DecrSpeed();
            }
            base.FixedUpdate();
        }

        protected override void StopTowardsWall(in Vector2 dir, ref float fieldToSet)
        {
            base.StopTowardsWall(dir, ref fieldToSet);
            
            var isAxisX = Abs(dir.x) > Abs(dir.y);
            if (!isAxisX) return;
            if (dir.x > 0)
            {
                var xRight = MFUtil.XRight(R2d);
                if (xRight >= ScrollInfo.Right)
                {
                    fieldToSet = 0;
                    transform.Translate(ScrollInfo.Right - xRight, 0, 0);
                }
            }
            else
            {
                var xLeft = MFUtil.XLeft(R2d);
                if (xLeft <= ScrollInfo.Left)
                {
                    fieldToSet = 0;
                    transform.Translate(ScrollInfo.Left - xLeft, 0, 0);
                }
            }
        }

        private void AddSpeed()
        {
            //区分当前是否属于转向阶段
            if (IsTurning)
            {
                XSpeed += CurProfile.turnAcc * AccDirection * Time.fixedDeltaTime;
            }
            else
            {
                //初速度
                if (Math.Abs(XSpeed) < minSpeed)
                {
                    XSpeed += minSpeed * AccDirection;
                }
                XSpeed += CurProfile.runAcc * AccDirection * Time.fixedDeltaTime;
            }
            XSpeed = Clamp(XSpeed, -CurProfile.maxSpeed, CurProfile.maxSpeed);
        }

        private void DecrSpeed()
        {
            int signBefore = Math.Sign(XSpeed);
            XSpeed -= CurProfile.runAcc * signBefore * Time.fixedDeltaTime;
            //如果减速后变号，说明已减速至0
            if (signBefore != Math.Sign(XSpeed))
            {
                XSpeed = 0f;
            }
        }

        protected override void HitWallY(in Collider2D[] colliders)
        {
            if (YSpeed <= 0)
            {
                mario.ComboInfo.ResetCombo();
            }
            base.HitWallY(in colliders);
        }
    }
}