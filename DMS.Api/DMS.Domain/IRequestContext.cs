using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Domain
{
    public interface IRequestContext
    {
        int UserId { get; set; }
    }
}
