using SweetMoleHouse.MarioForever.Level;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Enemy
{
    /// <summary>
    /// 怪物尸体
    /// </summary>
    public class Corpse : MonoBehaviour 
    {
        [SerializeField]
        private float gravity = 48;

        public float YSpeed { get; set; }
        private new SpriteRenderer renderer;

        private void Update()
        {
            YSpeed -= gravity * Time.deltaTime;
            transform.Translate(0, YSpeed * Time.deltaTime, 0);
            if (transform.position.y <= ScrollInfo.Bottom - 8)
            {
                Destroy(gameObject);
            }
        }

        public void AcceptBody(in SpriteRenderer sr)
        {
            if (renderer == null)
            {
                renderer = GetComponent<SpriteRenderer>();
            }
            transform.position = sr.transform.position;
            renderer.sprite = sr.sprite;
        }
    }
}