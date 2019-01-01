using DMS.Domain.Dtos.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Tests.Helpers
{
    public static class DtoHelper
    {
        public static LookupHeaderDto CreateLookupHeaderDto(string code, string description, int id = 0)
        {
            var dto = new LookupHeaderDto
            {
                Id = id,
                Code = code,
                Description = description
            };

            return dto;
        }
        public static LookupDto CreateLookupDto(int headerId, string code, string description, int sortOrder, int id = 0)
        {
            var dto = new LookupDto
            {
                Id = id,
                HeaderId = headerId,
                Code = code,
                Description = description,
                SortOrder = sortOrder
            };

            return dto;
        }
    }
}
