using System.ComponentModel.DataAnnotations;

namespace BnA.IAM.Presentation.API.Controllers.Account;

public class ForgotPasswordModel
***REMOVED***
    [Required]
    [EmailAddress]
    [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]***REMOVED***2,4***REMOVED***$", ErrorMessage = "The Email field is not a valid e-mail address.")]
    [DataType(DataType.EmailAddress)]
    public string Email ***REMOVED*** get; set; ***REMOVED***

***REMOVED***