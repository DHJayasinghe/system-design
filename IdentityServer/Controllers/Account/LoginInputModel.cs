using System.ComponentModel.DataAnnotations;

namespace BnA.IAM.Presentation.API.Controllers.Account;

public class LoginInputModel
***REMOVED***
    [Required(ErrorMessage ="Email address is required")]
    public string Username ***REMOVED*** get; set; ***REMOVED***
    [Required(ErrorMessage = "Password is required")]
    public string Password ***REMOVED*** get; set; ***REMOVED***
    public bool RememberLogin ***REMOVED*** get; set; ***REMOVED***
    public string ReturnUrl ***REMOVED*** get; set; ***REMOVED***
***REMOVED***