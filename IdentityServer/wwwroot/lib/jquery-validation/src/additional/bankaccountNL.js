/*
 * Dutch bank account numbers (not 'giro' numbers) have 9 digits
 * and pass the '11 check'.
 * We accept the notation with spaces, as that is common.
 * acceptable: 123456789 or 12 34 56 789
 */
$.validator.addMethod("bankaccountNL", function(value, element) ***REMOVED***
	if (this.optional(element)) ***REMOVED***
		return true;
	***REMOVED***
	if (!(/^[0-9]***REMOVED***9***REMOVED***|([0-9]***REMOVED***2***REMOVED*** )***REMOVED***3***REMOVED***[0-9]***REMOVED***3***REMOVED***$/.test(value))) ***REMOVED***
		return false;
	***REMOVED***
	// now '11 check'
	var account = value.replace(/ /g, ""), // remove spaces
		sum = 0,
		len = account.length,
		pos, factor, digit;
	for ( pos = 0; pos < len; pos++ ) ***REMOVED***
		factor = len - pos;
		digit = account.substring(pos, pos + 1);
		sum = sum + factor * digit;
	***REMOVED***
	return sum % 11 === 0;
***REMOVED***, "Please specify a valid bank account number");
