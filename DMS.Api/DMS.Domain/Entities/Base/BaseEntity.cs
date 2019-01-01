using System;
using System.ComponentModel.DataAnnotations;

namespace DMS.Domain.Entities.Base
{
    public class BaseEntity : IAuditedEntity, IConcurrencyHandledEntity, ISoftDeletableEntity
    {
        //public int Id { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public int LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDateUtc { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public long Deleted { get; set; }
    }
}
