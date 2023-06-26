$.validator.addMethod("postalcodeNL", function(value, element) ***REMOVED***
	return this.optional(element) || /^[1-9][0-9]***REMOVED***3***REMOVED***\s?[a-zA-Z]***REMOVED***2***REMOVED***$/.test(value);
***REMOVED***, "Please specify a valid postal code");
