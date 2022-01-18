using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Constants
{
    /// <summary>
    /// 全局常量
    /// 非专业用户不应该修改这些值
    /// </summary>
    public static class Consts
    {
        public const int PixelPerUnit = 32;
        public const float OnePixel = 1f / PixelPerUnit;
        public const float PixelEpsilon = OnePixel / 16;
        public const float FPS = 50;
        /// <summary>
        /// 像素/帧 -> 格/帧
        /// </summary>
        public static float LENGTH_UNIT_SCALE => OnePixel * FPS * Time.deltaTime;
    }
    
    /// <summary>
    /// 逻辑图层的名称
    /// </summary>
    public static class LayerNames
    {
        public static string AllMovable => "Physics";
        public static string DmgDetector => "Murderers";
        public static string IgnoreRaycast => "IgnoreRaycast";
        public static string PhysicsFX => "PhysicsFX";
        /// <summary>
        /// 互相之间不发生碰撞的物理特效
        /// </summary>
        public static string NoninterferencePhysicsFX => "NoninterferencePhysicsFX";
        public static string NoPhysics => "NoPhysics";
    }

    public static class Tags
    {
        public const string Player = "Player";
        public const string Platform = "Platform";
        public const string CoinEater = "CoinEater";
        public const string HiddenBlockHitter = "HiddenBlockHitter";
    }
}