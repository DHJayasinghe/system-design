// Older "accept" file extension method. Old docs: http://docs.jquery.com/Plugins/Validation/Methods/accept
$.validator.addMethod("extension", function(value, element, param) ***REMOVED***
	param = typeof param === "string" ? param.replace(/,/g, "|") : "png|jpe?g|gif";
	return this.optional(element) || value.match(new RegExp("\\.(" + param + ")$", "i"));
***REMOVED***, $.validator.format("Please enter a value with a valid extension."));
