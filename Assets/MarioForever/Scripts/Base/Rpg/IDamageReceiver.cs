using System.Diagnostics.CodeAnalysis;
using SweetMoleHouse.MarioForever.Scripts.Enemy;

namespace SweetMoleHouse.MarioForever.Scripts.Base.Rpg
{
    public interface IDamageReceiver: ISubObject
    {
        Faction Faction { get; }
        
        void Damage(DamageSource damager, EnumDamageType damageType);

        void SetDead(DamageSource damager, EnumDamageType damageType);

        /// <summary>
        /// 受击者的攻击方式，
        /// 用于踢龟壳 / 反击等情况
        /// </summary>
        [MaybeNull]
        DamageSource MyDamageSource => null;
    }
}
