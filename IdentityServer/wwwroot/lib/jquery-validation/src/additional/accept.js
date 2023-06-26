// Accept a value from a file input based on a required mimetype
$.validator.addMethod("accept", function(value, element, param) ***REMOVED***
	// Split mime on commas in case we have multiple types we can accept
	var typeParam = typeof param === "string" ? param.replace(/\s/g, "").replace(/,/g, "|") : "image/*",
	optionalValue = this.optional(element),
	i, file;

	// Element is optional
	if (optionalValue) ***REMOVED***
		return optionalValue;
	***REMOVED***

	if ($(element).attr("type") === "file") ***REMOVED***
		// If we are using a wildcard, make it regex friendly
		typeParam = typeParam.replace(/\*/g, ".*");

		// Check if the element has a FileList before checking each file
		if (element.files && element.files.length) ***REMOVED***
			for (i = 0; i < element.files.length; i++) ***REMOVED***
				file = element.files[i];

				// Grab the mimetype from the loaded file, verify it matches
				if (!file.type.match(new RegExp( "\\.?(" + typeParam + ")$", "i"))) ***REMOVED***
					return false;
				***REMOVED***
			***REMOVED***
		***REMOVED***
	***REMOVED***

	// Either return true because we've validated each file, or because the
	// browser does not support element.files and the FileList feature
	return true;
***REMOVED***, $.validator.format("Please enter a value with a valid mimetype."));
