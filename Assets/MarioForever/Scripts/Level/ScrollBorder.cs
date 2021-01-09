using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Level
{
    /// <summary>
    /// 
    /// </summary>
    public class ScrollBorder : MonoBehaviour
    {
        private Vector2 min;
        private Vector2 max;
        public void Start()
        {
            var box = GetComponent<Collider2D>();
            var bounds = box.bounds;
            min = bounds.min;
            max = bounds.max;
        }

        /// <summary>
        /// 由camera手动刷新
        /// </summary>
        public void Tick(Camera camera)
        {
            Vector3 pos = camera.transform.position;
            float camWidth = ScrollInfo.Width / 2;
            float camHeight = ScrollInfo.Height / 2;
            pos.x = Mathf.Clamp(pos.x, min.x + camWidth, max.x - camWidth);
            pos.y = Mathf.Clamp(pos.y, min.y + camHeight, max.y - camHeight);
            camera.transform.position = pos;
        }
    }
}