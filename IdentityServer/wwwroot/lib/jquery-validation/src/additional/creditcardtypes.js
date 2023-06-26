/* NOTICE: Modified version of Castle.Components.Validator.CreditCardValidator
 * Redistributed under the the Apache License 2.0 at http://www.apache.org/licenses/LICENSE-2.0
 * Valid Types: mastercard, visa, amex, dinersclub, enroute, discover, jcb, unknown, all (overrides all other settings)
 */
$.validator.addMethod("creditcardtypes", function(value, element, param) ***REMOVED***
	if (/[^0-9\-]+/.test(value)) ***REMOVED***
		return false;
	***REMOVED***

	value = value.replace(/\D/g, "");

	var validTypes = 0x0000;

	if (param.mastercard) ***REMOVED***
		validTypes |= 0x0001;
	***REMOVED***
	if (param.visa) ***REMOVED***
		validTypes |= 0x0002;
	***REMOVED***
	if (param.amex) ***REMOVED***
		validTypes |= 0x0004;
	***REMOVED***
	if (param.dinersclub) ***REMOVED***
		validTypes |= 0x0008;
	***REMOVED***
	if (param.enroute) ***REMOVED***
		validTypes |= 0x0010;
	***REMOVED***
	if (param.discover) ***REMOVED***
		validTypes |= 0x0020;
	***REMOVED***
	if (param.jcb) ***REMOVED***
		validTypes |= 0x0040;
	***REMOVED***
	if (param.unknown) ***REMOVED***
		validTypes |= 0x0080;
	***REMOVED***
	if (param.all) ***REMOVED***
		validTypes = 0x0001 | 0x0002 | 0x0004 | 0x0008 | 0x0010 | 0x0020 | 0x0040 | 0x0080;
	***REMOVED***
	if (validTypes & 0x0001 && /^(5[12345])/.test(value)) ***REMOVED*** //mastercard
		return value.length === 16;
	***REMOVED***
	if (validTypes & 0x0002 && /^(4)/.test(value)) ***REMOVED*** //visa
		return value.length === 16;
	***REMOVED***
	if (validTypes & 0x0004 && /^(3[47])/.test(value)) ***REMOVED*** //amex
		return value.length === 15;
	***REMOVED***
	if (validTypes & 0x0008 && /^(3(0[012345]|[68]))/.test(value)) ***REMOVED*** //dinersclub
		return value.length === 14;
	***REMOVED***
	if (validTypes & 0x0010 && /^(2(014|149))/.test(value)) ***REMOVED*** //enroute
		return value.length === 15;
	***REMOVED***
	if (validTypes & 0x0020 && /^(6011)/.test(value)) ***REMOVED*** //discover
		return value.length === 16;
	***REMOVED***
	if (validTypes & 0x0040 && /^(3)/.test(value)) ***REMOVED*** //jcb
		return value.length === 16;
	***REMOVED***
	if (validTypes & 0x0040 && /^(2131|1800)/.test(value)) ***REMOVED*** //jcb
		return value.length === 15;
	***REMOVED***
	if (validTypes & 0x0080) ***REMOVED*** //unknown
		return true;
	***REMOVED***
	return false;
***REMOVED***, "Please enter a valid credit card number.");
