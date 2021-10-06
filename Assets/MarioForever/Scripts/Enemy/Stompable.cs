using SweetMoleHouse.MarioForever.Scripts.Player;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Enemy
{
    /// <summary>
    /// 用于区分碰撞为踩踏还是
    /// </summary>
    public class Stompable : MonoBehaviour
    {
        private bool hasStompHandler;
        private StompHandler stompHandler;

        protected virtual void Awake()
        {
            Transform parent = transform.parent;
            hasStompHandler = parent != null && parent.TryGetComponent(out stompHandler);
        }

        public bool IsStomp(in Transform stomper, Mario mario = null)
        {
            return hasStompHandler && stompHandler.IsStomp(stomper, mario);
        }
    }
}