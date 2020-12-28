using System;

namespace SweetMoleHouse.MarioForever.Enemy
{
    /// <summary>
    /// 攻击方式
    /// </summary>
    [Flags]
    public enum EnumDamageType
    {
        STOMP = 1 << 0,
        FIREBALL = 1 << 1,
        BEETROOT = 1 << 2,
        OTHER = 1 << 3
    }
}