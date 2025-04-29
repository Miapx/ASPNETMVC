using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
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

        // Sätt status baserat på start/slutdatum
        var today = DateTime.Today;

        string statusName;

        if (today >= formData.StartDate && today <= formData.EndDate)
            statusName = "Started";
        else
            statusName = "Completed";

        // Hämta statusobjektet från databasen
        var status = await _statusService.GetStatusByNameAsync(statusName);

        if (status == null)
            return false;

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

    public async Task<Project> GetAsync(string id)
    {
        var entity = await _projectRepository.GetAsync(x => x.Id == id);

        if (entity == null || entity.Status == null)
            return null!;

        return new Project
        {
            Id = entity.Id,
            ProjectName = entity.ProjectName,
            ClientName = entity.ClientName,
            Description = entity.Description,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            Budget = entity.Budget,
            Status = new Status
            {
                Id = entity.Status.Id,
                StatusName = entity.Status.StatusName
            }
        };
    }

    public async Task<bool> EditAsync(EditProjectFormModel formData)
    {
        var project = await _projectRepository.GetAsync(x => x.Id == formData.ProjectId);
        if (project == null)
            return false;

        project.ProjectName = formData.ProjectName;
        project.ClientName = formData.ClientName;
        project.Description = formData.Description;
        project.StartDate = formData.StartDate;
        project.EndDate = formData.EndDate;
        project.Budget = formData.Budget;

        var today = DateTime.Today;
        var start = formData.StartDate.Date;
        var end = formData.EndDate.Date;

        int statusId;

        if (today >= start && today <= end)
            statusId = 1; // Started
        else if (today < start)
            statusId = 0; // Planned
        else
            statusId = 2; // Completed

        var status = await _statusService.GetStatusByIdAsync(statusId);
        project.StatusId = status.Id;

        var result = await _projectRepository.UpdateAsync(project);
        return result;
    }



    //För att hämta STARTED - ChatGPT

    public async Task<List<Project>> GetProjectsByStatusAsync(int status)
    {
        var projectEntities = await _projectRepository.GetAllAsync(
            orderByDescending: true,
            sortBy: x => x.Created,
            where: pe => pe.Status.Id == status, // Filtrera baserat på status
            include => include.Status
            );

        return projectEntities.Select(pe => new Project
        {
            Id = pe.Id,
            ProjectName = pe.ProjectName,
            ClientName = pe.ClientName,
            Description = pe.Description,
            StartDate = pe.StartDate,
            EndDate = pe.EndDate,
            Budget = pe.Budget,
            Status = new Status
            {
                Id = pe.Status.Id,
                StatusName = pe.Status.StatusName
            }
        }).ToList();


    }


    //ChatGpt slut

    //DELETE
    public bool DeleteProject(string id)
    {
        var project = _projectRepository.GetById(id);
        if (project == null)
        {
            return false;
        }

        _projectRepository.Delete(project);
        return true;
    }
}


