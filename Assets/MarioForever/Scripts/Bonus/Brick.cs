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
    [SerializeField] private GameObject fragment;
    
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
        SpawnFragments();
        Destroy(gameObject);
    }

    private void SpawnFragments() {
        Transform myTransform = transform;
        Vector3 myPosition = myTransform.position;
        
        foreach (FragmentInfo data in FragmentsPosition) {
            // 生成碎片物件，调整位置
            GameObject fragInstance = Instantiate(fragment, myTransform.parent);
            fragInstance.transform.position = myPosition + (Vector3)data.Offset;
            
            // 对碎片施加力和扭矩
            Rigidbody2D fragsRigidbody = fragInstance.GetComponent<Rigidbody2D>();
            fragsRigidbody.AddForce(data.Impulse, ForceMode2D.Impulse);
            fragsRigidbody.AddTorque(data.Torque, ForceMode2D.Impulse);
        }
    }
    
    private struct FragmentInfo {
        public Vector2 Offset;
        public Vector2 Impulse;
        public float Torque;
    }
    

    private static readonly FragmentInfo[] FragmentsPosition = {
        new() {
            Offset = new Vector2(5F / 32, -22F / 32),
            Impulse = new Vector2(30F, 18F),
            Torque = -2F
        },
        new() {
            Offset = new Vector2(-5F / 32, -22F / 32),
            Impulse = new Vector2(-30F, 18F),
            Torque = 2F
        },
        new() {
            Offset = new Vector2(-8F / 32, -9F / 32),
            Impulse = new Vector2(-18F, 30F),
            Torque = 2F
        },
        new() {
            Offset = new Vector2(8F / 32, -9F / 32),
            Impulse = new Vector2(18F, 30F),
            Torque = -2F
        },
    };

}
}