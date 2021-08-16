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

        protected virtual void Start()
        {
            Transform parent = transform.parent;
            hasStompHandler = parent != null && parent.TryGetComponent(out stompHandler);
        }

        protected bool IsStomp(in Transform mario)
        {
            return hasStompHandler && stompHandler.IsStomp(mario);
        }
    }
}