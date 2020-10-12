using System;
using UnityEngine;
using UnityEngine.UI;

namespace SweetMoleHouse.MarioForever
{
    /// <summary>
    /// 全局对象，单例运作
    /// 基本不作为Unity脚本使用
    /// </summary>
    public class Global : MonoBehaviour
    {
        public static RaycastHit2D[] RCAST_TEMP_ARRAY = new RaycastHit2D[64];

        private bool debugMode;
        private InputControl inputs;
        private new AudioSource audio;
        [SerializeField]
        private Text debugText = null;
        [SerializeField]
        private Transform debugStrip = null;

        #region 属性

        /// <summary>
        /// 单例实例
        /// </summary>
        private static Global instance;
        public static Global Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<Global>();
                }
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.AddComponent<Global>();
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    instance = obj.GetComponent<Global>();
                }
                return instance;
            }
            private set => instance = value;
        }
        public static AudioSource SoundPlayer { get => Instance.audio; }

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
                if (value == true)
                {
                    Instance.inputs.Debug.Enable();
                }
                else
                {
                    Instance.inputs.Debug.Disable();
                }
            }
        }
        public static InputControl Inputs
        {
            get => Instance.inputs;
        }
        public static Transform DebugStrip { get => Instance.debugStrip; }

        #endregion
        public static void PlaySound(in AudioClip sample)
        {
            SoundPlayer.PlayOneShot(sample);
        }

        private void Awake()
        {
            bool isValid = HandleSingleton();
            if (!isValid)
            {
                return;
            }
            InitInput();
            audio = GetComponent<AudioSource>();

            DebugMode = false;
#if UNITY_EDITOR
            DebugMode = true;
#endif
        }

        private bool HandleSingleton()
        {
            if (Instance == this)
            {
                DontDestroyOnLoad(this);
                return true;
            }
            else
            {
                Destroy(gameObject);
                return false;
            }
        }

        private void InitInput()
        {
            inputs = new InputControl();
            inputs.Enable();
        }
    }
}