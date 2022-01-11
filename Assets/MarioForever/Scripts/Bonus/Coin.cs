using SweetMoleHouse.MarioForever.Scripts.Persistent;
using SweetMoleHouse.MarioForever.Scripts.Player;
using SweetMoleHouse.MarioForever.Scripts.Util;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Bonus {
public class Coin : MonoBehaviour {
    [SerializeField] private int nominalValue = 1;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetHost().TryGetComponent(out Mario mario)) {
            MarioProperty.AddCoin(nominalValue);
            Destroy(gameObject);
        }
    }
}
}
