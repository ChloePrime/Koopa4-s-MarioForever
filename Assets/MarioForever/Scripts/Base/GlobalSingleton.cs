using System;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Base {
/// <summary>
/// 具有 DontDestroyOnLoad 属性的单例脚本
/// 任何时刻全局唯一
/// </summary>
public class GlobalSingleton<T> : Singleton<T> where T : GlobalSingleton<T> {
    /// <summary>
    /// 请勿覆盖此方法
    /// Don't override this
    /// </summary>
    protected void Awake() {
        if (!LazyInstance.IsValueCreated || Instance == this) {
            DontDestroyOnLoad(this);
            OnSingletonAwake();
            return;
        }

        Destroy(gameObject);
    }

    protected virtual void OnSingletonAwake() { }
}
}
