using DMS.Data;
using DMS.Domain;
using DMS.Domain.CustomExceptions;
using DMS.Domain.Dtos.Project;
using DMS.Domain.Entities;
using DMS.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Services
{
    public class ProjectsService : IProjectsService
    {
        private IDataContext _dataContext;

        public ProjectsService(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<int> CreateAsync(ProjectDto projectDto)
        {
            var project = new Project
            {
                Title = projectDto.Title,
                Description = projectDto.Description,
                ProjectOwnerId = projectDto.OwnerId,
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
            project.StartDate = dto.StartDate;
            project.EndDate = dto.EndDate;
            project.RowVersion = dto.RowVersion;

            await _dataContext.SaveChangesAsync();
        }
    }
}
