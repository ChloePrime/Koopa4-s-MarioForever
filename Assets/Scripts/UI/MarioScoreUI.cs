using System;
using SweetMoleHouse.MarioForever.Persistent;

namespace SweetMoleHouse.MarioForever.UI
{
    public class MarioScoreUI : Counter
    {
        private void FixedUpdate()
        {
            Value = MarioProperty.Score;
        }
    }
}
