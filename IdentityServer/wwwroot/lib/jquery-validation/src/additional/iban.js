/**
 * IBAN is the international bank account number.
 * It has a coun***REMOVED*** - specific format, that is checked here too
 */
$.validator.addMethod("iban", function(value, element) ***REMOVED***
	// some quick simple tests to prevent needless work
	if (this.optional(element)) ***REMOVED***
		return true;
	***REMOVED***

	// remove spaces and to upper case
	var iban = value.replace(/ /g, "").toUpperCase(),
		ibancheckdigits = "",
		leadingZeroes = true,
		cRest = "",
		cOperator = "",
		coun***REMOVED***code, ibancheck, charAt, cChar, bbanpattern, bbancoun***REMOVED***patterns, ibanregexp, i, p;

	// check the coun***REMOVED*** code and find the coun***REMOVED*** specific format
	coun***REMOVED***code = iban.substring(0, 2);
	bbancoun***REMOVED***patterns = ***REMOVED***
		"AL": "\\d***REMOVED***8***REMOVED***[\\dA-Z]***REMOVED***16***REMOVED***",
		"AD": "\\d***REMOVED***8***REMOVED***[\\dA-Z]***REMOVED***12***REMOVED***",
		"AT": "\\d***REMOVED***16***REMOVED***",
		"AZ": "[\\dA-Z]***REMOVED***4***REMOVED***\\d***REMOVED***20***REMOVED***",
		"BE": "\\d***REMOVED***12***REMOVED***",
		"BH": "[A-Z]***REMOVED***4***REMOVED***[\\dA-Z]***REMOVED***14***REMOVED***",
		"BA": "\\d***REMOVED***16***REMOVED***",
		"BR": "\\d***REMOVED***23***REMOVED***[A-Z][\\dA-Z]",
		"BG": "[A-Z]***REMOVED***4***REMOVED***\\d***REMOVED***6***REMOVED***[\\dA-Z]***REMOVED***8***REMOVED***",
		"CR": "\\d***REMOVED***17***REMOVED***",
		"HR": "\\d***REMOVED***17***REMOVED***",
		"CY": "\\d***REMOVED***8***REMOVED***[\\dA-Z]***REMOVED***16***REMOVED***",
		"CZ": "\\d***REMOVED***20***REMOVED***",
		"DK": "\\d***REMOVED***14***REMOVED***",
		"DO": "[A-Z]***REMOVED***4***REMOVED***\\d***REMOVED***20***REMOVED***",
		"EE": "\\d***REMOVED***16***REMOVED***",
		"FO": "\\d***REMOVED***14***REMOVED***",
		"FI": "\\d***REMOVED***14***REMOVED***",
		"FR": "\\d***REMOVED***10***REMOVED***[\\dA-Z]***REMOVED***11***REMOVED***\\d***REMOVED***2***REMOVED***",
		"GE": "[\\dA-Z]***REMOVED***2***REMOVED***\\d***REMOVED***16***REMOVED***",
		"DE": "\\d***REMOVED***18***REMOVED***",
		"GI": "[A-Z]***REMOVED***4***REMOVED***[\\dA-Z]***REMOVED***15***REMOVED***",
		"GR": "\\d***REMOVED***7***REMOVED***[\\dA-Z]***REMOVED***16***REMOVED***",
		"GL": "\\d***REMOVED***14***REMOVED***",
		"GT": "[\\dA-Z]***REMOVED***4***REMOVED***[\\dA-Z]***REMOVED***20***REMOVED***",
		"HU": "\\d***REMOVED***24***REMOVED***",
		"IS": "\\d***REMOVED***22***REMOVED***",
		"IE": "[\\dA-Z]***REMOVED***4***REMOVED***\\d***REMOVED***14***REMOVED***",
		"IL": "\\d***REMOVED***19***REMOVED***",
		"IT": "[A-Z]\\d***REMOVED***10***REMOVED***[\\dA-Z]***REMOVED***12***REMOVED***",
		"KZ": "\\d***REMOVED***3***REMOVED***[\\dA-Z]***REMOVED***13***REMOVED***",
		"KW": "[A-Z]***REMOVED***4***REMOVED***[\\dA-Z]***REMOVED***22***REMOVED***",
		"LV": "[A-Z]***REMOVED***4***REMOVED***[\\dA-Z]***REMOVED***13***REMOVED***",
		"LB": "\\d***REMOVED***4***REMOVED***[\\dA-Z]***REMOVED***20***REMOVED***",
		"LI": "\\d***REMOVED***5***REMOVED***[\\dA-Z]***REMOVED***12***REMOVED***",
		"LT": "\\d***REMOVED***16***REMOVED***",
		"LU": "\\d***REMOVED***3***REMOVED***[\\dA-Z]***REMOVED***13***REMOVED***",
		"MK": "\\d***REMOVED***3***REMOVED***[\\dA-Z]***REMOVED***10***REMOVED***\\d***REMOVED***2***REMOVED***",
		"MT": "[A-Z]***REMOVED***4***REMOVED***\\d***REMOVED***5***REMOVED***[\\dA-Z]***REMOVED***18***REMOVED***",
		"MR": "\\d***REMOVED***23***REMOVED***",
		"MU": "[A-Z]***REMOVED***4***REMOVED***\\d***REMOVED***19***REMOVED***[A-Z]***REMOVED***3***REMOVED***",
		"MC": "\\d***REMOVED***10***REMOVED***[\\dA-Z]***REMOVED***11***REMOVED***\\d***REMOVED***2***REMOVED***",
		"MD": "[\\dA-Z]***REMOVED***2***REMOVED***\\d***REMOVED***18***REMOVED***",
		"ME": "\\d***REMOVED***18***REMOVED***",
		"NL": "[A-Z]***REMOVED***4***REMOVED***\\d***REMOVED***10***REMOVED***",
		"NO": "\\d***REMOVED***11***REMOVED***",
		"PK": "[\\dA-Z]***REMOVED***4***REMOVED***\\d***REMOVED***16***REMOVED***",
		"PS": "[\\dA-Z]***REMOVED***4***REMOVED***\\d***REMOVED***21***REMOVED***",
		"PL": "\\d***REMOVED***24***REMOVED***",
		"PT": "\\d***REMOVED***21***REMOVED***",
		"RO": "[A-Z]***REMOVED***4***REMOVED***[\\dA-Z]***REMOVED***16***REMOVED***",
		"SM": "[A-Z]\\d***REMOVED***10***REMOVED***[\\dA-Z]***REMOVED***12***REMOVED***",
		"SA": "\\d***REMOVED***2***REMOVED***[\\dA-Z]***REMOVED***18***REMOVED***",
		"RS": "\\d***REMOVED***18***REMOVED***",
		"SK": "\\d***REMOVED***20***REMOVED***",
		"SI": "\\d***REMOVED***15***REMOVED***",
		"ES": "\\d***REMOVED***20***REMOVED***",
		"SE": "\\d***REMOVED***20***REMOVED***",
		"CH": "\\d***REMOVED***5***REMOVED***[\\dA-Z]***REMOVED***12***REMOVED***",
		"TN": "\\d***REMOVED***20***REMOVED***",
		"TR": "\\d***REMOVED***5***REMOVED***[\\dA-Z]***REMOVED***17***REMOVED***",
		"AE": "\\d***REMOVED***3***REMOVED***\\d***REMOVED***16***REMOVED***",
		"GB": "[A-Z]***REMOVED***4***REMOVED***\\d***REMOVED***14***REMOVED***",
		"VG": "[\\dA-Z]***REMOVED***4***REMOVED***\\d***REMOVED***16***REMOVED***"
	***REMOVED***;

	bbanpattern = bbancoun***REMOVED***patterns[coun***REMOVED***code];
	// As new countries will start using IBAN in the
	// future, we only check if the coun***REMOVED***code is known.
	// This prevents false negatives, while almost all
	// false positives introduced by this, will be caught
	// by the checksum validation below anyway.
	// Strict checking should return FALSE for unknown
	// countries.
	if (typeof bbanpattern !== "undefined") ***REMOVED***
		ibanregexp = new RegExp("^[A-Z]***REMOVED***2***REMOVED***\\d***REMOVED***2***REMOVED***" + bbanpattern + "$", "");
		if (!(ibanregexp.test(iban))) ***REMOVED***
			return false; // invalid coun***REMOVED*** specific format
		***REMOVED***
	***REMOVED***

	// now check the checksum, first convert to digits
	ibancheck = iban.substring(4, iban.length) + iban.substring(0, 4);
	for (i = 0; i < ibancheck.length; i++) ***REMOVED***
		charAt = ibancheck.charAt(i);
		if (charAt !== "0") ***REMOVED***
			leadingZeroes = false;
		***REMOVED***
		if (!leadingZeroes) ***REMOVED***
			ibancheckdigits += "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".indexOf(charAt);
		***REMOVED***
	***REMOVED***

	// calculate the result of: ibancheckdigits % 97
	for (p = 0; p < ibancheckdigits.length; p++) ***REMOVED***
		cChar = ibancheckdigits.charAt(p);
		cOperator = "" + cRest + "" + cChar;
		cRest = cOperator % 97;
	***REMOVED***
	return cRest === 1;
***REMOVED***, "Please specify a valid IBAN");
