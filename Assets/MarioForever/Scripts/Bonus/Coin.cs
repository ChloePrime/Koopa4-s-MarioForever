using System;
using SweetMoleHouse.MarioForever.Scripts.Base.Rpg;
using SweetMoleHouse.MarioForever.Scripts.Constants;
using SweetMoleHouse.MarioForever.Scripts.Effect;
using SweetMoleHouse.MarioForever.Scripts.Enemy;
using SweetMoleHouse.MarioForever.Scripts.Persistent;
using SweetMoleHouse.MarioForever.Scripts.Player;
using SweetMoleHouse.MarioForever.Scripts.Util;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Bonus {
public class Coin : MonoBehaviour, IDamageReceiver {
    [SerializeField] private int nominalValue = 1;
    [SerializeField] private GameObject bumpedFx;
    
    [SerializeField] private EnumDamageType fetchMethodsByDamage;
    [SerializeField] private Faction fetchMethodsByFaction;

    private void OnBumped() {
        if (_eaten) {
            return;
        }

        Transform myTransform = transform;
        Vector3 fxPosition = myTransform.position + new Vector3(0, -0.5F, 0);
        GameObject fx = Instantiate(bumpedFx, fxPosition, myTransform.rotation, myTransform.parent);
        fx.GetComponent<LeapingCoin>().nominalValue = nominalValue;

        _eaten = true;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (_eaten) {
            return;
        }
        
        if (other.GetHost().TryGetComponent(out Mario mario)) {
            MarioProperty.AddCoin(nominalValue);
            _eaten = true;
            Destroy(gameObject);
            return;
        }

        if (other.CompareTag(Tags.CoinEater)) {
            OnBumped();
        }
    }
    
    // IDamageReceiver impl
    // 实现此接口来让金币可以被砖块顶掉

    public Transform Host => transform;
    public Faction Faction => fetchMethodsByFaction;
    
    public void Damage(DamageEvent damage) {
        SetDead(damage);
    }

    public void SetDead(DamageEvent damage) {
        if (damage.Type.Contains(fetchMethodsByDamage)) {
            OnBumped();
        }
    }

    private bool _eaten;
}
}
