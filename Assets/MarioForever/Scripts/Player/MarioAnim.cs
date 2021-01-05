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
        private MarioPowerup recordedSize = (MarioPowerup)(-1);

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
            if ((mario.Mover.AccDirection + curAnimDir) == 0)
            {
                mario.transform.localScale *= VEC_M1_1;
            }
            //状态刷新
            if (recordedSize != mario.Powerup)
            {
                Refresh();
                recordedSize = mario.Powerup;
            }
        }

        private string lastStateName = StateStatic;
        private static readonly int PROP_X_SPEED = Animator.StringToHash("X Speed");

        private void ChangeAnimation(string animName)
        {
            var targetHash = STATE_IDX_CACHE[(animName, mario.Powerup)];
            if (anim.GetCurrentAnimatorStateInfo(0).shortNameHash == targetHash) return;
            lastStateName = animName;
            anim.Play(targetHash);
        }

        private void Refresh()
        {
            ChangeAnimation(lastStateName);
        }
    }
}