using System;
using System.Collections;
using System.Collections.Generic;
using SweetMoleHouse.MarioForever.Scripts.Base.Rpg;
using SweetMoleHouse.MarioForever.Scripts.Enemy;
using UnityEngine;

namespace SweetMoleHouse {
/// <summary>
/// 毒蘑菇
/// </summary>
public class Vn : MonoBehaviour {
    private void Awake() {
        _damageSource = GetComponent<DamageSource>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.TryGetComponent(out IDamageReceiver hitbox)) {
            return;
        }

        if (!_damageSource.CanHarm(hitbox)) {
            return;
        }

        _damageSource.Kill(hitbox);
        Destroy(_damageSource.Host.gameObject);
    }

    private DamageSource _damageSource;
}
}
