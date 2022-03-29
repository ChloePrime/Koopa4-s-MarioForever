using System;
using System.Collections.Generic;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Base.Rpg
{
    [CreateAssetMenu(fileName = null, menuName = "MarioForever/" + nameof(Faction), order = 0)]
    public class Faction : ScriptableObject
    {
        [SerializeField] private Faction[] hostiles;

        private HashSet<Faction> asSet;

        public bool IsHostileTo(in Faction faction)
        {
            if (faction == null)
            {
                throw new ArgumentNullException();
            }
            asSet ??= new HashSet<Faction>(hostiles);
            return asSet.Contains(faction);
        }
    }
}