using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Player
{
    /// <summary>
    /// 马里奥的伤害判定
    /// </summary>
    public class MarioRpgHitbox : MonoBehaviour 
    {
        public Mario Mario { get; private set; }

        private void Start()
        {
            Mario = transform.parent.parent.parent.GetComponent<Mario>();
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            
        }
    }
}