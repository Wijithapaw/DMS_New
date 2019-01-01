using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Domain.Dtos.Account
{
    public class LoginCredentialsDto
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
