using System;
using Cysharp.Threading.Tasks;
using SweetMoleHouse.MarioForever.Scripts.Level;
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

    /// <summary>
    /// 无敌到期提醒时间，单位为秒。
    /// </summary>
    [SerializeField, RenameInInspector("无敌到期提醒时间")]
    private float timeUpHintTime = 1.2F;

    [SerializeField, RenameInInspector("无敌音乐")]
    private AudioClip starMusic;
    [SerializeField, RenameInInspector("无敌到期提醒音效")]
    private AudioClip timeUpHintSound;
    
    public void OnPowerup(Mario mario) {
        mario.InvincibleController.RainbowFlashTime = invincibleTime;
        mario.InvincibleController.Invincible = true;
        mario.InvincibleController.HasActiveDamage = true;
        
        AreaManager.SetOverrideMusic(starMusic);
        ListenStarTime(mario);
    }

    private async void ListenStarTime(Mario mario) {
        if (_listenToStarTime) {
            return;
        }
        
        AreaManager.SetOverrideMusic(starMusic);
        _listenToStarTime = true;
        _warn = true;
        
        while (true) {
            await UniTask.NextFrame(PlayerLoopTiming.PostLateUpdate);
            if (mario == null) {
                break;
            }

            float invTime = mario.InvincibleController.RainbowFlashTime;
            switch (_warn) {
                case true when invTime <= timeUpHintTime:
                    Global.PlaySound(timeUpHintSound);
                    _warn = false;
                    break;
                // 无敌星到期提醒后又吃了一个无敌星的情况
                case false when invTime > timeUpHintTime:
                    _warn = true;
                    break;
            }

            if (mario.InvincibleController.RainbowFlashTime <= 0) {
                AreaManager.StopOverrideMusic();
                break;
            }
        }

        _listenToStarTime = false;
    }

    private static bool _listenToStarTime;
    private static bool _warn;
}
}
