using SweetMoleHouse.MarioForever.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SweetMoleHouse
{
    /// <summary>
    /// 可以伤害到马里奥的东西
    /// </summary>
    public class MarioDamager : MonoBehaviour
    {
        [SerializeField, RenameInInspector("是否可以踩")]
        private bool stompable;
        [SerializeField, RenameInInspector("踩踏判定分界线")]
        private Transform stompPos;

        private void OnCollisionStay2D(Collision2D collision)
        {
            var hitbox = collision.collider.GetComponent<MarioRpgHitbox>();
            if (hitbox == null)
            {
                return;
            }
            //可以踩
            if (stompable && hitbox.Mario.transform.position.y >= stompPos.position.y)
            {
                return;
            }
        }
    }
}