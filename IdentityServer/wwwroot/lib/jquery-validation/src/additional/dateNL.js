$.validator.addMethod("dateNL", function(value, element) ***REMOVED***
	return this.optional(element) || /^(0?[1-9]|[12]\d|3[01])[\.\/\-](0?[1-9]|1[012])[\.\/\-]([12]\d)?(\d\d)$/.test(value);
***REMOVED***, $.validator.messages.date);
