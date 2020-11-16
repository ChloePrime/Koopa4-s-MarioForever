using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Constants
{
    /// <summary>
    /// 全局常量
    /// 非专业用户不应该修改这些值
    /// </summary>
    public class Consts
    {
        public static int PIXEL_PER_UNIT { get; } = 32;
        public static float ONE_PIXEL { get; } = 1f / PIXEL_PER_UNIT;
        public static float PIXEL_EPSLION { get; } = ONE_PIXEL / 16;
        public static float FPS = 50;
        /// <summary>
        /// 像素/帧 -> 格/帧
        /// </summary>
        public static float LENGTH_UNIT_SCALE
        {
            get => ONE_PIXEL * FPS * Time.deltaTime;
        }
        public static string LAYER_ALL_MOVEABLE { get; } = "所有移动物品";
        public static string LAYER_DMG_DETECTOR { get; } = "伤害判定";
        public static string TAG_PLAYER { get; } = "Player";
    }
}