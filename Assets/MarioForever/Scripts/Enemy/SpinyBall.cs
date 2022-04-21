using Cysharp.Threading.Tasks;
using SweetMoleHouse.MarioForever.Scripts.Base.Physics;
using SweetMoleHouse.MarioForever.Scripts.Player;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Enemy {
[RequireComponent(typeof(BasePhysics))]
public class SpinyBall : MonoBehaviour {
    [SerializeField] private GameObject spiny;
    
    private void Awake() {
        GetComponent<BasePhysics>().OnHitWallY += (_ => SpawnSpiny());
    }

    private void SpawnSpiny() {
        var myTransform = transform;
        GameObject spinyInstance = Instantiate(spiny, myTransform.position, myTransform.rotation, myTransform.parent);
        
        if (spinyInstance.TryGetComponent(out BasePhysics p)) {
            Mario mario = FindObjectOfType<Mario>();
            if (mario != null) {
                SetDirectionLaterAsync(p, mario.transform.position.x - myTransform.position.x);
            }
        }
        Destroy(gameObject);
    }

    private static async void SetDirectionLaterAsync(BasePhysics p, float dir) {
        await UniTask.Yield();
        if (p != null) {
            p.SetDirection(dir);
        }
    }
}
}
