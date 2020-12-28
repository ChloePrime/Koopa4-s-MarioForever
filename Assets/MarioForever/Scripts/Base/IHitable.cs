using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Base
{
    /// <summary>
    /// 可以顶的东西
    /// </summary>
    public interface IHitable
    {
        /// <summary>
        /// 被马里奥顶起时执行的动作
        /// </summary>
        /// <returns>如果顶起后有反应，无需播放默认的bump音效，则返回true</returns>
        bool OnHit(Transform hitter);
    }
}