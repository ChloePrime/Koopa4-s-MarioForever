using SweetMoleHouse.MarioForever.Scripts.Base.Physics;
using SweetMoleHouse.MarioForever.Scripts.Constants;
using SweetMoleHouse.MarioForever.Scripts.Util;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Enemy.Turtle
{
    /// <summary>
    /// 龟壳被踢
    /// </summary>
    public class Shell : MonoBehaviour
    {
        private void Start()
        {
            kickHandler = this.BfsComponentInChildren<DamageReceiver>();
            this.BfsComponentInChildren<DamageSource>().OnPreDamage += (_, kicker) =>
            {
                // 一般敌人: OnPreDamage的第二个参数为被攻击者
                // 贵客:    OnPreDamage的第二个参数为攻击者
                kickHandler.Damage(kicker.Host, EnumDamageType.KICK_SHELL);
                return ActionResult.CANCEL;
            };
            kickHandler.OnGetDeathSound += dmgType => dmgType == EnumDamageType.KICK_SHELL ? kickSound : null;
        }
        
        [SerializeField, RenameInInspector("踢龟壳音效")]
        private AudioClip kickSound;

        private DamageReceiver kickHandler;
    }
}