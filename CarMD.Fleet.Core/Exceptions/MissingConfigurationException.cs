using System;

namespace CarMD.Fleet.Core.Exceptions
{
    [Serializable]
    public class MissingConfigurationException : CarMDException
    {
        public MissingConfigurationException(string message) : base(message)
        {
        }

        public MissingConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}