using System;
using SweetMoleHouse.MarioForever.Scripts.Base.Physics;
using SweetMoleHouse.MarioForever.Scripts.Base.Rpg;
using SweetMoleHouse.MarioForever.Scripts.Enemy;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Bonus
{
    public abstract class HittableBase : MonoBehaviour, IHittable {
        private static readonly Vector2 HitRange = new(0, 0.2f);
        private static readonly Collider2D[] ColliderPool = new Collider2D[64];
        private static readonly ContactFilter2D ColliderFilter = new ContactFilter2D().NoFilter();
        
        protected virtual void Awake() {
            _collider = GetComponent<Collider2D>();
            _bumpDamageSource = GetComponent<DamageSource>();
        }

        public abstract bool OnHit(Transform hitter);

        protected void DamageEnemiesAbove() {
            Vector2 physicsPos = _collider.offset;
            _collider.offset = physicsPos + HitRange;
            try {
                int c = _collider.OverlapCollider(ColliderFilter, ColliderPool);
                for (var i = 0; i < c; i++) {
                    Collider2D enemy = ColliderPool[i];
                    if (enemy.TryGetComponent(out DamageReceiver dr)) {
                        _bumpDamageSource.DoDamageTo(dr);
                    }
                }
            } finally {
                _collider.offset = physicsPos;
            }
        }

        private Collider2D _collider;
        private DamageSource _bumpDamageSource;
    }
}
