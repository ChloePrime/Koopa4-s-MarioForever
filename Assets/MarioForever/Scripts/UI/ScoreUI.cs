using SweetMoleHouse.MarioForever.Scripts.Persistent;

namespace SweetMoleHouse.MarioForever.Scripts.UI
{
    public class ScoreUI : Counter
    {
        private void FixedUpdate()
        {
            Value = MarioProperty.Score;
        }
    }
}
