using JetBrains.Annotations;
using SweetMoleHouse.MarioForever.Scripts.Constants;
using SweetMoleHouse.MarioForever.Scripts.Util;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Level
{
    public class ChangeMusic : HideInPlayMode
    {
        [SerializeField, CanBeNull] private AudioClip music;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Tags.Player))
            {
                AreaManager.SetMusic(music);
            }
        }
    }
}