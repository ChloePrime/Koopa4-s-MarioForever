using SweetMoleHouse.MarioForever.Scripts.Persistent;
using SweetMoleHouse.MarioForever.Scripts.UI;

namespace SweetMoleHouse {
public class MarioCoinsUI : Counter {
    private void FixedUpdate() {
        Value = MarioProperty.Coins;
    }
}
}
