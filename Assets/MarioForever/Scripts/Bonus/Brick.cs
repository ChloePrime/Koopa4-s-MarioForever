using SweetMoleHouse.MarioForever.Scripts.Base.Physics;
using SweetMoleHouse.MarioForever.Scripts.Constants;
using SweetMoleHouse.MarioForever.Scripts.Persistent;
using SweetMoleHouse.MarioForever.Scripts.Player;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Bonus {
public class Brick : HittableBase {
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip breakSound;
    [SerializeField] private long score = 50;

    public override bool OnHit(Transform hitter) {
        DamageEnemiesAbove();

        if (hitter.TryGetComponent(out Mario mario) && mario.Powerup == MarioPowerup.SMALL) {
            Global.PlaySound(hitSound);
            base.OnHit(hitter);
        } else {
            Break();
        }

        return true;
    }

    /// <summary>
    /// 碎了
    /// </summary>
    private void Break() {
        MarioProperty.Score += score;
        Global.PlaySound(breakSound);
        Destroy(gameObject);
    }
}
}