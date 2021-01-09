using SweetMoleHouse.MarioForever.Scripts.Level;
using SweetMoleHouse.MarioForever.Scripts.Util;
using UnityEngine;
using static UnityEngine.Mathf;

namespace SweetMoleHouse.MarioForever.Scripts.UI
{
    /// <summary>
    /// 关卡倒计时的显示
    /// </summary>
    public class TimerUI : MonoBehaviour
    {
        [SerializeField, RenameInInspector("超时警戒线")]
        private int warnThreshold = 100;
        [SerializeField, RenameInInspector("超时警戒音效")]
        private AudioClip warnSound;
        
        [SerializeField] private Counter numberDisplay;
        [SerializeField] private Transform timerText;

        private Timer levelTimer;
        private bool warned;
        private float warningCountdown;
        private void Start()
        {
            levelTimer = FindObjectOfType<Timer>();
            if (levelTimer.Infinity)
            {
                numberDisplay.Start();
                numberDisplay.DisplayText = "∞";
            }
        }

        private void FixedUpdate()
        {
            if (levelTimer.Infinity) return;
            var time = levelTimer.TimeInUnit;
            numberDisplay.Value = time;
            if (time <= warnThreshold)
            {
                if (warned) return;
                WarnTimeout();
            }
            else if (warned)
            {
                warned = false;
            }
        }

        private const float WarnAnimTime = 3;
        private const float WarnAnimLoop = 9;
        private void WarnTimeout()
        {
            warningCountdown = WarnAnimTime;
            Global.PlaySound(warnSound);
            warned = true;
        }

        /// <summary>
        /// 动画振幅
        /// </summary>
        private const float AnimAmplitude = 0.1F;
        private void Update()
        {
            if (warningCountdown <= 0) return;
            warningCountdown -= Time.deltaTime;
            
            if (warningCountdown > 0)
            {
                const float animSpeed = 2 * PI * WarnAnimLoop / WarnAnimTime;
                const float baseVibOffset = 1F - AnimAmplitude / 2;
                var scale = timerText.transform.localScale;
                scale.y = baseVibOffset + AnimAmplitude * Sin(warningCountdown * animSpeed);
                timerText.transform.localScale = scale;
            }
            else
            {
                timerText.transform.localScale = Vector3.one;
            }
        }
    }
}
