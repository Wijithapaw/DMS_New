using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Domain.ConfigSettings
{
    public class JwtSettings
    {
        public string SigningKey { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }
    }
}
