using Crit.Abstractions.Models;
using Crit.Application.RepositoryInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CritApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StatusController : ControllerBase
{
    private readonly Logging.IFileLogger _logger;
    private readonly IStatusRepository _statusRepository;

    public StatusController(Logging.IFileLogger logger, IStatusRepository statusRepository)
    {
        _logger = logger;
        _statusRepository = statusRepository;
    }

    [HttpGet]
    [Route("GetAllStatuses/{projectId}")]
    [ProducesResponseType<List<Status>>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> GetAllStatuses([FromRoute] Guid projectId)
    {
        try
        {
            return Ok(await _statusRepository.GetAllStatuses(projectId));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving statuses: {ex.Message}");
        }
    }

    [HttpGet]
    [Route("{statusId}")]
    [ProducesResponseType<Status>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> GetStatus([FromRoute] Guid statusId)
    {
        try
        {
            return Ok(await _statusRepository.GetStatus(statusId));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving status: {ex.Message}");
        }
    }

    [HttpPost]
    [ProducesResponseType<Status>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> CreateStatus([FromBody] Status status)
    {
        try
        {
            var createdStatus = await _statusRepository.CreateStatus(status);
            return Ok(createdStatus);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating status: {ex.Message}");
        }
    }

    [HttpPut]
    [ProducesResponseType<Status>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> UpdateStatus([FromBody] Status status)
    {
        try
        {
            var updatedStatus = await _statusRepository.UpdateStatus(status);
            return Ok(updatedStatus);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating status: {ex.Message}");
        }
    }

    [HttpDelete]
    [Route("{statusId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> DeleteStatus([FromRoute] Guid statusId)
    {
        try
        {
            await _statusRepository.DeleteStatus(statusId);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting status: {ex.Message}");
        }
    }
}
