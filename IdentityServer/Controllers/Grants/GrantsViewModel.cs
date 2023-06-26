// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


***REMOVED***
using System.Collections.Generic;

namespace BnA.IAM.Presentation.API.Controllers.Grants
***REMOVED***
    public class GrantsViewModel
***REMOVED***
        public IEnumerable<GrantViewModel> Grants ***REMOVED*** get; set; ***REMOVED***
    ***REMOVED***

    public class GrantViewModel
***REMOVED***
        public string ClientId ***REMOVED*** get; set; ***REMOVED***
        public string ClientName ***REMOVED*** get; set; ***REMOVED***
        public string ClientUrl ***REMOVED*** get; set; ***REMOVED***
        public string ClientLogoUrl ***REMOVED*** get; set; ***REMOVED***
        public string Description ***REMOVED*** get; set; ***REMOVED***
        public DateTime Created ***REMOVED*** get; set; ***REMOVED***
        public DateTime? Expires ***REMOVED*** get; set; ***REMOVED***
        public IEnumerable<string> IdentityGrantNames ***REMOVED*** get; set; ***REMOVED***
        public IEnumerable<string> ApiGrantNames ***REMOVED*** get; set; ***REMOVED***
    ***REMOVED***
***REMOVED***