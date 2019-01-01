using DMS.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DMS.Domain.Entities.System
{
    public class Lookup : BaseEntity
    {
        public int Id { get; set; }
        [ForeignKey("Header")]
        public int HeaderId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Code { get; set; }
        [Required]
        [MaxLength(200)]
        public string Description { get; set; }
        public int SortOrder { get; set; }

        public virtual LookupHeader Header { get; set; }
    }
}
