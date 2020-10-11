using SweetMoleHouse.MarioForever.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Reflection;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace SweetMoleHouse.MarioForever
{
    /// <summary>
    /// 马里奥跳跃
    /// </summary>
    public class MarioJump : MonoBehaviour
    {
        [RenameInInspector("跳跃高度")]
        public float jumpHeight;
        [Header("重力加速度倍率")]
        [SerializeField, RenameInInspector("走")]
        private float gravityScaleNonRunning = 0.6f;
        [SerializeField, RenameInInspector("跑")]
        private float gravityScaleRunning = 0.5f;
        /// <summary>
        /// X速度大于这个值才算跑起来
        /// </summary>
        [Space]
        [SerializeField, RenameInInspector("跑动高跳的最小x速度")]
        private float runSpeedThreshold = 5f / 8f;

        private Player.MarioMove mover;
        private bool isReadyToJump;
        public bool IsHoldingJumpKey { get; private set; }
        public float GetGravityScale()
        {
            if (!IsHoldingJumpKey || mover.YSpeed <= 0)
            {
                return 1f;
            }
            return (Math.Abs(mover.XSpeed) > runSpeedThreshold)
                ? gravityScaleRunning : gravityScaleNonRunning;
        }

        private void Start()
        {
            mover = GetComponent<Player.MarioMove>();
            Global.Inputs.Mario.Jump.performed += OnJumpInput;
            Global.Inputs.Mario.Jump.canceled += OnJumpRelease;
        }
        private void OnDisable()
        {
            Global.Inputs.Mario.Jump.performed -= OnJumpInput;
            Global.Inputs.Mario.Jump.canceled -= OnJumpRelease;
        }
        private void OnJumpInput(CallbackContext ctx)
        {
            IsHoldingJumpKey = true;
            if (mover.YSpeed <= 0)
            {
                isReadyToJump = true;
            }
        }
        private void OnJumpRelease(CallbackContext ctx)
        {
            IsHoldingJumpKey = false;
            isReadyToJump = false;
        }
        private void Update()
        {
            if (isReadyToJump && mover.IsOnGround)
            {
                Jump();
            }
        }

        public void Jump()
        {
            mover.YSpeed = jumpHeight;
            isReadyToJump = false;
        }
    }
}