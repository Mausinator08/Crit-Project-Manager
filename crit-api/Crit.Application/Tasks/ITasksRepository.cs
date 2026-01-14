using Crit.Abstractions.Models;

namespace Crit.Application.RepositoryInterfaces;

public interface ITasksRepository
{
    Task<List<ProjectTask>> GetAllTasks(Guid projectId);
    Task<ProjectTask?> GetTask(Guid projectId, Guid taskId);
    Task<ProjectTask?> CreateTask(ProjectTask task);
    Task UpdateTask(ProjectTask task);
    Task DeleteTask(Guid projectId, Guid taskId);
}
