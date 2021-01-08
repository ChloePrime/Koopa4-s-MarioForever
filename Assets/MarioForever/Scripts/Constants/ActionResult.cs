using System.Runtime.CompilerServices;

namespace SweetMoleHouse.MarioForever.Constants
{
    public enum ActionResult
    {
        /// <summary>
        /// 强制通过
        /// </summary>
        ACCEPT,
        /// <summary>
        /// 交给默认逻辑处理
        /// </summary>
        PASS,
        /// <summary>
        /// 取消默认结果
        /// </summary>
        CANCEL
    }

    public static class ActionResultOp
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsCanceled(this ActionResult actionResult)
        {
            return actionResult == ActionResult.CANCEL;
        }
    }
}