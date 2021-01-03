using JetBrains.Annotations;
using SweetMoleHouse.MarioForever.Constants;
using SweetMoleHouse.MarioForever.Util;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Level
{
    public class ChangeMusic : HideInPlayMode
    {
        [SerializeField, CanBeNull] private AudioClip music;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Consts.TAG_PLAYER))
            {
                AreaManager.SetMusic(music);
            }
        }
    }
}