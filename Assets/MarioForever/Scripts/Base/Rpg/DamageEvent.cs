using System;
using SweetMoleHouse.MarioForever.Scripts.Enemy;

namespace SweetMoleHouse.MarioForever.Scripts.Base.Rpg {
public struct DamageEvent {
    public DamageEvent(
        DamageSource source,
        EnumDamageType type
    ) : this() {
        Source = source;
        Type = type;
    }

    /// <summary>
    /// 伤害来源（攻击者，凶手）
    /// </summary>
    public DamageSource Source { get; }

    /// <summary>
    /// 伤害类型
    /// </summary>
    public EnumDamageType Type { get; }

    /// <summary>
    /// 自定义生成分数事件，
    /// 用于被连踩 / 被龟壳踢死等情况
    /// </summary>
    public Action CreateScoreOverride { get; set; }
}
}
