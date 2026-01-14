using Crit.Abstractions.Models;
using Crit.Application.RepositoryInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CritApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CustomFieldController : ControllerBase
{
    private readonly Logging.IFileLogger _logger;
    private readonly ICustomFieldRepository _customFieldRepository;

    public CustomFieldController(Logging.IFileLogger logger, ICustomFieldRepository customFieldRepository)
    {
        _logger = logger;
        _customFieldRepository = customFieldRepository;
    }

    [HttpGet]
    [Route("GetAllCustomFields/{taskId}")]
    [ProducesResponseType<List<CustomField>>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> GetAllCustomFields([FromRoute] Guid taskId)
    {
        try
        {
            return Ok(await _customFieldRepository.GetAllCustomFields(taskId));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving Custom Fields: {ex.Message}");
        }
    }

    [HttpGet]
    [Route("{customFieldTypeId}")]
    [ProducesResponseType<CustomField>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> GetCustomField([FromRoute] Guid customFieldId)
    {
        try
        {
            return Ok(await _customFieldRepository.GetCustomField(customFieldId));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving Custom Field: {ex.Message}");
        }
    }

    [HttpPost]
    [ProducesResponseType<CustomField>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> CreateCustomField([FromBody] CustomField customField)
    {
        try
        {
            var createdCustomField = await _customFieldRepository.CreateCustomField(customField);
            return Ok(createdCustomField);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating Custom Field: {ex.Message}");
        }
    }

    [HttpPut]
    [ProducesResponseType<CustomField>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> UpdateCustomField([FromBody] CustomField customField)
    {
        try
        {
            var updatedCustomField = await _customFieldRepository.UpdateCustomField(customField);
            return Ok(updatedCustomField);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating Custom Field: {ex.Message}");
        }
    }

    [HttpDelete]
    [Route("{customFieldId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> DeleteCustomField([FromRoute] Guid customFieldId)
    {
        try
        {
            await _customFieldRepository.DeleteCustomField(customFieldId);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting Custom Field: {ex.Message}");
        }
    }
}
