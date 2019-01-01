using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Domain.Dtos.Base
{
    public class BaseDto
    {
        public int Id { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
