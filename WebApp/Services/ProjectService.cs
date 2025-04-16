using WebApp.Data.Entities;
using WebApp.Data.Repositories;
using WebApp.Models;

namespace WebApp.Services;

public class ProjectService(ProjectRepository projectRepository, StatusService statusService)
{
   private readonly ProjectRepository _projectRepository = projectRepository;
    private readonly StatusService _statusService = statusService;

    public async Task<bool> CreateProjectAsync(AddProjectFormModel formData)
    {
        var project = await _projectRepository.GetAsync(x => x.ProjectName == formData.ProjectName);
        if (project != null) 
            return false;

        project = new ProjectEntity
        {
            ProjectName = formData.ProjectName,
            Description = formData.Description,
            ClientName = formData.ClientName,
            StartDate = formData.StartDate,
            EndDate = formData.EndDate,
            Budget = formData.Budget
        };

        //Sätter status HUUUUUR ????? Hans sätt:
        //var status = await _statusService.GetStatusByIdAsync(1);
        //project.StatusId = status.Id;

        //ChatGPT:
        // Sätt status baserat på start/slutdatum
        var today = DateTime.Today;

        string statusName;

        if (today >= formData.StartDate && today <= formData.EndDate)
            statusName = "Started";
        else
            statusName = "Completed";

        // Hämta statusobjektet från databasen
        var status = await _statusService.GetStatusByNameAsync(statusName);
        project.StatusId = status.Id;


        var result = await _projectRepository.CreateAsync(project);
        return result;
    }

    public async Task<IEnumerable<Project>> GetAllProjectsAsync()
    {
        var entities = await _projectRepository.GetAllAsync
            (
            orderByDescending: true,
            sortBy: x => x.Created,
            where: null,
            include => include.Status
            );

        var result = entities.Select(x => new Project
        {
            Id = x.Id,
            ProjectName = x.ProjectName,
            ClientName = x.ClientName,
            Description = x.Description,
            StartDate = x.StartDate,
            EndDate = x.EndDate,
            Budget = x.Budget,
            Status = new Status
            {
                Id = x.Status.Id,
                StatusName = x.Status.StatusName
            }
        });

        return result;
        
    }
}
