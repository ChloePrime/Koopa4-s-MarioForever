using System;
using Cysharp.Threading.Tasks;
using SweetMoleHouse.MarioForever.Scripts.Base.Rpg;
using SweetMoleHouse.MarioForever.Scripts.Constants;
using SweetMoleHouse.MarioForever.Scripts.Player;
using SweetMoleHouse.MarioForever.Scripts.Util;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Enemy.Turtle {
/// <summary>
/// 移动的龟壳：自身
/// </summary>
public class MovingShell : MonoBehaviour {
    private void Awake() {
        _damageSource = this.BfsComponentInChildren<DamageSource>();
        // 在刚刚生成的短时间内不对马里奥造成伤害
        _damageSource.OnPreDamage +=
            (source, receiver) => ShouldCancel(receiver) ? ActionResult.CANCEL : ActionResult.PASS;
        DisableProtectionAsync().Forget();
        // 防止动态龟壳击杀静止龟壳导致报错
        this.BfsComponentInChildren<TurtleSwapper>().OnTurtleSwap += _ => _damageHalted = true;
    }

    private async UniTaskVoid DisableProtectionAsync() {
        await UniTask.Delay(TimeSpan.FromSeconds(invulnerableTime));
        _protectionEnabled = false;
    }

    private bool ShouldCancel(IDamageReceiver receiver) =>
        _damageHalted || (_protectionEnabled && receiver.Host.TryGetComponent(out Mario _));

    /// <summary>
    /// 强制造成伤害
    /// </summary>
    private void OnCollisionEnter2D(Collision2D other) {
        if (_damageHalted) return;
        // 防止误伤踩龟壳的马里奥
        if (other.transform.TryGetComponent(out Mario mario) && _damageSource.IsStomp(other.transform, mario)) {
            return;
        }

        // 造成伤害
        if (other.transform.TryGetComponent(out IDamageReceiver receiver)) {
            _damageSource.DoDamageTo(receiver);
        }
    }

    [SerializeField] private float invulnerableTime = 0.6f;

    private bool _protectionEnabled = true;
    private DamageSource _damageSource;
    private bool _damageHalted;
}
}
