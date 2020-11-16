using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Util.Widget
{
    /// <summary>
    /// 计数器
    /// </summary>
    public class Counter : MonoBehaviour 
    {
        public long value;
        private long lastValue = long.MinValue;

        private class NumberRenderer : MonoBehaviour
        {
            private Counter parent;
            private void Start()
            {
                parent = transform.parent.GetComponent<Counter>();
            }
        }
    }
}