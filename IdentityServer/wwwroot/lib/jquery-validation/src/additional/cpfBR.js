/*
 * Brazillian CPF number (Cadastrado de Pessoas Físicas) is the equivalent of a Brazilian tax registration number.
 * CPF numbers have 11 digits in total: 9 numbers followed by 2 check numbers that are being used for validation.
 */
$.validator.addMethod("cpfBR", function(value) ***REMOVED***
	// Removing special characters from value
	value = value.replace(/([~!@#$%^&*()_+=`***REMOVED******REMOVED***\[\]\-|\\:;'<>,.\/? ])+/g, "");

	// Checking value to have 11 digits only
	if (value.length !== 11) ***REMOVED***
		return false;
	***REMOVED***

	var sum = 0,
		firstCN, secondCN, checkResult, i;

	firstCN = parseInt(value.substring(9, 10), 10);
	secondCN = parseInt(value.substring(10, 11), 10);

	checkResult = function(sum, cn) ***REMOVED***
		var result = (sum * 10) % 11;
		if ((result === 10) || (result === 11)) ***REMOVED***result = 0;***REMOVED***
		return (result === cn);
	***REMOVED***;

	// Checking for dump data
	if (value === "" ||
		value === "00000000000" ||
		value === "11111111111" ||
		value === "22222222222" ||
		value === "33333333333" ||
		value === "44444444444" ||
		value === "55555555555" ||
		value === "66666666666" ||
		value === "77777777777" ||
		value === "88888888888" ||
		value === "99999999999"
	) ***REMOVED***
		return false;
	***REMOVED***

	// Step 1 - using first Check Number:
	for ( i = 1; i <= 9; i++ ) ***REMOVED***
		sum = sum + parseInt(value.substring(i - 1, i), 10) * (11 - i);
	***REMOVED***

	// If first Check Number (CN) is valid, move to Step 2 - using second Check Number:
	if ( checkResult(sum, firstCN) ) ***REMOVED***
		sum = 0;
		for ( i = 1; i <= 10; i++ ) ***REMOVED***
			sum = sum + parseInt(value.substring(i - 1, i), 10) * (12 - i);
		***REMOVED***
		return checkResult(sum, secondCN);
	***REMOVED***
	return false;

***REMOVED***, "Please specify a valid CPF number");
