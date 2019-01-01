using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Domain.Dtos.Account
{
    public class AuthResultDto
    {
        public bool Succeeded { get; set; }
        public string ErrorCode { get; set; }
        public string  AuthToken { get; set; }
        public DateTime Expiration { get; set; }
        public string RefreshToken { get; set; }
    }
}
