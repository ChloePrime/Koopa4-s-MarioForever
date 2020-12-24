using UnityEngine;

namespace SweetMoleHouse.MarioForever.Enemy
{
    /// <summary>
    /// 用于区分碰撞为踩踏还是
    /// </summary>
    public class Stompable : MonoBehaviour
    {
        private bool hasStompHandler;
        private StompHandler stompHandler;

        protected virtual void Start()
        {
            hasStompHandler = transform.parent.TryGetComponent(out stompHandler);
        }

        protected bool IsStomp(in Transform mario)
        {
            return hasStompHandler && stompHandler.IsStomp(mario);
        }
    }
}