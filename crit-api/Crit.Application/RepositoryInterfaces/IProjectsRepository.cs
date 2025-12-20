using Crit.Application.Models;
using Crit.Contracts.Models;

namespace Crit.Application.RepositoryInterfaces;

public interface IProjectsRepository
{
    Task<List<Project>> GetAllProjects();
    Task<Project?> GetProject(Guid projectId);
    Task<Project> CreateProject(ProjectRequest project);
    Task UpdateProject(Project project);
    Task DeleteProject(Guid projectId);
}
