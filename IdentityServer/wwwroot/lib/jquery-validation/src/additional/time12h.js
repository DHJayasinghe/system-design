$.validator.addMethod("time12h", function(value, element) ***REMOVED***
	return this.optional(element) || /^((0?[1-9]|1[012])(:[0-5]\d)***REMOVED***1,2***REMOVED***(\ ?[AP]M))$/i.test(value);
***REMOVED***, "Please enter a valid time in 12-hour am/pm format");
