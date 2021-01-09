using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Base
{
    /// <summary>
    /// 可以从水管/问号块内出现的物品
    /// </summary>
    public interface IAppearable
    {
        void Appear(in Vector2 direction, in Vector2 size);
    }
}