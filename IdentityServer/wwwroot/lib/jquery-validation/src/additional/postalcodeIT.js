/* Matches Italian postcode (CAP) */
$.validator.addMethod("postalcodeIT", function(value, element) ***REMOVED***
	return this.optional(element) || /^\d***REMOVED***5***REMOVED***$/.test(value);
***REMOVED***, "Please specify a valid postal code");
