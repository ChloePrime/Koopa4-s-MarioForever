using SweetMoleHouse.MarioForever.Level;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SweetMoleHouse.MarioForever.Player
{
    /// <summary>
    /// 你看那马里奥笑得多开心啊
    /// </summary>
    public class MarioCorpse : MonoBehaviour
    {
        private float countdown = 0.5F;
        [SerializeField]
        private float ySpeed = 16F;
        [SerializeField]
        private float gravity = 48;

        private float restartCd = 4;
        private bool restartStarted;
        private void FixedUpdate()
        {
            TryRestart();
            if (countdown > 0)
            {
                countdown -= Time.fixedDeltaTime;
                return;
            }

            ySpeed -= gravity * Time.fixedDeltaTime;
        }

        private void TryRestart()
        {
            if (restartCd <= 0)
            {
                if (!restartStarted)
                {
                    SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
                }
                restartStarted = true;
            }
            restartCd -= Time.fixedDeltaTime;
        }

        private void Update()
        {
            if (countdown > 0) return;
            transform.Translate(0, ySpeed * Time.deltaTime, 0);
        }
    }
}
