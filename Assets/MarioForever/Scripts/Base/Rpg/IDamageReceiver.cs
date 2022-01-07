using System.Diagnostics.CodeAnalysis;
using SweetMoleHouse.MarioForever.Scripts.Enemy;

namespace SweetMoleHouse.MarioForever.Scripts.Base.Rpg
{
    public interface IDamageReceiver: ISubObject
    {
        Faction Faction { get; }
        
        void Damage(DamageEvent damage);

        void SetDead(DamageEvent damage);

        /// <summary>
        /// 受击者的攻击方式，
        /// 用于踢龟壳 / 反击等情况
        /// </summary>
        [MaybeNull]
        DamageSource MyDamageSource => null;
    }
}
