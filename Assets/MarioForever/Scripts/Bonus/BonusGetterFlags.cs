using System;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Bonus {
[Flags]
public enum EnumBonusGetterFlags {
    CanEatCoin = 1 << 0,
    CanBumpHiddenBlock = 1 << 1,
    MaxValue = 1 << 31
}

public static class BonusGetterFlagsOp {
    public static bool HasFlag(this GameObject obj, EnumBonusGetterFlags flag) {
        return obj.TryGetComponent(out BonusGetterFlags flags) && (flags.Value & flag) > 0;
    }
    
    public static bool HasFlag(this Component com, EnumBonusGetterFlags flag) {
        return com.TryGetComponent(out BonusGetterFlags flags) && (flags.Value & flag) > 0;
    }
}

public class BonusGetterFlags : MonoBehaviour {
    public EnumBonusGetterFlags Value => flags;
    
    [SerializeField] private EnumBonusGetterFlags flags;
}
}
