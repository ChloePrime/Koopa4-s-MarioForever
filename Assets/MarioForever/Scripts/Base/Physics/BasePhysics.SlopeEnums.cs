using System;

namespace SweetMoleHouse.MarioForever.Scripts.Base.Physics {
public partial class BasePhysics {
    public enum SlopeState {
        FLAT = 0,
        UP = 1,
        DOWN = -1
    }

    [Flags]
    public enum SlopeFlags {
        /// <summary>
        /// 忽视下坡。
        /// 会把下坡当成正常的悬崖对待。
        /// </summary>
        IGNORE_UPSLOPE = 1 << 0,

        /// <summary>
        /// 忽视上坡，
        /// 会把上坡当成墙壁对待。
        /// </summary>
        IGNORE_DOWNSLOPE = 1 << 1,
        
        /// <summary>
        /// 防止 Unity Inspector 中对此 enum 勾选 Everything 用的，
        /// 勿删
        /// </summary>
        UNUSED_DUMMY = 1 << 2
    }
}
}