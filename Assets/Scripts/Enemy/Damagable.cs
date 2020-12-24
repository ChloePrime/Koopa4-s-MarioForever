using SweetMoleHouse.MarioForever.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Enemy
{
    /// <summary>
    /// 敌人的攻击判定
    /// 该脚本使用这个物品的父物体作为敌人本体
    /// </summary>
    public class Damagable : Stompable
    {
        [SerializeField, RenameInInspector("可被攻击的方式")]
        private EnumDamageType acceptedDamageTypes = 0;
        [SerializeField, RenameInInspector("本体")]
        private Transform self;
        [SerializeField, RenameInInspector("尸体")]
        private GameObject corpse;
        [SerializeField, RenameInInspector("踩踏音效")]
        private AudioClip stompSound;

        private bool isDead;
        private Corpse runtimeCorpse;

        protected override void Start()
        {
            base.Start();
            if (self == null)
            {
                self = transform.parent;
            }

            corpse = Instantiate(corpse, self.parent);
            runtimeCorpse = corpse.GetComponent<Corpse>();
            corpse.SetActive(false);
        }

        public virtual void Attack(in EnumDamageType type)
        {
            if ((acceptedDamageTypes & type) > 0)
            {
                SetDead(type);
            }
        }

        public virtual void SetDead(in EnumDamageType type)
        {
            if (isDead)
            {
                return;
            }
            isDead = true;

            if (type == EnumDamageType.STOMP)
            {
                Global.PlaySound(stompSound);
            }

            corpse.SetActive(true);
            runtimeCorpse.AcceptBody(self.GetComponent<SpriteRenderer>());
            if (TryGetComponent(out MarioDamager damager))
            {
                damager.enabled = false;
            }
            Destroy(self.gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnCollisionStay2D(collision);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            bool isMario = collision.transform.TryGetComponent(out Mario mario);
            if (isMario && IsStomp(collision.transform))
            {
                Attack(EnumDamageType.STOMP);
                mario.OnStomp(GetStompPower(mario), true);
            }
        }

        private static float GetStompPower(in Mario mario)
        {
            return mario.Jumper.IsHoldingJumpKey ? 20 : 13;
        }
    }
}