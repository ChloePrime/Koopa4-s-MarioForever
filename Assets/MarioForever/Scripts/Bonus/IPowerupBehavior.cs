using SweetMoleHouse.MarioForever.Scripts.Player;

namespace SweetMoleHouse.MarioForever.Scripts.Bonus {
/// <summary>
/// 奖励道具上的给予状态以外的行为、
/// 注意：毒蘑菇因为其潜在的杀死怪物功能而未使用此接口。
/// </summary>
public interface IPowerupBehavior {
    void OnPowerup(Mario mario);
}
}