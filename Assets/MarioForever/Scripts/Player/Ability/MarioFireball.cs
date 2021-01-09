using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Player.Ability
{
    public class MarioFireball : MonoBehaviour
    {
        public static int Count { get; private set;}

        private void Start()
        {
            Count++;
        }

        private void OnDestroy()
        {
            Count--;
        }
    }
}
