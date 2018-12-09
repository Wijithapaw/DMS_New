using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Domain.Dtos.User
{
    public class UserSDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public bool Active { get; set; }
    }
}
