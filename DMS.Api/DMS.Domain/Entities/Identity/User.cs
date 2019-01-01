using DMS.Domain.Entities.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DMS.Domain.Entities.Identity
{
    public class User : IdentityUser<int>, IAuditedEntity
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        public DateTime Birthday { get; set; }

        [Required]
        public bool Active { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDateUtc { get; set; }

        public int LastUpdatedBy { get; set; }

        public DateTime LastUpdatedDateUtc { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
