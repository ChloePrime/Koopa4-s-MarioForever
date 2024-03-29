using Cysharp.Threading.Tasks;
using SweetMoleHouse.MarioForever.Scripts.Level;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Base.Physics {
/// <summary>
/// 禁用实体直到实体进入屏幕内。
/// </summary>
public class Activator : MonoBehaviour {
    [SerializeField] private float size = 1;

    private void Awake() {
        _physics = GetComponent<BasePhysics>();
        _myGameObject = gameObject;
    }

    private async void Start() {
        await UniTask.NextFrame(PlayerLoopTiming.FixedUpdate);
        if (this == null) {
            return;
        }
        
        Vector2 myPos = transform.position;
        // 如果此 object 一开始就放在视野中
        if (ShouldActivate(myPos)) {
            _Awake();
            return;
        }

        _myGameObject.SetActive(false);
        WaitForActivate(myPos);
    }
    
    private void _Awake() {
        if (_myGameObject != null) {
            _myGameObject.SetActive(true);
        }

        if (_physics != null) {
            _physics.SetDirection(ScrollInfo.Center.x - transform.position.x);
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
    
    private BasePhysics _physics;
    private GameObject _myGameObject;
}
}
