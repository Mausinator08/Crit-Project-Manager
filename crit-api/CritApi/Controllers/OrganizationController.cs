using Crit.Application.RepositoryInterfaces;
using Crit.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CritApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrganizationController : ControllerBase
{
    private readonly Logging.IFileLogger _logger;
    private readonly IOrganizationRepository _organizationRepository;
    public OrganizationController(Logging.IFileLogger logger, IOrganizationRepository organizationRepository)
    {
        _logger = logger;
        _organizationRepository = organizationRepository;
    }

    [HttpPost]
    [Route("CreateFirstOrganization")]
    [ProducesResponseType<Organization>(StatusCodes.Status201Created)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> CreateFirstOrganization([FromBody] Organization organization)
    {
        try
        {
            if (organization == null)
            {
                return BadRequest("Invalid organization data.");
            }

            Organization? createdOrganization = await _organizationRepository.CreateFirstOrganization(organization);
            return CreatedAtAction(nameof(GetOrganization), new { organizationId = createdOrganization.Id }, createdOrganization);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating organization: {ex.Message}");
        }
    }

    [HttpGet]
    [ProducesResponseType<List<Organization>>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> GetAllOrganizations()
    {
        try
        {
            return Ok(await _organizationRepository.GetAllOrganizations());
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving organizations: {ex.Message}");
        }
    }

    [HttpGet]
    [Route("GetAllOrganizationsForProjectId/{projectId}")]
    [ProducesResponseType<List<Organization>>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> GetAllOrganizationsForProjectId([FromRoute] Guid projectId)
    {
        try
        {
            return Ok(await _organizationRepository.GetAllOrganizationsForProjectId(projectId));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving organizations: {ex.Message}");
        }
    }

    [HttpGet]
    [Route("GetAllOrganizationsForUserId/{userId}")]
    [ProducesResponseType<List<Organization>>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> GetAllOrganizationsForUserId([FromRoute] Guid userId)
    {
        try
        {
            return Ok(await _organizationRepository.GetAllOrganizationsForUserId(userId));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving organizations: {ex.Message}");
        }
    }

    [HttpGet]
    [Route("GetOrganizationByUserId/{userId}")]
    [ProducesResponseType<Organization>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> GetOrganizationByUserId([FromRoute] Guid userId)
    {
        try
        {
            return Ok(await _organizationRepository.GetOrganizationByUserId(userId));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving organization: {ex.Message}");
        }
    }

    [HttpGet]
    [Route("{organizationId}")]
    [ProducesResponseType<Organization>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> GetOrganization(Guid organizationId)
    {
        try
        {
            Organization? organization = await _organizationRepository.GetOrganization(organizationId);
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
    [ProducesResponseType<Organization>(StatusCodes.Status201Created)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> CreateOrganization([FromBody] Organization organization)
    {
        try
        {
            if (organization == null)
            {
                return BadRequest("Invalid organization data.");
            }

            Organization? createdOrganization = await _organizationRepository.CreateOrganization(organization);
            return CreatedAtAction(nameof(GetOrganization), new { organizationId = createdOrganization.Id }, createdOrganization);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating organization: {ex.Message}");
        }
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> UpdateOrganization([FromBody] Organization organization)
    {
        try
        {
            if (organization == null || organization.Id == null || organization.Id == Guid.Empty)
            {
                return BadRequest("Invalid organization data.");
            }

            await _organizationRepository.UpdateOrganization(organization);
            return NoContent();
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
            await _organizationRepository.DeleteOrganization(organizationId);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting organization: {ex.Message}");
        }
    }
}
