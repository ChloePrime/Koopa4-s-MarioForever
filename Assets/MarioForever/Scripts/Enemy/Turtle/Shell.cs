using SweetMoleHouse.MarioForever.Scripts.Base.Rpg;
using SweetMoleHouse.MarioForever.Scripts.Constants;
using SweetMoleHouse.MarioForever.Scripts.Util;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Enemy.Turtle {
/// <summary>
/// 龟壳被踢
/// </summary>
public class Shell : MonoBehaviour {
    private void Start() {
        _kickHandler = this.BfsComponentInChildren<DamageReceiver>();
        this.BfsComponentInChildren<DamageSource>().OnPreDamage += damage => {
            IDamageReceiver kicker = damage.Target;
            // 一般敌人: OnPreDamage的第二个参数为被攻击者
            // 贵客:    OnPreDamage的第二个参数为攻击者
            if (ReferenceEquals(kicker.MyDamageSource, null)) {
                return ActionResult.PASS;
            }

            kicker.MyDamageSource.DoDamageTo(_kickHandler, EnumDamageType.KICK_SHELL);
            return ActionResult.CANCEL;
        };
        _kickHandler.OnGetDeathSound += dmg => dmg.Type == EnumDamageType.KICK_SHELL ? kickSound : null;
    }

    [SerializeField, RenameInInspector("踢龟壳音效")]
    private AudioClip kickSound;

    private DamageReceiver _kickHandler;
}
}
