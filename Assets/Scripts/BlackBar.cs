using UnityEngine;

namespace SweetMoleHouse.MarioForever
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

        private void FixedUpdate()
        {
            if (mainCam == null) return;
            
            Vector2 cameraScale = mainCam.transform.localScale;
            float screenRate = cameraScale.y / cameraScale.x;
            transform.localScale /= new Vector2(screenRate, 1);
        }
    }
}