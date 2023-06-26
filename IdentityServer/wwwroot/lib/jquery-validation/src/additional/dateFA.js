$.validator.addMethod("dateFA", function(value, element) ***REMOVED***
	return this.optional(element) || /^[1-4]\d***REMOVED***3***REMOVED***\/((0?[1-6]\/((3[0-1])|([1-2][0-9])|(0?[1-9])))|((1[0-2]|(0?[7-9]))\/(30|([1-2][0-9])|(0?[1-9]))))$/.test(value);
***REMOVED***, $.validator.messages.date);
