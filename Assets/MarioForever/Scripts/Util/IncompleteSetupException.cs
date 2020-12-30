using System;

namespace SweetMoleHouse.MarioForever.Util
{
    public class IncompleteSetupException : Exception
    {
        public IncompleteSetupException(string message) : base(message) {}
    }
}
