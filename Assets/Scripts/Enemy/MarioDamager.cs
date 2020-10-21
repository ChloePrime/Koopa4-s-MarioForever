using SweetMoleHouse.MarioForever.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Enemy
{
    /// <summary>
    /// 可以伤害到马里奥的东西
    /// </summary>
    public class MarioDamager : Stompable
    {
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