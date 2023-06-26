using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BnA.IAM.Presentation.API.Controllers.Diagnostics
***REMOVED***
    [SecurityHeaders]
    [Authorize]
    public class DiagnosticsController : Controller
***REMOVED***
        public async Task<IActionResult> Index()
    ***REMOVED***
            var localAddresses = new string[] ***REMOVED*** "127.0.0.1", "::1", HttpContext.Connection.LocalIpAddress.ToString() ***REMOVED***;
            if (!localAddresses.Contains(HttpContext.Connection.RemoteIpAddress.ToString()))
        ***REMOVED***
                return NotFound();
            ***REMOVED***

            var model = new DiagnosticsViewModel(await HttpContext.AuthenticateAsync());
            return View(model);
        ***REMOVED***
    ***REMOVED***
***REMOVED***