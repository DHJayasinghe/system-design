(function() ***REMOVED***

	function stripHtml(value) ***REMOVED***
		// remove html tags and space chars
		return value.replace(/<.[^<>]*?>/g, " ").replace(/&nbsp;|&#160;/gi, " ")
		// remove punctuation
		.replace(/[.(),;:!?%#$'\"_+=\/\-“”’]*/g, "");
	***REMOVED***

	$.validator.addMethod("maxWords", function(value, element, params) ***REMOVED***
		return this.optional(element) || stripHtml(value).match(/\b\w+\b/g).length <= params;
	***REMOVED***, $.validator.format("Please enter ***REMOVED***0***REMOVED*** words or less."));

	$.validator.addMethod("minWords", function(value, element, params) ***REMOVED***
		return this.optional(element) || stripHtml(value).match(/\b\w+\b/g).length >= params;
	***REMOVED***, $.validator.format("Please enter at least ***REMOVED***0***REMOVED*** words."));

	$.validator.addMethod("rangeWords", function(value, element, params) ***REMOVED***
		var valueStripped = stripHtml(value),
			regex = /\b\w+\b/g;
		return this.optional(element) || valueStripped.match(regex).length >= params[0] && valueStripped.match(regex).length <= params[1];
	***REMOVED***, $.validator.format("Please enter between ***REMOVED***0***REMOVED*** and ***REMOVED***1***REMOVED*** words."));

***REMOVED***());
