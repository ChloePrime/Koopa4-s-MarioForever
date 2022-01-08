using System;
using Cysharp.Threading.Tasks;
using SweetMoleHouse.MarioForever.Scripts.Base.Physics;
using SweetMoleHouse.MarioForever.Scripts.Constants;
using SweetMoleHouse.MarioForever.Scripts.Persistent;
using SweetMoleHouse.MarioForever.Scripts.Player;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Bonus {
public class Brick : HittableBase {
    private static readonly int BlockHitAnim = Animator.StringToHash("顶起");
    private static readonly TimeSpan AnimLen = TimeSpan.FromSeconds(0.3f);

    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip breakSound;
    [SerializeField] private long score = 50;

    protected override void Awake() {
        base.Awake();
        _animator = GetComponentInChildren<Animator>();
    }

    public override bool OnHit(Transform hitter) {
        DamageEnemiesAbove();
        
        if (hitter.TryGetComponent(out Mario mario) && mario.Powerup == MarioPowerup.SMALL) {
            Hit();
        } else {
            Break();
        }

        return true;
    }

    /// <summary>
    /// 顶起来没顶碎
    /// </summary>
    private void Hit() {
        if (!_ready) {
            return;
        }

        Global.PlaySound(hitSound);
        _animator.SetTrigger(BlockHitAnim);
        HitCooldown();
    }

    private async void HitCooldown() {
        _ready = false;
        await UniTask.Delay(AnimLen);
        _ready = true;
        MarioProperty.Score += score;
    }

    /// <summary>
    /// 碎了
    /// </summary>
    private void Break() {
        Global.PlaySound(breakSound);
        Destroy(gameObject);
    }

    private Animator _animator;
    private bool _ready = true;
}
}