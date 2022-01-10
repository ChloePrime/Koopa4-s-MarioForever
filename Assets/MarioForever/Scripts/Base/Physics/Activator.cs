using Cysharp.Threading.Tasks;
using SweetMoleHouse.MarioForever.Scripts.Level;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Base.Physics {
/// <summary>
/// 禁用实体直到实体进入屏幕内。
/// </summary>
public class Activator : MonoBehaviour {
    [SerializeField] private float size = 1;
    
    private BasePhysics physics;
    private GameObject myGameObject;

    private void Awake() {
        physics = GetComponent<BasePhysics>();
        myGameObject = gameObject;
    }

    private async void Start() {
        await UniTask.NextFrame(PlayerLoopTiming.FixedUpdate);
        if (this == null) {
            return;
        }
        
        Vector2 myPos = transform.position;
        // 如果此 object 一开始就放在视野中
        if (ShouldActivate(myPos)) {
            return;
        }

        myGameObject.SetActive(false);
        WaitForActivate(myPos);
    }
    
    private void _Awake() {
        if (myGameObject != null) {
            myGameObject.SetActive(true);
        }

        if (physics != null) {
            physics.SetDirection(ScrollInfo.Center.x - transform.position.x);
        }
    }

    private async void WaitForActivate(Vector2 myPos) {
        while (true) {
            await UniTask.NextFrame(PlayerLoopTiming.FixedUpdate);
            if (ShouldActivate(myPos)) {
                break;
            }
        }

        _Awake();
    }

    private bool ShouldActivate(in Vector2 myPos) {
        return ScrollHelper.DistanceOutOfScreen(myPos) <= size;
    }
}
}
