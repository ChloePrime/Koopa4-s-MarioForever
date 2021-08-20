using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Base.Rpg
{
    public interface IHaltableDamageReceiver : IDamageReceiver
    {
        UniTaskVoid SetInvulnerableFrom(Transform targetHost, TimeSpan time);
    }
}
