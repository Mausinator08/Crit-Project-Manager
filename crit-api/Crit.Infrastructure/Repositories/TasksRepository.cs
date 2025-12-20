using Crit.Application.RepositoryInterfaces;
using Crit.Domain.Contexts;
using Crit.Domain.Entities;
using Crit.Domain.Identity;
using Microsoft.EntityFrameworkCore;

namespace Crit.Infrastructure.Repositories;

public class TasksRepository : ITasksRepository
{
    private CritDbContext? _critDbContext = null;
    private readonly IUserRepository _userRepository;
    public TasksRepository(CritDbContext critDbContext, IUserRepository userRepository)
    {
        _critDbContext = critDbContext;
        _userRepository = userRepository;
    }

    public async Task<List<ProjectTask>> GetAllTasks(Guid projectId)
    {
        try
        {
            Organization? organization = await _userRepository.GetLoggedInUserOrganization();
            if (organization == null || organization.Id == null || organization.Id == Guid.Empty)
            {
                throw new Exception("User is not a member of any organization and therefore cannot create projects.");
            }

            ApplicationUser? applicationUser = await _userRepository.GetLoggedInUser();
            if (applicationUser == null)
            {
                throw new Exception("User is not logged in.");
            }

            if (_critDbContext == null)
            {
                throw new Exception("Tenant database context is not initialized.");
            }

            List<ProjectTask> tasksQuery = await _critDbContext.Tasks.AsNoTracking().Where(t =>
            t.ProjectId == projectId &&
            t.Project != null &&
            t.Project.ProjectUsers.Any(pu => pu.UserId == applicationUser.Id) &&
            t.Project.OrganizationProjects.Any(op => op.OrganizationId == organization.Id)).ToListAsync();

            if (tasksQuery.Any())
            {
                return tasksQuery;
            }

            return new List<ProjectTask>();
        }
        catch (Exception ex)
        {
            throw new Exception($"Could not get all tasks for project {projectId}.", ex);
        }
    }

    public async Task<ProjectTask?> GetTask(Guid projectId, Guid taskId)
    {
        try
        {
            Organization? organization = await _userRepository.GetLoggedInUserOrganization();
            if (organization == null || organization.Id == null || organization.Id == Guid.Empty)
            {
                throw new Exception("User is not a member of any organization and therefore cannot create projects.");
            }

            ApplicationUser? applicationUser = await _userRepository.GetLoggedInUser();
            if (applicationUser == null)
            {
                throw new Exception("User is not logged in.");
            }

            if (_critDbContext == null)
            {
                throw new Exception("Tenant database context is not initialized.");
            }

            List<ProjectTask> taskQuery = await _critDbContext.Tasks.AsNoTracking().Where(t =>
            t.Id == taskId &&
            t.ProjectId == projectId &&
            t.Project != null &&
            t.Project.ProjectUsers.Any(pu => pu.UserId == applicationUser.Id) &&
            t.Project.OrganizationProjects.Any(op => op.OrganizationId == organization.Id)).ToListAsync();

            if (taskQuery.Any())
            {
                return taskQuery.First();
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
            if (organization == null || organization.Id == null || organization.Id == Guid.Empty)
            {
                throw new Exception("User is not a member of any organization and therefore cannot create projects.");
            }

            ApplicationUser? applicationUser = await _userRepository.GetLoggedInUser();
            if (applicationUser == null)
            {
                throw new Exception("User is not logged in.");
            }

            if (_critDbContext == null)
            {
                throw new Exception("Tenant database context is not initialized.");
            }

            List<Project> projectQuery = await _critDbContext.Projects.AsNoTracking().Where(p =>
            p.Id == task.ProjectId &&
            p.ProjectAdmins.Any(pa => pa.AdminId == applicationUser.Id) &&
            p.OrganizationProjects.Any(op => op.OrganizationId == organization.Id)).ToListAsync();

            if (!projectQuery.Any())
            {
                throw new Exception($"User {applicationUser.Id} is not an admin of project {task.ProjectId} or {task.ProjectId} does not exist.");
            }

            _critDbContext.Tasks.Add(task);
            int savedChanges = await _critDbContext.SaveChangesAsync();

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

    public async Task UpdateTask(ProjectTask task)
    {
        try
        {
            Organization? organization = await _userRepository.GetLoggedInUserOrganization();
            if (organization == null || organization.Id == null || organization.Id == Guid.Empty)
            {
                throw new Exception("User is not a member of any organization and therefore cannot create projects.");
            }

            ApplicationUser? applicationUser = await _userRepository.GetLoggedInUser();
            if (applicationUser == null)
            {
                throw new Exception("User is not logged in.");
            }

            if (_critDbContext == null)
            {
                throw new Exception("Tenant database context is not initialized.");
            }

            List<Project> projectQuery = await _critDbContext.Projects.AsNoTracking().Where(p =>
            p.Id == task.ProjectId &&
            p.ProjectAdmins.Any(pa => pa.AdminId == applicationUser.Id) &&
            p.OrganizationProjects.Any(op => op.OrganizationId == organization.Id)).ToListAsync();

            if (!projectQuery.Any())
            {
                throw new Exception($"User {applicationUser.Id} is not an admin of project {task.ProjectId}.");
            }

            _critDbContext.Tasks.Update(task);
            int savedChanges = await _critDbContext.SaveChangesAsync();

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

    public async Task DeleteTask(Guid projectId, Guid taskId)
    {
        try
        {
            Organization? organization = await _userRepository.GetLoggedInUserOrganization();
            if (organization == null || organization.Id == null || organization.Id == Guid.Empty)
            {
                throw new Exception("User is not a member of any organization and therefore cannot create projects.");
            }

            ApplicationUser? applicationUser = await _userRepository.GetLoggedInUser();
            if (applicationUser == null)
            {
                throw new Exception("User is not logged in.");
            }

            if (_critDbContext == null)
            {
                throw new Exception("Tenant database context is not initialized.");
            }

            List<Project> projectQuery = await _critDbContext.Projects.AsNoTracking().Where(p =>
            p.Id == projectId &&
            p.ProjectAdmins.Any(pa => pa.AdminId == applicationUser.Id) &&
            p.OrganizationProjects.Any(op => op.OrganizationId == organization.Id)).ToListAsync();

            if (!projectQuery.Any())
            {
                throw new Exception($"User {applicationUser.Id} is not an admin of project {projectId}.");
            }

            List<ProjectTask> taskQuery = await _critDbContext.Tasks.AsNoTracking().Where(t => t.Id == taskId && t.ProjectId == projectId).ToListAsync();

            if (!taskQuery.Any())
            {
                throw new Exception($"Task {taskId} does not exist in project {projectId}.");
            }

            _critDbContext.Tasks.Remove(taskQuery.First());
            int savedChanges = await _critDbContext.SaveChangesAsync();

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
