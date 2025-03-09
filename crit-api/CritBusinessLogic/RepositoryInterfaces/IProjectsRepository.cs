using CritBusinessLogic.Models;
using CritDTO.Models;

namespace CritBusinessLogic.RepositoryInterfaces;

public interface IProjectsRepository
{
    Task<List<Project>> GetAllProjects();
    Task<Project?> GetProject(Guid projectId);
    Task<Project> CreateProject(ProjectRequest project);
    System.Threading.Tasks.Task UpdateProject(Project project);
    System.Threading.Tasks.Task DeleteProject(Guid projectId);
}
