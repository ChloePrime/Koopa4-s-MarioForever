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
        private void Start()
        {
            mario = transform.parent.GetComponent<Mario>();
            anim = GetComponent<Animator>();
        }

        private static readonly Vector2 VEC_M1_1 = new Vector2(-1, 1);
        private void LateUpdate()
        {
            anim.SetBool("马里奥在空中", !mario.Mover.IsOnGround);
            anim.SetFloat("x速度", Mathf.Abs(mario.Mover.XSpeed) / mario.Mover.RunProfile.maxSpeed);
            int curAnimDir = Math.Sign(mario.transform.localScale.x);
            if ((mario.Mover.AccDirection + curAnimDir) == 0)
            {
                mario.transform.localScale *= VEC_M1_1;
            }
        }
    }
}