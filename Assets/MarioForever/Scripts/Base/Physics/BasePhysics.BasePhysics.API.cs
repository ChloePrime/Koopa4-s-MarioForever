using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Base.Physics {
public partial class BasePhysics {
    
    public bool IsOnGround { get; private set; }

    public float XSpeed {
        get => vel.x;
        set => vel.x = value;
    }

    public float YSpeed {
        get => vel.y;
        set => vel.y = value;
    }

    public float Direction => XSpeed;

    public virtual float Gravity {
        get => gravity;
        set => gravity = value;
    }
    
    public bool IgnoreCollision {
        get => ignoreCollision;
        set => ignoreCollision = value;
    }
    
    public void TeleportTo(float x, float y) => TeleportTo(new Vector2(x, y));

    public void TeleportTo(Vector2 pos) {
        transform.position = tickStartPos = tickEndPos = pos;
    }

    public void TeleportBy(float x, float y) => TeleportBy(new Vector2(x, y));

    public void TeleportBy(Vector2 offset) {
        var pos = (Vector2)transform.position + offset;
        TeleportTo(pos);
    }
}
}