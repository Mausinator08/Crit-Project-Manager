namespace CritBusinessLogic;

public interface ITasksRepository
{
    Task<List<CritDTO.Models.Task>> GetAllTasks(Guid projectId);
    Task<CritDTO.Models.Task?> GetTask(Guid projectId, Guid taskId);
    Task<CritDTO.Models.Task?> CreateTask(CritDTO.Models.Task task);
    System.Threading.Tasks.Task UpdateTask(CritDTO.Models.Task task);
    System.Threading.Tasks.Task DeleteTask(Guid projectId, Guid taskId);
}
