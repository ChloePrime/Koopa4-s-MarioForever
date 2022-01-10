using System;
using SweetMoleHouse.MarioForever.Scripts.Constants;
using SweetMoleHouse.MarioForever.Scripts.Player;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Bonus {
/// <summary>
/// 奖励道具。
/// 吃了会加分，并在自身没有特殊效果组件的情况下赋予马里奥高级状态、
/// </summary>
/// <seealso cref="IPowerupBehavior"/>
public class Powerup : MonoBehaviour {
    private const float RainbowTime = 1F;

    [SerializeField] private MarioPowerup target = MarioPowerup.BIG;
    [SerializeField] private ScoreType score = ScoreType.S1000;
    [SerializeField] private AudioClip sound;

    private void Awake() {
        _customBehavior = GetComponent<IPowerupBehavior>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag(Tags.Player)) return;
        if (!other.attachedRigidbody.TryGetComponent(out Mario mario)) return;

        if (_customBehavior != null) {
            _customBehavior.OnPowerup(mario);
        } else {
            // 默认效果: 给予状态
            // 大于蘑菇的补给总是会把马里奥拉到自己的目标补给级别
            if (target > MarioPowerup.BIG || target > mario.Powerup) {
                mario.SetPowerup(target, RainbowTime);
            }
        }

        Transform parent = transform.parent;
        score.Summon(parent, 0, 0.5F);
        Global.PlaySound(sound);

        Destroy(parent.gameObject);
    }

    private IPowerupBehavior _customBehavior;
}
}
