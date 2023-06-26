$.validator.addMethod("ziprange", function(value, element) ***REMOVED***
	return this.optional(element) || /^90[2-5]\d\***REMOVED***2\***REMOVED***-\d***REMOVED***4***REMOVED***$/.test(value);
***REMOVED***, "Your ZIP-code must be in the range 902xx-xxxx to 905xx-xxxx");
