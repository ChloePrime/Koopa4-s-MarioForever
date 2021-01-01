using Cinemachine;
using SweetMoleHouse.MarioForever.Base;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Level
{
    /// <summary>
    /// 需要GameObject上附带Camera组件
    /// </summary>
    public class MarioCamera : Singleton<MarioCamera>
    {
        [SerializeField, RenameInInspector("滚屏界限")]
        private ScrollBorder activeBorder;

        private Camera cam;
        private CinemachineBrain actualCam;

        public static void SetScrollBorder(ScrollBorder border)
        {
            Instance.activeBorder = border;
        }

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