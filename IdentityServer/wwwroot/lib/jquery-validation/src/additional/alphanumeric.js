$.validator.addMethod("alphanumeric", function(value, element) ***REMOVED***
	return this.optional(element) || /^\w+$/i.test(value);
***REMOVED***, "Letters, numbers, and underscores only please");
