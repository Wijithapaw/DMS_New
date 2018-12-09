using DMS.Domain.Services;
using System.Collections.Generic;
using System.Linq;
using DMS.Data.Entities;
using DMS.Data;
using DMS.Domain.Dtos;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DMS.Domain.Dtos.Project;
using DMS.Utills.CustomExceptions;

namespace DMS.Services
{
    public class ProjectsService : IProjectsService
    {
        private DataContext _dataContext;

        public ProjectsService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<int> CreateAsync(ProjectDto projectDto)
        {
            var project = new Project
            {
                Title = projectDto.Title,
                Description = projectDto.Description,
                ProjectCategoryId = projectDto.ProjectCategoryId,
                StartDate = projectDto.StartDate,
                EndDate = projectDto.EndDate                
            };

            _dataContext.Projects.Add(project);
            await _dataContext.SaveChangesAsync();

            return project.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var project = _dataContext.Projects.Find(id);

            if (project == null)
                throw new RecordNotFoundException("Project", id);

            _dataContext.Projects.Remove(project);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<ProjectDto> GetAsync(int id)
        {
            var project = await _dataContext.Projects.Where(p => p.Id == id)
                    .Select(p => new ProjectDto
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Description = p.Description,
                        ProjectCategoryId = p.ProjectCategoryId,
                        ProjectCategory = p.ProjectCategory.Title,
                        StartDate = p.StartDate,
                        EndDate = p.EndDate,
                        RowVersion = p.RowVersion
                    }).FirstOrDefaultAsync();

            return project;
        }

        public async Task<ICollection<ProjectDto>> GetAllAsync()
        {
            var projects = await _dataContext.Projects
                    .Select(p => new ProjectDto
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Description = p.Description,
                        ProjectCategoryId = p.ProjectCategoryId,
                        ProjectCategory = p.ProjectCategory.Title,
                        StartDate = p.StartDate,
                        EndDate = p.EndDate
                    }).ToListAsync();

            return projects;
        }

        public async Task<ICollection<ProjectDto>> GetAllAsync(string category)
        {
            var projects = await _dataContext.Projects
                    .Where(p => p.ProjectCategory.Title == category)
                     .Select(p => new ProjectDto
                     {
                         Id = p.Id,
                         Title = p.Title,
                         Description = p.Description,
                         ProjectCategoryId = p.ProjectCategoryId,
                         ProjectCategory = p.ProjectCategory.Title,
                         StartDate = p.StartDate,
                         EndDate = p.EndDate
                     }).ToListAsync();

            return projects;
        }

        public async Task UpdateAsync(ProjectDto dto)
        {
            var project = _dataContext.Projects.Find(dto.Id);

            if (project == null)
                throw new RecordNotFoundException("Project", dto.Id);

            project.Title = dto.Title;
            project.Description = dto.Description;
            project.ProjectCategoryId = dto.ProjectCategoryId;
            project.StartDate = dto.StartDate;
            project.EndDate = dto.EndDate;
            project.RowVersion = dto.RowVersion;

            await _dataContext.SaveChangesAsync();
        }
    }
}
