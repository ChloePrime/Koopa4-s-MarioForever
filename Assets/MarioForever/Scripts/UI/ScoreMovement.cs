using SweetMoleHouse.MarioForever.Persistent;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.UI
{
    public class ScoreMovement : MonoBehaviour
    {
        [SerializeField] private float moveDistance = 2F;
        [SerializeField] private float moveSpeed = 1.76F;
        [SerializeField] private float stayTime = 2.4F;

        private void Update()
        {
            Move();
            stayTime -= Time.deltaTime;
            if (stayTime <= 0)
            {
                Destroy(gameObject);
            }
        }

        private void Move()
        {
            var offset = moveSpeed * Time.deltaTime;
            if (offset > moveDistance)
            {
                offset = moveDistance;
            }
            transform.Translate(0, offset, 0);
            moveDistance -= offset;
        }
    }
}
