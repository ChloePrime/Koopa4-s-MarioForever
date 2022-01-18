using SweetMoleHouse.MarioForever.Scripts.Base.Physics;
using SweetMoleHouse.MarioForever.Scripts.Enemy;
using SweetMoleHouse.MarioForever.Scripts.Level;
using SweetMoleHouse.MarioForever.Scripts.Util;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Common {
public class Fireball : MonoBehaviour {
    [SerializeField, RenameInInspector("爆炸音效")]
    private AudioClip explodeSound;

    private void Start() {
        var bound = GetComponent<Collider2D>().bounds.size;
        _size = Mathf.Max(bound.x, bound.y);

        _physics = GetComponent<BasePhysics>();
        InstallExplodeConditions();

        var particleCom = GetComponentInChildren<ParticleSystem>();
        if (particleCom != null) {
            _particle = particleCom.gameObject;
            _particle.SetActive(false);
        }
    }

    private void InstallExplodeConditions() {
        _physics.OnHitWallX += _ => Explode();
        _physics.OnHitWallY += OnHitWallY;

        GetComponentInChildren<DamageSource>().OnPostDamage += _ => Explode();
    }

    private void FixedUpdate() {
        if (Mathf.Abs(_physics.XSpeed) <= 1e-2) {
            Explode();
        }

        if (transform.DistanceOutOfScreen() > _size) {
            Explode(false);
        }
    }

    private void OnHitWallY(Collider2D[] colliders) {
        if (_physics.YSpeed > 0) {
            Explode();
        }
    }

    private void Explode() => Explode(true);

    private void Explode(bool enableEffect) {
        TrailRenderer tr;
        if ((tr = GetComponentInChildren<TrailRenderer>()) != null) {
            tr.transform.parent = transform.parent;
        }

        if (enableEffect) {
            if (_particle != null) {
                _particle.SetActive(true);
                _particle.transform.parent = transform.parent;
            }

            Global.PlaySound(explodeSound);
        }

        Destroy(gameObject);
    }

    private BasePhysics _physics;
    private float _size;
    private GameObject _particle;
}
}
