using SweetMoleHouse.MarioForever.Base;
using SweetMoleHouse.MarioForever.Generated;
using UnityEngine;
using UnityEngine.UI;

namespace SweetMoleHouse.MarioForever
{
    /// <summary>
    /// 全局对象，单例运作
    /// 基本不作为Unity脚本使用
    /// </summary>
    public class Global : GlobalSingleton<Global>
    {
        public static readonly RaycastHit2D[] RcastTempArray = new RaycastHit2D[64];

        private bool debugMode;
        private InputControl inputs;
        private new AudioSource audio;
        [SerializeField]
        private Text debugText;
        [SerializeField]
        private Transform debugStrip;

        #region 属性

        public static AudioSource SoundPlayer => Instance.audio;

        public static string DebugText
        {
            get => Instance.debugText.text;
            set => Instance.debugText.text = value;
        }
        /// <summary>
        /// 开关调试模式
        /// </summary>
        public static bool DebugMode
        {
            get => Instance.debugMode;
            set
            {
                Instance.debugMode = value;
                if (value)
                {
                    Instance.inputs.Debug.Enable();
                }
                else
                {
                    Instance.inputs.Debug.Disable();
                }
            }
        }
        public static InputControl Inputs => Instance.inputs;
        public static Transform DebugStrip => Instance.debugStrip;

        #endregion
        public static void PlaySound(in AudioClip sample)
        {
            SoundPlayer.PlayOneShot(sample);
        }

        protected override void OnSingletonAwake()
        {
            base.OnSingletonAwake();
            InitInput();
            audio = GetComponent<AudioSource>();
#if UNITY_EDITOR
            DebugMode = true;
#else
            DebugMode = false;
#endif
        }

        private void InitInput()
        {
            inputs = new InputControl();
            inputs.Enable();
        }
    }
}