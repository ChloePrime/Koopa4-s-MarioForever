using SweetMoleHouse.MarioForever.Base;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Bonus
{
    public class MushroomAnimation : MonoBehaviour
    {
        private BasePhysics physics;
        private Animator animator;
        private static readonly int DUANG = Animator.StringToHash("Duang");

        private void Start()
        {
            animator = GetComponentInChildren<Animator>();
            physics = GetComponent<BasePhysics>();
            physics.OnHitWallY += OnHitWallY;
        }

        private void OnHitWallY(Collider2D[] _)
        {
            if (physics.YSpeed >= 0) return;
            animator.SetTrigger(DUANG);
        }
    }
}
