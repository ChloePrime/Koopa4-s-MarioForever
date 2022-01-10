using System;
using SweetMoleHouse.MarioForever.Scripts.Util;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Player {
/// <summary>
/// 玩家无敌特效。
/// </summary>
public class MarioInvincibleController : MonoBehaviour {
    private const float RainbowPower = 0.5F;
    [SerializeField] private float flashCycle = 0.2f;
    [SerializeField] private float rainbowCycle = 0.2f;
    private static readonly int ShaderParHue = Shader.PropertyToID("_Hue");
    private static readonly int ShaderParSaturation = Shader.PropertyToID("_Saturation");
    private static readonly int ShaderParBrightness = Shader.PropertyToID("_Brightness");

    public float FlashTime {
        get => _flashTime;
        set => _flashTime = Math.Max(_flashTime, value);
    }

    public float RainbowFlashTime {
        get => _rainbowFlashTime;
        set => _rainbowFlashTime = Math.Max(_rainbowFlashTime, value);
    }

    public bool Invincible { get; set; }

    public bool HasActiveDamage {
        get => _hasActiveDamage;
        set => SetHasActiveDamageImpl(value);
    }

    private void Awake() {
        _mario = GetComponentInParent<Mario>();
        _invincibleCombo = invincibleDamageSource.GetComponent<ComboCalculator>();
    }

    private void Update() {
        if (_flashTime <= 0 && _rainbowFlashTime <= 0) {
            FinishInvincible();
            return;
        }
        
        if (_flashTime > 0) {
            var color = renderer.material.color;
            color.a = PingPong(ref _flashTime, flashCycle);
            renderer.material.color = color;
        }

        if (RainbowFlashTime > 0) {
            Material mat = renderer.material;
            // 0 - 1 最终停在 0
            float prog = 1 - PingPong(ref _rainbowFlashTime, rainbowCycle);
            if (_rainbowFlashTime > 0) {
                // -power - power，停留在 0
                float amount = RainbowPower * ((prog + 0.5F) % 1F - 0.5F);
                float hue = 3 * amount;
                float sat = 1 + RainbowPower * prog;
                float brightness = 1 - RainbowPower + 6 * RainbowPower * prog;
                mat.SetFloat(ShaderParHue, hue);
                mat.SetFloat(ShaderParSaturation, sat);
                mat.SetFloat(ShaderParBrightness, brightness);
            } else {
                mat.SetFloat(ShaderParHue, 0);
                mat.SetFloat(ShaderParSaturation, 1);
                mat.SetFloat(ShaderParBrightness, 1);
            }
        }
    }

    private void FinishInvincible() {
        if (Invincible) {
            Invincible = false;
        }

        if (HasActiveDamage) {
            HasActiveDamage = false;
        }
    }

    private static float PingPong(ref float time, float cycle) {
        // 0 ~ INVUL_CYCLE
        var alpha = Time.time % cycle;
        if (alpha > cycle / 2) {
            alpha = cycle - alpha;
        }

        alpha *= 2 / cycle;
        time -= Time.deltaTime;
        if (time < 0) {
            return 1;
        }

        return alpha;
    }

    private void SetHasActiveDamageImpl(bool value) {
        _hasActiveDamage = value;
        invincibleDamageSource.SetActive((value));

        if (!value && _invincibleCombo != null) {
            _invincibleCombo.ResetCombo();
        }
    }

    [Header("高级设置")]
    [SerializeField] private new SpriteRenderer renderer;
    [SerializeField] private GameObject invincibleDamageSource;
    
    private Mario _mario;
    private ComboCalculator _invincibleCombo;
    private float _flashTime;
    private float _rainbowFlashTime;
    private bool _hasActiveDamage;
}
}
