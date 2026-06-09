using Crit.Application.CustomFieldTypes;
using Crit.Application.RepositoryInterfaces;
using Crit.Contracts.RequestModels;
using Crit.Contracts.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CritApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CustomFieldTypeController : ControllerBase
{
    private readonly Logging.IFileLogger _logger;
    private readonly ICustomFieldTypeService _customFieldTypeService;

    public CustomFieldTypeController(Logging.IFileLogger logger, ICustomFieldTypeService customFieldTypeService)
    {
        _logger = logger;
        _customFieldTypeService = customFieldTypeService;
    }

    [HttpGet]
    [Route("GetAllCustomFieldTypes/{projectId}")]
    [ProducesResponseType<List<CustomFieldTypeResponse>>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> GetAllCustomFieldTypes([FromRoute] Guid projectId)
    {
        try
        {
            return Ok(await _customFieldTypeService.GetAllCustomFieldTypes(projectId));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving Custom Field Types: {ex.Message}");
        }
    }

    [HttpGet]
    [Route("{customFieldTypeId}")]
    [ProducesResponseType<CustomFieldTypeResponse>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> GetCustomFieldType([FromRoute] Guid customFieldTypeId)
    {
        try
        {
            return Ok(await _customFieldTypeService.GetCustomFieldType(customFieldTypeId));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving Custom Field Type: {ex.Message}");
        }
    }

    [HttpPost]
    [ProducesResponseType<CustomFieldTypeResponse>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> CreateCustomFieldType([FromBody] CreateCustomFieldTypeRequest customFieldType)
    {
        try
        {
            CustomFieldTypeResponse? createdCustomFieldType = await _customFieldTypeService.CreateCustomFieldType(customFieldType);
            return Ok(createdCustomFieldType);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating Custom Field Type: {ex.Message}");
        }
    }

    [HttpPut]
    [Route("{customFieldTypeId}")]
    [ProducesResponseType<CustomFieldTypeResponse>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> UpdateCustomFieldType([FromRoute] Guid customFieldTypeId, [FromBody] UpdateCustomFieldTypeRequest customFieldType)
    {
        try
        {
            if (customFieldType == null || customFieldTypeId == Guid.Empty)
            {
                return BadRequest("Invalid organization data.");
            }

            CustomFieldTypeResponse? updatedCustomFieldType = await _customFieldTypeService.UpdateCustomFieldType(customFieldTypeId, customFieldType);
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
            await _customFieldTypeService.DeleteCustomFieldType(customFieldTypeId);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting Custom Field Type: {ex.Message}");
        }
    }
}
