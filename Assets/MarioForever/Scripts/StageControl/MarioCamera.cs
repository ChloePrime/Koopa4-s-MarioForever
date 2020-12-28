using Cinemachine;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.StageControl
{
    /// <summary>
    /// 需要GameObject上附带Camera组件
    /// </summary>
    public class MarioCamera : MonoBehaviour
    {
        [SerializeField, RenameInInspector("滚屏界限")]
        private ScrollBorder activeBorder;

        private Camera cam;
        private CinemachineBrain actualCam;

        private void Start()
        {
            cam = GetComponent<Camera>();
            actualCam = GetComponent<CinemachineBrain>();
        }

        private void LateUpdate()
        {
            if (actualCam.isActiveAndEnabled)
            {
                actualCam.ManualUpdate();
            }
            if (activeBorder != null)
            {
                activeBorder.Tick(cam);
            }
        }

        public void Stop()
        {
            actualCam.enabled = false;
        }
    }
}