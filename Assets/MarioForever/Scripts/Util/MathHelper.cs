using System.Diagnostics.Contracts;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Util {
public static class MathHelper {
    
    [Pure]
    public static Vector2 AngleToDirection(float angle) {
        angle += 45;
        angle %= 360;
        return angle switch {
            <= 90 => Vector2.right,
            <= 180 => Vector2.up,
            <= 270 => Vector2.left,
            _ => Vector2.down
        };
    }

    [Pure]
    public static Vector2 GetAxis(in Vector2 vec) {
        return AngleToDirection(Vector2.Angle(Vector2.right, vec));
    }
}
}