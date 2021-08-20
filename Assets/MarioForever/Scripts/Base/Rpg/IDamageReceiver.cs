using System.Collections;
using Cysharp.Threading.Tasks;
using SweetMoleHouse.MarioForever.Scripts.Enemy;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Base.Rpg
{
    public interface IDamageReceiver
    {
        Faction Faction { get; }

        Transform Host { get; }
        
        void Damage(Transform damager, EnumDamageType type);

        void SetDead(Transform damager, EnumDamageType type);
    }
}
