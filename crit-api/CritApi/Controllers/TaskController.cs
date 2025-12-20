using Crit.Domain.Entities;
using Crit.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CritApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TaskController : ControllerBase
{
    private readonly Logging.IFileLogger _logger;
    private readonly ITasksRepository _tasksRepository;
    public TaskController(ITasksRepository tasksRepository, Logging.IFileLogger logger)
    {
        _logger = logger;
        _tasksRepository = tasksRepository;
    }

    [HttpGet]
    [Route("{projectId}")]
    [ProducesResponseType<List<Task>>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    [Authorize(Roles = "User,ProjectAdmin,ProjectOwner,OrganizationAdmin,OrganizationOwner,SuperAdmin")]
    public async Task<IActionResult> GetTasks([FromRoute] Guid projectId)
    {
        try
        {
            List<ProjectTask> tasks = await _tasksRepository.GetAllTasks(projectId);
            return Ok(tasks);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving tasks: {ex.Message}");
        }
    }

    [HttpGet]
    [Route("{projectId}/{taskId}")]
    [ProducesResponseType<Task>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    [Authorize(Roles = "User,ProjectAdmin,ProjectOwner,OrganizationAdmin,OrganizationOwner,SuperAdmin")]
    public async Task<IActionResult> GetTask([FromRoute] Guid projectId, [FromRoute] Guid taskId)
    {
        try
        {
            ProjectTask? task = await _tasksRepository.GetTask(projectId, taskId);
            if (task == null)
            {
                return NotFound("Task not found.");
            }

            return Ok(task);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving task: {ex.Message}");
        }
    }

    [HttpPost]
    [ProducesResponseType<Task>(StatusCodes.Status201Created)]
    [ProducesErrorResponseType(typeof(string))]
    [Authorize(Roles = "ProjectAdmin,ProjectOwner,OrganizationAdmin,OrganizationOwner,SuperAdmin")]
    public async Task<IActionResult> CreateTask([FromBody] ProjectTask task)
    {
        try
        {
            if (task == null)
            {
                return BadRequest("Task data is null.");
            }

            ProjectTask? createdTask = await _tasksRepository.CreateTask(task);
            if (createdTask == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating task.");
            }

            return CreatedAtAction(nameof(GetTask), new { projectId = task.ProjectId, taskId = createdTask.Id }, createdTask);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating task: {ex.Message}");
        }
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    [Authorize(Roles = "ProjectAdmin,ProjectOwner,OrganizationAdmin,OrganizationOwner,SuperAdmin")]
    public async Task<IActionResult> UpdateTask([FromBody] ProjectTask task)
    {
        try
        {
            if (task == null)
            {
                return BadRequest("Task data is null.");
            }

            await _tasksRepository.UpdateTask(task);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating task: {ex.Message}");
        }
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesErrorResponseType(typeof(string))]
    [Authorize(Roles = "ProjectAdmin,ProjectOwner,OrganizationAdmin,OrganizationOwner,SuperAdmin")]
    public async Task<IActionResult> DeleteTask([FromQuery] Guid projectId, [FromQuery] Guid taskId)
    {
        try
        {
            await _tasksRepository.DeleteTask(projectId, taskId);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting task: {ex.Message}");
        }
    }
}
