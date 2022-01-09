using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Util
{
    public static class ComTreeUtil
    {
        #region DFS, GameObject

        public static T DfsComponentInChildren<T>(this GameObject thiz) => thiz.GetComponentInChildren<T>();

        public static bool TryDfsComponentInChildren<T>(this GameObject thiz, out T result)
        {
            result = thiz.GetComponentInChildren<T>();
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T DfsComponentInChildren<T>(this Component thiz) => 
            thiz.gameObject.DfsComponentInChildren<T>();

        public static bool TryDfsComponentInChildren<T>(this Component thiz, out T result) =>
            thiz.gameObject.TryDfsComponentInChildren(out result);

        public static IEnumerable<T> DfsComponentsInChildren<T>(this Component thiz) => 
            thiz.gameObject.DfsComponentsInChildren<T>();

        #endregion


        private static readonly Queue<Transform> UNVISITED_BFS = new Queue<Transform>();

        public static T BfsComponentInChildren<T>(this GameObject thiz)
        {
            try
            {
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
            finally
            {
                UNVISITED_BFS.Clear();
            }
        }


        public static bool TryBfsComponentInChildren<T>(this GameObject thiz, out T result)
        {
            result = thiz.BfsComponentInChildren<T>();
            return result != null;
        }

        private static readonly Pool<Queue<Transform>> QUEUE_FOR_ASYNC_BFS = Pool.Of(
            finalizer: (Queue<Transform> queue) => queue.Clear()
        );

        public static IEnumerable<T> BfsComponentsInChildren<T>(this GameObject thiz)
        {
            var queue = QUEUE_FOR_ASYNC_BFS.Rent();
            
            try
            {
                queue.Enqueue(thiz.transform);
                while (queue.Count != 0)
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
            finally
            {
                QUEUE_FOR_ASYNC_BFS.Return(queue);
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
