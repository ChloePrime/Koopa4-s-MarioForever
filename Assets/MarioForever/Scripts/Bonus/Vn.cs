using SweetMoleHouse.MarioForever.Scripts.Base.Rpg;
using SweetMoleHouse.MarioForever.Scripts.Enemy;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Bonus {
/// <summary>
/// 毒蘑菇。
/// 此实现在 <see cref="DamageSource.Faction"/> 正确配置的情况下允许 VN 杀死怪物。
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
