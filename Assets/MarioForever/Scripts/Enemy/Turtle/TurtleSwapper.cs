using System;
using SweetMoleHouse.MarioForever.Scripts.Base.Physics;
using SweetMoleHouse.MarioForever.Scripts.Base.Rpg;
using SweetMoleHouse.MarioForever.Scripts.Constants;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Enemy.Turtle {
/// <summary>
/// 乌龟被踢后切换到其他形态（其他GameObject）
/// </summary>
public class TurtleSwapper : MonoBehaviour {
    /// <summary>
    /// 被打死后是否会正常产生分数
    /// </summary>
    [SerializeField] private bool hasScore = true;
    public event Action<Transform> OnTurtleSwap;

    private void Start() {
        damageReceiver = GetComponentInChildren<DamageReceiver>();
        damageReceiver.OnDeath += SwapOrDie;
    }

    private ActionResult SwapOrDie(DamageEvent damage) {
        // 非踩踏攻击
        if (!damage.Type.ContainsAny(EnumDamageType.STOMP, EnumDamageType.KICK_SHELL)) {
            return ActionResult.PASS;
        }

        OnTurtleSwap?.Invoke(damage.Source.Host);
        // 继承的死亡效果：播放音效，产生分数
        damageReceiver.PlayDeathSound(damage);
        if (hasScore) {
            damageReceiver.SummonScore(damage);
        }
        // 生成切换对象并销毁自身
        var myTransform = transform;
        float x = myTransform.position.x;
        Transform myParent = myTransform.parent;

        Destroy(gameObject);
        SwapTo(Instantiate(swapTarget, myParent), x - damage.Source.Host.position.x);
        // 取消默认死亡逻辑
        return ActionResult.CANCEL;

    }

    private void SwapTo(GameObject target, float walkDirection) {
        // 设置位置
        if (target.TryGetComponent(out BasePhysics targetPhysics)) {
            targetPhysics.TeleportTo(transform.position);
            // 设置位置为马里奥踢龟壳的方向
            targetPhysics.SetDirection(walkDirection);
        } else {
            target.transform.position = transform.position;
        }
    }

    [SerializeField] private GameObject swapTarget;
    private DamageReceiver damageReceiver;
}
}
