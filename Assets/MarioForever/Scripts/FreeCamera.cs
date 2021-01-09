using UnityEngine;
using UnityEngine.InputSystem;

namespace SweetMoleHouse.MarioForever.Scripts
{
    /// <summary>
    /// 摄像机飞行
    /// 类似MW上帝模式下的8按钮
    /// </summary>
    public class FreeCamera : MonoBehaviour 
    {
        [SerializeField,
            Range(0, 2)]
        private float freeCameraSpeed = 0.25f;

        private Vector2 moveTarget = Vector2.zero;
        private Vector2 moveCur = Vector2.zero;

        private Vector2 MoveTargetScaled => moveTarget * freeCameraSpeed;

        private void Start()
        {
            InputAction moveActions = Global.Inputs.Debug.FreeCamera;
            moveActions.performed += (ctx => moveTarget = ctx.ReadValue<Vector2>());
            moveActions.canceled += (ctx => moveTarget = Vector2.zero);
        }

        private void Update()
        {
            moveCur += (MoveTargetScaled - moveCur) * 0.5f;
            //moveCur.add(MoveTarget.clone().subtract(moveCur).multiply(0.5f));
            transform.Translate(moveCur);
        }
    }
}