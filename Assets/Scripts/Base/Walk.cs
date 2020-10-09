using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Base
{
    /// <summary>
    /// 行走的东西
    /// 类似于RE的组1
    /// </summary>
    public class Walk : BasePhysics
    {
        [Header("物品行走设置")]
        [SerializeField, RenameInInspector("行走速度")]
        protected float walkSpeed = 1.5f;
        [SerializeField, RenameInInspector("与其他行走物品相撞")]
        private bool collideWithOthers = true;
        /// <summary>
        /// 带符号的行走速度
        /// </summary>
        protected float realWalkSpeed;

        protected override void Start()
        {
            base.Start();
            realWalkSpeed = walkSpeed;
        }

        protected override void Update()
        {
            XSpeed = realWalkSpeed;
            base.Update();
        }
        protected override void OnHitWallX(in Collider2D[] colliders)
        {
            base.OnHitWallX(colliders);
            realWalkSpeed *= -1;
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            var that = collision.collider.GetComponent<Walk>();
            if (that == null)
            {
                return;
            }
            //如果任意一方设置了不与其他物体碰撞则忽略
            if (!(collideWithOthers && that.collideWithOthers))
            {
                return;
            }
            if (transform.position.x < that.transform.position.x)
            {
                realWalkSpeed = -walkSpeed;
            }
            else
            {
                realWalkSpeed = walkSpeed;
            }
        }
    }
}