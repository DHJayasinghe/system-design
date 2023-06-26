$.validator.addMethod("lettersonly", function(value, element) ***REMOVED***
	return this.optional(element) || /^[a-z]+$/i.test(value);
***REMOVED***, "Letters only please");
