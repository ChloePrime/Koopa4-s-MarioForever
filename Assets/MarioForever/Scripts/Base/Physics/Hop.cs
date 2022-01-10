using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Base.Physics {
/// <summary>
/// 跳乌龟，无敌星的跳跃代码
/// </summary>
[RequireComponent(typeof(BasePhysics))]
public class Hop : MonoBehaviour {
    [SerializeField] private float jumpHeight = 12;

    private void Awake() {
        _physics = GetComponent<BasePhysics>();
        _physics.OnHitWallY += _ => {
            if (enabled && _physics.YSpeed < 0) {
                JumpAtNextFrame();
            }
        };
    }

    private async void JumpAtNextFrame() {
        await UniTask.NextFrame(PlayerLoopTiming.FixedUpdate);
        if (_physics != null) {
            _physics.YSpeed = jumpHeight;
        }
    }

    private BasePhysics _physics;
}
}

