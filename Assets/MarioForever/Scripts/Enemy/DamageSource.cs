using System;
using SweetMoleHouse.MarioForever.Scripts.Base;
using SweetMoleHouse.MarioForever.Scripts.Base.Rpg;
using SweetMoleHouse.MarioForever.Scripts.Constants;
using SweetMoleHouse.MarioForever.Scripts.Player;
using SweetMoleHouse.MarioForever.Scripts.Util;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Enemy {
/// <summary>
/// 伤害来源。
/// </summary>
public class DamageSource : Stompable, ISubObject {
    [SerializeField] private EnumDamageType damageType;
    [SerializeField] private Faction faction;

    [SerializeField, RenameInInspector("固定尸体飘动方向")]
    private bool fixCorpseDirection;

    [Header("高级设置")] [SerializeField] private Transform host;

    public Transform Host => host;
    public Faction Faction => faction;
    public bool FixCorpseDirection => fixCorpseDirection;

    public event Func<DamageSource, IDamageReceiver, ActionResult> OnPreDamage;
    public event Action<DamageSource, IDamageReceiver> OnDamaging;

    public delegate void DamageModifier(ref DamageEvent damage);

    /// <summary>
    /// 对 DamageEvent 进行一些后处理
    /// </summary>
    public event DamageModifier OnModifyDamageProperties;

    public bool CanHarm<T>(T hitbox) where T : IDamageReceiver {
        if (!faction.IsHostileTo(hitbox.Faction)) return false;
        // 判断是否是踩
        if (hitbox.Host.TryGetComponent(out Mario mario) && IsStomp(hitbox.Host, mario)) {
            return false;
        }

        return true;
    }

    public void DoDamageTo<T>(T hitbox) where T : IDamageReceiver => DoDamageTo(hitbox, damageType);

    public void DoDamageTo<T>(T hitbox, EnumDamageType damageTypeIn) where T : IDamageReceiver {
        DamageEvent damage = new(this, damageTypeIn);
        DoDamageTo(hitbox, damage);
    }

    public void DoDamageTo<T>(T hitbox, DamageEvent damage) where T : IDamageReceiver {
        if (damage.Source != this) {
            throw new ArgumentException("Damage source not match!");
        }

        if (!CanHarm(hitbox)) {
            return;
        }

        if (OnPreDamage?.Invoke(this, hitbox).IsCanceled() != true) {
            OnModifyDamageProperties?.Invoke(ref damage);
            hitbox.Damage(damage);
        }

        OnDamaging?.Invoke(this, hitbox);
    }

    public DamageEvent CreateDamageEvent() {
        return CreateDamageEvent(this.damageType);
    }

    public DamageEvent CreateDamageEvent(EnumDamageType damageTypeIn) {
        return new DamageEvent(this, damageTypeIn);
    }

    public void Kill<T>(T hitbox) where T : IDamageReceiver => Kill(hitbox, damageType);

    public void Kill<T>(T hitbox, EnumDamageType damageTypeIn) where T : IDamageReceiver {
        if (!CanHarm(hitbox)) {
            return;
        }

        DamageEvent damage = new(this, damageTypeIn);
        hitbox.SetDead(damage);
    }
}
}