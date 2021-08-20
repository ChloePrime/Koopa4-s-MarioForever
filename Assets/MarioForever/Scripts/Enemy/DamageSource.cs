using System;
using SweetMoleHouse.MarioForever.Scripts.Base.Rpg;
using SweetMoleHouse.MarioForever.Scripts.Constants;
using SweetMoleHouse.MarioForever.Scripts.Player;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SweetMoleHouse.MarioForever.Scripts.Enemy
{
    /// <summary>
    /// 伤害来源
    /// 需要自身带有 Trigger 类碰撞箱
    /// </summary>
    public class DamageSource : Stompable
    {
        public Faction Faction => faction;
        public EnumDamageType DamageType => damageType;

        public event Func<DamageSource, IDamageReceiver, ActionResult> OnPreDamage; 
        public event Action<DamageSource, IDamageReceiver> OnDamaging;

        public void DoDamageTo(IDamageReceiver hitbox)
        {
            if (!faction.IsHostileTo(hitbox.Faction)) return;
            // 判断是否是踩
            if (IsStomp(hitbox.Host, hitbox.Host.GetComponent<Mario>()))
            {
                return;
            }

            if (OnPreDamage?.Invoke(this, hitbox).IsCanceled() != true)
            {
                hitbox.Damage(transform, damageType);
            }
            OnDamaging?.Invoke(this, hitbox);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            OnTriggerStay2D(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.TryGetComponent(out IDamageReceiver hitbox)) return;
            DoDamageTo(hitbox);
        }
        
        [SerializeField] private Faction faction;
        [SerializeField] private EnumDamageType damageType;

    }
}