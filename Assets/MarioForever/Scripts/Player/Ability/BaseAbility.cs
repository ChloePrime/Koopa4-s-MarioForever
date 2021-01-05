using System;
using SweetMoleHouse.MarioForever.Constants;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Player.Ability
{
    public abstract class BaseAbility : MonoBehaviour
    {
        protected AbilityManager Manager;

        protected virtual void Start()
        {
            Manager = GetComponent<AbilityManager>();
        }

        public abstract MarioPowerup CorrespondingPowerup { get; }
        public abstract void OnShoot();
    }
}
