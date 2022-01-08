using System;
using System.Collections;
using System.Collections.Generic;
using SweetMoleHouse.MarioForever.Scripts.Base.Rpg;
using SweetMoleHouse.MarioForever.Scripts.Enemy;
using SweetMoleHouse.MarioForever.Scripts.Util;
using UnityEngine;

namespace SweetMoleHouse {
/// <summary>
/// 毒蘑菇
/// </summary>
public class Vn : MonoBehaviour {
    [SerializeField] private GameObject explodeEffect;
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

        ExplodeAt(hitbox.Host);
        
        _damageSource.Kill(hitbox);
        Destroy(_damageSource.Host.gameObject);
    }

    private void ExplodeAt(Transform at) {
        GameObject fx = Instantiate(explodeEffect, at.parent);
        fx.transform.position = at.position;
    }

    private DamageSource _damageSource;
}
}
