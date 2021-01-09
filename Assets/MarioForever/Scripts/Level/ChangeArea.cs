using SweetMoleHouse.MarioForever.Scripts.Constants;
using SweetMoleHouse.MarioForever.Scripts.Util;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Level
{
    public class ChangeArea : HideInPlayMode
    {
        [SerializeField] private Area target;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Consts.TAG_PLAYER))
            {
                AreaManager.SetCurrentArea(target);
            }
        }
    }
}
