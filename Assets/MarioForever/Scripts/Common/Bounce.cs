using SweetMoleHouse.MarioForever.Base;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Common
{
    public class Bounce : MonoBehaviour
    {
        [SerializeField] private float bounceStrength;

        private BasePhysics physics;
        private void Start()
        {
            physics = GetComponent<BasePhysics>();
        }

        private void FixedUpdate()
        {
            if (physics.IsOnGround)
            {
                physics.YSpeed = bounceStrength;
            }
        }
    }
}
