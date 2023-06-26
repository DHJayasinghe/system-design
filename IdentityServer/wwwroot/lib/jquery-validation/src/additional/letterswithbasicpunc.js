$.validator.addMethod("letterswithbasicpunc", function(value, element) ***REMOVED***
	return this.optional(element) || /^[a-z\-.,()'"\s]+$/i.test(value);
***REMOVED***, "Letters or punctuation only please");
