using SweetMoleHouse.MarioForever.Constants;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Player
{
    /// <summary>
    /// 马里奥动画
    /// </summary>
    public class MarioAnim : MonoBehaviour 
    {
        private Mario mario;
        private Animator anim;
        private float shootAnimTime;

        private const float ShootAnimLength = 0.1F;

        private const string 
            StateStatic = "Static",
            StateWalk   = "Walk",
            StateJump   = "Jump",
            StateShoot  = "Shoot",
            StateCrouch = "Crouch";
        private static readonly string[] POSSIBLE_STATES =
        {
            StateStatic, StateWalk, StateJump, StateShoot, StateCrouch
        };
        private static readonly Dictionary<(string, MarioPowerup), int> STATE_IDX_CACHE 
            = new Dictionary<(string, MarioPowerup), int>(
                POSSIBLE_STATES.Length * Enum.GetValues(typeof(MarioPowerup)).Length);

        static MarioAnim()
        {
            foreach (string state in POSSIBLE_STATES)
            {
                foreach (MarioPowerup powerup in Enum.GetValues(typeof(MarioPowerup)))
                {
                    STATE_IDX_CACHE[(state, powerup)] = Animator.StringToHash(
                        $"{state}-{(int)powerup}");
                }
            }
        }

        public void StartShooting()
        {
            shootAnimTime = ShootAnimLength;
        }
        private void Start()
        {
            mario = transform.parent.GetComponent<Mario>();
            anim = GetComponent<Animator>();
        }

        private static readonly Vector2 VEC_M1_1 = new Vector2(-1, 1);
        private const float
            MinWalkSpeed = 0.36f,
            MaxWalkSpeed = 1.25f;
        private void LateUpdate()
        {
            if (mario.Crouching)
            {
                ChangeAnimation(StateCrouch);
            }
            else if (shootAnimTime > 0)
            {
                // 修复: 必须在马里奥可以射击的状态才会切换射击动画
                if (mario.PowerupManager.CanShoot)
                {
                    ChangeAnimation(StateShoot);
                    shootAnimTime -= Time.deltaTime;
                }
                else
                {
                    shootAnimTime = 0;
                }
            }
            else if (!mario.Mover.IsOnGround)
            {
                ChangeAnimation(StateJump);
            }
            else if (Mathf.Abs(mario.Mover.XSpeed) > 1e-4)
            {
                ChangeAnimation(StateWalk);
                float xSpeed = Mathf.Abs(mario.Mover.XSpeed) / mario.Mover.RunProfile.maxSpeed;
                xSpeed = Mathf.Clamp(xSpeed, MinWalkSpeed, MaxWalkSpeed);
                anim.SetFloat(PROP_X_SPEED, xSpeed);
            }
            else
            {
                ChangeAnimation(StateStatic);
            }
            //马里奥动画左右方向
            int curAnimDir = Math.Sign(mario.transform.localScale.x);
            if (mario.Mover.AccDirection + curAnimDir == 0)
            {
                mario.Direction = mario.Mover.AccDirection;
                mario.transform.localScale *= VEC_M1_1;
            }
        }

        private static readonly int PROP_X_SPEED = Animator.StringToHash("X Speed");

        private void ChangeAnimation(string animName)
        {
            var targetHash = STATE_IDX_CACHE[(animName, mario.Powerup)];
            if (anim.GetCurrentAnimatorStateInfo(0).shortNameHash == targetHash) return;
            anim.Play(targetHash);
        }
    }
}