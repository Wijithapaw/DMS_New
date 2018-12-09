using System;

namespace DMS.Domain.Dtos.User
{
    public class UserLDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime Birthday { get; set; }

        public bool Active { get; set; }

        public string[] Roles { get; set; }

        public string[] PermissionClaims { get; set; }
    }
}
