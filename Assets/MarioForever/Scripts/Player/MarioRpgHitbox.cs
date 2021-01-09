using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Player
{
    /// <summary>
    /// 马里奥的伤害判定
    /// </summary>
    public class MarioRpgHitbox : MonoBehaviour
    {
        public Mario Mario { get; private set; }

        private void Start()
        {
            Mario = GetComponentInParent<Mario>();
        }
    }
}