using SweetMoleHouse.MarioForever.Scripts.Base.Physics;
using SweetMoleHouse.MarioForever.Scripts.Constants;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Facility
{
    public static class PlatformDefinitions
    {
        public static bool IsStandingOnPlatform(this BasePhysics thiz, in Vector3 platform)
        {
            return thiz.transform.position.y >= platform.y - Consts.OnePixel;
        }
    }
}
