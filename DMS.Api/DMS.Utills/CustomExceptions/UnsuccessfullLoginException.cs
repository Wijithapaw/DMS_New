using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Utills.CustomExceptions
{
    public class UnsuccessfulLoginException : DMSException
    {
        public UnsuccessfulLoginException() : base("Invalid username or password") { }
    }
}
