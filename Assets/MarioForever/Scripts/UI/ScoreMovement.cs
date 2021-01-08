using UnityEngine;

namespace SweetMoleHouse.MarioForever.UI
{
    public class ScoreMovement : MonoBehaviour
    {
        [SerializeField] private float moveDistance = 2F;
        [SerializeField] private float moveSpeed = 1.76F;

        private void Update()
        {
            Move();
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
