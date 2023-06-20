using System.ComponentModel.DataAnnotations;

namespace BnA.IAM.Presentation.API.Controllers.Account;

public class ForgotPasswordModel
{
    [Required]
    [EmailAddress]
    [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "The Email field is not a valid e-mail address.")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

}