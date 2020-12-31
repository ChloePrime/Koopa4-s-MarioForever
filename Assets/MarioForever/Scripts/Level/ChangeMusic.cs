using UnityEngine;

namespace SweetMoleHouse.MarioForever.Level
{
    public class ChangeMusic : MonoBehaviour
    {
        [SerializeField] private AudioClip music;

        private void Start()
        {
            if (TryGetComponent(out SpriteRenderer sr))
            {
                Destroy(sr);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            MusicHolder.Instance.SetMusic(music);
        }
    }
}