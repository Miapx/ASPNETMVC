using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Services;

public class UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
{
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly SignInManager<AppUser> _signInManager = signInManager;

    public async Task<bool> ExistsAsync(string email)
    {
        if (await _userManager.Users.AnyAsync(u => u.Email == email))
            return true;

        return false;
    }

    public async Task<bool> CreateAsync(SignUpFormModel form)
    {
        if (form != null)
        {
            var appUser = new AppUser
            {
                FullName = form.FullName,
                Email = form.Email,
                UserName = form.Email 
            };

            var result = await _userManager.CreateAsync(appUser, form.Password);
            return result.Succeeded;
        }
            return false;
    }
}
