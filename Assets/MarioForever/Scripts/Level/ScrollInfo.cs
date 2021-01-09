using SweetMoleHouse.MarioForever.Scripts.Base;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Level
{
    public class ScrollInfo : Singleton<ScrollInfo>
    {
        public static Vector2 Center { get; private set; }
        public static float Width { get; private set; }
        public static float Height { get; private set; }
        public static float Left => Center.x - Width / 2;
        public static float Right => Center.x + Width / 2;
        public static float Top => Center.y + Height / 2;
        public static float Bottom => Center.y - Height / 2;
        
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

    public static class ScrollHelper
    {
        public static Vector2 OffsetOutOfScreen(this Transform transform)
        {
            Vector2 pos = transform.position;
            var screenCenter = ScrollInfo.Center;
            var halfWidth = new Vector2(ScrollInfo.Width / 2F, ScrollInfo.Height / 2F);
            
            var result = pos - screenCenter;
            result.x = Mathf.Abs(result.x);
            result.y = Mathf.Abs(result.y);
            result -= halfWidth;
            return result;
        }

        public static float DistanceOutOfScreen(this Transform transform)
        {
            var pos = transform.OffsetOutOfScreen();
            return Mathf.Max(pos.x, pos.y);
        }
    }
}
