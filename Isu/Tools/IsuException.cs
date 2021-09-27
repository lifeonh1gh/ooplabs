using System;

namespace Isu.Tools
{
    public class IsuException : Exception
    {
        private string messageDetails = string.Empty;

        public IsuException()
        {
        }

        public IsuException(string message)
            : base(message)
        {
        }

        public IsuException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public override string Message => $"Error Message: {messageDetails}";
    }
}