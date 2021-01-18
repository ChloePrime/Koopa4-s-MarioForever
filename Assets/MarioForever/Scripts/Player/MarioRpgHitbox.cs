using SweetMoleHouse.MarioForever.Scripts.Base.Rpg;
using SweetMoleHouse.MarioForever.Scripts.Enemy;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Player
{
    /// <summary>
    /// 马里奥的伤害判定
    /// </summary>
    public class MarioRpgHitbox : MonoBehaviour, IDamageReceiver
    {
        [SerializeField] private Faction faction;

        public Transform Host => Mario.transform;
        public Mario Mario { get; private set; }
        public Faction Faction => faction;
        public void Damage(in EnumDamageType type)
        {
            Mario.Damage();
        }

        public void SetDead(in EnumDamageType type)
        {
            if (Mario != null)
            {
                Mario.Kill();
            }
        }

        private void Start()
        {
            Mario = GetComponentInParent<Mario>();
        }
    }
}