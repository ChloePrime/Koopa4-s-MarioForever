using SweetMoleHouse.MarioForever.Scripts.Persistent;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Player
{
    public class ScoreGranter : MonoBehaviour
    {
        [SerializeField] private long amount;

        private void Start()
        {
            MarioProperty.Score += amount;
        }
    }
}
