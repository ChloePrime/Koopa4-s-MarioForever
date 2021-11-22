using System;
using SweetMoleHouse.MarioForever.Scripts.Base.Physics;
using SweetMoleHouse.MarioForever.Scripts.Level;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SweetMoleHouse.MarioForever.Scripts.Enemy {
/// <summary>
/// 怪物尸体
/// </summary>
public class Corpse : MonoBehaviour {
    [SerializeField] private Vector2 flySpeed = new (5, 12);
    
    private new SpriteRenderer renderer;
    private BasePhysics physics;

    private void Awake() {
        physics = GetComponent<BasePhysics>();
        renderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Fly(DamageSource damageSrc) {
        float dir;
        if (damageSrc.FixCorpseDirection && damageSrc.Host.TryGetComponent(out BasePhysics bullet)) {
            dir = MathF.Sign(bullet.Direction);
        } else {
            dir = Random.Range(0, 2) * 2 - 1;
        }

        if (physics is Walk walk) {
            walk.WalkSpeed = dir * flySpeed.x;
        } else {
            physics.XSpeed = dir * flySpeed.x;
        }

        physics.YSpeed = flySpeed.y;
    }

    private void Update() {
        if (transform.position.y <= ScrollInfo.Bottom - 8) {
            Destroy(gameObject);
        }
    }

    public void InitCorpse(DamageSource damageSrc, SpriteRenderer sr) {
        Vector2 initialPos = (Vector2)sr.transform.position + new Vector2(0, sr.size.y);
        physics.TeleportTo(initialPos);
        transform.localScale = sr.transform.lossyScale;
        renderer.sprite = sr.sprite;
        Fly(damageSrc);
    }
}
}