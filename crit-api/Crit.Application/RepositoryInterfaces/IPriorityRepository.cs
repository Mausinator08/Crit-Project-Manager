using Crit.Contracts.Models;

namespace Crit.Application.RepositoryInterfaces;

public interface IPriorityRepository
{
    Task<List<Priority>> GetAllPriorities(Guid projectId);
    Task<Priority> GetPriority(Guid priorityId);
    Task<Priority> CreatePriority(Priority priority);
    Task<Priority> UpdatePriority(Priority priority);
    Task DeletePriority(Guid priorityId);
}
