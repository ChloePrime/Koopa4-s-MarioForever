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
    [SerializeField] private bool autoDestroy = true;

    private void Awake() {
        _physics = GetComponent<BasePhysics>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Fly(DamageSource damageSrc) {
        float dir;
        if (damageSrc.FixCorpseDirection && damageSrc.Host.TryGetComponent(out BasePhysics bullet)) {
            dir = MathF.Sign(bullet.Direction);
        } else {
            dir = Random.Range(0, 2) * 2 - 1;
        }

        if (_physics is Walk walk) {
            walk.WalkSpeed = dir * flySpeed.x;
        } else {
            _physics.XSpeed = dir * flySpeed.x;
        }

        _physics.YSpeed = flySpeed.y;
    }

    private void FixedUpdate() {
        if (autoDestroy && (transform.position.y <= ScrollInfo.Bottom - 8)) {
            Destroy(gameObject);
        }
    }

    public void InitCorpse(DamageSource damageSrc, SpriteRenderer sr) {
        Vector2 initialPos = (Vector2)sr.transform.position + new Vector2(0, sr.size.y);
        _physics.TeleportTo(initialPos);
        transform.localScale = sr.transform.lossyScale;
        _renderer.sprite = sr.sprite;
        Fly(damageSrc);
    }
    
    private SpriteRenderer _renderer;
    private BasePhysics _physics;
}
}