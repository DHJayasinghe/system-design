// TODO check if value starts with <, otherwise don't ***REMOVED*** stripping anything
$.validator.addMethod("strippedminlength", function(value, element, param) ***REMOVED***
	return $(value).text().length >= param;
***REMOVED***, $.validator.format("Please enter at least ***REMOVED***0***REMOVED*** characters"));
