using CritDTO.Models;

namespace CritBusinessLogic.RepositoryInterfaces;

public interface IPriorityRepository
{
    Task<List<Priority>> GetAllPriorities(string projectId);
    Task<Priority> GetPriority(string priorityId);
    Task<Priority> CreatePriority(Priority priority);
    Task<Priority> UpdatePriority(Priority priority);
    Task DeletePriority(string priorityId);
}
