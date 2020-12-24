using SweetMoleHouse.MarioForever.Persistent;

namespace SweetMoleHouse.MarioForever.UI
{
    public class MarioLifeUI : Counter
    {
        private void FixedUpdate()
        {
            Value = MarioProperty.Lives;
        }
    }
}
