using System;
using SweetMoleHouse.MarioForever.Scripts.Base.Rpg;
using SweetMoleHouse.MarioForever.Scripts.Constants;
using SweetMoleHouse.MarioForever.Scripts.Enemy;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Util {
/// <summary>
/// 和 <see cref="DamageSource"/> 放在一个 GameObject 上可以让伤害来源获得连杀效果。
/// </summary>
public class ComboCalculator : MonoBehaviour {
    private const ScoreType InitialScore = ScoreType.S100;

    [SerializeField] private bool loop;

    private ScoreType current;

    private void Awake() {
        if (TryGetComponent(out DamageSource damager)) {
            Transform host = damager.Host;
            damager.OnModifyDamageProperties +=
                (ref DamageEvent damage) => damage.CreateScoreOverride += () => Hit(host);
        }
    }

    public void Hit(Transform trr) {
        current.Summon(trr);
        bool end = !Enum.IsDefined(typeof(ScoreType), current + 1);
        if (end) {
            if (loop) {
                current = InitialScore;
            }
        } else {
            current += 1;
        }
    }

    public void ResetCombo() {
        current = InitialScore;
    }
}
}
