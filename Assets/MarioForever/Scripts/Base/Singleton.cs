using System;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Base {
/// <summary>
/// 单例类，
/// 默认的实现会在切换场景时消失
/// </summary>
public class Singleton<T> : MonoBehaviour where T : Singleton<T> {
    /// <summary>
    /// 单例实例
    /// </summary>
    protected static Lazy<T> LazyInstance = NewLazyInstance(); 

    public static T Instance {
        get {
            if (LazyInstance.Value == null) {
                LazyInstance = NewLazyInstance();
            }

            return LazyInstance.Value;
        }
    }

    private static Lazy<T> NewLazyInstance() {
        return new Lazy<T>(
            () => {
                T ret = FindObjectOfType<T>();
                if (ret != null) {
                    return ret;
                }

                GameObject obj = new() {
                    hideFlags = HideFlags.HideAndDontSave
                };
                return obj.AddComponent<T>();
            }
        );
    }
}
}
