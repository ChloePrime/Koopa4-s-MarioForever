using System.Collections.Generic;
using SweetMoleHouse.MarioForever.Scripts.Constants;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Util
{
    /// <summary>
    /// 为unity带来一些MMF的功能
    /// 比如物体的边界等
    /// </summary>
    public static class MFUtil
    {
        public static float XLeft(GameObject obj) => XLeft(obj.GetComponent<Rigidbody2D>());
        public static float XLeft(Component component) => XLeft(component.GetComponent<Rigidbody2D>());
        public static float XLeft(Rigidbody2D r2d)
        {
            float result = float.MaxValue;
            foreach (var collider in GetColliders(r2d))
            {
                var xLeft = collider.bounds.min.x;
                if (xLeft < result)
                {
                    result = xLeft;
                }
            }
            return result;
        }
        /// <summary>
        /// 检测某个方向（左侧）是否撞到东西
        /// </summary>
        /// <param name="aggresive">伸出去1px还是缩进去1px</param>
        /// <returns></returns>
        public static bool XLeftCast(Component objectRef, bool aggresive = true, ContactFilter2D? filter = null)
            => XLeftCast(objectRef, out RaycastHit2D? _, aggresive, filter);
        public static bool XLeftCast(Component objectRef, out RaycastHit2D? result, bool aggresive = true, ContactFilter2D? filter = null)
        {
            var origin = new Vector2(XLeft(objectRef) - GetDelta(aggresive), YCenter(objectRef));
            var size = new Vector2(Consts.OnePixel, Height(objectRef));
            return CastCulled(objectRef, origin, size, out result, filter);
        }

        public static float XRight(GameObject obj) => XRight(obj.GetComponent<Rigidbody2D>());
        public static float XRight(Component component) => XRight(component.GetComponent<Rigidbody2D>());
        public static float XRight(Rigidbody2D r2d)
        {
            float result = -float.MaxValue;
            foreach (var collider in GetColliders(r2d))
            {
                var xRight = collider.bounds.max.x;
                if (xRight > result)
                {
                    result = xRight;
                }
            }
            return result;
        }
        public static bool XRightCast(Component objectRef, bool aggresive = true, ContactFilter2D? filter = null)
            => XRightCast(objectRef, out RaycastHit2D? _, aggresive, filter);
        public static bool XRightCast(Component objectRef, out RaycastHit2D? result, bool aggresive = true, ContactFilter2D? filter = null)
        {
            var origin = new Vector2(XRight(objectRef) + GetDelta(aggresive), YCenter(objectRef));
            var size = new Vector2(Consts.OnePixel, Height(objectRef));
            return CastCulled(objectRef, origin, size, out result, filter);
        }
        public static float XCenter(GameObject obj) => XCenter(obj.GetComponent<Rigidbody2D>());
        public static float XCenter(Component component) => XCenter(component.GetComponent<Rigidbody2D>());
        public static float XCenter(Rigidbody2D r2d) => (XLeft(r2d) + XRight(r2d)) / 2;
        public static float Width(GameObject obj) => Width(obj.GetComponent<Rigidbody2D>());
        public static float Width(Component component) => Width(component.GetComponent<Rigidbody2D>());
        public static float Width(Rigidbody2D r2d) => XRight(r2d) - XLeft(r2d);

        public static float YTop(GameObject obj) => YTop(obj.GetComponent<Rigidbody2D>());
        public static float YTop(Component component) => YTop(component.GetComponent<Rigidbody2D>());
        public static float YTop(Rigidbody2D r2d)
        {
            float result = -float.MaxValue;
            foreach (var collider in GetColliders(r2d))
            {
                var yTop = collider.bounds.max.y;
                if (yTop > result)
                {
                    result = yTop;
                }
            }
            return result;
        }
        public static bool YTopCast(Component objectRef, bool aggresive = true, ContactFilter2D? filter = null)
            => YTopCast(objectRef, out RaycastHit2D? _, aggresive, filter);
        public static bool YTopCast(Component objectRef, out RaycastHit2D? result, bool aggresive = true, ContactFilter2D? filter = null)
        {
            var origin = new Vector2(XCenter(objectRef), YTop(objectRef) + GetDelta(aggresive));
            var size = new Vector2(Width(objectRef), Consts.OnePixel);
            return CastCulled(objectRef, origin, size, out result, filter);
        }
        public static float YBottom(GameObject obj) => YBottom(obj.GetComponent<Rigidbody2D>());
        public static float YBottom(Component component) => YBottom(component.GetComponent<Rigidbody2D>());
        public static float YBottom(Rigidbody2D r2d)
        {
            float result = float.MaxValue;
            foreach (var collider in GetColliders(r2d))
            {
                var yBottom = collider.bounds.min.y;
                if (yBottom < result)
                {
                    result = yBottom;
                }
            }
            return result;
        }
        public static bool YBottomCast(Component objectRef, bool aggresive = true, ContactFilter2D? filter = null)
            => YBottomCast(objectRef, out RaycastHit2D? _, aggresive, filter);
        public static bool YBottomCast(Component objectRef, out RaycastHit2D? result, bool aggresive = true, ContactFilter2D? filter = null)
        {
            var origin = new Vector2(XCenter(objectRef), YBottom(objectRef) - GetDelta(aggresive));
            var size = new Vector2(Width(objectRef), Consts.OnePixel);
            return CastCulled(objectRef, origin, size, out result, filter);
        }
        public static float YCenter(GameObject obj) => YCenter(obj.GetComponent<Rigidbody2D>());
        public static float YCenter(Component component) => YCenter(component.GetComponent<Rigidbody2D>());
        public static float YCenter(Rigidbody2D r2d) => (YTop(r2d) + YBottom(r2d)) / 2;
        public static float Height(GameObject obj) => Height(obj.GetComponent<Rigidbody2D>());
        public static float Height(Component component) => Height(component.GetComponent<Rigidbody2D>());
        public static float Height(Rigidbody2D r2d) => YTop(r2d) - YBottom(r2d);

        private static readonly List<Collider2D> TEMP_LIST = new List<Collider2D>();
        private static List<Collider2D> GetColliders(Rigidbody2D r2d)
        {
            r2d.GetAttachedColliders(TEMP_LIST);
            return TEMP_LIST;
        }

        private static readonly RaycastHit2D[] CAST_RESULT_STORAGE = new RaycastHit2D[64];
        private static float GetDelta(bool isAggresive)
            => (isAggresive ? 1 : -1) * Consts.OnePixel / 2;
        private static readonly ContactFilter2D NO_FLITER = new ContactFilter2D().NoFilter();
        private static bool CastCulled(Component obj, Vector2 origin, Vector2 size, out RaycastHit2D? outputInfo, ContactFilter2D? filter)
        {
            Rigidbody2D r2d = obj is Rigidbody2D r2d0 ? r2d0 : obj.GetComponent<Rigidbody2D>();
            var resultNum = filter.HasValue
                    ? Physics2D.BoxCast(origin, size, 0, Vector2.zero, filter.Value, CAST_RESULT_STORAGE, 0)
                    : Physics2D.BoxCast(origin, size, 0, Vector2.zero, NO_FLITER, CAST_RESULT_STORAGE, 0);
            var selfColliders = GetColliders(r2d);
            for (int i = 0; i < resultNum; i++)
            {
                var result = CAST_RESULT_STORAGE[i];
                if (selfColliders.Contains(result.collider)) continue;
                outputInfo = result;
                return true;
            }
            outputInfo = null;
            return false;
        }
    }
}