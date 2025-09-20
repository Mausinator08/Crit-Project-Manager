using System.Security.Cryptography.X509Certificates;
using CritBusinessLogic.Models;
using CritBusinessLogic.RepositoryInterfaces;
using CritDataAccess.Contexts;
using CritDTO.Identity;
using CritDTO.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CritBusinessLogic.Repositories;

public class ProjectsRepository : IProjectsRepository
{
    private readonly CritDbContext _critDbContext;
    private readonly IUserRepository _userRepository;
    public ProjectsRepository(CritDbContext critDbContext, IUserRepository userRepository)
    {
        _critDbContext = critDbContext;
        _userRepository = userRepository;
    }

    public async Task<List<Project>> GetAllProjects()
    {
        try
        {
            Organization? organization = await _userRepository.GetLoggedInUserOrganization();
            if (organization == null || organization.Id == null || organization.Id == Guid.Empty)
            {
                throw new Exception("User is not a member of any organization and therefore cannot access projects.");
            }

            ApplicationUser? applicationUser = await _userRepository.GetLoggedInUser();
            if (applicationUser == null)
            {
                throw new Exception("User is not logged in.");
            }

            List<Project> projectsQuery = await _critDbContext.Projects.AsNoTracking().Where(p =>
            p.ProjectUsers.Any(pu => pu.UserId == applicationUser.Id) ||
            p.ProjectAdmins.Any(pu => pu.AdminId == applicationUser.Id) ||
            p.ProjectOwnerUserId == applicationUser.Id).ToListAsync();

            if (projectsQuery.Any())
            {
                return projectsQuery;
            }

            return new List<Project>();
        }
        catch (Exception ex)
        {
            throw new Exception("Could not get projects.", ex);
        }
    }

    public async Task<Project?> GetProject(Guid projectId)
    {
        try
        {
            Organization? organization = await _userRepository.GetLoggedInUserOrganization();
            if (organization == null || organization.Id == null || organization.Id == Guid.Empty)
            {
                throw new Exception("User is not a member of any organization and therefore cannot access projects.");
            }

            ApplicationUser? applicationUser = await _userRepository.GetLoggedInUser();
            if (applicationUser == null)
            {
                throw new Exception("User is not logged in.");
            }

            if (_critDbContext == null)
            {
                throw new Exception("Crit database context is not initialized.");
            }

            List<Project> projectQuery = await _critDbContext.Projects.AsNoTracking().Where(p =>
            p.Id == projectId &&
            (p.ProjectUsers.Any(pu => pu.UserId == applicationUser.Id) ||
            p.ProjectAdmins.Any(pu => pu.AdminId == applicationUser.Id) ||
            p.ProjectOwnerUserId == applicationUser.Id)).ToListAsync();

            if (projectQuery.Any())
            {
                return projectQuery.First();
            }

            return null;
        }
        catch (Exception ex)
        {
            throw new Exception($"Could not get project {projectId}.", ex);
        }
    }

    public async Task<Project> CreateProject(ProjectRequest project)
    {
        try
        {
            Organization? organization = await _userRepository.GetLoggedInUserOrganization();
            if (organization == null || organization.Id == null || organization.Id == Guid.Empty)
            {
                throw new Exception("User is not a member of any organization and therefore cannot create projects.");
            }

            ApplicationUser? applicationUser = await _userRepository.GetLoggedInUser();
            if (applicationUser == null)
            {
                throw new Exception("User is not logged in.");
            }

            if (organization.Id == null || organization.Id == Guid.Empty)
            {
                throw new Exception("Organization ID is not valid.");
            }

            Project newProject = new Project(project.Name, project.Description, project.OwningOrganizationId != null && project.OwningOrganizationId != Guid.Empty ? project.OwningOrganizationId.Value : organization.Id.Value, applicationUser.Id);

            if (_critDbContext == null)
            {
                throw new Exception("Crit database context is not initialized.");
            }

            EntityEntry<Project>? savedProj = _critDbContext.Projects.Add(newProject);

            if (savedProj == null || savedProj.Entity == null)
            {
                throw new Exception($"The project {newProject.Name} did not save correctly.");
            }

            if (savedProj.Entity.Id == null || savedProj.Entity.Id == Guid.Empty)
            {
                throw new Exception($"The project {newProject.Name} did not generate the Id correctly.");
            }


            _critDbContext.ProjectUsers.Add(new ProjectUser(savedProj.Entity.Id.Value, applicationUser.Id));
            _critDbContext.ProjectAdmins.Add(new ProjectAdmin(savedProj.Entity.Id.Value, applicationUser.Id));

            foreach (Guid userId in project.ProjectUserIds)
            {
                _critDbContext.ProjectUsers.Add(new ProjectUser(savedProj.Entity.Id.Value, userId));
            }

            foreach (Guid adminId in project.ProjectAdminUserIds)
            {
                _critDbContext.ProjectUsers.Add(new ProjectUser(savedProj.Entity.Id.Value, adminId));
            }

            _critDbContext.OrganizationProjects.Add(new OrganizationProject(savedProj.Entity.OwningOrganizationId, savedProj.Entity.Id.Value));

            foreach (Guid organizationId in project.OrganizationIds)
            {
                _critDbContext.OrganizationProjects.Add(new OrganizationProject(organizationId, savedProj.Entity.Id.Value));
            }

            int savedChanges = await _critDbContext.SaveChangesAsync();

            if (savedChanges <= 0)
            {
                throw new Exception("Project failed to save.");
            }

            return newProject;
        }
        catch (Exception ex)
        {
            throw new Exception("Could not create project.", ex);
        }
    }

    public async Task UpdateProject(Project project)
    {
        try
        {
            if (project.Id == null || project.Id == Guid.Empty)
            {
                throw new Exception("Project ID is not valid.");
            }

            Project? existingProject = await GetProject(project.Id.Value);
            if (existingProject == null)
            {
                throw new Exception($"Project {project.Id} does not exist.");
            }

            existingProject = project;

            if (_critDbContext == null)
            {
                throw new Exception("Crit database context is not initialized.");
            }

            _critDbContext.Projects.Update(existingProject);
            int savedChanges = await _critDbContext.SaveChangesAsync();

            if (savedChanges <= 0)
            {
                throw new Exception("Project failed to updated.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Could not update project.", ex);
        }
    }

    public async Task DeleteProject(Guid projectId)
    {
        try
        {
            Project? existingProject = await GetProject(projectId);
            if (existingProject == null)
            {
                throw new Exception($"Project {projectId} does not exist.");
            }

            if (_critDbContext == null)
            {
                throw new Exception("Crit database context is not initialized.");
            }

            _critDbContext.Projects.Remove(existingProject);
            int savedChanges = await _critDbContext.SaveChangesAsync();

            if (savedChanges <= 0)
            {
                throw new Exception("Project failed to delete.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Could not delete project.", ex);
        }
    }
}
