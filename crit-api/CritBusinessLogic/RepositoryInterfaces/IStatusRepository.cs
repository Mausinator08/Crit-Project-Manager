using CritDTO.Models;

namespace CritBusinessLogic.RepositoryInterfaces;

public interface IStatusRepository
{
    Task<List<Status>> GetAllStatuses(string projectId);
    Task<Status> GetStatus(string statusId);
    Task<Status> CreateStatus(Status status);
    Task<Status> UpdateStatus(Status status);
    Task DeleteStatus(string statusId);
}
