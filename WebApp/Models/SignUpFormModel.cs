using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class SignUpFormModel
{
    [Display(Name = "Name", Prompt = "Enter your name")]
    [Required(ErrorMessage = "Required")]
    public string FullName { get; set; } = null!;

    [Display(Name = "Email", Prompt = "Enter your email")]
    [Required(ErrorMessage = "Required")]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "You must enter a valid email")]
    public string Email { get; set; } = null!;

    [Display(Name = "Password", Prompt = "Enter your password")]
    [Required(ErrorMessage = "Required")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-ö])(?=.*[A-Ö])(?=.*\d)(?=.*[\W_]).{8,}$", ErrorMessage = "Enter a strong password")]
    //Minst ett litet tecken a-ö + stort tecken A-Ö, minst en siffra 0-9, minst ett specialtecken, minst 8 tecken
    public string Password { get; set; } = null!;

    [Display(Name = "Confirm password", Prompt = "Enter your password again")]
    [Required(ErrorMessage = "Required")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = null!;

    
    [Range(typeof(bool), "true", "true", ErrorMessage = "Required") ]
    public bool AcceptTC { get; set; } = false;
}


