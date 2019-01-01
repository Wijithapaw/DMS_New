using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Domain.Dtos.Account
{
    public class RefreshTokenDto
    {
        public string AuthToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
