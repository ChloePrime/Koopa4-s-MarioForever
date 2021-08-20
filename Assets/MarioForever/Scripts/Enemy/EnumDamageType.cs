using System;
using System.Runtime.CompilerServices;

namespace SweetMoleHouse.MarioForever.Scripts.Enemy
{
    /// <summary>
    /// 攻击方式
    /// </summary>
    [Flags]
    public enum EnumDamageType : uint
    {
        STOMP = 1 << 0,
        FIREBALL = 1 << 1,
        BEETROOT = 1 << 2,
        ENVIRONMENT = 1 << 3,
        KICK_SHELL = 1 << 4,
        ENEMY = 1 << 30
    }

    public static class EnumDamageTypeExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this EnumDamageType self, EnumDamageType target) => (self & target) > 0;
    }
}