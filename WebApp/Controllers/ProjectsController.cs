using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Services;
using WebApp.ViewModels;

namespace WebApp.Controllers;

[Authorize]
public class ProjectsController(ProjectService projectService, StatusService statusService) : Controller
{
    private readonly ProjectService _projectService = projectService;
    private readonly StatusService _statusService = statusService;

    public async Task<IActionResult> Projects()
    {
        var vm = new ProjectsViewModel
        {
            Projects = await _projectService.GetAllProjectsAsync(),
        };

        return View(vm);
    }

    public async Task<IActionResult> GetProject(string id)
    {
        var project = await _projectService.GetAsync(id);
        if (project == null) return NotFound();

        var model = new EditProjectFormModel
        {
            ProjectId = project.Id,
            ProjectName = project.ProjectName,
            ClientName = project.ClientName,
            Description = project.Description,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            Budget = project.Budget
        };

        return Json(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddProject(AddProjectFormModel formData)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToList()
                );
            return BadRequest(new { success = false, errors });
        }

        ViewBag.Description = formData.Description;

        var result = await _projectService.CreateProjectAsync(formData);
        if (result)
        {
            return Ok(new { success = true });
        }
        else
        {
            return Problem("Unable to submit data");
        }
    }

    [HttpGet]
    public async Task<IActionResult> EditProject(string id)
    {
        var project = await _projectService.GetAsync(id);

        if (project == null || project.Status == null)
        {
            ViewData["ErrorMessage"] = "Could not find status.";
            return PartialView("Partials/_EditProjectModal", null);
        }

        var model = new Project
        {
            Id = project.Id,
            ProjectName = project.ProjectName,
            ClientName = project.ClientName,
            Description = project.Description,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            Budget = project.Budget,
            Status = project.Status
        };

        return PartialView("Partials/_EditProjectModal", model);
    }

    [HttpPost]
    public async Task<IActionResult> EditProject(EditProjectFormModel editFormData)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToList()
                );
            return BadRequest(new { success = false, errors });
        }

        var project = await _projectService.EditAsync(editFormData);
        return Ok(new { success = true });
    }

    [HttpPost]
    [Route("Projects/DeleteProject/{id}")]
    public IActionResult DeleteProject(string id)
    {
        var success = _projectService.DeleteProject(id);
        if (success)
        {
            return Ok();
        }
        return NotFound();
    }

    //För att filtrera på status
    public async Task<IActionResult> ProjectsByStatus(string statusName)
    {
        var status = await _statusService.GetStatusByNameAsync(statusName);
        if (status == null)
        {
            TempData["ErrorMessage"] = $"Status '{statusName}' not found.";
            return RedirectToAction("Projects"); 
        }

        var statusId = status.Id;

        var vm = new ProjectsViewModel
        {
            Projects = await _projectService.GetProjectsByStatusAsync(statusId)
        };

        return View("Projects", vm); 
    }
}



