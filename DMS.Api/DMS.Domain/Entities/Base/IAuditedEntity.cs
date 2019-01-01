using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Domain.Entities.Base
{
    public interface IAuditedEntity
    {
        int CreatedBy { get; set; }

        DateTime CreatedDateUtc { get; set; }

        int LastUpdatedBy { get; set; }

        DateTime LastUpdatedDateUtc { get; set; }
    }
}
