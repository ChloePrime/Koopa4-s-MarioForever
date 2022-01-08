using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Effect {
public class PlaySoundOnStateEnter : StateMachineBehaviour {
    [SerializeField] private AudioClip sound;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Global.PlaySound(sound);
    }
}
}
