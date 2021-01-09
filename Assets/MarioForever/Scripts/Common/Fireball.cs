using SweetMoleHouse.MarioForever.Scripts.Base;
using SweetMoleHouse.MarioForever.Scripts.Level;
using SweetMoleHouse.MarioForever.Scripts.Util;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Common
{
    public class Fireball : MonoBehaviour
    {
        [SerializeField, RenameInInspector("爆炸音效")]
        private AudioClip explodeSound;
        
        private BasePhysics physics;
        private float size;
        private GameObject particle;

        private void Start()
        {
            var bound = GetComponent<Collider2D>().bounds.size;
            size = Mathf.Max(bound.x, bound.y);
            
            physics = GetComponent<BasePhysics>();
            physics.OnHitWallX += _ => Explode();
            physics.OnHitWallY += OnHitWallY;

            var particleCom = GetComponentInChildren<ParticleSystem>();
            if (particleCom != null)
            {
                particle = particleCom.gameObject;
                particle.SetActive(false);
            }
        }

        private void FixedUpdate()
        {
            if (Mathf.Abs(physics.XSpeed) <= 1e-2)
            {
                Explode();
            }
            if (transform.DistanceOutOfScreen() > size)
            {
                Explode(false);
            }
        }

        private void OnHitWallY(Collider2D[] colliders)
        {
            if (physics.YSpeed > 0)
            {
                Explode();
            }
        }

        private void Explode() => Explode(true);
        private void Explode(bool enableEffect)
        {
            TrailRenderer tr;
            if ((tr = GetComponentInChildren<TrailRenderer>()) != null)
            {
                tr.transform.parent = transform.parent;
            }

            if (enableEffect)
            {
                if (particle != null)
                {
                    particle.SetActive(true);
                    particle.transform.parent = transform.parent;
                }
                Global.PlaySound(explodeSound);
            }
            Destroy(gameObject);
        }
    }
}
