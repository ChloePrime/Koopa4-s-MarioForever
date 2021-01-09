using SweetMoleHouse.MarioForever.Scripts.Base;
using SweetMoleHouse.MarioForever.Scripts.Util;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Enemy
{
    /// <summary>
    /// 用于区分碰撞为踩踏还是
    /// </summary>
    public class StompHandler : MonoBehaviour 
    {
        [SerializeField, RenameInInspector("是否可以踩")]
        private bool stompable = true;
        [SerializeField, RenameInInspector("踩踏判定分界线")]
        private Transform stompPos;

        private static bool ySpeedConditionCache;

        public bool IsStomp(Transform mario)
        {
            var baseResult = stompable && mario.transform.position.y >= stompPos.position.y;
            if (!baseResult)
            {
                return false;
            }
            if (ySpeedConditionCache)
            {
                return ySpeedConditionCache;
            }
            var physic = mario.GetComponent<BasePhysics>();
            if (physic != null && physic.YSpeed > 0)
            {
                return false;
            }
            ySpeedConditionCache = true;
            return true;
        }

        private void LateUpdate()
        {
            ySpeedConditionCache = false;
        }
    }
}