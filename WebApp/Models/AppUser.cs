using Microsoft.AspNetCore.Identity;

namespace WebApp.Models;

public class AppUser : IdentityUser
{
    [ProtectedPersonalData]
    public string FullName { get; set; } = null!;

}
