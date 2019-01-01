using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DMS.Domain.Entities.Base
{
    public interface IConcurrencyHandledEntity
    {
        [Timestamp]
        byte[] RowVersion { get; set; }
    }
}
