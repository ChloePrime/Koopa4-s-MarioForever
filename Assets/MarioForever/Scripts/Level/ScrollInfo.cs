using System;
using SweetMoleHouse.MarioForever.Scripts.Base;
using UnityEngine;
using UnityEngine.U2D;

namespace SweetMoleHouse.MarioForever.Scripts.Level {
public class ScrollInfo : Singleton<ScrollInfo> {
    public static Vector2 Center { get; private set; }
    public static float Width { get; private set; }
    public static float Height { get; private set; }
    public static float Left => Center.x - Width / 2;
    public static float Right => Center.x + Width / 2;
    public static float Top => Center.y + Height / 2;
    public static float Bottom => Center.y - Height / 2;

    private Camera main;
    private PixelPerfectCamera ppc;
    private bool hasPpc;

    private void Awake() {
        main = Camera.main;
        if (main != null) {
            hasPpc = main.TryGetComponent(out ppc);
        }

        ComputeScrollData();
    }

    private void FixedUpdate() {
        ComputeScrollData();
    }

    private void ComputeScrollData() {
        if (main == null) return;
        Height = main.orthographicSize * 2;
        Width = Height * GetScreenRatio();
        Center = main.transform.position;
    }

    private float GetScreenRatio() {
        var rect = main.rect;
        if (hasPpc) {
            return ppc.refResolutionX / (float)ppc.refResolutionY;
        }

        // 使用 PixelPerfectCamera 以后 Viewport 的矩形的长宽比会反过来
        return rect.width / rect.height;
    }
}

public static class ScrollHelper {
    public static Vector2 OffsetOutOfScreen(in Vector2 pos) {
        Vector2 screenCenter = ScrollInfo.Center;
        Vector2 halfWidth = new(ScrollInfo.Width / 2F, ScrollInfo.Height / 2F);

        Vector2 result = pos - screenCenter;
        result.x = MathF.Abs(result.x);
        result.y = MathF.Abs(result.y);
        result -= halfWidth;
        return result;
    }

    public static float DistanceOutOfScreen(in Vector2 pos) {
        Vector2 oos = OffsetOutOfScreen(pos);
        return Mathf.Max(oos.x, oos.y);
    }

    public static Vector2 OffsetOutOfScreen(this Transform transform) {
        return OffsetOutOfScreen(transform.position);
    }

    public static float DistanceOutOfScreen(this Transform transform) {
        return DistanceOutOfScreen(transform.position);
    }
}
}
