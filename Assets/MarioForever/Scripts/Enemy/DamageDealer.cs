using SweetMoleHouse.MarioForever.Scripts.Player;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Enemy
{
    /// <summary>
    /// 可以伤害到马里奥的东西
    /// </summary>
    public class DamageDealer : Stompable
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            OnTriggerStay2D(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            var isMario = other.transform.TryGetComponent(out MarioRpgHitbox hitbox);
            if (!isMario)
            {
                return;
            }

            var mario = hitbox.Mario;
            //可以踩
            if (IsStomp(mario.transform))
            {
                return;
            }
            mario.Damage();
        }
    }
}