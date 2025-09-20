using CritDTO.Models;

namespace CritBusinessLogic.RepositoryInterfaces;

public interface IStatusRepository
{
    Task<List<Status>> GetAllStatuses(Guid projectId);
    Task<Status> GetStatus(Guid statusId);
    Task<Status> CreateStatus(Status status);
    Task<Status> UpdateStatus(Status status);
    Task DeleteStatus(Guid statusId);
}
