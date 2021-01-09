using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Base
{
    /// <summary>
    /// 单例类，
    /// 默认的实现会在切换场景时消失
    /// </summary>
    public class Singleton<T>: MonoBehaviour where T: Singleton<T>
    {
        /// <summary>
        /// 单例实例
        /// </summary>
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                }
                if (instance == null)
                {
                    var obj = new GameObject();
                    obj.AddComponent<Global>();
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    instance = obj.GetComponent<T>();
                }
                return instance;
            }
        }
    }
}
