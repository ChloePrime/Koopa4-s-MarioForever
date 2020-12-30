using SweetMoleHouse.MarioForever.Persistent;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Player
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
