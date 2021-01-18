using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Util
{
    public static class ComTreeUtil
    {
        #region DFS, GameObject

        public static T DfsComponentInChildren<T>(this GameObject thiz)
        {
            if (thiz.TryGetComponent(out T result))
            {
                return result;
            }

            foreach (Transform child in thiz.transform)
            {
                return child.gameObject.DfsComponentInChildren<T>();
            }

            return default;
        }

        public static bool TryDfsComponentInChildren<T>(this GameObject thiz, out T result)
        {
            result = thiz.DfsComponentInChildren<T>();
            return result != null;
        }
        
        public static IEnumerable<T> DfsComponentsInChildren<T>(this GameObject thiz)
        {
            if (thiz.TryGetComponent(out T result))
            {
                yield return result;
            }

            foreach (Transform child in thiz.transform)
            {
                foreach (T t in child.gameObject.DfsComponentsInChildren<T>())
                {
                    yield return t;
                }
            }
        }

        #endregion

        #region DFS, Component

        public static T DfsComponentInChildren<T>(this Component thiz)
        {
            return DfsComponentInChildren<T>(thiz.gameObject);
        }

        public static bool TryDfsComponentInChildren<T>(this Component thiz, out T result)
        {
            return thiz.gameObject.TryDfsComponentInChildren(out result);
        }

        public static IEnumerable<T> DfsComponentsInChildren<T>(this Component thiz)
        {
            return thiz.gameObject.DfsComponentsInChildren<T>();
        }

        #endregion


        private static readonly Queue<Transform> UNVISITED_BFS = new Queue<Transform>();

        public static T BfsComponentInChildren<T>(this GameObject thiz)
        {
            UNVISITED_BFS.Clear();
            UNVISITED_BFS.Enqueue(thiz.transform);
            while (UNVISITED_BFS.Count != 0)
            {
                var cur = UNVISITED_BFS.Dequeue();
                if (cur.TryGetComponent(out T result))
                {
                    return result;
                }

                foreach (Transform child in cur)
                {
                    UNVISITED_BFS.Enqueue(child);
                }
            }

            return default;
        }


        public static bool TryBfsComponentInChildren<T>(this GameObject thiz, out T result)
        {
            result = thiz.BfsComponentInChildren<T>();
            return result != null;
        }

        public static IEnumerable<T> BfsComponentsInChildren<T>(this GameObject thiz)
        {
            var queue = new Queue<Transform>();
            queue.Enqueue(thiz.transform);
            while (UNVISITED_BFS.Count != 0)
            {
                var cur = queue.Dequeue();
                if (cur.TryGetComponent(out T component))
                {
                    yield return component;
                }

                foreach (Transform child in cur)
                {
                    queue.Enqueue(child);
                }
            }
        }
        
        public static T BfsComponentInChildren<T>(this Component thiz)
        {
            return thiz.gameObject.BfsComponentInChildren<T>();
        }

        public static bool TryBfsComponentInChildren<T>(this Component thiz, out T result)
        {
            return thiz.gameObject.TryBfsComponentInChildren(out result);
        }
        
        public static IEnumerable<T> BfsComponentsInChildren<T>(this Component thiz)
        {
            return thiz.gameObject.BfsComponentsInChildren<T>();
        }
    }
}
