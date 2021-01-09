using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Base
{
    /// <summary>
    /// 斜坡接口
    /// 通过把斜坡的属性写在类里面而不是对象的字段里面，可以减少内存占用
    /// </summary>
    public abstract class BaseSlope : MonoBehaviour
    {
        public abstract float Degree { get; }
        /// <summary>
        /// 斜坡上坡的方向，为待测物件X速度的符号值
        /// </summary>
        public abstract int Dir { get; }
    }
}