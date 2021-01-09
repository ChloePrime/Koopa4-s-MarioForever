using System;
using SweetMoleHouse.MarioForever.Scripts.Base.Rpg;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Enemy
{
    /// <summary>
    /// 伤害来源
    /// 需要自身带有 Trigger 类碰撞箱
    /// </summary>
    public class DamageSource : Stompable
    {
        [SerializeField] private Faction faction;
        [SerializeField] private EnumDamageType damageType;

        public event Action<IDamageReceiver> OnDamaging;
        private void OnTriggerEnter2D(Collider2D other)
        {
            OnTriggerStay2D(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.TryGetComponent(out IDamageReceiver hitbox)) return;
            if (!faction.IsHostileTo(hitbox.Faction)) return;
            // 判断是否是踩
            if (IsStomp(hitbox.Host))
            {
                return;
            }

            hitbox.Damage(damageType);
            OnDamaging?.Invoke(hitbox);
        }
    }
}