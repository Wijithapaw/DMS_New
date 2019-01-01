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
    public class LookupService
    {
        private readonly IDataContext _dataContext;

        public LookupService(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<EntityUpdateResultDto> CreateAsync(LookupDto dto)
        {
            var lookup = new Lookup
            {
                HeaderId = dto.HeaderId,
                Code = dto.Code,
                Description = dto.Description,
                SortOrder = dto.SortOrder
            };

            _dataContext.Lookups.Add(lookup);

            await _dataContext.SaveChangesAsync();

            return new EntityUpdateResultDto
            {
                Id = lookup.Id,
                RowVersion = lookup.RowVersion
            };
        }

        public async Task DeleteAsync(int id)
        {
            var lookup = await _dataContext.Lookups.FindAsync(id);

            if (lookup == null)
                throw new RecordNotFoundException("Lookup", id);

            lookup.Deleted = DateTime.UtcNow.Ticks;

            _dataContext.Lookups.Update(lookup);

            await _dataContext.SaveChangesAsync();
        }

        public async Task<List<LookupDto>> GetAllByHeaderCodeAsync(string headerCode)
        {
            var lookups = await _dataContext.Lookups
                    .Where(h => h.Header.Code == headerCode)
                    .Select(h => new LookupDto
                    {
                        Id = h.Id,
                        Code = h.Code,
                        Description = h.Description,
                        SortOrder = h.SortOrder,
                        HeaderId = h.HeaderId
                    }).ToListAsync();

            return lookups;
        }

        public async Task<List<LookupDto>> GetAllByHeaderIdAsync(int headerId)
        {
            var lookups = await _dataContext.Lookups
                    .Where(h => h.HeaderId== headerId)
                    .Select(h => new LookupDto
                    {
                        Id = h.Id,
                        Code = h.Code,
                        Description = h.Description,
                        SortOrder = h.SortOrder,
                        HeaderId = h.HeaderId
                    }).ToListAsync();

            return lookups;
        }

        public async Task<LookupDto> GetAsync(int id)
        {
            var lookup = await _dataContext.Lookups
                    .Where(h => h.Id == id)
                    .Select(l => new LookupDto
                    {
                        Id = l.Id,
                        HeaderId = l.HeaderId,
                        Code = l.Code,
                        Description = l.Description,
                        SortOrder = l.SortOrder,
                        RowVersion = l.RowVersion
                    }).FirstOrDefaultAsync();

            if (lookup == null)
                throw new RecordNotFoundException("Lookup", id);

            return lookup;
        }

        public async Task<EntityUpdateResultDto> UpdateAsync(LookupDto dto)
        {
            var lookup = await _dataContext.Lookups.FindAsync(dto.Id);

            if (lookup == null)
                throw new RecordNotFoundException("Lookup", dto.Id);

            lookup.Code = dto.Code;
            lookup.Description = dto.Description;
            lookup.HeaderId = dto.HeaderId;
            lookup.SortOrder = dto.SortOrder;
            lookup.RowVersion = dto.RowVersion;

            _dataContext.Lookups.Update(lookup);

            await _dataContext.SaveChangesAsync();

            return new EntityUpdateResultDto
            {
                Id = lookup.Id,
                RowVersion = lookup.RowVersion
            };
        }
    }
}
