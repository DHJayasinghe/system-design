$.validator.addMethod("mobileNL", function(value, element) ***REMOVED***
	return this.optional(element) || /^((\+|00(\s|\s?\-\s?)?)31(\s|\s?\-\s?)?(\(0\)[\-\s]?)?|0)6((\s|\s?\-\s?)?[0-9])***REMOVED***8***REMOVED***$/.test(value);
***REMOVED***, "Please specify a valid mobile number");
