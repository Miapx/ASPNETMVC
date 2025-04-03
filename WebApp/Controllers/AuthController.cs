using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers;

public class AuthController(UserService userService, SignInManager<AppUser> signInManager) : Controller
{
    private readonly UserService _userService = userService;
    private readonly SignInManager<AppUser> _signInManager = signInManager;

    [HttpGet]
    //[Route("signin")]
    public IActionResult SignIn()
    {
        var model = new LoginFormModel();

        return View();
    }

    [HttpPost]
    //[Route("signin")]
    public async Task<IActionResult> SignIn(LoginFormModel formData)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(formData.Email, formData.Password, false, false);
            if (result.Succeeded)
                return RedirectToAction("Projects", "Projects");
        }

        ViewData["ErrorMessage"] = "Incorrect email or password";
        return View(formData);
    }


    [HttpGet]
    [Route("signup")]
    public IActionResult SignUp()
    {
        var model = new SignUpFormModel();
        return View(model);
    }

    [HttpPost]
    [Route("signup")]
    public async Task<IActionResult> SignUp(SignUpFormModel formData)
    {
        if (!ModelState.IsValid)
            return View(formData);

        if (await _userService.ExistsAsync(formData.Email))
        {
            ModelState.AddModelError("Exists", "User already exists");
            return View(formData);
        }

        var result = await _userService.CreateAsync(formData);

        if (result) 
            return RedirectToAction("signin", "Auth");

        ModelState.AddModelError("NotCreated", "User was not created");
        return View(formData);
    }

    public new async Task<IActionResult> SignOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("signin", "Auth");
    }
}
