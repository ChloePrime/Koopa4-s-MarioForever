using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Util.Widget
{
    /// <summary>
    /// 贴图计数器
    /// </summary>
    public class Counter : MonoBehaviour 
    {
        [SerializeField]
        private long value;
        public long Value
        {
            get => value;
            set
            {
                if (this.value == value) return;
                this.value = value;
                ResetSprites();
            }
        }

        private void ResetSprites()
        {
            throw new NotImplementedException();
        }

        private NumberRenderer[] renderers;

        private const int DefaultDigits = 5;

        private void Start()
        {
            Expand(DefaultDigits);
        }

        private void Expand(in int extraSize)
        {
            var size = (renderers?.Length ?? 0) + extraSize;
            var newArray = new NumberRenderer[size];
            renderers?.CopyTo(newArray, 0);
            for (var i = 0; i < DefaultDigits; i++)
            {
                var go = new GameObject();
                go.transform.parent = transform.parent;
                var com = go.AddComponent<NumberRenderer>();
                com.parent = this;
                newArray[i] = com;
            }

            renderers = newArray;
        }

        private class NumberRenderer : MonoBehaviour
        {
            public Counter parent;
            private void Start()
            {
                parent = transform.parent.GetComponent<Counter>();
            }
        }
    }
}