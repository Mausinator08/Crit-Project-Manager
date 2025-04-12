using CritBusinessLogic.RepositoryInterfaces;
using CritDataAccess.Contexts;
using CritDataAccess.Services;
using CritDTO.Identity;
using CritDTO.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CritBusinessLogic.Repositories;

public class TasksRepository : IDisposable, IAsyncDisposable, ITasksRepository
{
    private TenantDbContext? _tenantDbContext = null;
    private readonly IUserRepository _userRepository;
    public TasksRepository(ITenantDbContextService tenantDbContextService, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true)
        {
            Task<TenantDbContext> tenantDbContextTask = tenantDbContextService.GetAuthenticatedTenantDb(httpContextAccessor.HttpContext.User);
            tenantDbContextTask.Wait();
            _tenantDbContext = tenantDbContextTask.Result;
        }

        _userRepository = userRepository;
    }

    public async Task<List<ProjectTask>> GetAllTasks(string projectId)
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

            if (_tenantDbContext == null)
            {
                throw new Exception("Tenant database context is not initialized.");
            }

            IQueryable<ProjectTask> tasksQuery = _tenantDbContext.Tasks.AsNoTracking().Where(t =>
            t.ProjectId == projectId &&
            t.Project != null &&
            t.Project.ProjectUserIds.Contains(applicationUser.Id) &&
            t.Project.OrganizationIds.Contains(organization.Id!));

            if (tasksQuery.Any())
            {
                return await tasksQuery.ToListAsync();
            }

            return new List<ProjectTask>();
        }
        catch (Exception ex)
        {
            throw new Exception($"Could not get all tasks for project {projectId}.", ex);
        }
    }

    public async Task<ProjectTask?> GetTask(string projectId, string taskId)
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

            if (_tenantDbContext == null)
            {
                throw new Exception("Tenant database context is not initialized.");
            }

            IQueryable<ProjectTask> taskQuery = _tenantDbContext.Tasks.AsNoTracking().Where(t =>
            t.Id == taskId &&
            t.ProjectId == projectId &&
            t.Project != null &&
            t.Project.ProjectUserIds.Contains(applicationUser.Id) &&
            t.Project.OrganizationIds.Contains(organization.Id!));

            if (taskQuery.Any())
            {
                return await taskQuery.FirstAsync();
            }

            return null;
        }
        catch (Exception ex)
        {
            throw new Exception($"Could not get task {taskId} from project {projectId}.", ex);
        }
    }

    public async Task<ProjectTask?> CreateTask(ProjectTask task)
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

            if (_tenantDbContext == null)
            {
                throw new Exception("Tenant database context is not initialized.");
            }

            IQueryable<Project> projectQuery = _tenantDbContext.Projects.AsNoTracking().Where(p =>
            p.Id == task.ProjectId &&
            p.ProjectAdminUserIds.Contains(applicationUser.Id) &&
            p.OrganizationIds.Contains(organization.Id!));

            if (!projectQuery.Any())
            {
                throw new Exception($"User {applicationUser.Id} is not an admin of project {task.ProjectId} or {task.ProjectId} does not exist.");
            }

            _tenantDbContext.Tasks.Add(task);
            int savedChanges = await _tenantDbContext.SaveChangesAsync();

            if (savedChanges <= 0)
            {
                throw new Exception($"Task {task.Id} failed to create.");
            }

            return task;
        }
        catch (Exception ex)
        {
            throw new Exception($"Could not create task {task.Id} for project {task.ProjectId}.", ex);
        }
    }

    public async System.Threading.Tasks.Task UpdateTask(ProjectTask task)
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

            if (_tenantDbContext == null)
            {
                throw new Exception("Tenant database context is not initialized.");
            }

            IQueryable<Project> projectQuery = _tenantDbContext.Projects.AsNoTracking().Where(p =>
            p.Id == task.ProjectId &&
            p.ProjectAdminUserIds.Contains(applicationUser.Id) &&
            p.OrganizationIds.Contains(organization.Id!));

            if (!projectQuery.Any())
            {
                throw new Exception($"User {applicationUser.Id} is not an admin of project {task.ProjectId}.");
            }

            _tenantDbContext.Tasks.Update(task);
            int savedChanges = await _tenantDbContext.SaveChangesAsync();

            if (savedChanges <= 0)
            {
                throw new Exception($"Task {task.Id} failed to update.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Could not update task {task.Id} for project {task.ProjectId}.", ex);
        }
    }

    public async System.Threading.Tasks.Task DeleteTask(string projectId, string taskId)
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

            if (_tenantDbContext == null)
            {
                throw new Exception("Tenant database context is not initialized.");
            }

            IQueryable<Project> projectQuery = _tenantDbContext.Projects.AsNoTracking().Where(p =>
            p.Id == projectId &&
            p.ProjectAdminUserIds.Contains(applicationUser.Id) &&
            p.OrganizationIds.Contains(organization.Id!));

            if (!projectQuery.Any())
            {
                throw new Exception($"User {applicationUser.Id} is not an admin of project {projectId}.");
            }

            IQueryable<ProjectTask> taskQuery = _tenantDbContext.Tasks.AsNoTracking().Where(t => t.Id == taskId && t.ProjectId == projectId);

            if (!taskQuery.Any())
            {
                throw new Exception($"Task {taskId} does not exist in project {projectId}.");
            }

            _tenantDbContext.Tasks.Remove(taskQuery.First());
            int savedChanges = await _tenantDbContext.SaveChangesAsync();

            if (savedChanges <= 0)
            {
                throw new Exception($"Task {taskId} failed to delete.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Could not delete task {taskId} from project {projectId}.", ex);
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
