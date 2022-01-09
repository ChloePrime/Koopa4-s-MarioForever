using SweetMoleHouse.MarioForever.Scripts.Bonus;
using SweetMoleHouse.MarioForever.Scripts.Persistent;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Effect {
/// <summary>
/// 金币问号中顶出的金币特效
/// </summary>
public class LeapingCoin : MonoBehaviour {
    private const float LeapTime = 0.428F;
    private const float LeapDistance = 3;
    
    private void Start() {
        _bornTime = Time.time;
        MarioProperty.AddCoin();
    }

    private void Update() {
        float t = Time.time - _bornTime;
        if (t > LeapTime) {
            return;
        }

        // a = 2s/t², s = 0.5at²
        const float acc = 2 * LeapDistance / (LeapTime * LeapTime);
        const float vmax = acc * LeapTime;
        float v = vmax - acc * t;
        transform.Translate(0, v * Time.deltaTime, 0);
    }

    private void OnDestroy() {
        // ScoreType.S200.Summon(transform, 0, 0.5F);
    }

    private float _bornTime;
}
}
