using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.Rendering;

namespace SweetMoleHouse.MarioForever.Level
{
    public class Background : MonoBehaviour
    {
        [SerializeField] private bool loopX = true;
        [SerializeField] private bool loopY;
        
        private Vector2 size;
        private ScrollInfo scrollInfo;
        private bool stopped;
        
        private void Start()
        {
            if (stopped) return;
            
            scrollInfo = FindObjectOfType<ScrollInfo>();
            var sr = GetComponent<SpriteRenderer>();
            size = sr.bounds.size;

            EnsureSize();
            GenerateChildren();
        }

        private void EnsureSize()
        {
            var screenSize = new Vector2(scrollInfo.Width, scrollInfo.Height);
            var rootPos = transform.position;
            var parts = new HashSet<GameObject>();
            for (var xCursor = 0F; xCursor <= screenSize.x; xCursor += size.x)
            {
                for (var yCursor = 0F; yCursor <= screenSize.y; yCursor += size.y)
                {
                    if (xCursor < size.x && yCursor < size.y) continue;
                    
                    var go = CreateParallelGO();
                    go.transform.position = rootPos + new Vector3(xCursor, yCursor, 0);
                    parts.Add(go);
                }
            }
            foreach (var part in parts)
            {
                part.transform.parent = transform;
            }

            size.x = GetMinIntTimes(size.x, screenSize.x);
            size.y = GetMinIntTimes(size.y, screenSize.y);
        }

        private static float GetMinIntTimes(float baseNum, float lowBound)
        {
            return Mathf.Max(1, Mathf.CeilToInt(lowBound / baseNum)) * baseNum;
        }

        private void GenerateChildren()
        {
            var partCount = (loopX ? 3 : 1) * (loopY ? 3 : 1) - 1;
            var children = new List<GameObject>(partCount);
            for (var x = -1; x <= 1; x++)
            {
                for (var y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;
                    if (x != 0 && !loopX) continue;
                    if (y != 0 && !loopY) continue;
                    
                    var go = CreateParallelGO();
                    go.transform.position = transform.position + new Vector3(size.x * x, size.y * y, 0);
                    children.Add(go);
                }
            }

            children.ForEach(o => o.transform.parent = transform);
        }

        private GameObject CreateParallelGO()
        {
            var go = Instantiate(gameObject, transform.parent);
            var bgScript = go.GetComponent<Background>();
            if (bgScript != null)
            {
                bgScript.stopped = true;
                Destroy(bgScript);
            }
            return go;
        }

        private void Update()
        {
            if (stopped) return;
            if (loopX)
            {
                if (scrollInfo.Left < transform.position.x)
                {
                    transform.Translate(-size.x, 0, 0);
                }
                else if (scrollInfo.Right > transform.position.x + size.x)
                {
                    transform.Translate(size.x, 0, 0);
                }
            }
            if (loopY)
            {
                if (scrollInfo.Bottom < transform.position.y)
                {
                    transform.Translate(0, -size.y, 0);
                } 
                else if (scrollInfo.Top > transform.position.y + size.y)
                {
                    transform.Translate(0, size.y, 0);
                }
            }
        }
    }
}
