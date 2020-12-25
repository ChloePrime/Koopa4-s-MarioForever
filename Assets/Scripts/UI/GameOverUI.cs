using System.Collections;
using SweetMoleHouse.MarioForever.Util.Editor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SweetMoleHouse.MarioForever.UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField, RenameInInspector("GO小曲儿")]
        private AudioClip jingle;

        [SerializeField] private SceneReference jumpTarget;

        private Image uiImage;
        private bool waitForAnyKey;
        private void Start()
        {
            uiImage = GetComponent<Image>();
            uiImage.enabled = false;
        }

        private void Update()
        {
            if (waitForAnyKey && Keyboard.current.anyKey.isPressed)
            {
                waitForAnyKey = false;
                SceneManager.LoadSceneAsync(jumpTarget);
            }
        }

        public void Active()
        {
            uiImage.enabled = true;
            Global.PlaySound(jingle);
            StartCoroutine(RestartGame());
        }

        private IEnumerator RestartGame()
        {
            yield return new WaitForSecondsRealtime(jingle.length);
            waitForAnyKey = true;
        }
    }
}
