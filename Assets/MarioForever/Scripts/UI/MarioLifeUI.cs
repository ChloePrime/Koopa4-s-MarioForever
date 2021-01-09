using SweetMoleHouse.MarioForever.Scripts.Persistent;

namespace SweetMoleHouse.MarioForever.Scripts.UI
{
    public class MarioLifeUI : Counter
    {
        private void FixedUpdate()
        {
            Value = MarioProperty.Lives;
        }
    }
}
