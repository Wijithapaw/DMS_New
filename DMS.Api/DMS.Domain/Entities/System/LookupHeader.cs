using DMS.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Domain.Entities.System
{
    public class LookupHeader : BaseEntity
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
