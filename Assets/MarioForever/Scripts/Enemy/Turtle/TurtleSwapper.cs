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
                // 播放音效
                damageReceiver.PlayDeathSound(damageType);
                // 生成切换对象并销毁自身
                var myTransform = transform;
                SwapTo(Instantiate(swapTarget, myTransform.parent), myTransform.position.x - damager.position.x);
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

            // 销毁自身
            Destroy(gameObject);
        }

        private async void DestroyTurtleAtNextFrame()
        {
            await UniTask.NextFrame(PlayerLoopTiming.FixedUpdate);
            Destroy(gameObject);
        }

        [SerializeField] private GameObject swapTarget;
        private DamageReceiver damageReceiver;
    }
}
