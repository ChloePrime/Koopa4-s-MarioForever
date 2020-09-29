using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

namespace SweetMoleHouse
{
    /// <summary>
    /// 屏幕两边的黑边
    /// </summary>
    public class BlackBar : MonoBehaviour 
    {
        private void Start()
        {
            //不设置z轴无法被摄像机拍到
            transform.Translate(0, 0, 1);
        }

        private void FixedUpdate()
        {
            Vector2 cameraScale = Camera.main.transform.localScale;
            float screenRate = cameraScale.y / cameraScale.x;
            transform.localScale /= new Vector2(screenRate, 1);
        }
    }
}