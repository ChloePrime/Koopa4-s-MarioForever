using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts
{
    /// <summary>
    /// 屏幕两边的黑边
    /// </summary>
    public class BlackBar : MonoBehaviour
    {
        private Camera mainCam;
        private void Start()
        {
            mainCam = Camera.main;
            //不设置z轴无法被摄像机拍到
            transform.Translate(0, 0, 1);
        }
    }
}