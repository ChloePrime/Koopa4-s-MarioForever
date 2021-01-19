using SweetMoleHouse.MarioForever.Scripts.Base;
using SweetMoleHouse.MarioForever.Scripts.Constants;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Facility
{
    /// <summary>
    /// 马里奥踩了会掉下去的平台
    /// </summary>
    public class FallingPlatform : MonoBehaviour
    {
        private static bool classInited;
        private static ContactFilter2D playerFilter;
        private static readonly RaycastHit2D[] CAST_RESULT_CACHE = new RaycastHit2D[16];
            
        [SerializeField] private float fallGravity;
        
        private BasePhysics physics;
        private bool fallen;

        private static void InitClass()
        {
            if(classInited) return;
            playerFilter.layerMask = LayerMask.GetMask(LayerNames.AllMovable);
            classInited = true;
        }
        private void Start()
        {
            InitClass();
            physics = GetComponent<BasePhysics>();
        }

        private void FixedUpdate()
        {
            if (fallen) return;
            
            int hits = physics.R2d.Cast(Vector2.up, playerFilter, CAST_RESULT_CACHE, Consts.OnePixel);
            for (int i = 0; i < hits; i++)
            {
                var rig = CAST_RESULT_CACHE[i].rigidbody;
                if (rig == null || !rig.CompareTag(Tags.Player)) continue;
                // 确保速度向下
                if (rig.TryGetComponent(out BasePhysics thatPhysics))
                {
                    if (thatPhysics.YSpeed > 0
                        || !thatPhysics.IsStandingOnPlatform(CAST_RESULT_CACHE[i].centroid))
                    {
                        continue;
                    }
                }
                SetFallen();
            }
        }

        private void SetFallen()
        {
            physics.Gravity = fallGravity;
            fallen = true;
        }
    }
}
