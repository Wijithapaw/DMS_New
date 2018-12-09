using System;
using System.ComponentModel.DataAnnotations;

namespace DMS.Data.Entities
{
    public class BaseEntity
    {
        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastUpdatedBy { get; set; }

        public DateTime? LastUpdatedDate { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
