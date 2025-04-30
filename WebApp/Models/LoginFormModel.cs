using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class LoginFormModel
{
    [Display(Name = "Email", Prompt = "Enter your email")]
    [Required(ErrorMessage = "Required")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Display(Name = "Password", Prompt = "Enter your password")]
    [Required(ErrorMessage = "Required")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Range(typeof(bool), "false", "true")]
    public bool RememberMe { get; set; } = false;
}
