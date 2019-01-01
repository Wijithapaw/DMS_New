using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Domain.Entities.Identity
{
    public class Role : IdentityRole<int>
    {
        public Role() : base()
        {
        }

        public Role(string roleName)
        {
            Name = roleName;
        }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int LastUpdatedBy { get; set; }

        public DateTime LastUpdatedDate { get; set; }
    }
}
