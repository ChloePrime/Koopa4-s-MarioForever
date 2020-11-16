using SweetMoleHouse.MarioForever.Constants;
using System;
using System.Collections;
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

        private static readonly string 
            STATE_STATIC = "Static",
            STATE_WALK   = "Walk",
            STATE_JUMP   = "Jump",
            STATE_CROUCH = "Crouch";
        private static readonly string[] POSSIBLE_STATES = { STATE_STATIC, STATE_WALK, STATE_JUMP, STATE_CROUCH };
        private static readonly Dictionary<(string, MarioPowerup), string> STATE_NAME_CACHE
            = new Dictionary<(string, MarioPowerup), string>(
                POSSIBLE_STATES.Length * Enum.GetValues(typeof(MarioPowerup)).Length);

        static MarioAnim()
        {
            foreach (string state in POSSIBLE_STATES)
            {
                foreach (MarioPowerup powerup in Enum.GetValues(typeof(MarioPowerup)))
                {
                    STATE_NAME_CACHE[(state, powerup)] = $"{state}-{(int)powerup}";
                }
            }
        }
        private void Start()
        {
            mario = transform.parent.GetComponent<Mario>();
            anim = GetComponent<Animator>();
        }

        private static readonly Vector2 VEC_M1_1 = new Vector2(-1, 1);
        private static readonly float
            MIN_WALK_SPEED = 0.36f,
            MAX_WALK_SPEED = 1.25f;
        private void LateUpdate()
        {
            if (mario.Crouching)
            {
                ChangeAnimation(STATE_CROUCH);
            }
            else if (!mario.Mover.IsOnGround)
            {
                ChangeAnimation(STATE_JUMP);
            }
            else if (Mathf.Abs(mario.Mover.XSpeed) > 1e-4)
            {
                ChangeAnimation(STATE_WALK);
                float xSpeed = Mathf.Abs(mario.Mover.XSpeed) / mario.Mover.RunProfile.maxSpeed;
                xSpeed = Mathf.Clamp(xSpeed, MIN_WALK_SPEED, MAX_WALK_SPEED);
                anim.SetFloat("X Speed", xSpeed);
            }
            else
            {
                ChangeAnimation(STATE_STATIC);
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

        private string lastStateName = STATE_STATIC;
        private void ChangeAnimation(string name)
        {
            var targetName = STATE_NAME_CACHE[(name, mario.Powerup)];
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName(targetName))
            {
                lastStateName = name;
                anim.Play(targetName);
            }
        }

        private void Refresh()
        {
            ChangeAnimation(lastStateName);
        }
    }
}