using Crit.Abstractions.Models;

namespace Crit.Application.RepositoryInterfaces;

public interface IProjectsRepository
{
    Task<List<Project>> GetAllProjects();
    Task<Project?> GetProject(Guid projectId);
    Task<Project> CreateProject(Project project);
    Task UpdateProject(Project project);
    Task DeleteProject(Guid projectId);
}
