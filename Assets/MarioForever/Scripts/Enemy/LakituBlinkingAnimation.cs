using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SweetMoleHouse.MarioForever.Scripts.Enemy {
public class LakituBlinkingAnimation : MonoBehaviour {

    private void Awake() {
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start() {
        ScheduleBlinkingAsync();
    }

    private async void ScheduleBlinkingAsync() {
        while (true) {
            if (this == null) {
                return;
            }
            switch (Random.Range(0, 20)) {
                case 0:
                    // Blink_Up
                    _animator.SetTrigger(StartBlinkingUpTrigger);
                    await UniTask.Delay(TimeSpan.FromSeconds(0.266666));
                    continue;
                case 1:
                    // Blink_Down
                    _animator.SetTrigger(StartBlinkingDownTrigger);
                    await UniTask.Delay(TimeSpan.FromSeconds(0.2));
                    continue;
                default:
                    await UniTask.Delay(TimeSpan.FromSeconds(0.1));
                    continue;
            }
        }
    }

    private Animator _animator;
    private static readonly int StartBlinkingUpTrigger = Animator.StringToHash("StartBlinking_Up");
    private static readonly int StartBlinkingDownTrigger = Animator.StringToHash("StartBlinking_Down");
}
}
