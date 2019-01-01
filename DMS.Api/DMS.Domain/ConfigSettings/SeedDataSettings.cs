using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Domain.ConfigSettings
{
    public class SeedDataSettings
    {
        public SuperAdmin SuperAdmin { get; set; }
    }
    public class SuperAdmin
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
    }
}
