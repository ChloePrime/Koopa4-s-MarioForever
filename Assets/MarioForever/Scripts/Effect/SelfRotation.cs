using System;
using SweetMoleHouse.MarioForever.Base;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Effect
{
    /// <summary>
    /// 物体自转
    /// 默认的旋转正方向是逆时针，
    /// 对于走动的物体，运动速度为正时，自转正方向为顺时针
    /// </summary>
    public class SelfRotation : MonoBehaviour
    {
        [SerializeField, RenameInInspector("自转速度(度/秒)")]
        private float rotSpeed;

        private bool hasPhysics;
        private BasePhysics physics;

        private void Start()
        {
            hasPhysics = TryGetComponent(out physics);
            if (!hasPhysics)
            {
                hasPhysics = transform.parent.TryGetComponent(out physics);
            }
        }

        private void Update()
        {
            float deltaAngle = rotSpeed * Time.deltaTime;
            if (hasPhysics)
            {
                deltaAngle *= -Math.Sign(physics.XSpeed);
            }
            transform.Rotate(0, 0, deltaAngle);
        }
    }
}
