using System.ComponentModel.DataAnnotations;

namespace BnA.IAM.Presentation.API.Controllers.Account;

public class ResetPasswordModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
    [DataType(DataType.Password)]
    [RegularExpression(@"(?=^.{8,}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\s)[0-9a-zA-Z!@#$%^&*()]*$", ErrorMessage = "Your password must contain minimum of 8 characters in length with at least 1 lowercase letter, 1 uppercase letter, 1 numeric character  and a special character.")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    [RegularExpression(@"(?=^.{8,}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\s)[0-9a-zA-Z!@#$%^&*()]*$", ErrorMessage = "Your password must contain minimum of 8 characters in length with at least 1 lowercase letter, 1 uppercase letter, 1 numeric character  and a special character.")]
    public string ConfirmPassword { get; set; }

    public string Code { get; set; }

}