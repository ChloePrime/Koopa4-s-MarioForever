using SweetMoleHouse.MarioForever.Scripts.Base.Physics;
using SweetMoleHouse.MarioForever.Scripts.Constants;
using SweetMoleHouse.MarioForever.Scripts.Player;
using SweetMoleHouse.MarioForever.Scripts.Util;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Bonus {
/// <summary>
/// 隐藏块
/// </summary>
public class HiddenBlock : MonoBehaviour {
    private const float MarioBumpTolerance = 0.5F;
    [SerializeField] private GameObject bonus;

    private void Awake() {
        _collider = GetComponent<Collider2D>();

        if (TryGetComponent(out SpriteRenderer sr)) {
            sr.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Component hisHost = other.GetHost();
        bool bBumped = false;
        
        if (hisHost.TryGetComponent(out Mario mario)) {
            TryBumpedByMario(mario, out bBumped);
        } else if (hisHost.HasFlag(EnumBonusGetterFlags.CanBumpHiddenBlock)) {
            BumpedByObject(hisHost.transform, out bBumped);
        }
        
        if (bBumped) {
            DoBump(hisHost.transform);
        }
    }

    private void TryBumpedByMario(Mario mario, out bool bBumped) {
        float myBottom = _collider.bounds.min.y;
        float hisTop = MFUtil.YTop(mario.Mover.R2d);

        // 马里奥下落时碰到隐藏块，直接返回
        if (mario.Mover.YSpeed <= 0) {
            bBumped = false;
            return;
        }

        float heightDifference = hisTop - myBottom;
        if (heightDifference > MarioBumpTolerance) {
            bBumped = false;
            return;
        }

        mario.Mover.TeleportBy(0, -heightDifference - Consts.OnePixel);
        mario.Mover.HitWallY(new[] { _collider });
        bBumped = true;
    }

    /// <summary>
    /// 把顶起砖块的实体弹出去
    /// </summary>
    /// <param name="obj">顶到这块砖的实体</param>
    /// <param name="bBumped">将被赋值为 true</param>
    private void BumpedByObject(Transform obj, out bool bBumped) {
        bBumped = true;

        ColliderDistance2D distance;
        if (obj.TryGetComponent(out Rigidbody2D rigid)) {
            distance = rigid.Distance(_collider);
        } else if (obj.TryGetComponent(out Collider2D c2d)) {
            distance = c2d.Distance(_collider);
        } else {
            return;
        }

        Vector3 offset = distance.normal * (distance.distance + Consts.OnePixel);
        obj.position += offset;
    }

    private void DoBump(Transform cause) {
        Transform myTransform = transform;
        // 生成问号块
        GameObject bumped = Instantiate(bonus, myTransform.position, Quaternion.identity, myTransform.parent);
        if (bumped.TryGetComponent(out IHittable hittable)) {
            hittable.OnHit(cause);
        }

        // 销毁自己
        Destroy(gameObject);
    }

    private Collider2D _collider;
}
}
