using System.Collections.Generic;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Level
{
    public class Background : MonoBehaviour
    {
        [SerializeField] private BackgroundAxisMethod loopX = BackgroundAxisMethod.LOOP;
        [SerializeField] private BackgroundAxisMethod loopY = BackgroundAxisMethod.NO_ACTION;
        
        private Vector2 size;
        private bool stopped;
        
        private void Start()
        {
            if (stopped) return;
            
            var sr = GetComponent<SpriteRenderer>();
            size = sr.bounds.size;

            EnsureSize();
            GenerateChildren();
        }

        private void EnsureSize()
        {
            var screenSize = new Vector2(ScrollInfo.Width, ScrollInfo.Height);
            var rootPos = transform.position;
            var parts = new HashSet<GameObject>();
            for (var xCursor = 0F; xCursor <= screenSize.x; xCursor += size.x)
            {
                for (var yCursor = 0F; yCursor <= screenSize.y; yCursor += size.y)
                {
                    bool x0 = xCursor < size.x;
                    bool y0 = yCursor < size.y;
                    if (x0 && y0) continue;
                    if (!x0 && !loopX.ShouldFill()) continue;
                    if (!y0 && !loopY.ShouldFill()) continue;

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
            var partCount = (loopX.ShouldLoop() ? 3 : 1) * (loopY.ShouldLoop() ? 3 : 1) - 1;
            var children = new List<GameObject>(partCount);
            for (var x = -1; x <= 1; x++)
            {
                for (var y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;
                    if (x != 0 && !loopX.ShouldLoop()) continue;
                    if (y != 0 && !loopY.ShouldLoop()) continue;
                    
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
            if (loopX.ShouldLoop())
            {
                if (ScrollInfo.Left < transform.position.x)
                {
                    transform.Translate(-size.x, 0, 0);
                }
                else if (ScrollInfo.Right > transform.position.x + size.x)
                {
                    transform.Translate(size.x, 0, 0);
                }
            }
            if (loopY.ShouldLoop())
            {
                if (ScrollInfo.Bottom < transform.position.y)
                {
                    transform.Translate(0, -size.y, 0);
                } 
                else if (ScrollInfo.Top > transform.position.y + size.y)
                {
                    transform.Translate(0, size.y, 0);
                }
            }
        }
    }
    
    public enum BackgroundAxisMethod
    {
        NO_ACTION,
        FILL,
        LOOP
    }

    public static class EnumMethods
    {
        public static bool ShouldFill(this BackgroundAxisMethod method)
        {
            return method != BackgroundAxisMethod.NO_ACTION;
        }
        public static bool ShouldLoop(this BackgroundAxisMethod method)
        {
            return method == BackgroundAxisMethod.LOOP;
        }
    }
}
