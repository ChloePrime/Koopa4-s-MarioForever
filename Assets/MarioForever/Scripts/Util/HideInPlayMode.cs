using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Util
{
    public class HideInPlayMode : MonoBehaviour
    {
        protected virtual void Start()
        {
            if (TryGetComponent(out SpriteRenderer sr))
            {
                Destroy(sr);
            }
        }
    }
}
