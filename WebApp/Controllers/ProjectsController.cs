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

        //Skicka data till vår service
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


    [HttpPost]
    public IActionResult EditProject(EditProjectFormModel editFormData)
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

        //Skicka data till vår service
        //var result = await _projectService.UpdateProjectAsync(editFormData)
        //if (result) { return Ok( new { success = true });
        //else { return Problem("Unable to submit data") }
        //Ta bort nedan return OK()

        return Ok();
    }

    //Lägg till för att omdirigera utloggning till förstasidan
    //IActionResult SignOut() { return RedirectToAction("Index", "Home"}
}

