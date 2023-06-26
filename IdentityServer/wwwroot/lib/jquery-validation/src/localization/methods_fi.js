/*
 * Localized default methods for the jQuery validation plugin.
 * Locale: FI
 */
$.extend($.validator.methods, ***REMOVED***
	date: function(value, element) ***REMOVED***
		return this.optional(element) || /^\d***REMOVED***1,2***REMOVED***\.\d***REMOVED***1,2***REMOVED***\.\d***REMOVED***4***REMOVED***$/.test(value);
	***REMOVED***,
	number: function(value, element) ***REMOVED***
		return this.optional(element) || /^-?(?:\d+)(?:,\d+)?$/.test(value);
	***REMOVED***
***REMOVED***);
