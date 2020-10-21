using SweetMoleHouse.MarioForever.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Enemy
{
    /// <summary>
    /// 用于区分碰撞为踩踏还是
    /// </summary>
    public class Stompable : MonoBehaviour 
    {
        [SerializeField, RenameInInspector("是否可以踩")]
        private bool stompable = true;
        [SerializeField, RenameInInspector("踩踏判定分界线")]
        private Transform stompPos = null;

        private static bool ySpeedRequireCache;
        private Coroutine stopper;

        public bool IsStomp(Transform mario)
        {
            bool baseResult = stompable && mario.transform.position.y >= stompPos.position.y;
            if (!baseResult)
            {
                return baseResult;
            }
            if (ySpeedRequireCache)
            {
                TryClearCache();
                return ySpeedRequireCache && baseResult;
            }
            var physic = mario.GetComponent<BasePhysics>();
            if (physic != null && physic.YSpeed > 0)
            {
                return false;
            }
            ySpeedRequireCache = true;
            TryClearCache();
            return baseResult;
        }

        private void TryClearCache()
        {
            if (stopper != null)
            {
                StopCoroutine(stopper);
            }
            stopper = StartCoroutine(ClearCache());
        }

        public IEnumerator ClearCache()
        {
            yield return new WaitForSeconds(0.1f);
            ySpeedRequireCache = false;
        }
    }
}