using JetBrains.Annotations;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Level {
/// <summary>
/// 表示一个区域
/// 控制背景，BGM，滚屏边界。
/// </summary>
public class Area : MonoBehaviour {
    [SerializeField, CanBeNull] private AudioClip music;
    [SerializeField] private ScrollBorder border;

    private void Awake() {
        if (border == null) {
            border = GetComponentInChildren<ScrollBorder>();
        }

        AreaManager.Instance.Register(this);
    }

    public void SetEnabled(bool enabledIn) {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(enabledIn);
        }

        if (enabledIn) {
            Activate();
        }
    }

    private void Activate() {
        AreaManager.SetMusic(music);
        if (border != null) {
            MarioCamera.SetScrollBorder(border);
        }
    }
}
}
