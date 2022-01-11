using System;
using SweetMoleHouse.MarioForever.Scripts.Base.Rpg;
using SweetMoleHouse.MarioForever.Scripts.Constants;
using SweetMoleHouse.MarioForever.Scripts.Enemy;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Util {
/// <summary>
/// 连杀计分器。
/// 和 <see cref="DamageSource"/> 放在一个 GameObject 上可以让伤害来源获得连杀效果。
/// </summary>
public class ComboCalculator : MonoBehaviour {
    private const ScoreType InitialScore = ScoreType.S100;

    [SerializeField] private bool loop;
    [SerializeField] private bool overrideDeathSound;
    
    public void Hit(Transform trr) {
        _current.Summon(trr);
        bool end = !Enum.IsDefined(typeof(ScoreType), _current + 1);
        if (end) {
            if (loop) {
                _current = InitialScore;
            }
        } else {
            _current += 1;
        }
    }

    public void ResetCombo() {
        _current = InitialScore;
    }

    /// <summary>
    /// 让 this 接管指定的 <see cref="damager"/>，
    /// 让 damager 造成伤害时获得 Combo 计分效果。
    /// </summary>
    public void TakeOver(DamageSource damager) {
        damager.OnModifyDamageProperties += ModifyDamageProperties;
    }

    private void ModifyDamageProperties(ref DamageEvent damage) {
        Transform host = damage.Source.Host;
        
        damage.CreateScoreOverride += () => Hit(host);
        if (overrideDeathSound) {
            damage.PlayDeathSoundOverride += () => _current.PlayHitSound();
        }
    }
    
    private void Awake() {
        if (TryGetComponent(out DamageSource damager)) {
            TakeOver(damager);
        }
    }
    
    private ScoreType _current;
}
}
