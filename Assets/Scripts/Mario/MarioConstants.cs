namespace SweetMoleHouse.MarioForever.Player
{
    public enum MarioState
    {
        STAND,
        WALK,
        JUMP,
        CLIMB,
        WALL_JUMP
    }
    public enum MarioHoldingState
    {
        NOTHING,
        OVER_HEAD,
        ON_FRONT,
        CANT_HOLD_STUFF
    }
    public enum MarioInvincibleState
    {
        NORMAL,
        INVINCIBLE,
        /// <summary>
        /// 过场动画的无敌状态
        /// </summary>
        PHASE_THROUGH
    }
    public enum MarioStompStyle
    {
        JUMP,
        SPIN_JUMP
    }
}