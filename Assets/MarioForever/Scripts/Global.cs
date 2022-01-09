using SweetMoleHouse.MarioForever.Scripts.Base;
using SweetMoleHouse.MarioForever.Scripts.Generated;
using UnityEngine;
using UnityEngine.UI;

namespace SweetMoleHouse.MarioForever.Scripts {
/// <summary>
/// 全局对象，单例运作
/// 基本不作为Unity脚本使用
/// </summary>
public class Global : GlobalSingleton<Global> {
    public static readonly RaycastHit2D[] RcastTempArray = new RaycastHit2D[64];

    private bool _debugMode;
    private InputControl _inputs;
    private AudioSource _audio;
    [SerializeField] private Text debugText;
    [SerializeField] private Transform debugStrip;

    #region 属性

    public static AudioSource SoundPlayer => Instance._audio;

    public static string DebugText {
        get => Instance.debugText.text;
        set => Instance.debugText.text = value;
    }

    /// <summary>
    /// 开关调试模式
    /// </summary>
    public static bool DebugMode {
        get => Instance._debugMode;
        set => Instance.SetDebugModeImpl(value);
    }

    public static InputControl Inputs => Instance._inputs;
    public static Transform DebugStrip => Instance.debugStrip;

    #endregion

    public static void PlaySound(in AudioClip sample) {
        SoundPlayer.PlayOneShot(sample);
    }

    protected override void OnSingletonAwake() {
        base.OnSingletonAwake();
        InitInput();
        _audio = GetComponent<AudioSource>();
#if UNITY_EDITOR
        SetDebugModeImpl(true);
#else
        SetDebugModeImpl(false);
#endif
    }

    private void InitInput() {
        _inputs = new InputControl();
        _inputs.Enable();
    }

    private void SetDebugModeImpl(bool value) {
        _debugMode = value;
        if (value) {
            _inputs.Debug.Enable();
        } else {
            _inputs.Debug.Disable();
        }
    }
}
}