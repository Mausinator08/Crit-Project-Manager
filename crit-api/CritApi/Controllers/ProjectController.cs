using CritDataAccess.Contexts;
using CritDTO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CritApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectController : ControllerBase
{
    private readonly CritDbContext _critContext;
    public ProjectController(CritDbContext critContext)
    {
        _critContext = critContext;
    }

    [HttpGet]
    [Route("GetAllProjects")]
    [ProducesResponseType<List<Project>>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> GetAllProjects()
    {
        try
        {
            return Ok(new List<Project>([new Project("Test", "Testing out this project!", Guid.NewGuid(), Guid.NewGuid())]));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not get all projects.");
        }
    }

    [HttpGet]
    [Route("{projectId}")]
    [ProducesResponseType<Project>(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> GetProject([FromRoute] Guid projectId)
    {
        try
        {
            return Ok(new Project("Test", "Testing out this project!", Guid.NewGuid(), Guid.NewGuid()));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not get all projects.");
        }
    }
}
