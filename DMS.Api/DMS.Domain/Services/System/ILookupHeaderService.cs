using DMS.Domain.Dtos.Base;
using DMS.Domain.Dtos.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Services.System
{
    public interface ILookupHeaderService
    {
        Task<List<LookupHeaderDto>> GetAllAsync();
        Task<LookupHeaderDto> GetAsync(int id);
        Task<EntityUpdateResultDto> CreateAsync(LookupHeaderDto dto);
        Task<EntityUpdateResultDto> UpdateAsync(LookupHeaderDto dto);
        Task DeleteAsync(int id);
    }
}
