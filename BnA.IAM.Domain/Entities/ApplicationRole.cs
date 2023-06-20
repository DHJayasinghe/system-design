using BnA.PM.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;

namespace BnA.IAM.Domain.Entities;

public class ApplicationRole : IdentityRole<Guid>, IAggregateRoot
{
    public const string SuperAdminRole = "SuperAdmins";
    public static readonly string[] PropertyManagerRoles = new string[] { "PropertyManagers", "OrganizationAdmins", "LeadOrganizationAdmins" };
    public ApplicationRole() : base() { }
    public ApplicationRole(string roleName) : base(roleName) { }

    public bool IsSuperAdmin() => Name == SuperAdminRole;
    public bool IsPropertyManager() => PropertyManagerRoles.Contains(Name);
}
