using System;

namespace SweetMoleHouse.MarioForever.Scripts.Enemy
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
        ENVIRONMENT = 1 << 4,
        ENEMY = 1 << 31
    }
}