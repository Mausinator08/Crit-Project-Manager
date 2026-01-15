using System.Security.Claims;
using Crit.Application.Organizations;
using Crit.Application.Users;
using Crit.Contracts.RequestModels;
using Crit.Contracts.ResponseModels;
using Crit.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CritApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrganizationController : ControllerBase
{
    private readonly Logging.IFileLogger _logger;
    private readonly IOrganizationService _organizationService;
    private readonly ICurrentUserService _userService;
    public OrganizationController(Logging.IFileLogger logger, IOrganizationService organizationService, CurrentUserService userService)
    {
        _logger = logger;
        _organizationService = organizationService;
        _userService = userService;
    }

    [HttpGet]
    [Route("GetAllOrganizationsForLoggedInUser")]
    [ProducesResponseType<List<OrganizationResponse>>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> GetAllOrganizationsForLoggedInUser()
    {
        try
        {
            return Ok(await _organizationService.GetAllOrganizationsForLoggedInUser());
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving organizations: {ex.Message}");
        }
    }

    [HttpGet]
    [Route("GetAllOrganizationsForProjectId/{projectId}")]
    [ProducesResponseType<List<OrganizationResponse>>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> GetAllOrganizationsForProjectId([FromRoute] Guid projectId)
    {
        try
        {
            return Ok(await _organizationService.GetAllOrganizationsForProjectId(projectId));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving organizations: {ex.Message}");
        }
    }

    [HttpGet]
    [Route("GetAllOrganizationsForUserId/{userId}")]
    [ProducesResponseType<List<OrganizationResponse>>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> GetAllOrganizationsForUserId([FromRoute] Guid userId)
    {
        try
        {
            return Ok(await _organizationService.GetAllOrganizationsForUserId(userId));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving organizations: {ex.Message}");
        }
    }

    [HttpGet]
    [Route("GetOrganizationByUserId/{userId}")]
    [ProducesResponseType<OrganizationResponse>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> GetOrganizationByUserId([FromRoute] Guid userId)
    {
        try
        {
            return Ok(await _organizationService.GetPrimaryOrganizationForUserId(userId));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving organization: {ex.Message}");
        }
    }

    [HttpGet]
    [Route("{organizationId}")]
    [ProducesResponseType<OrganizationResponse>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> GetOrganization(Guid organizationId)
    {
        try
        {
            OrganizationResponse? organization = await _organizationService.GetOrganization(organizationId);
            if (organization == null || organization.Id == null || organization.Id == Guid.Empty)
            {
                return NotFound($"Organization with ID {organizationId} not found.");
            }

            return Ok(organization);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving organization: {ex.Message}");
        }
    }

    [HttpPost]
    [ProducesResponseType<OrganizationResponse>(StatusCodes.Status201Created)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> CreateOrganization([FromBody] CreateOrganizationRequest organization)
    {
        try
        {
            if (organization == null)
            {
                return BadRequest("Invalid organization data.");
            }

            OrganizationResponse? createdOrganization = await _organizationService.CreateOrganization(organization);
            return CreatedAtAction(nameof(GetOrganization), new { organizationId = createdOrganization.Id }, createdOrganization);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating organization: {ex.Message}");
        }
    }

    [HttpPut]
    [Route("{organizationId}")]
    [ProducesResponseType<OrganizationResponse>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> UpdateOrganization([FromRoute] Guid organizationId, [FromBody] UpdateOrganizationRequest organization)
    {
        try
        {
            if (organization == null || organizationId == Guid.Empty)
            {
                return BadRequest("Invalid organization data.");
            }

            return Ok(await _organizationService.UpdateOrganization(organizationId, organization));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating organization: {ex.Message}");
        }
    }

    [HttpDelete]
    [Route("{organizationId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> DeleteOrganization(Guid organizationId)
    {
        try
        {
            await _organizationService.DeleteOrganization(organizationId);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting organization: {ex.Message}");
        }
    }

    [HttpGet]
    [Route("GetLoggedInUserOrganization")]
    [ProducesResponseType<OrganizationResponse>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> GetLoggedInUserOrganization()
    {
        try
        {
            OrganizationResponse? organization = await _organizationService.GetPrimaryOrganizationForLoggedInUser();

            if (organization == null)
            {
                return NotFound("An organization for the currently logged in user could not be found.");
            }

            return Ok(organization);
        }
        catch (Exception ex)
        {
            throw new Exception("Could not get logged in user organization.", ex);
        }
    }
}
