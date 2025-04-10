using CritBusinessLogic.Models;
using CritBusinessLogic.RepositoryInterfaces;
using CritDataAccess.Contexts;
using CritDataAccess.Services;
using CritDTO.Identity;
using CritDTO.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CritBusinessLogic.Repositories;

public class ProjectsRepository : IDisposable, IAsyncDisposable, IProjectsRepository
{
    private readonly CritDbContext _critDbContext;
    private TenantDbContext? _tenantDbContext;
    private readonly IUserRepository _userRepository;
    public ProjectsRepository(ITenantDbContextService tenantDbContextService, IHttpContextAccessor httpContextAccessor, CritDbContext critDbContext, IUserRepository userRepository)
    {
        _critDbContext = critDbContext;
        _userRepository = userRepository;
        if (httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true)
        {
            Task<TenantDbContext> tenantDbContextTask = tenantDbContextService.GetAuthenticatedTenantDb(httpContextAccessor.HttpContext.User);
            tenantDbContextTask.Wait();
            _tenantDbContext = tenantDbContextTask.Result;
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

            if (_tenantDbContext == null)
            {
                throw new Exception("Tenant database context is not initialized.");
            }

            IQueryable<Project> projectsQuery = _tenantDbContext.Projects.AsNoTracking().Where(p =>
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

    public async Task<Project?> GetProject(string projectId)
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

            if (_tenantDbContext == null)
            {
                throw new Exception("Tenant database context is not initialized.");
            }

            IQueryable<Project> projectQuery = _tenantDbContext.Projects.AsNoTracking().Where(p =>
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

            if (string.IsNullOrWhiteSpace(organization.Id))
            {
                throw new Exception("Organization ID is not valid.");
            }

            Project newProject = new Project(project.Name, project.Description, project.OwningOrganizationId ?? organization.Id, applicationUser.Id);

            foreach (string userId in project.ProjectUserIds)
            {
                newProject.ProjectUserIds.Add(userId);
            }
            foreach (string adminId in project.ProjectAdminUserIds)
            {
                newProject.ProjectAdminUserIds.Add(adminId);
            }
            foreach (string organizationId in project.OrganizationIds)
            {
                newProject.OrganizationIds.Add(organizationId);
            }

            if (_tenantDbContext == null)
            {
                throw new Exception("Tenant database context is not initialized.");
            }

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
            if (string.IsNullOrWhiteSpace(project.Id))
            {
                throw new Exception("Project ID is not valid.");
            }

            Project? existingProject = await GetProject(project.Id);
            if (existingProject == null)
            {
                throw new Exception($"Project {project.Id} does not exist.");
            }

            existingProject = project;

            if (_tenantDbContext == null)
            {
                throw new Exception("Tenant database context is not initialized.");
            }

            _tenantDbContext.Projects.Update(existingProject);
            await _tenantDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Could not update project.", ex);
        }
    }

    public async System.Threading.Tasks.Task DeleteProject(string projectId)
    {
        try
        {
            Project? existingProject = await GetProject(projectId);
            if (existingProject == null)
            {
                throw new Exception($"Project {projectId} does not exist.");
            }

            if (_tenantDbContext == null)
            {
                throw new Exception("Tenant database context is not initialized.");
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

    public void Dispose()
    {
        if (_tenantDbContext != null)
        {
            _tenantDbContext?.Dispose();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_tenantDbContext != null)
        {
            await _tenantDbContext.DisposeAsync();
        }
    }
}
