using SweetMoleHouse.MarioForever.Scripts.Player;
using SweetMoleHouse.MarioForever.Scripts.Util;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Bonus {
public class InvincibleStar : MonoBehaviour, IPowerupBehavior {
    /// <summary>
    /// 无敌时间，单位为秒。
    /// </summary>
    [SerializeField, RenameInInspector("无敌时间")]
    private float invincibleTime = 10;
    
    public void OnPowerup(Mario mario) {
        mario.InvincibleController.RainbowFlashTime = invincibleTime;
        mario.InvincibleController.Invincible = true;
        mario.InvincibleController.HasActiveDamage = true;
    }
}
}
