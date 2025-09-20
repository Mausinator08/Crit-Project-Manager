using CritBusinessLogic.Models;
using CritDTO.Models;

namespace CritBusinessLogic.RepositoryInterfaces;

public interface IProjectsRepository
{
    Task<List<Project>> GetAllProjects();
    Task<Project?> GetProject(Guid projectId);
    Task<Project> CreateProject(ProjectRequest project);
    Task UpdateProject(Project project);
    Task DeleteProject(Guid projectId);
}
