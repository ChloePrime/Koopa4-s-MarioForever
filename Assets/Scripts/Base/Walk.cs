using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Base
{
    /// <summary>
    /// 行走的东西
    /// </summary>
    public class Walk : BasePhysics
    {
        [Header("物品行走设置")]
        [SerializeField, RenameInInspector("行走速度")]
        protected float walkSpeed = 1.5f;
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
            Global.DebugText = XSpeed.ToString();
        }
    }
}