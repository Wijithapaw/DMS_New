using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Domain.CustomExceptions
{
    public class DMSException : Exception
    {
        public DMSException() : base("Unknown error occurred") { }

        public DMSException(string message) : base(message) { }

        public DMSException(string message, Exception innerException) : base(message, innerException) { }
    }
}
