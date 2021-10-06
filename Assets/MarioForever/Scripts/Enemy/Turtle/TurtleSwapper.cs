using System;
using Cysharp.Threading.Tasks;
using SweetMoleHouse.MarioForever.Scripts.Base.Physics;
using SweetMoleHouse.MarioForever.Scripts.Constants;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Enemy.Turtle
{
    /// <summary>
    /// 乌龟被踢后切换到其他形态（其他GameObject）
    /// </summary>
    public class TurtleSwapper : MonoBehaviour
    {
        public event Action<Transform> OnTurtleSwap;
        
        private void Start()
        {
            damageReceiver = GetComponentInChildren<DamageReceiver>();
            damageReceiver.OnDeath += SwapOrDie;
        }

        private ActionResult SwapOrDie(Transform damager, EnumDamageType damageType)
        {
            // 非踩踏攻击
            if (damageType.Contains(EnumDamageType.STOMP) || damageType.Contains(EnumDamageType.KICK_SHELL))
            {
                OnTurtleSwap?.Invoke(damager);
                // 播放音效
                damageReceiver.PlayDeathSound(damageType);
                // 生成切换对象并销毁自身
                var myTransform = transform;
                var x = myTransform.position.x;
                var myParent = myTransform.parent;
                
                Destroy(gameObject);
                SwapTo(Instantiate(swapTarget, myParent), x - damager.position.x);
                // 取消默认死亡逻辑
                return ActionResult.CANCEL;
            }

            return ActionResult.PASS;
        }

        private void SwapTo(GameObject target, float walkDirection)
        {
            // 设置位置
            if (target.TryGetComponent(out BasePhysics targetPhysics))
            {
                targetPhysics.TeleportTo(transform.position);
                // 设置位置为马里奥踢龟壳的方向
                targetPhysics.SetDirection(walkDirection);
            }
            else
            {
                target.transform.position = transform.position;
            }
        }

        [SerializeField] private GameObject swapTarget;
        private DamageReceiver damageReceiver;
    }
}
