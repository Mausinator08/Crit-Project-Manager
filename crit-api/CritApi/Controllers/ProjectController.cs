using CritBusinessLogic.Models;
using CritBusinessLogic.RepositoryInterfaces;
using CritDTO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CritApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectController : ControllerBase
{
    private readonly Logging.ILogger _logger;
    private readonly IProjectsRepository _projectsRepository;
    public ProjectController(Logging.ILogger logger, IProjectsRepository projectsRepository)
    {
        _logger = logger;
        _projectsRepository = projectsRepository;
    }

    [HttpGet]
    [ProducesResponseType<List<Project>>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> GetAllProjects()
    {
        try
        {
            return Ok(await _projectsRepository.GetAllProjects());
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not get all projects.");
        }
    }

    [HttpGet]
    [Route("{projectId}")]
    [ProducesResponseType<Project>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> GetProject([FromRoute] string projectId)
    {
        try
        {
            return Ok(await _projectsRepository.GetProject(projectId));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Could not get project {projectId}.");
        }
    }

    [HttpPost]
    [ProducesResponseType<Project>(StatusCodes.Status201Created)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> CreateProject([FromBody] ProjectRequest project)
    {
        try
        {
            Project createdProject = await _projectsRepository.CreateProject(project);
            return CreatedAtAction(nameof(GetProject), new { projectId = createdProject.Id }, createdProject);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Could not get project {project.Name}.");
        }
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> UpdateProject([FromBody] Project project)
    {
        try
        {
            await _projectsRepository.UpdateProject(project);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Could not update project {project.Name}.");
        }
    }

    [HttpDelete]
    [Route("{projectId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> DeleteProject([FromRoute] string projectId)
    {
        try
        {
            await _projectsRepository.DeleteProject(projectId);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Could not delete project {projectId}.");
        }
    }
}
