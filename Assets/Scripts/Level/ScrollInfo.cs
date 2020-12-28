using UnityEngine;

namespace SweetMoleHouse.MarioForever.Level
{
    public class ScrollInfo : MonoBehaviour
    {
        public Vector2 Center { get; private set; }
        public float Width { get; private set; }
        public float Height { get; private set; }
        public float Left => Center.x - Width / 2;
        public float Right => Center.x + Width / 2;
        public float Top => Center.y + Height / 2;
        public float Bottom => Center.y - Height / 2;
        
        private Camera main;
        private void Awake()
        {
            main = Camera.main;
            ComputeScrollData();
        }

        private void FixedUpdate()
        {
            ComputeScrollData();
        }

        private void ComputeScrollData()
        {
            if (main == null) return;
            Height = main.orthographicSize * 2;
            Width = Height * GetScreenRatio();
            Center = main.transform.position;
        }

        private float GetScreenRatio()
        {
            var rect = main.rect;
            return rect.width / rect.height;
        }
    }
}
