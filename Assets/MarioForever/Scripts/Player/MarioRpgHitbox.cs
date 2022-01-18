using SweetMoleHouse.MarioForever.Scripts.Base.Rpg;
using SweetMoleHouse.MarioForever.Scripts.Enemy;
using SweetMoleHouse.MarioForever.Scripts.Util;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Player {
/// <summary>
/// 马里奥的伤害判定
/// </summary>
public class MarioRpgHitbox : MonoBehaviour, IDamageReceiver {
    [SerializeField, RenameInInspector("被顶起跳跃高度")]
    private float bumpHeight = 10;
    
    [SerializeField] private Faction faction;
    public Transform Host => Mario.transform;
    public Mario Mario { get; private set; }
    public Faction Faction => faction;
    public DamageSource MyDamageSource => Mario.StompDamageSource;

    public void Damage(DamageEvent damage) {
        if (damage.Type == EnumDamageType.BUMP) {
            Mario.Jumper.Jump(bumpHeight);
        } else {
            Mario.Damage(damage);
        }
    }

    public void SetDead(DamageEvent damage) {
        if (Mario != null) {
            Mario.Kill();
        }
    }

    private void Start() {
        Mario = GetComponentInParent<Mario>();
    }
}
}