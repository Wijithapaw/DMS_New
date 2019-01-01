using DMS.Domain.Dtos.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Domain.Dtos.System
{
    public class LookupHeaderDto : BaseDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
