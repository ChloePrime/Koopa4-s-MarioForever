using System.Collections.Generic;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Util
{
    public static class ComTreeUtil
    {
        public static T DfsComponentInChildren<T>(this GameObject thiz)
        {
            if (thiz.TryGetComponent(out T result))
            {
                return result;
            }

            foreach (Transform child in thiz.transform)
            {
                return child.DfsComponentInChildren<T>();
            }

            return default;
        }

        public static bool TryDfsComponentInChildren<T>(this GameObject thiz, out T result)
        {
            result = thiz.DfsComponentInChildren<T>();
            return result != null;
        }

        public static T DfsComponentInChildren<T>(this Component thiz)
        {
            return DfsComponentInChildren<T>(thiz.gameObject);
        }

        public static bool TryDfsComponentInChildren<T>(this Component thiz, out T result)
        {
            return thiz.gameObject.TryDfsComponentInChildren(out result);
        }


        private static readonly Queue<Transform> UNVISITED_BFS = new Queue<Transform>();

        public static T BfsComponentInChildren<T>(this GameObject thiz)
        {
            UNVISITED_BFS.Enqueue(thiz.transform);
            while (UNVISITED_BFS.Count != 0)
            {
                var cur = UNVISITED_BFS.Dequeue();
                if (cur.TryGetComponent(out T result))
                {
                    UNVISITED_BFS.Clear();
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

        public static T BfsComponentInChildren<T>(this Component thiz)
        {
            return thiz.gameObject.BfsComponentInChildren<T>();
        }

        public static bool TryBfsComponentInChildren<T>(this Component thiz, out T result)
        {
            return thiz.gameObject.TryBfsComponentInChildren(out result);
        }
    }
}
