using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Util
{
    public static class PhysicHelper
    {
        public static Component GetHost(in this RaycastHit2D hit2d)
        {
            var rig = hit2d.rigidbody;
            return rig == null ? (Component) hit2d.collider : rig;
        }


        public static Component GetHost(this Collider2D c2d)
        {
            var rig = c2d.attachedRigidbody;
            return rig == null ? (Component) c2d : rig;
        }
    }
}