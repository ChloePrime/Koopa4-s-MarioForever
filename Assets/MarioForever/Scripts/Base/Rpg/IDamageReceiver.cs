using System.Collections;
using SweetMoleHouse.MarioForever.Scripts.Enemy;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Base.Rpg
{
    public interface IDamageReceiver
    {
        Faction Faction { get; }

        Transform Host { get; }
        
        void Damage(in EnumDamageType type);

        void SetDead(in EnumDamageType type);
    }
}
