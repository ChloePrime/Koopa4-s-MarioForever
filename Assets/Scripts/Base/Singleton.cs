using UnityEngine;

namespace SweetMoleHouse.MarioForever.Base
{
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

        /// <summary>
        /// 请勿覆盖此方法
        /// Don't override this
        /// </summary>
        protected void Awake()
        {
            if (Instance == this)
            {
                DontDestroyOnLoad(this);
                OnSingletonAwake();
                return;
            }
            Destroy(gameObject);
        }
        
        protected virtual void OnSingletonAwake() {}
    }
}
