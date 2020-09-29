using Cinemachine;
using SweetMoleHouse.MarioForever.Debug;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SweetMoleHouse.MarioForever
{
    /// <summary>
    /// 全局对象，单例运作
    /// 基本不作为Unity脚本使用
    /// </summary>
    public class Global:MonoBehaviour
    {
        private bool debugMode;
        private InputControl inputs;
        [SerializeField]
        private Text debugText = null;
        [SerializeField]
        private Transform debugStrip = null;

        #region 属性

        /// <summary>
        /// 单例实例
        /// </summary>
        public static Global Instance { get; private set; }

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

        private void Awake()
        {
            bool isValid = HandleSingleton();
            if (!isValid)
            {
                return;
            }
            InitInput();

            DebugMode = false;
#if UNITY_EDITOR
            DebugMode = true;
#endif
        }

        private bool HandleSingleton()
        {
            if (Instance == null)
            {
                Instance = this;
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