using System;
using System.ComponentModel.DataAnnotations;

namespace BnA.IAM.Presentation.API.Controllers.Account;

public class UserCreateModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }  
    public string PhoneNumber { get; set; }

    public Guid OrganizationId { get; set; }

    public Guid? CountryId { get; set; }
    public string Role { get; set; }
}