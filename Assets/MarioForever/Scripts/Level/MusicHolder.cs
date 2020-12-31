using SweetMoleHouse.MarioForever.Base;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Level
{
    public class MusicHolder : Singleton<MusicHolder>
    {
        private new AudioSource audio;

        private void Start()
        {
            audio = GetComponent<AudioSource>();
        }

        public void SetMusic(AudioClip music)
        {
            if (audio.clip == music) return;
            
            audio.clip = music;
            audio.Play();
        }
    }
}
