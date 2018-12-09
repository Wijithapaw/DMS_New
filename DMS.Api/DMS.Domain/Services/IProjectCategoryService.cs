using DMS.Domain.Dtos.Project;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DMS.Domain.Services
{
    public interface IProjectCategoryService
    {
        Task<int> CreateAsync(ProjectCategoryDto project);

        Task DeleteAsync(int id);

        Task<ProjectCategoryDto> GetAsync(int id);

        Task<ICollection<ProjectCategoryDto>> GetAllAsync();

        Task UpdateAsync(ProjectCategoryDto project);
    }
}
