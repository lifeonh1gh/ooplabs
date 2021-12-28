using System;
using System.Globalization;

namespace Reports.Helpers
{
    public class ReportsException : Exception
    {
        public ReportsException() : base()
        {
        }

        public ReportsException(string message) : base(message)
        {
        }

        public ReportsException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}