using System;
using System.Runtime.CompilerServices;
using SweetMoleHouse.MarioForever.Scripts.Base;
using SweetMoleHouse.MarioForever.Scripts.Base.Rpg;
using SweetMoleHouse.MarioForever.Scripts.Constants;
using SweetMoleHouse.MarioForever.Scripts.Player;
using SweetMoleHouse.MarioForever.Scripts.Util;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Enemy {
/// <summary>
/// 伤害来源
/// 需要自身带有 Trigger 类碰撞箱
/// </summary>
public class DamageSource : Stompable, ISubObject {
    [SerializeField] private EnumDamageType damageType;
    [SerializeField] private Faction faction;

    [SerializeField, RenameInInspector("固定尸体飘动方向")]
    private bool fixCorpseDirection;


    [Header("高级设置")] [SerializeField] private Transform host;
    [SerializeField] private bool disableActiveDamage;

    public Transform Host => host;
    public Faction Faction => faction;
    public bool FixCorpseDirection => fixCorpseDirection;

    public event Func<DamageSource, IDamageReceiver, ActionResult> OnPreDamage;
    public event Action<DamageSource, IDamageReceiver> OnDamaging;

    public void DoDamageTo(IDamageReceiver hitbox) {
        if (!faction.IsHostileTo(hitbox.Faction)) return;
        // 判断是否是踩
        if (IsStomp(hitbox.Host, hitbox.Host.GetComponent<Mario>())) {
            return;
        }

        if (OnPreDamage?.Invoke(this, hitbox).IsCanceled() != true) {
            hitbox.Damage(this, damageType);
        }

        OnDamaging?.Invoke(this, hitbox);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnTriggerEnter2D(Collider2D other) {
        OnTriggerStay2D(other);
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (disableActiveDamage) {
            return;
        }

        if (!other.TryGetComponent(out IDamageReceiver hitbox)) {
            return;
        }
        DoDamageTo(hitbox);
    }
}
}