using SweetMoleHouse.MarioForever.Scripts.Base;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Effect {
public class DestroySelfOnStateEnter : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        GameObject host = animator.TryGetComponent(out ISubObject subObject)
            ? subObject.Host.gameObject
            : animator.gameObject;
        Destroy(host);
    }
}
}
