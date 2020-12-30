using System;
using SweetMoleHouse.MarioForever.Constants;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Util
{
    public class ComboCalculator : MonoBehaviour
    {
        private const ScoreType InitialScore = ScoreType.S100;
        
        [SerializeField] private bool loop;

        private ScoreType current;

        public void Hit(Transform trr)
        {
            current.Summon(trr);
            bool end = !Enum.IsDefined(typeof(ScoreType), current + 1);
            if (end)
            {
                if (loop)
                {
                    current = InitialScore;
                }
            }
            else
            {
                current += 1;
            }
        }

        public void ResetCombo()
        {
            current = InitialScore;
        }
    }
}
