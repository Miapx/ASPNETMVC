using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers;

// Lägg till alla sidor/endpoints/actions här (eller gör ny controller) 
// Högerklicka View och Add view (razor empty) - där skrivs razorkoden.
// Index ska vara login, sen behövs Create account och portalen. 

[Authorize]
public class ProjectsController(ProjectService projectService) : Controller
{
    private readonly ProjectService _projectService = projectService;

    public IActionResult Projects()
    {
        var formData = new AddProjectFormModel();
        var editFormData = new EditProjectFormModel();


        return View();
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


    //ChatGPT för att hämta projektinfo
    //Tror jag ska mappa till Project istället för EditProjectFormModel
    //Och sedan i EditProjectModal ta in en Project istället för EditProjectFormModel

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


    //ChatGPT slut




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
}

