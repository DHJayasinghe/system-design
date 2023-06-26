// ajax mode: abort
// usage: $.ajax(***REMOVED*** mode: "abort"[, port: "uniqueport"]***REMOVED***);
// if mode:"abort" is used, the previous request on that port (port can be undefined) is aborted via XMLHttpRequest.abort()

var pendingRequests = ***REMOVED******REMOVED***,
	ajax;
// Use a prefilter if available (1.5+)
if ( $.ajaxPrefilter ) ***REMOVED***
	$.ajaxPrefilter(function( settings, _, xhr ) ***REMOVED***
		var port = settings.port;
		if ( settings.mode === "abort" ) ***REMOVED***
			if ( pendingRequests[port] ) ***REMOVED***
				pendingRequests[port].abort();
			***REMOVED***
			pendingRequests[port] = xhr;
		***REMOVED***
	***REMOVED***);
***REMOVED*** else ***REMOVED***
	// Proxy ajax
	ajax = $.ajax;
	$.ajax = function( settings ) ***REMOVED***
		var mode = ( "mode" in settings ? settings : $.ajaxSettings ).mode,
			port = ( "port" in settings ? settings : $.ajaxSettings ).port;
		if ( mode === "abort" ) ***REMOVED***
			if ( pendingRequests[port] ) ***REMOVED***
				pendingRequests[port].abort();
			***REMOVED***
			pendingRequests[port] = ajax.***REMOVED***ly(this, arguments);
			return pendingRequests[port];
		***REMOVED***
		return ajax.***REMOVED***ly(this, arguments);
	***REMOVED***;
***REMOVED***
