using SweetMoleHouse.MarioForever.Scripts.Base.Physics;
using SweetMoleHouse.MarioForever.Scripts.Util;
using UnityEngine;

namespace SweetMoleHouse
{
    public class BreakBrickUponXHit : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Awake()
        {
            if (TryGetComponent(out BasePhysics physics)) {
                physics.OnHitWallX += OnHitWallX;
            }
        }

        private static void OnHitWallX(Collider2D[] targets) {
            foreach (Collider2D hit in targets) {
                if (hit.TryGetComponent(out IHittable hittable)) {
                    hittable.OnHit(hit.GetHost().transform);
                }
            }
        }
    }
}
