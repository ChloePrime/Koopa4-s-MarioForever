using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.StageControl
{
    /// <summary>
    /// 需要GameObject上附带Camera组件
    /// </summary>
    public class MarioCamera : MonoBehaviour
    {
        [SerializeField, RenameInInspector("滚屏界限")]
        private ScrollBorder activeBorder = null;

        private Camera cam;
        private CinemachineBrain actualCam;

        private void Start()
        {
            cam = GetComponent<Camera>();
            actualCam = GetComponent<CinemachineBrain>();
        }

        private void LateUpdate()
        {
            if (actualCam != null)
            {
                actualCam.ManualUpdate();
            }
            if (activeBorder != null)
            {
                activeBorder.Tick(cam);
            }
        }
    }
}