using System.Collections;
using SweetMoleHouse.MarioForever.Persistent;
using SweetMoleHouse.MarioForever.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

namespace SweetMoleHouse.MarioForever.Player
{
    /// <summary>
    /// 你看那马里奥笑得多开心啊
    /// </summary>
    public class MarioCorpse : MonoBehaviour
    {
        [SerializeField]
        private float ySpeed = 16F;
        [SerializeField]
        private float gravity = 48;

        private float countdownBeforeMove = 0.5F;
        private const float RestartCd = 4;

        private void Start()
        {
            StartCoroutine(Restart());
        }

        private void FixedUpdate()
        {
            if (countdownBeforeMove > 0)
            {
                countdownBeforeMove -= Time.fixedDeltaTime;
                return;
            }

            ySpeed -= gravity * Time.fixedDeltaTime;
        }

        private static IEnumerator Restart()
        {
            yield return new WaitForSeconds(RestartCd);
            if (MarioProperty.Lives > 0)
            {
                MarioProperty.Lives -= 1;
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                FindObjectOfType<GameOverUI>().Active();
            }
        }

        private void Update()
        {
            if (countdownBeforeMove > 0) return;
            transform.Translate(0, ySpeed * Time.deltaTime, 0);
        }
    }
}
