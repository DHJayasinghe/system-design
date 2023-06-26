$.validator.addMethod("integer", function(value, element) ***REMOVED***
	return this.optional(element) || /^-?\d+$/.test(value);
***REMOVED***, "A positive or negative non-decimal number please");
