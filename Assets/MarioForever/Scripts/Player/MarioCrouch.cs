using SweetMoleHouse.MarioForever.Scripts.Base.Physics;
using SweetMoleHouse.MarioForever.Scripts.Constants;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace SweetMoleHouse.MarioForever.Scripts.Player {
/// <summary>
/// 马里奥下蹲
/// </summary>
public class MarioCrouch : MonoBehaviour {
    private Mario mario;
    private bool crouchInput;
    private bool crouching;

    public bool Crouching {
        get {
            // 杜绝任何时候发生小个子下蹲的情况
            // 解决了下蹲时受伤卡住的bug
            if (mario.GetRealSize() == MarioSize.SMALL) {
                Crouching = false;
            }

            return crouching;
        }
        set {
            crouching = value;
            mario.ControlDisabled = value;
            if (value) {
                mario.Size = MarioSize.SMALL;
            } else {
                mario.RefreshSize();
            }
        }
    }

    private void Start() {
        mario = GetComponent<Mario>();
        Global.Inputs.Mario.Down.performed += OnCrouch;
        Global.Inputs.Mario.Down.canceled += OnLeaveCrouch;
    }

    private void OnDisable() {
        Global.Inputs.Mario.Down.performed -= OnCrouch;
        Global.Inputs.Mario.Down.canceled -= OnLeaveCrouch;
    }

    private void OnCrouch(CallbackContext ctx) => crouchInput = true;
    private void OnLeaveCrouch(CallbackContext ctx) => crouchInput = false;

    private void FixedUpdate() {
        //小个子没有下蹲操作
        if (mario.GetRealSize() == MarioSize.SMALL) {
            if (Crouching) {
                Crouching = false;
            }

            return;
        }

        if (crouchInput && !Crouching) {
            Crouching = true;
        } else if (Crouching && !crouchInput) {
            TryCancelCrouch();
        }

        bool shouldDisableXMove = Crouching && mario.Mover.IsOnGround;
        mario.ControlDisabled = shouldDisableXMove;
    }

    private void TryCancelCrouch() {
        bool touchedTop = mario.Mover.Cast(Vector2.up, BasePhysics.GlobalFilter, Global.RcastTempArray,
            Mario.DeltaSizeSmallToBig) > 0;
        if (!touchedTop) {
            Crouching = false;
        }
    }
}
}