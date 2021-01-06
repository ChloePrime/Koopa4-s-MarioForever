using System.Collections.Generic;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Util
{
    public static class ComTreeUtil
    {
        public static T DfsComponentInChildren<T>(this GameObject thiz) where T : MonoBehaviour
        {
            if (thiz.TryGetComponent(out T result))
            {
                return result;
            }

            foreach (Transform child in thiz.transform)
            {
                return child.DfsComponentInChildren<T>();
            }

            return null;
        }

        public static bool TryDfsComponentInChildren<T>(this GameObject thiz, out T result) where T : MonoBehaviour
        {
            result = thiz.DfsComponentInChildren<T>();
            return result != null;
        }

        public static T DfsComponentInChildren<T>(this Component thiz) where T : MonoBehaviour
        {
            return DfsComponentInChildren<T>(thiz.gameObject);
        }

        public static bool TryDfsComponentInChildren<T>(this Component thiz, out T result) where T : MonoBehaviour
        {
            return thiz.gameObject.TryDfsComponentInChildren(out result);
        }


        private static readonly Queue<Transform> UNVISITED_BFS = new Queue<Transform>();

        public static T BfsComponentInChildren<T>(this GameObject thiz) where T : MonoBehaviour
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

            return null;
        }

        public static bool TryBfsComponentInChildren<T>(this GameObject thiz, out T result) where T : MonoBehaviour
        {
            result = thiz.BfsComponentInChildren<T>();
            return result != null;
        }

        public static T BfsComponentInChildren<T>(this Component thiz) where T : MonoBehaviour
        {
            return thiz.gameObject.BfsComponentInChildren<T>();
        }

        public static bool TryBfsComponentInChildren<T>(this Component thiz, out T result) where T : MonoBehaviour
        {
            return thiz.gameObject.TryBfsComponentInChildren(out result);
        }
    }
}
