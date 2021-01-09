using System.Collections;
using SweetMoleHouse.MarioForever.Scripts.Util;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Common
{
    public class DelayedDisappear : MonoBehaviour
    {
        [SerializeField] private float startDisappearDelay;
        [SerializeField] private float gradientTime;

        private float startA = -1;
        private new SpriteRenderer renderer;
        private float gradientProg;

        private void Start()
        {
            if (!this.TryBfsComponentInChildren(out renderer))
            {
                UnityEngine.Debug.LogError($"Applying {nameof(DelayedDisappear)} to an invisible object!");
                Destroy(this);
            }

            StartCoroutine(Countdown());
        }

        private void Update()
        {
            if (startA < 0) return;
            
            gradientProg += Time.deltaTime;
            if (gradientProg >= gradientTime)
            {
                Destroy(gameObject);
            }
            else
            {
                var mat = renderer.material;
                var c = mat.color;
                c.a = (1 - gradientProg / gradientTime) * startA;
                mat.color = c;
            }
        }

        private IEnumerator Countdown()
        {
            yield return new WaitForSeconds(startDisappearDelay);
            startA = renderer.material.color.a;
        }
    }
}
