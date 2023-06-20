using BnA.IAM.Domain.Enums;
using BnA.PM.MasterData.Domain;
using BnA.PM.SharedKernel.Helpers;
using BnA.PM.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BnA.IAM.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>, IAggregateRoot
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ProfileImageUrl { get; set; }
    public Guid? PreferredCountryId { get; private set; }
    public Country PreferredCountry { get; private set; }
    public ActivationStatus ActivationStatus { get; private set; }
    private readonly List<ApplicationUserRole> _roles = new();
    public IReadOnlyList<ApplicationUserRole> Roles => _roles.ToList();

    public int DefultTimeZoneDiffMinutes { get; private set; }
    public DateTime CreatedDateTime { get; private set; }
    public DateTime LastLoggedInDateTime { get; private set; }

    public string GetFullName()
    {
        if (FirstName is null && LastName is null) return null;
        return $"{FirstName} {LastName}".Trim();
    }

    public List<ApplicationRole> GetUserGroupsByOrganizationId(Guid? organizationId) => Roles
        .Where(assignedGroup => assignedGroup.ApplicationRole.IsSuperAdmin()
            || !organizationId.HasValue
            || assignedGroup.OrganizationId == organizationId.Value)
        .Select(assignedGroup => assignedGroup.ApplicationRole)
        .ToList();

    public Guid? GetDefaultOrganizationId() => Roles.Where(assignedGroup => assignedGroup.ApplicationRole.IsPropertyManager()).FirstOrDefault()?.OrganizationId;

    public bool IsAccountLockedOut() => LockoutEnd != null && LockoutEnd.Value.UtcDateTime > DateTimeProvider.UtcNow;

    public void RecordLastLoggedInTime() =>
        LastLoggedInDateTime = DateTimeProvider.UtcNow;

    public bool IsSuperAdmin() => _roles.Any(assignedGroup => assignedGroup.ApplicationRole.IsSuperAdmin());
    public string StripeCustomerRef { get; set; }
    public string StripeDefaultSourceId { get; set; }
    public string BusinessName { get; set; }
    public string HomePhoneNumber { get; set; }
    public string OfficePhoneNumber { get; set; }

    public bool IsAccountActive() => 
        !new[] { ActivationStatus.Suspended, ActivationStatus.PendingApproval, ActivationStatus.Deactivated }.Contains(ActivationStatus);

    public string CustomInvitationNote { get;  set; }
    public bool ProfileImageChanged { get;  set; }
    public long StripePaymentAttempts { get; set; }
    public Guid? SettingId { get; set; }
    public void AssignUserGroup(ApplicationUserRole applicationUserRole)
    {
        _roles.Add(applicationUserRole);
    }
    public void RemoveUserGroup(ApplicationUserRole applicationUserRole)
    {
        _roles.Remove(applicationUserRole);
    }

    private ApplicationUser() { }

    public ApplicationUser(ActivationStatus activationStatus,Guid? preferredCountryId)
    {
        ActivationStatus = activationStatus;
        PreferredCountryId = preferredCountryId;
    }
}
