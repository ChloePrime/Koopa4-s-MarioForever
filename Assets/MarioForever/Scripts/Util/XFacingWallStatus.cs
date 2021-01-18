using System;

namespace SweetMoleHouse.MarioForever.Scripts.Util
{
    public enum XFacingWallStatus
    {
        LEFT = -1,
        NONE = 0,
        RIGHT = 1
    }
    
    public static class XFacingWallStatusOp
    {
        public static bool IsBlocked(this XFacingWallStatus thiz, float xSpeed)
        {
            return Math.Sign(xSpeed) == (int) thiz;
        }
    }

    public static class XFacingWallStatusFactory
    {
        public static XFacingWallStatus FromSpeed(in float speed)
        {
            return (XFacingWallStatus) Math.Sign(speed);
        }
    }
}