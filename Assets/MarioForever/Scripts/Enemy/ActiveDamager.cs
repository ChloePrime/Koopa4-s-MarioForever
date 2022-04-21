using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using SweetMoleHouse.MarioForever.Scripts.Base.Rpg;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SweetMoleHouse.MarioForever.Scripts.Enemy {
/// <summary>
/// 让当前物体主动攻击可攻击的目标。
/// 需要自身带有 Trigger 类碰撞箱。
/// </summary>
public class ActiveDamager : MonoBehaviour {
    
    private void Awake() {
        _damageSource = GetComponent<DamageSource>();
    }

    private async void OnTriggerEnter2D(Collider2D other) {
        while (_bIteratingObjectsInTrigger) {
            // Unlikely
            await UniTask.Yield();
        }
        if (other != null && other.TryGetComponent(out IDamageReceiver dr)) {
            (_inTrigger ??= new HashSet<IDamageReceiver>()).Add(dr);
        }
    }

    private void FixedUpdate() {
        if (ReferenceEquals(_inTrigger, null)) {
            return;
        }
        _inTrigger.RemoveWhere(dr => ((Object)dr) == null);
        _bIteratingObjectsInTrigger = true;
        foreach (IDamageReceiver dr in _inTrigger) {
            _damageSource.DoDamageTo(dr);
        }
        _bIteratingObjectsInTrigger = false;
    }

    private async void OnTriggerExit2D(Collider2D other) {
        while (_bIteratingObjectsInTrigger) {
            // Unlikely
            await UniTask.Yield();
        }
        if (other != null && other.TryGetComponent(out IDamageReceiver dr)) {
            (_inTrigger ??= new HashSet<IDamageReceiver>()).Remove(dr);
        }
    }

    private DamageSource _damageSource;
    private HashSet<IDamageReceiver> _inTrigger;
    private bool _bIteratingObjectsInTrigger;
}
}
