using SweetMoleHouse.MarioForever.Scripts.Base.Physics;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Common
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
