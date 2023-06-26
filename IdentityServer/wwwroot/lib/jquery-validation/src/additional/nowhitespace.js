$.validator.addMethod("nowhitespace", function(value, element) ***REMOVED***
	return this.optional(element) || /^\S+$/i.test(value);
***REMOVED***, "No white space please");
