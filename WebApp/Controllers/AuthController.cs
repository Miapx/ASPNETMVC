using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

public class AuthController : Controller
{
    [HttpGet]
    [Route("signin")]
    public IActionResult SignIn()
    {
        var model = new LoginFormModel();

        return View();
    }

    [HttpPost]
    [Route("signin")]
    public IActionResult SignIn(LoginFormModel formData)
    {
        if (!ModelState.IsValid)
            return View(formData);

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
    public IActionResult SignUp(SignUpFormModel formData)
    {
        if (!ModelState.IsValid)
            return View(formData);

        // usermanager med identity 
        // redirect to login?

        return View();
    }


    //Skapa SignOut här
}
