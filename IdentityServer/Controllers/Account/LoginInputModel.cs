using System.ComponentModel.DataAnnotations;

namespace BnA.IAM.Presentation.API.Controllers.Account;

public class LoginInputModel
{
    [Required(ErrorMessage ="Email address is required")]
    public string Username { get; set; }
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
    public bool RememberLogin { get; set; }
    public string ReturnUrl { get; set; }
}