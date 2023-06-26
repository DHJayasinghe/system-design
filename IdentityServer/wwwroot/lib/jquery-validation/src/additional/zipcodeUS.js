$.validator.addMethod("zipcodeUS", function(value, element) ***REMOVED***
	return this.optional(element) || /^\d***REMOVED***5***REMOVED***(-\d***REMOVED***4***REMOVED***)?$/.test(value);
***REMOVED***, "The specified US ZIP Code is invalid");
