using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Base {
/// <summary>
/// 某种物件的子物件。
/// 可由此接口寻找到物件的本体。
/// </summary>
public interface ISubObject {
    Transform Host { get; }
}
}
