using SweetMoleHouse.MarioForever.Persistent;

namespace SweetMoleHouse.MarioForever.UI
{
    public class MarioLives : TMPCounter
    {
        private void FixedUpdate()
        {
            Value = MarioProperty.Lives;
        }
    }
}
