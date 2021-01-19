using SweetMoleHouse.MarioForever.Scripts.Base;
using SweetMoleHouse.MarioForever.Scripts.Util;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace SweetMoleHouse.MarioForever.Scripts.Facility
{
    /// <summary>
    /// 将基于 Tilemap 的平台的 Tilemap 分为两份
    /// 一份用于渲染，另一份用于碰撞
    /// </summary>
    public class TilemapPlatformIniter : MonoBehaviour
    {
        private void Start()
        {
            var plat = GetComponent<MovingObstacle>();
            var colliderGrid = this.DfsComponentInChildren<Grid>();
            
            var displayGridGo = Instantiate(colliderGrid.gameObject, transform.root);

            Destroy(colliderGrid.GetComponentInChildren<TilemapRenderer>());
            Destroy(displayGridGo.GetComponentInChildren<TilemapCollider2D>());
            Destroy(displayGridGo.GetComponentInChildren<CompositeCollider2D>());
            Destroy(displayGridGo.GetComponentInChildren<Rigidbody2D>());

            var colliderGridTransform = colliderGrid.transform;
            displayGridGo.transform.parent = colliderGridTransform.parent;
            displayGridGo.transform.localPosition = colliderGridTransform.localPosition;
            plat.SetDisplay(displayGridGo.transform);
        }
    }
}
