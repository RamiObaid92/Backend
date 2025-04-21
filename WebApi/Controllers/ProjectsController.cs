using Business.Services;
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions.Attributes;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [RequireKey("AdminKey", "UserKey")]
    [ApiController]
    public class ProjectsController(IProjectService projectService) : ControllerBase
    {
        private readonly IProjectService _projectService = projectService;

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromForm] AddProjectForm formData)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            var result = await _projectService.CreateProjectAsync(formData);

            if (result is null) 
                return BadRequest();

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var result = await _projectService.GetAllProjectsAsync();

            if (result is null) 
                return NotFound();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(Guid id)
        {
            var result = await _projectService.GetProjectByIdAsync(id);

            if (result is null) 
                return NotFound();

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProject([FromForm] EditProjectForm formData)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            var result = await _projectService.UpdateProjectAsync(formData);

            if (result is null) 
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            var result = await _projectService.DeleteProjectAsync(id);

            if (!result) 
                return NotFound();

            return Ok();
        }
    }
}
