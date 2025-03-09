using CritBusinessLogic.RepositoryInterfaces;
using CritDataAccess.Contexts;
using CritDataAccess.Services;
using CritDTO.Identity;
using CritDTO.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CritBusinessLogic.Repositories;

public class TasksRepository : ITasksRepository
{
    private readonly TenantDbContext _tenantDbContext;
    private readonly IUserRepository _userRepository;
    public TasksRepository(TenantDbContextService tenantDbContextService, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor.HttpContext != null)
        {
            Task<TenantDbContext> tenantDbContextTask = tenantDbContextService.GetAuthenticatedTenantDb(httpContextAccessor.HttpContext.User);
            tenantDbContextTask.Wait();
            _tenantDbContext = tenantDbContextTask.Result;
            _userRepository = userRepository;
        }
        else
        {
            throw new Exception("HttpContext is null.");
        }
    }

    public async Task<List<CritDTO.Models.Task>> GetAllTasks(Guid projectId)
    {
        try
        {
            IQueryable<CritDTO.Models.Task> tasksQuery = _tenantDbContext.Tasks.Where(t => t.ProjectId == projectId);

            if (tasksQuery.Any())
            {
                return await tasksQuery.ToListAsync();
            }

            return new List<CritDTO.Models.Task>();
        }
        catch (Exception ex)
        {
            throw new Exception($"Could not get all tasks for project {projectId}.", ex);
        }
    }

    public async Task<CritDTO.Models.Task?> GetTask(Guid projectId, Guid taskId)
    {
        try
        {
            IQueryable<CritDTO.Models.Task> taskQuery = _tenantDbContext.Tasks.Where(t => t.Id == taskId && t.ProjectId == projectId);

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

    public async Task<CritDTO.Models.Task?> CreateTask(CritDTO.Models.Task task)
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

            IQueryable<Project> projectQuery = _tenantDbContext.Projects.Where(p =>
            p.Id == task.ProjectId &&
            p.ProjectAdminUserIds.Contains(applicationUser.Id));

            if (!projectQuery.Any())
            {
                throw new Exception($"User {applicationUser.Id} is not an admin of project {task.ProjectId}.");
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

    public async System.Threading.Tasks.Task UpdateTask(CritDTO.Models.Task task)
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

            IQueryable<Project> projectQuery = _tenantDbContext.Projects.Where(p =>
            p.Id == task.ProjectId &&
            p.ProjectAdminUserIds.Contains(applicationUser.Id));

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

    public async System.Threading.Tasks.Task DeleteTask(Guid projectId, Guid taskId)
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

            IQueryable<Project> projectQuery = _tenantDbContext.Projects.Where(p =>
            p.Id == projectId &&
            p.ProjectAdminUserIds.Contains(applicationUser.Id));

            if (!projectQuery.Any())
            {
                throw new Exception($"User {applicationUser.Id} is not an admin of project {projectId}.");
            }

            IQueryable<CritDTO.Models.Task> taskQuery = _tenantDbContext.Tasks.Where(t => t.Id == taskId && t.ProjectId == projectId);

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
}
