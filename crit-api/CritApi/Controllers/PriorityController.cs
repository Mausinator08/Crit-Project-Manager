using Crit.Application.RepositoryInterfaces;
using Crit.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CritApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PriorityController : ControllerBase
{
    private readonly Logging.IFileLogger _logger;
    private readonly IPriorityRepository _priorityRepository;

    public PriorityController(Logging.IFileLogger logger, IPriorityRepository priorityRepository)
    {
        _logger = logger;
        _priorityRepository = priorityRepository;
    }

    [HttpGet]
    [Route("GetAllPriorities/{projectId}")]
    [ProducesResponseType<List<Priority>>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> GetAllPriorities([FromRoute] Guid projectId)
    {
        try
        {
            return Ok(await _priorityRepository.GetAllPriorities(projectId));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving priorities: {ex.Message}");
        }
    }

    [HttpGet]
    [Route("{priorityId}")]
    [ProducesResponseType<Status>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> GetPriority([FromRoute] Guid priorityId)
    {
        try
        {
            return Ok(await _priorityRepository.GetPriority(priorityId));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving priority: {ex.Message}");
        }
    }

    [HttpPost]
    [ProducesResponseType<Status>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> CreatePriority([FromBody] Priority priority)
    {
        try
        {
            Priority createdPriority = await _priorityRepository.CreatePriority(priority);
            return Ok(createdPriority);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating priority: {ex.Message}");
        }
    }

    [HttpPut]
    [ProducesResponseType<Status>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> UpdatePriority([FromBody] Priority priority)
    {
        try
        {
            Priority updatedPriority = await _priorityRepository.UpdatePriority(priority);
            return Ok(updatedPriority);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating priority: {ex.Message}");
        }
    }

    [HttpDelete]
    [Route("{priorityId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> DeletePriority([FromRoute] Guid priorityId)
    {
        try
        {
            await _priorityRepository.DeletePriority(priorityId);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting priority: {ex.Message}");
        }
    }
}
