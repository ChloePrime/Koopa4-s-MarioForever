using System;

namespace SweetMoleHouse.MarioForever.Scripts.Util
{
    public class IncompleteSetupException : Exception
    {
        public IncompleteSetupException(string message) : base(message) {}
    }
}
