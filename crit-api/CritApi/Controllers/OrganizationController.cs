using System.Security.Claims;
using Crit.Abstractions.Identity;
using Crit.Application.Organizations;
using Crit.Contracts.ResponseModels;
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
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;
    public OrganizationController(Logging.IFileLogger logger, IOrganizationService organizationService, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _organizationService = organizationService;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
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
    [ProducesResponseType<List<Organization>>(StatusCodes.Status200OK)]
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
    [ProducesResponseType<List<Organization>>(StatusCodes.Status200OK)]
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
    [ProducesResponseType<Organization>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> GetOrganizationByUserId([FromRoute] Guid userId)
    {
        try
        {
            return Ok(await _organizationService.GetOrganizationByUserId(userId));
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
            Organization? organization = await _organizationService.GetOrganization(organizationId);
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

            Organization? createdOrganization = await _organizationService.CreateOrganization(organization);
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

            await _organizationService.UpdateOrganization(organization);
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
            await _organizationService.DeleteOrganization(organizationId);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting organization: {ex.Message}");
        }
    }

    private async Task<OrganizationResponse?> GetLoggedInUserOrganization()
    {
        try
        {
            ClaimsPrincipal? user = null;

            if (_httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true)
            {
                user = _httpContextAccessor.HttpContext.User;
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            ApplicationUser? applicationUser = await _userManager.GetUserAsync(user);

            if (applicationUser == null)
            {
                throw new ArgumentNullException(nameof(applicationUser));
            }

            List<Organization> organizationsMemberQuery = await _critDbContext.Organizations.AsNoTracking().Where(o => o.OrganizationMembers.Any(ou => ou.MemberUserId == applicationUser.Id)).ToListAsync();
            List<Organization> organizationsAdminQuery = await _critDbContext.Organizations.AsNoTracking().Where(o => o.OrganizationAdmins.Any(ou => ou.AdminUserId == applicationUser.Id)).ToListAsync();

            Organization? organization = null;

            if (organizationsMemberQuery.Any())
            {
                organization = organizationsMemberQuery.First();
            }
            else if (organizationsAdminQuery.Any())
            {
                organization = organizationsAdminQuery.First();
            }
            else
            {
                throw new AuthenticationException("User is not a member of any organization.");
            }

            return organization;
        }
        catch (Exception ex)
        {
            throw new Exception("Could not get logged in user organization.", ex);
        }
    }
}
