using JetBrains.Annotations;
using SweetMoleHouse.MarioForever.Util;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Level
{
    /// <summary>
    /// 表示一个区域
    /// 控制背景，BGM，滚屏边界。
    /// </summary>
    public class Area : HideInPlayMode
    {
        [SerializeField, CanBeNull] private AudioClip music;
        [SerializeField] private ScrollBorder border;

        protected override void Start()
        {
            base.Start();
            if (border == null)
            {
                border = GetComponentInChildren<ScrollBorder>();
            }
            AreaManager.Instance.Register(this);
        }

        public void SetEnabled(bool enabledIn)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(enabledIn);
            }

            if (enabledIn)
            {
                Activate();
            }
        }

        private void Activate()
        {
            AreaManager.SetMusic(music);
            if (border != null)
            {
                MarioCamera.SetScrollBorder(border);
            }
        }
    }
}
