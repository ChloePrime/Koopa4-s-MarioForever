using System;
using SweetMoleHouse.MarioForever.Scripts.Base.Rpg;
using SweetMoleHouse.MarioForever.Scripts.Constants;
using SweetMoleHouse.MarioForever.Scripts.Player;
using SweetMoleHouse.MarioForever.Scripts.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace SweetMoleHouse.MarioForever.Scripts.Enemy
{
    /// <summary>
    /// 敌人的攻击判定
    /// 该脚本使用这个物品的父物体作为敌人本体
    /// </summary>
    public class DamageReceiver : Stompable, IDamageReceiver
    {
        [SerializeField, RenameInInspector("可被攻击的方式")]
        private EnumDamageType acceptedDamageTypes = 0;
        [FormerlySerializedAs("self")] [SerializeField, RenameInInspector("本体")]
        private Transform host;
        [SerializeField, RenameInInspector("尸体")]
        private GameObject corpse;
        [SerializeField, RenameInInspector("踩踏音效")]
        private AudioClip stompSound;
        [SerializeField, RenameInInspector("死亡音效")]
        private AudioClip defeatSound;
        [SerializeField, RenameInInspector("奖分")]
        private ScoreType score;

        [SerializeField] private Faction faction;

        private bool isDead;
        private Corpse runtimeCorpse;
        public Transform Host => host;
        public SpriteRenderer Renderer { get; private set; }

        public Faction Faction => faction;


        public event Func<EnumDamageType, ActionResult> OnGenerateCorpse;

        protected override void Start()
        {
            base.Start();
            Renderer = host.GetComponentInChildren<SpriteRenderer>();
            if (host == null)
            {
                host = GetComponent<Collider2D>().attachedRigidbody.transform;
            }

            corpse = Instantiate(corpse, host.parent);
            runtimeCorpse = corpse.GetComponent<Corpse>();
            corpse.SetActive(false);
        }


        public virtual void Damage(in EnumDamageType type)
        {
            if ((acceptedDamageTypes & type) > 0)
            {
                SetDead(type);
            }
        }

        public virtual void SetDead(in EnumDamageType type)
        {
            // 防止死亡代码重复执行
            if (isDead) return;
            isDead = true;

            if (type == EnumDamageType.STOMP)
            {
                Global.PlaySound(stompSound);
            }
            else
            {
                Global.PlaySound(defeatSound);
                score.Summon(host.transform);
            }

            GenerateCorpse(type);

            if (TryGetComponent(out DamageSource damager))
            {
                damager.enabled = false;
            }
            Destroy(host.gameObject);
        }

        private void GenerateCorpse(in EnumDamageType type)
        {
            bool overriden = OnGenerateCorpse?.Invoke(type).IsCanceled() ?? false;
            if (overriden)
            {
                Destroy(corpse);
                return;
            }
            
            corpse.SetActive(true);
            runtimeCorpse.AcceptBody(Renderer);
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
                Damage(EnumDamageType.STOMP);
                mario.OnStomp(GetStompPower(mario), true);
            }
        }

        private static float GetStompPower(in Mario mario)
        {
            return mario.Jumper.IsHoldingJumpKey ? 20 : 13;
        }
    }
}