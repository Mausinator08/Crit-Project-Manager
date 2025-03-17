using CritDTO.Models;

namespace CritBusinessLogic;

public interface ITasksRepository
{
    Task<List<ProjectTask>> GetAllTasks(string projectId);
    Task<ProjectTask?> GetTask(string projectId, string taskId);
    Task<ProjectTask?> CreateTask(ProjectTask task);
    Task UpdateTask(ProjectTask task);
    Task DeleteTask(string projectId, string taskId);
}
