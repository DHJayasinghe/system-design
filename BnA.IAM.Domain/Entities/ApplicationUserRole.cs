using BnA.PM.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;

namespace BnA.IAM.Domain.Entities;

public class ApplicationUserRole : IdentityUserRole<Guid>
{
    public Guid Id { get; set; }
    public ApplicationRole ApplicationRole { get; set; }

    public Guid? OrganizationId { get; set; }
}
