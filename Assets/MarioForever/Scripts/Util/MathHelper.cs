using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Util
{
    public static class MathHelper 
    {
        public static Vector2 AngleToDirection(float angle)
        {
            angle += 45;
            angle %= 360;
            Vector2 result;
            if (angle <= 90)
            {
                result = Vector2.right;
            }
            else if (angle <= 180)
            {
                result = Vector2.up;
            }
            else if (angle <= 270)
            {
                result = Vector2.left;
            }
            else
            {
                result = Vector2.down;
            }
            return result;
        }

        public static Vector2 GetAxis(in Vector2 vec)
        {
            return AngleToDirection(Vector2.Angle(Vector2.right, vec));
        }
    }
}