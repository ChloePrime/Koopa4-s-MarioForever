using UnityEngine;

namespace SweetMoleHouse.MarioForever.Effect
{
    public class Flashing : MonoBehaviour
    {
        private const float RainbowPower = 0.5F;
        [SerializeField] private float flashCycle = 0.2f;
        [SerializeField] private float rainbowCycle = 0.2f;
        private static readonly int SHADER_PAR_HUE = Shader.PropertyToID("_Hue");
        private static readonly int SHADER_PAR_SATURATION = Shader.PropertyToID("_Saturation");
        private static readonly int SHADER_PAR_BRIGHTNESS = Shader.PropertyToID("_Brightness");
        
        private new SpriteRenderer renderer;
        private float flashTime;
        private float rainbowFlashTime;

        public float FlashTime
        {
            get => flashTime;
            set => flashTime = value;
        }
        
        public float RainbowFlashTime
        {
            get => rainbowFlashTime;
            set => rainbowFlashTime = value;
        }

        private void Start()
        {
            renderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (flashTime > 0)
            {
                var color = renderer.material.color;
                color.a = PingPong(ref flashTime, flashCycle);
                renderer.material.color = color;
            }

            if (RainbowFlashTime > 0)
            {
                var mat = renderer.material;
                // 0 - 1 最终停在 0
                float prog = 1 - PingPong(ref rainbowFlashTime, rainbowCycle);
                if (rainbowFlashTime > 0)
                {
                    // -power - power，停留在 0
                    float amount = RainbowPower * ((prog + 0.5F) % 1F - 0.5F);
                    float hue = 3 * amount;
                    float sat = 1 + RainbowPower * prog;
                    float brightness = 1 - RainbowPower + 6 * RainbowPower * prog;
                    mat.SetFloat(SHADER_PAR_HUE, hue);
                    mat.SetFloat(SHADER_PAR_SATURATION, sat);
                    mat.SetFloat(SHADER_PAR_BRIGHTNESS, brightness);
                }
                else
                {
                    mat.SetFloat(SHADER_PAR_HUE, 0);
                    mat.SetFloat(SHADER_PAR_SATURATION, 1);
                    mat.SetFloat(SHADER_PAR_BRIGHTNESS, 1);
                }
            }
        }
        
        private static float PingPong(ref float time, float cycle)
        {
            // 0 ~ INVUL_CYCLE
            var alpha = Time.time % cycle;
            if (alpha > cycle / 2)
            {
                alpha = cycle - alpha;
            }

            alpha *= 2 / cycle;
            time -= Time.deltaTime;
            if (time < 0)
            {
                return 1;
            }
            return alpha;
        }
    }
}
