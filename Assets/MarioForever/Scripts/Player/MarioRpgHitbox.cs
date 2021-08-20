using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SweetMoleHouse.MarioForever.Scripts.Base.Rpg;
using SweetMoleHouse.MarioForever.Scripts.Enemy;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Player
{
    /// <summary>
    /// 马里奥的伤害判定
    /// </summary>
    public class MarioRpgHitbox : MonoBehaviour, IDamageReceiver
    {
        public Transform Host => Mario.transform;
        public Mario Mario { get; private set; }
        public Faction Faction => faction;
        public void Damage(Transform damager, EnumDamageType type)
        {
            Mario.Damage(damager);
        }

        public void SetDead(Transform damager, EnumDamageType type)
        {
            if (Mario != null)
            {
                Mario.Kill();
            }
        }

        private void Start()
        {
            Mario = GetComponentInParent<Mario>();
        }
        
        [SerializeField] private Faction faction;
    }
}