using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

// Lägg till alla sidor/endpoints/actions här (eller gör ny controller) 
// Högerklicka View och Add view (razor empty) - där skrivs razorkoden.
// Index ska vara login, sen behövs Create account och portalen. 

public class ProjectsController : Controller

// Ändra sökvägsnamnet = [Route("")]

{
    [Route("projects")]
    public IActionResult Projects()
    {
        //Kopplingen till modellen(med listan av projekt): -EJ NÖDVÄNDIG! Kör servicen i Index direkt istället
        //var viewModel = new HomeViewModel();

        return View(/*viewModel*/);
    }

    //Lägg till för att omdirigera utloggning till förstasidan
    //IActionResult SignOut() { return RedirectToAction("Index", "Home"}
}
