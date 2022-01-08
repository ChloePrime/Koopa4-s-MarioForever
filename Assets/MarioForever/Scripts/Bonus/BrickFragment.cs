using System;
using Cysharp.Threading.Tasks;
using SweetMoleHouse.MarioForever.Scripts.Constants;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Bonus {
/// <summary>
/// 在碎砖飞了一段路程以后打开它的碰撞
/// </summary>
public class BrickFragment : MonoBehaviour {
    private static readonly Lazy<int> PhysFxLayer = new(
        () => LayerMask.NameToLayer(LayerNames.PhysicsFX)
    );

    private static readonly TimeSpan LayerSwapDelay = TimeSpan.FromSeconds(0.2);

    private async void Start() {
        await UniTask.Delay(LayerSwapDelay, false, PlayerLoopTiming.FixedUpdate);
        
        gameObject.layer = PhysFxLayer.Value;
        foreach (Transform child in transform) {
            child.gameObject.layer = PhysFxLayer.Value;
        }
    }
}
}
