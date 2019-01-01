using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using DMS.Domain.Services;
using DMS.Domain.Dtos;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using DMS.Domain.Dtos.Project;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace DMS.WebApi.Controllers
{
    [Route("api/[controller]")]
    //[Authorize("ManageProjects")]
    //[AllowAnonymous]
    public class ProjectsController : Controller
    {
        private IProjectsService _projectsService;

        public ProjectsController(IProjectsService projectsService)
        {
            _projectsService = projectsService;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        // GET: api/values
        [HttpGet]
        public async Task<IEnumerable<ProjectDto>> Get()
        {
            var projectsList = await _projectsService.GetAllAsync();
            return projectsList;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ProjectDto> Get(int id)
        {
            var project = await _projectsService.GetAsync(id);
            return project;
        }


        // POST api/values
        [HttpPost]
        public async Task<int> Post([FromBody]ProjectDto projectDto)
        {
            var id = await _projectsService.CreateAsync(projectDto);
            return id;
        }

        // PUT api/values/5
        [HttpPut]
        public async Task Put([FromBody]ProjectDto project)
        {
            await _projectsService.UpdateAsync(project);
        }        
    }
}
