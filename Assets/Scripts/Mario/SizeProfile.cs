using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Player
{
    /// <summary>
    /// 马里奥的单个个子的一些子对象引用
    /// </summary>
    public class SizeProfile : MonoBehaviour 
    {
        public Transform PhysicHitbox { get; private set; }
        public Transform DamageHitbox { get; private set; }
        public Transform Center { get; private set; }

        private void Start()
        {
            PhysicHitbox = transform.GetChild(0);
            DamageHitbox = transform.GetChild(1);
            Center = transform.GetChild(2);
        }
    }
}