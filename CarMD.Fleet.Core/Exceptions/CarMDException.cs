using System;

namespace CarMD.Fleet.Core.Exceptions
{
    [Serializable]
    public class CarMDException : Exception
    {
        public CarMDException(string message) : base(message)
        {
        }

        public CarMDException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}