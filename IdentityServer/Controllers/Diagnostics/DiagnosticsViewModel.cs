using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace BnA.IAM.Presentation.API.Controllers.Diagnostics
***REMOVED***
    public class DiagnosticsViewModel
***REMOVED***
        public DiagnosticsViewModel(AuthenticateResult result)
    ***REMOVED***
            AuthenticateResult = result;

            if (result.Properties.Items.ContainsKey("client_list"))
        ***REMOVED***
                var encoded = result.Properties.Items["client_list"];
                var bytes = Base64Url.Decode(encoded);
                var value = Encoding.UTF8.GetString(bytes);

                Clients = JsonConvert.DeserializeObject<string[]>(value);
            ***REMOVED***
        ***REMOVED***

        public AuthenticateResult AuthenticateResult ***REMOVED*** get; ***REMOVED***
        public IEnumerable<string> Clients ***REMOVED*** get; ***REMOVED*** = new List<string>();
    ***REMOVED***
***REMOVED***