using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts
{
    /// <summary>
    /// 锁定纵横比
    /// </summary>
    public class AspectLock : MonoBehaviour 
    {
        [SerializeField]
        private float targetAspect = 4f / 3;
        private void Update()
        {
            if (Camera.main.aspect != targetAspect)
            {
                Camera.main.aspect = targetAspect;
            }
        }
    }
}