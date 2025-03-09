using CritBusinessLogic.Models;
using CritBusinessLogic.RepositoryInterfaces;
using CritDataAccess.Contexts;
using CritDataAccess.Services;
using CritDTO.Identity;
using CritDTO.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CritBusinessLogic.Repositories;

public class ProjectsRepository : IProjectsRepository
{
    private readonly CritDbContext _critDbContext;
    private readonly TenantDbContext _tenantDbContext;
    private readonly IUserRepository _userRepository;
    public ProjectsRepository(ITenantDbContextService tenantDbContextService, IHttpContextAccessor httpContextAccessor, CritDbContext critDbContext, IUserRepository userRepository)
    {
        _critDbContext = critDbContext;
        _userRepository = userRepository;
        if (httpContextAccessor.HttpContext != null)
        {
            Task<TenantDbContext> tenantDbContextTask = tenantDbContextService.GetAuthenticatedTenantDb(httpContextAccessor.HttpContext.User);
            tenantDbContextTask.Wait();
            _tenantDbContext = tenantDbContextTask.Result;
        }
        else
        {
            throw new Exception("HttpContext is null.");
        }
    }

    public async Task<List<Project>> GetAllProjects()
    {
        try
        {
            Organization? organization = await _userRepository.GetLoggedInUserOrganization();
            if (organization == null)
            {
                throw new Exception("User is not a member of any organization and therefore cannot access projects.");
            }

            ApplicationUser? applicationUser = await _userRepository.GetLoggedInUser();
            if (applicationUser == null)
            {
                throw new Exception("User is not logged in.");
            }

            IQueryable<Project> projectsQuery = _tenantDbContext.Projects.Where(p =>
            p.ProjectUserIds.Contains(applicationUser.Id) ||
            p.ProjectAdminUserIds.Contains(applicationUser.Id) ||
            p.ProjectOwnerUserId == applicationUser.Id);

            if (projectsQuery.Any())
            {
                return await projectsQuery.ToListAsync();
            }

            return new List<Project>();
        }
        catch (Exception ex)
        {
            throw new Exception("Could not get projects.", ex);
        }
    }

    public async Task<Project?> GetProject(Guid projectId)
    {
        try
        {
            Organization? organization = await _userRepository.GetLoggedInUserOrganization();
            if (organization == null)
            {
                throw new Exception("User is not a member of any organization and therefore cannot access projects.");
            }

            ApplicationUser? applicationUser = await _userRepository.GetLoggedInUser();
            if (applicationUser == null)
            {
                throw new Exception("User is not logged in.");
            }

            IQueryable<Project> projectQuery = _tenantDbContext.Projects.Where(p =>
            p.Id == projectId &&
            (p.ProjectUserIds.Contains(applicationUser.Id) ||
            p.ProjectAdminUserIds.Contains(applicationUser.Id) ||
            p.ProjectOwnerUserId == applicationUser.Id));

            if (projectQuery.Any())
            {
                return await projectQuery.FirstAsync();
            }

            return null;
        }
        catch (Exception ex)
        {
            throw new Exception($"Could not get project {projectId}.", ex);
        }
    }

    public async Task<Project> CreateProject(ProjectRequest project)
    {
        try
        {
            Organization? organization = await _userRepository.GetLoggedInUserOrganization();
            if (organization == null)
            {
                throw new Exception("User is not a member of any organization and therefore cannot create projects.");
            }

            ApplicationUser? applicationUser = await _userRepository.GetLoggedInUser();
            if (applicationUser == null)
            {
                throw new Exception("User is not logged in.");
            }

            Project newProject = new Project()
            {
                Name = project.Name,
                Description = project.Description,
                OrganizationIds = project.OrganizationIds,
                ProjectUserIds = project.ProjectUserIds,
                ProjectAdminUserIds = project.ProjectAdminUserIds,
                ProjectOwnerUserId = project.ProjectOwnerUserId ?? applicationUser.Id
            };

            _tenantDbContext.Projects.Add(newProject);
            int savedChanges = await _tenantDbContext.SaveChangesAsync();

            if (savedChanges <= 0)
            {
                throw new Exception("Project failed to save.");
            }

            return newProject;
        }
        catch (Exception ex)
        {
            throw new Exception("Could not create project.", ex);
        }
    }

    public async System.Threading.Tasks.Task UpdateProject(Project project)
    {
        try
        {
            Project? existingProject = await GetProject(project.Id);
            if (existingProject == null)
            {
                throw new Exception($"Project {project.Id} does not exist.");
            }

            existingProject = project;

            _tenantDbContext.Projects.Update(existingProject);
            int savedChanges = await _tenantDbContext.SaveChangesAsync();

            if (savedChanges <= 0)
            {
                throw new Exception("Project failed to update.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Could not update project.", ex);
        }
    }

    public async System.Threading.Tasks.Task DeleteProject(Guid projectId)
    {
        try
        {
            Project? existingProject = await GetProject(projectId);
            if (existingProject == null)
            {
                throw new Exception($"Project {projectId} does not exist.");
            }

            _tenantDbContext.Projects.Remove(existingProject);
            int savedChanges = await _tenantDbContext.SaveChangesAsync();

            if (savedChanges <= 0)
            {
                throw new Exception("Project failed to delete.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Could not delete project.", ex);
        }
    }
}
