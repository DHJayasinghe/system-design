namespace BnA.IAM.Presentation.API.Controllers.Account;

public class LogoutInputModel
{
    public string LogoutId { get; set; }
    public string PostLogoutRedirectUri { get; set; }
}