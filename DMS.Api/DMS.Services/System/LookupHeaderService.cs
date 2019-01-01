using DMS.Domain;
using DMS.Domain.CustomExceptions;
using DMS.Domain.Dtos.Base;
using DMS.Domain.Dtos.System;
using DMS.Domain.Entities.System;
using DMS.Domain.Services.System;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Services.System
{
    public class LookupHeaderService : ILookupHeaderService
    {
        private readonly IDataContext _dataContext;

        public LookupHeaderService(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<EntityUpdateResultDto> CreateAsync(LookupHeaderDto dto)
        {
            var lookupHeader = new LookupHeader
            {
                Code = dto.Code,
                Description = dto.Description
            };

            _dataContext.LookupHeaders.Add(lookupHeader);

            await _dataContext.SaveChangesAsync();

            return new EntityUpdateResultDto
            {
                Id = lookupHeader.Id,
                RowVersion = lookupHeader.RowVersion
            };
        }

        public async Task DeleteAsync(int id)
        {
            var lookupHeader = await _dataContext.LookupHeaders.FindAsync(id);

            if (lookupHeader == null)
                throw new RecordNotFoundException("LookupHeader", id);

            lookupHeader.Deleted = DateTime.UtcNow.Ticks;

            _dataContext.LookupHeaders.Update(lookupHeader);

            await _dataContext.SaveChangesAsync();
        }

        public async Task<List<LookupHeaderDto>> GetAllAsync()
        {
            var lookupHeaders = await _dataContext.LookupHeaders
                    .Select(h => new LookupHeaderDto
                    {
                        Id = h.Id,
                        Code = h.Code,
                        Description = h.Description
                    }).ToListAsync();

            return lookupHeaders;
        }

        public async Task<LookupHeaderDto> GetAsync(int id)
        {
            var lookupHeader = await _dataContext.LookupHeaders
                    .Where(h => h.Id == id)
                    .Select(h => new LookupHeaderDto
                    {
                        Id = h.Id,
                        Code = h.Code,
                        Description = h.Description,
                        RowVersion = h.RowVersion
                    }).FirstOrDefaultAsync();

            if (lookupHeader == null)
                throw new RecordNotFoundException("LookupHeader", id);

            return lookupHeader;
        }

        public async Task<EntityUpdateResultDto> UpdateAsync(LookupHeaderDto dto)
        {
            var lookupHeader = await _dataContext.LookupHeaders.FindAsync(dto.Id);

            if (lookupHeader == null)
                throw new RecordNotFoundException("LookupHeader", dto.Id);

            lookupHeader.Code = dto.Code;
            lookupHeader.Description = dto.Description;
            lookupHeader.RowVersion = dto.RowVersion;

            _dataContext.LookupHeaders.Update(lookupHeader);

            await _dataContext.SaveChangesAsync();

            return new EntityUpdateResultDto
            {
                Id = lookupHeader.Id,
                RowVersion = lookupHeader.RowVersion
            };
        }
    }
}
