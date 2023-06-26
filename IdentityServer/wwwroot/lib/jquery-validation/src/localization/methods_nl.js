/*
 * Localized default methods for the jQuery validation plugin.
 * Locale: NL
 */
$.extend($.validator.methods, ***REMOVED***
	date: function(value, element) ***REMOVED***
		return this.optional(element) || /^\d\d?[\.\/\-]\d\d?[\.\/\-]\d\d\d?\d?$/.test(value);
	***REMOVED***
***REMOVED***);
