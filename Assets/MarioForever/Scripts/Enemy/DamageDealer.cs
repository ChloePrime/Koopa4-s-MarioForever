using SweetMoleHouse.MarioForever.Player;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Enemy
{
    /// <summary>
    /// 可以伤害到马里奥的东西
    /// </summary>
    public class DamageDealer : Stompable
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            OnCollisionStay2D(other);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            var isMario = collision.transform.TryGetComponent(out Mario mario);
            if (!isMario)
            {
                return;
            }
            //可以踩
            if (IsStomp(mario.transform))
            {
                return;
            }
            mario.Damage();
        }
    }
}