using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace SweetMoleHouse.MarioForever.Player
{
    /// <summary>
    /// 马里奥横向移动
    /// </summary>
    public class MarioMove : MovingThing
    {
        #region 可配置属性
        [SerializeField]
        private AccProfile walking = new AccProfile(35f / 8f, 0.125f, 0.5f);
        [SerializeField]
        private AccProfile running = new AccProfile(8f, 0.125f, 0.5f);
        [SerializeField]
        private float minSpeed = 1f;
        [Serializable]
        private class AccProfile
        {
            public readonly float maxSpeed;
            public readonly float runAcc;
            public readonly float turnAcc;

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
        private int accDirection;
        private bool IsHoldingRunKey { get; set; }
        private bool IsTowardsWall { get; set; }
        public bool IsRunning { get => IsHoldingRunKey && !IsTowardsWall; }
        private AccProfile CurProfile { get => IsRunning ? running : walking; }
        private bool IsTurning { get => (Math.Sign(XSpeed) != accDirection); }
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
        private void OnMoveX(CallbackContext ctx) => accDirection = Math.Sign(ctx.ReadValue<float>());
        private void OnRunPressed(CallbackContext ctx) => IsHoldingRunKey = true;
        private void OnRunReleased(CallbackContext ctx) => IsHoldingRunKey = false;

        public override float Gravity { get => base.Gravity * jumper.GetGravityScale(); }

        protected override void FixedUpdate()
        {
            if (accDirection != 0)
            {
                AddSpeed();
            }
            else if (XSpeed != 0f)
            {
                DecrSpeed();
            }
            base.FixedUpdate();
        }

        private void AddSpeed()
        {
            //区分当前是否属于转向阶段
            if (IsTurning)
            {
                XSpeed += CurProfile.turnAcc * accDirection;
            }
            else
            {
                //初速度
                if (Math.Abs(XSpeed) < minSpeed)
                {
                    XSpeed += minSpeed * accDirection;
                }
                XSpeed += CurProfile.runAcc * accDirection;
            }
            XSpeed = Mathf.Clamp(XSpeed, -CurProfile.maxSpeed, CurProfile.maxSpeed);
        }

        private void DecrSpeed()
        {
            int signBefore = Math.Sign(XSpeed);
            XSpeed -= CurProfile.runAcc * signBefore;
            //如果减速后变号，说明已减速至0
            if (signBefore != Math.Sign(XSpeed))
            {
                XSpeed = 0f;
            }
        }

        private void OnGUI()
        {
            if (GUILayout.Button("变小"))
            {
                transform.localScale *= new Vector2(1, 0.5f);
            }
        }
    }
}