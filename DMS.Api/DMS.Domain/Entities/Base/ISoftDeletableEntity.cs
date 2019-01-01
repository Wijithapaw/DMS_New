using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Domain.Entities.Base
{
    public interface ISoftDeletableEntity
    {
        long Deleted { get; set; }
    }
}
