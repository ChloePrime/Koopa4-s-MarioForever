using System.Runtime.CompilerServices;
using SweetMoleHouse.MarioForever.Scripts.Base.Rpg;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Enemy {
/// <summary>
/// 让当前物体主动攻击可攻击的目标。
/// 需要自身带有 Trigger 类碰撞箱。
/// </summary>
public class ActiveDamager : MonoBehaviour {
    
    private void Awake() {
        _damageSource = GetComponent<DamageSource>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnTriggerEnter2D(Collider2D other) {
        OnTriggerStay2D(other);
    }

    private void OnTriggerStay2D(Collider2D other) {
        
        if (other.TryGetComponent(out IDamageReceiver hitbox)) {
            _damageSource.DoDamageTo(hitbox);
        }
    }

    private DamageSource _damageSource;
}
}
