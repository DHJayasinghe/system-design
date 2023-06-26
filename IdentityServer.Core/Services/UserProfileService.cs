using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.Extensions.Logging;
***REMOVED***
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BnA.IAM.Application.Services;

/// <summary>
/// Profile service for users
/// </summary>
/// <seealso cref="IProfileService" />
public sealed class UserProfileService : IProfileService
***REMOVED***
    private readonly ILogger<UserProfileService> _logger;

    public UserProfileService(ILogger<UserProfileService> logger)
***REMOVED***
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    ***REMOVED***

    /// <summary>
    /// This method is called whenever claims about the user are requested (e.g. during token creation or via the userinfo endpoint)
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
***REMOVED***
        context.LogProfileRequest(_logger);

        if (!context.RequestedClaimTypes.Any())
    ***REMOVED***
            context.LogIssuedClaims(_logger);
            return;
        ***REMOVED***

        var claims = AddDefaultClaims();

        context.AddRequestedClaims(claims);
        context.LogIssuedClaims(_logger);

        //user.Value.RecordLastLoggedInTime();
        //await _accountRepo.SaveChangesAsync();
    ***REMOVED***

    private static List<Claim> AddDefaultClaims() =>
         new()
     ***REMOVED***
             new Claim(JwtClaimTypes.Name, "John Doe"),
             new Claim(JwtClaimTypes.GivenName, "John Doe"),
             new Claim(JwtClaimTypes.FamilyName, "John Doe"),
             new Claim(JwtClaimTypes.Email, "john.doe@gmail.com"),
             new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
             new Claim(JwtClaimTypes.PhoneNumber, "+94771110447")
 ***REMOVED***

    /// <summary>
    /// This method gets called whenever identity server needs to determine if the user is valid or active 
    /// (e.g. if the user's account has been deactivated since they logged in).
    /// (e.g. during token issuance or validation).
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    public async Task IsActiveAsync(IsActiveContext context)
***REMOVED***
        _logger.LogDebug("IsActive called from: ***REMOVED***caller***REMOVED***", context.Caller);

        //var user = await _accountRepo.GetByIdAsync(Guid.Parse(context.Subject.GetSubjectId()));
        //context.IsActive = user.HasValue && !user.Value.IsAccountLockedOut();
        context.IsActive = true;
    ***REMOVED***
***REMOVED***