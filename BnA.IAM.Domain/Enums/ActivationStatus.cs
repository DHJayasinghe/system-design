using System.ComponentModel;

namespace BnA.IAM.Domain.Enums;

public enum ActivationStatus
{
    [Description("Pending Approval")]
    PendingApproval = 1,
    [Description("Active")]
    Approved = 2,
    [Description("Suspended")]
    Suspended = 3,
    [Description("Expired / Deactivated")]
    Expired = 4,
    [Description("Pending Payment")]
    PaymentPending = 5,
    [Description("Pending Mobile Number Verification")]
    MobileVerificationPending = 6,
    [Description("Account Deactivated")]
    Deactivated = 7,
    [Description("Not Registered")]
    NotRegistered = 8
}
