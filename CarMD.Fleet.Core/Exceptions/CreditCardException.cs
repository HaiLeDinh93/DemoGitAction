using System;

namespace CarMD.Fleet.Core.Exceptions
{
    [Serializable]
    public class CreditCardException : Exception
    {
        public CreditCardException(CreditCardExceptionType exceptionType)
            : base(exceptionType.ToString())
        {
        }

        public CreditCardException(string message)
            : base(message)
        {
        }

        public CreditCardException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    public enum CreditCardExceptionType
    {
        InvalidCardInformation,
        InvalidExpirationDate,
        InvalidCardHolderName
    }
}