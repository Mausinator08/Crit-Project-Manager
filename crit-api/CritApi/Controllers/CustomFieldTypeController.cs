using Crit.Application.RepositoryInterfaces;
using Crit.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CritApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CustomFieldTypeController : ControllerBase
{
    private readonly Logging.IFileLogger _logger;
    private readonly ICustomFieldTypeRepository _customFieldTypeRepository;

    public CustomFieldTypeController(Logging.IFileLogger logger, ICustomFieldTypeRepository customFieldTypeRepository)
    {
        _logger = logger;
        _customFieldTypeRepository = customFieldTypeRepository;
    }

    [HttpGet]
    [Route("GetAllCustomFieldTypes/{projectId}")]
    [ProducesResponseType<List<CustomFieldType>>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> GetAllCustomFieldTypes([FromRoute] Guid projectId)
    {
        try
        {
            return Ok(await _customFieldTypeRepository.GetAllCustomFieldTypes(projectId));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving Custom Field Types: {ex.Message}");
        }
    }

    [HttpGet]
    [Route("{customFieldTypeId}")]
    [ProducesResponseType<CustomFieldType>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> GetCustomFieldType([FromRoute] Guid customFieldTypeId)
    {
        try
        {
            return Ok(await _customFieldTypeRepository.GetCustomFieldType(customFieldTypeId));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving Custom Field Type: {ex.Message}");
        }
    }

    [HttpPost]
    [ProducesResponseType<CustomFieldType>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> CreateCustomFieldType([FromBody] CustomFieldType customFieldType)
    {
        try
        {
            var createdCustomFieldType = await _customFieldTypeRepository.CreateCustomFieldType(customFieldType);
            return Ok(createdCustomFieldType);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating Custom Field Type: {ex.Message}");
        }
    }

    [HttpPut]
    [ProducesResponseType<CustomFieldType>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> UpdateCustomFieldType([FromBody] CustomFieldType customFieldType)
    {
        try
        {
            var updatedCustomFieldType = await _customFieldTypeRepository.UpdateCustomFieldType(customFieldType);
            return Ok(updatedCustomFieldType);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating Custom Field Type: {ex.Message}");
        }
    }

    [HttpDelete]
    [Route("{customFieldTypeId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> DeleteCustomFieldType([FromRoute] Guid customFieldTypeId)
    {
        try
        {
            await _customFieldTypeRepository.DeleteCustomFieldType(customFieldTypeId);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting Custom Field Type: {ex.Message}");
        }
    }
}
