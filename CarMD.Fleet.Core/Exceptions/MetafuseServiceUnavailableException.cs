using System;

namespace CarMD.Fleet.Core.Exceptions
{
    public class MetafuseServiceUnavailableException : CarMDException
    {
        public MetafuseServiceUnavailableException(string message) : base(message)
        {
        }

        public MetafuseServiceUnavailableException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}