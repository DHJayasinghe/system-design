/*
 * Localized default methods for the jQuery validation plugin.
 * Locale: ES_CL
 */
$.extend($.validator.methods, ***REMOVED***
	date: function(value, element) ***REMOVED***
		return this.optional(element) || /^\d\d?\-\d\d?\-\d\d\d?\d?$/.test(value);
	***REMOVED***,
	number: function(value, element) ***REMOVED***
		return this.optional(element) || /^-?(?:\d+|\d***REMOVED***1,3***REMOVED***(?:\.\d***REMOVED***3***REMOVED***)+)(?:,\d+)?$/.test(value);
	***REMOVED***
***REMOVED***);
