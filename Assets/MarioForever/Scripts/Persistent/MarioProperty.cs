using System;
using Cysharp.Threading.Tasks;
using SweetMoleHouse.MarioForever.Scripts.Constants;
using SweetMoleHouse.MarioForever.Scripts.Player;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SweetMoleHouse.MarioForever.Scripts.Persistent {
public static class MarioProperty {
    /// <summary>
    /// 使用 <see cref="Mario.Powerup"/>
    /// 而不是直接设置这个字段
    /// </summary>
    public static MarioPowerup CurPowerup = MarioPowerup.BIG;

    public static int Lives = 4;
    /// <summary>
    /// 建议使用 <see cref="AddCoin"/> 来添加金币值。
    /// </summary>
    public static int Coins;
    public static int TimeFromLastScene = -1;
    public static long Score;

    /// <summary>
    /// 添加金币并给予这些金币的分数
    /// </summary>
    public static void AddCoin(int amount = 1) {
        const int pricePerLife = 100;
        Coins += amount;
        Score += amount * 200;
        if (Coins <= 100) {
            Global.PlaySound(ScoreTypeData.CoinSound);
            return;
        }

        int ups = Math.DivRem(Coins, pricePerLife, out Coins);
        Mario mario = Object.FindObjectOfType<Mario>();
        
        if (mario == null) {
            Lives += ups;
        } else {
            SummonOneUps(mario.transform, ups);
        }
    }

    private static readonly TimeSpan ContinuousOneUpDelay = TimeSpan.FromSeconds(0.5);
    private static async void SummonOneUps(Transform at, int count) {
        for (int i = 0; i < count; i++) {
            if (at == null) {
                Lives += count - i;
                return;
            }
            
            ScoreType.ONE_UP.Summon(at);

            if (i < count - 1) {
                await UniTask.Delay(ContinuousOneUpDelay);
            }
        }
    }
}
}
