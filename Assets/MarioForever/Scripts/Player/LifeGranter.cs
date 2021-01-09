using SweetMoleHouse.MarioForever.Scripts.Persistent;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Player
{
    public class LifeGranter : MonoBehaviour
    {
        [SerializeField] private AudioClip jingle;
        [SerializeField] private int amount = 1;

        private void Start()
        {
            Global.PlaySound(jingle);
            MarioProperty.Lives += amount;
        }
    }
}
