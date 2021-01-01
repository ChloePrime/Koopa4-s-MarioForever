using SweetMoleHouse.MarioForever.Constants;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Level
{
    public class ChangeArea : MonoBehaviour
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
