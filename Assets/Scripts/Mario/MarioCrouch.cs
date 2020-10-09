using SweetMoleHouse.MarioForever.Base;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace SweetMoleHouse.MarioForever.Player
{
    /// <summary>
    /// 马里奥下蹲
    /// </summary>
    public class MarioCrouch : MonoBehaviour 
    {
        private Mario mario;
        private bool crouchInput;
        private bool crouching;
        public bool Crouching
        {
            get => crouching;
            set
            {
                crouching = value;
                mario.Mover.ControlEnabled = !value;
                mario.Anims.SetBool("下蹲", value);
                if (value)
                {
                    mario.Size = MarioSize.SMALL;
                }
                else
                {
                    mario.RefreshSize();
                }
            }
        }
        private void Start()
        {
            mario = GetComponent<Mario>();
            Global.Inputs.Mario.Down.performed += OnCrouch;
            Global.Inputs.Mario.Down.canceled += OnLeaveCrouch;
        }

        private void OnDisable()
        {
            Global.Inputs.Mario.Down.performed -= OnCrouch;
            Global.Inputs.Mario.Down.canceled -= OnLeaveCrouch;
        }
        private void OnCrouch(CallbackContext ctx) => crouchInput = true;
        private void OnLeaveCrouch(CallbackContext ctx) => crouchInput = false;

        private void FixedUpdate()
        {
            //小个子没有下蹲操作
            if (mario.GetRealSize() == MarioSize.SMALL)
            {
                return;
            }

            if (crouchInput && !Crouching)
            {
                Crouching = true;
            }
            else if(Crouching && !crouchInput)
            {
                TryCancelCrouch();
            }

            bool shouldDisableXMove = Crouching && mario.Mover.IsOnGround;
            mario.Mover.ControlEnabled = !shouldDisableXMove;
        }

        private void TryCancelCrouch()
        {
            bool touchedTop = mario.Mover.R2d.Cast(Vector2.up, BasePhysics.GlobalFilter, Global.RCAST_TEMP_ARRAY, Mario.DeltaSizeSmallToBig) > 0;
            if (!touchedTop)
            {
                Crouching = false;
            }
        }
    }
}