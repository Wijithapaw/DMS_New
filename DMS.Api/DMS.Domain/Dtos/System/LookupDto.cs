using DMS.Domain.Dtos.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Domain.Dtos.System
{
    public class LookupDto : BaseDto
    {
        public int HeaderId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; }
    }
}
