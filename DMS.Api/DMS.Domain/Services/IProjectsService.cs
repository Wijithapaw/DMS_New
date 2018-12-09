using DMS.Domain.Dtos;
using DMS.Domain.Dtos.Project;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DMS.Domain.Services
{
    public interface IProjectsService
    {
        Task<int> CreateAsync(ProjectDto project);

        Task DeleteAsync(int id);

        Task<ProjectDto> GetAsync(int id);

        Task<ICollection<ProjectDto>> GetAllAsync();

        Task<ICollection<ProjectDto>> GetAllAsync(string category);

        Task UpdateAsync(ProjectDto project);
    }
}
