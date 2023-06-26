using System;

namespace BnA.IAM.Presentation.API.Controllers.Account;

public class UserRoleUpdateModel
{
    public Guid UserId { get; set; }
    public Guid OrganizationId { get; set; }
    public string Role { get; set; }
}