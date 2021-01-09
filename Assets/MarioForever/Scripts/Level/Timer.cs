using SweetMoleHouse.MarioForever.Scripts.Persistent;
using SweetMoleHouse.MarioForever.Scripts.Player;
using SweetMoleHouse.MarioForever.Scripts.Util;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Level
{
    /// <summary>
    /// 存放关卡时间的地方
    /// </summary>
    public class Timer : MonoBehaviour
    {
        [SerializeField, RenameInInspector("关卡时间（秒）")]
        private float time = 400;
        [SerializeField, RenameInInspector("无限时间")]
        private bool infinity;
        public bool Infinity => infinity;

        [SerializeField]
        private float unitPerSecond = 2;

        public int TimeInUnit => Mathf.RoundToInt(time * unitPerSecond);

        private Mario mario;
        private bool killed;
        // Start is called before the first frame update
        private void Start()
        {
            mario = FindObjectOfType<Mario>();
            if (time < 0)
            {
                infinity = true;
            }
            if (infinity)
            {
                time = -1;
                return;
            }
            // 从上一个 frame 继承时间
            if (MarioProperty.TimeFromLastScene > 0)
            {
                time = MarioProperty.TimeFromLastScene;
                MarioProperty.TimeFromLastScene = -1;
            }
        }

        private void FixedUpdate()
        {
            if (infinity) return;
            if (time <= 0)
            {
                OnTimeUp();
                return;
            }
            if (time <= 0) return;
            
            time -= Time.fixedDeltaTime;
        }

        private void OnTimeUp()
        {
            if (time < 0)
            {
                time = 0;
            }

            if (!killed)
            {
                mario.Kill();
                killed = true;
            }
        }
    }
}
