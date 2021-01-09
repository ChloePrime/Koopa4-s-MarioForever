using SweetMoleHouse.MarioForever.Scripts.Constants;
using SweetMoleHouse.MarioForever.Scripts.Player;

namespace SweetMoleHouse.MarioForever.Scripts.Persistent
{
    public static class MarioProperty
    {
        /// <summary>
        /// 使用 <see cref="Mario.Powerup"/>
        /// 而不是直接设置这个字段
        /// </summary>
        public static MarioPowerup CurPowerup = MarioPowerup.BIG;
        public static int Lives = 4;
        public static int Coins;
        public static int TimeFromLastScene = -1;
        public static long Score;
    }
}
