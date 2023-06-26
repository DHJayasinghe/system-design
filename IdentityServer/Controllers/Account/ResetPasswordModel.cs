using System.ComponentModel.DataAnnotations;

namespace BnA.IAM.Presentation.API.Controllers.Account;

public class ResetPasswordModel
***REMOVED***
    [Required]
    [EmailAddress]
    public string Email ***REMOVED*** get; set; ***REMOVED***

    [Required]
    [StringLength(100, ErrorMessage = "The ***REMOVED***0***REMOVED*** must be at least ***REMOVED***2***REMOVED*** and at max ***REMOVED***1***REMOVED*** characters long.", MinimumLength = 8)]
    [DataType(DataType.Password)]
    [RegularExpression(@"(?=^.***REMOVED***8,***REMOVED***$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\s)[0-9a-zA-Z!@#$%^&*()]*$", ErrorMessage = "Your password must contain minimum of 8 characters in length with at least 1 lowercase letter, 1 uppercase letter, 1 numeric character  and a special character.")]
    public string Password ***REMOVED*** get; set; ***REMOVED***

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    [RegularExpression(@"(?=^.***REMOVED***8,***REMOVED***$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\s)[0-9a-zA-Z!@#$%^&*()]*$", ErrorMessage = "Your password must contain minimum of 8 characters in length with at least 1 lowercase letter, 1 uppercase letter, 1 numeric character  and a special character.")]
    public string ConfirmPassword ***REMOVED*** get; set; ***REMOVED***

    public string Code ***REMOVED*** get; set; ***REMOVED***

***REMOVED***