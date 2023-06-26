$.validator.addMethod("bankorgiroaccountNL", function(value, element) ***REMOVED***
	return this.optional(element) ||
			($.validator.methods.bankaccountNL.call(this, value, element)) ||
			($.validator.methods.giroaccountNL.call(this, value, element));
***REMOVED***, "Please specify a valid bank or giro account number");
