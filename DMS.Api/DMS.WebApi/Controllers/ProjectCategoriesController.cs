using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using DMS.Domain.Services;
using DMS.Domain.Dtos;
using System.Threading.Tasks;
using DMS.Domain.Dtos.Project;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace DMS.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize("ManageSystemSettings")]
    public class ProjectCategoriesController : Controller
    {
        private IProjectCategoryService _projectCategoryService;

        public ProjectCategoriesController(IProjectCategoryService projectCategoryService)
        {
            _projectCategoryService = projectCategoryService;
        }

        // GET: api/values
        [HttpGet]
        public async Task<IEnumerable<ProjectCategoryDto>> Get()
        {
            var projectCategories = await _projectCategoryService.GetAllAsync();
            return projectCategories;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ProjectCategoryDto> Get(int id)
        {
            var projectCategory = await _projectCategoryService.GetAsync(id);
            return projectCategory;
        }

        // POST api/values
        [HttpPost]
        public async Task<int> Post([FromBody]ProjectCategoryDto data)
        {
            var id = await _projectCategoryService.CreateAsync(data);
            return id;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task Put([FromBody]ProjectCategoryDto data)
        {
            await _projectCategoryService.UpdateAsync(data);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _projectCategoryService.DeleteAsync(id);
        }
    }
}
