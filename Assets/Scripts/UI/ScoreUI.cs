using SweetMoleHouse.MarioForever.Persistent;

namespace SweetMoleHouse.MarioForever.UI
{
    public class ScoreUI : Counter
    {
        private void FixedUpdate()
        {
            Value = MarioProperty.Score;
        }
    }
}
