using System;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Base.Physics {
/// <summary>
/// 悬崖边转头（红乌龟）
/// </summary>
public class TurnOnCliff : MonoBehaviour {
    [SerializeField] private new bool enabled;
    [SerializeField] private Collider2D left;
    [SerializeField] private Collider2D right;
    private Walk walk;

    public bool Enabled {
        get => enabled;
        set => enabled = value;
    }

    private void Awake() {
        if (!enabled) {
            // 关闭看悬崖碰撞以节约性能
            left.gameObject.SetActive(false);
            right.gameObject.SetActive(false);
        }
        
        walk = GetComponent<Walk>();
    }

    private void FixedUpdate() {
        if (!enabled || !walk.IsOnGround) {
            return;
        }
        
        Collider2D col;
        switch (Math.Sign(walk.Direction)) {
            case -1:
                col = left;
                break;
            case 0:
                return;
            case 1:
                col = right;
                break;
            default:
                throw new ArithmeticException($"Math.sign() returns error");
        }

        bool isDownSlope;
        float width;
        if (walk.CurSlopeState == BasePhysics.SlopeState.DOWN) {
            isDownSlope = true;
            width = right.transform.localPosition.x - left.transform.localPosition.x - 0.3F;
            
            col.transform.Translate(0, -width, 0);
        } else {
            isDownSlope = false;
            width = 0;
        }

        try {
            bool onGround = col.OverlapCollider(BasePhysics.GlobalFilter, BUFFER) > 0;
            if (onGround) {
                return;
            }
            
            walk.TurnRound();
        } finally {
            if (isDownSlope) {
                col.transform.Translate(0, width, 0);
            }
        }

    }

    private static readonly Collider2D[] BUFFER = new Collider2D[1];
}
}
