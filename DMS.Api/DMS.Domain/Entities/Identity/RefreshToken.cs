using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DMS.Domain.Entities.Identity
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Token { get; set; }
        [Required]
        public string AuthToken { get; set; }
    }
}
