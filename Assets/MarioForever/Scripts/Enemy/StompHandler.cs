using SweetMoleHouse.MarioForever.Scripts.Base.Physics;
using SweetMoleHouse.MarioForever.Scripts.Player;
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

        public bool IsStomp(Transform stomper, Mario mario = null)
        {
            var baseResult = stompable && stomper.transform.position.y >= stompPos.position.y;
            if (!baseResult)
            {
                return false;
            }

            BasePhysics physic = mario != null ? mario.Mover : stomper.GetComponent<BasePhysics>();
            return physic == null || physic.YSpeed <= 0;
        }
    }
}