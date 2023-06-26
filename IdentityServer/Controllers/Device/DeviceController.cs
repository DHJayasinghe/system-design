***REMOVED***
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BnA.IAM.Presentation.API.Controllers.Consent;
***REMOVED***
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BnA.IAM.Presentation.API.Controllers.Device
***REMOVED***
    [Authorize]
    [SecurityHeaders]
    public class DeviceController : Controller
***REMOVED***
        private readonly IDeviceFlowInteractionService _interaction;
        private readonly IEventService _events;
        private readonly IOptions<IdentityServerOptions> _options;
        private readonly ILogger<DeviceController> _logger;

        public DeviceController(
            IDeviceFlowInteractionService interaction,
            IEventService eventService,
            IOptions<IdentityServerOptions> options,
            ILogger<DeviceController> logger)
    ***REMOVED***
            _interaction = interaction;
            _events = eventService;
            _options = options;
            _logger = logger;
        ***REMOVED***

        [HttpGet]
        public async Task<IActionResult> Index()
    ***REMOVED***
            string userCodeParamName = _options.Value.UserInteraction.DeviceVerificationUserCodeParameter;
            string userCode = Request.Query[userCodeParamName];
            if (string.IsNullOrWhiteSpace(userCode)) return View("UserCodeCapture");

            var vm = await BuildViewModelAsync(userCode);
            if (vm == null) return View("Error");

            vm.ConfirmUserCode = true;
            return View("UserCodeConfirmation", vm);
        ***REMOVED***

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserCodeCapture(string userCode)
    ***REMOVED***
            var vm = await BuildViewModelAsync(userCode);
            if (vm == null) return View("Error");

            return View("UserCodeConfirmation", vm);
        ***REMOVED***

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Callback(DeviceAuthorizationInputModel model)
    ***REMOVED***
            if (model == null) throw new ArgumentNullException(nameof(model));

            var result = await ProcessConsent(model);
            if (result.HasValidationError) return View("Error");

            return View("Success");
        ***REMOVED***

        private async Task<ProcessConsentResult> ProcessConsent(DeviceAuthorizationInputModel model)
    ***REMOVED***
            var result = new ProcessConsentResult();

            var request = await _interaction.GetAuthorizationContextAsync(model.UserCode);
            if (request == null) return result;

            ConsentResponse grantedConsent = null;

            // user clicked 'no' - send back the standard 'access_denied' response
            if (model.Button == "no")
        ***REMOVED***
                grantedConsent = new ConsentResponse ***REMOVED*** Error = AuthorizationError.AccessDenied ***REMOVED***;

                // emit event
                await _events.RaiseAsync(new ConsentDeniedEvent(User.GetSubjectId(), request.Client.ClientId, request.ValidatedResources.RawScopeValues));
            ***REMOVED***
            // user clicked 'yes' - validate the data
            else if (model.Button == "yes")
        ***REMOVED***
                // if the user consented to some scope, build the response model
                if (model.ScopesConsented != null && model.ScopesConsented.Any())
            ***REMOVED***
                    var scopes = model.ScopesConsented;
                    if (ConsentOptions.EnableOfflineAccess == false)
                ***REMOVED***
                        scopes = scopes.Where(x => x != Duende.IdentityServer.IdentityServerConstants.StandardScopes.OfflineAccess);
                    ***REMOVED***

                    grantedConsent = new ConsentResponse
                ***REMOVED***
                        RememberConsent = model.RememberConsent,
                        ScopesValuesConsented = scopes.ToArray(),
                        Description = model.Description
            ***REMOVED***

                    // emit event
                    await _events.RaiseAsync(new ConsentGrantedEvent(User.GetSubjectId(), request.Client.ClientId, request.ValidatedResources.RawScopeValues, grantedConsent.ScopesValuesConsented, grantedConsent.RememberConsent));
                ***REMOVED***
                else
            ***REMOVED***
                    result.ValidationError = ConsentOptions.MustChooseOneErrorMessage;
                ***REMOVED***
            ***REMOVED***
            else
        ***REMOVED***
                result.ValidationError = ConsentOptions.InvalidSelectionErrorMessage;
            ***REMOVED***

            if (grantedConsent != null)
        ***REMOVED***
                // communicate outcome of consent back to identityserver
                await _interaction.HandleRequestAsync(model.UserCode, grantedConsent);

                // indicate that's it ok to redirect back to authorization endpoint
                result.RedirectUri = model.ReturnUrl;
                result.Client = request.Client;
            ***REMOVED***
            else
        ***REMOVED***
                // we need to redisplay the consent UI
                result.ViewModel = await BuildViewModelAsync(model.UserCode, model);
            ***REMOVED***

            return result;
        ***REMOVED***

        private async Task<DeviceAuthorizationViewModel> BuildViewModelAsync(string userCode, DeviceAuthorizationInputModel model = null)
    ***REMOVED***
            var request = await _interaction.GetAuthorizationContextAsync(userCode);
            if (request != null)
        ***REMOVED***
                return CreateConsentViewModel(userCode, model, request);
            ***REMOVED***

            return null;
        ***REMOVED***

        private DeviceAuthorizationViewModel CreateConsentViewModel(string userCode, DeviceAuthorizationInputModel model, DeviceFlowAuthorizationRequest request)
    ***REMOVED***
            var vm = new DeviceAuthorizationViewModel
        ***REMOVED***
                UserCode = userCode,
                Description = model?.Description,

                RememberConsent = model?.RememberConsent ?? true,
                ScopesConsented = model?.ScopesConsented ?? Enumerable.Empty<string>(),

                ClientName = request.Client.ClientName ?? request.Client.ClientId,
                ClientUrl = request.Client.ClientUri,
                ClientLogoUrl = request.Client.LogoUri,
                AllowRememberConsent = request.Client.AllowRememberConsent
    ***REMOVED***

            vm.IdentityScopes = request.ValidatedResources.Resources.IdentityResources.Select(x => CreateScopeViewModel(x, vm.ScopesConsented.Contains(x.Name) || model == null)).ToArray();

            var apiScopes = new List<ScopeViewModel>();
            foreach (var parsedScope in request.ValidatedResources.ParsedScopes)
        ***REMOVED***
                var apiScope = request.ValidatedResources.Resources.FindApiScope(parsedScope.ParsedName);
                if (apiScope != null)
            ***REMOVED***
                    var scopeVm = CreateScopeViewModel(parsedScope, apiScope, vm.ScopesConsented.Contains(parsedScope.RawValue) || model == null);
                    apiScopes.Add(scopeVm);
                ***REMOVED***
            ***REMOVED***
            if (ConsentOptions.EnableOfflineAccess && request.ValidatedResources.Resources.OfflineAccess)
        ***REMOVED***
                apiScopes.Add(GetOfflineAccessScope(vm.ScopesConsented.Contains(Duende.IdentityServer.IdentityServerConstants.StandardScopes.OfflineAccess) || model == null));
            ***REMOVED***
            vm.ApiScopes = apiScopes;

            return vm;
        ***REMOVED***

        private ScopeViewModel CreateScopeViewModel(IdentityResource identity, bool check)
    ***REMOVED***
            return new ScopeViewModel
        ***REMOVED***
                Value = identity.Name,
                DisplayName = identity.DisplayName ?? identity.Name,
                Description = identity.Description,
                Emphasize = identity.Emphasize,
                Required = identity.Required,
                Checked = check || identity.Required
    ***REMOVED***
        ***REMOVED***

        public ScopeViewModel CreateScopeViewModel(ParsedScopeValue parsedScopeValue, ApiScope apiScope, bool check)
    ***REMOVED***
            return new ScopeViewModel
        ***REMOVED***
                Value = parsedScopeValue.RawValue,
                // todo: use the parsed scope value in the display?
                DisplayName = apiScope.DisplayName ?? apiScope.Name,
                Description = apiScope.Description,
                Emphasize = apiScope.Emphasize,
                Required = apiScope.Required,
                Checked = check || apiScope.Required
    ***REMOVED***
        ***REMOVED***
        private ScopeViewModel GetOfflineAccessScope(bool check)
    ***REMOVED***
            return new ScopeViewModel
        ***REMOVED***
                Value = Duende.IdentityServer.IdentityServerConstants.StandardScopes.OfflineAccess,
                DisplayName = ConsentOptions.OfflineAccessDisplayName,
                Description = ConsentOptions.OfflineAccessDescription,
                Emphasize = true,
                Checked = check
    ***REMOVED***
        ***REMOVED***
    ***REMOVED***
***REMOVED***