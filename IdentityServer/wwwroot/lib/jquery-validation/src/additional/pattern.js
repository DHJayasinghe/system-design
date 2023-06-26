/**
* Return true if the field value matches the given format RegExp
*
* @example $.validator.methods.pattern("AR1004",element,/^AR\d***REMOVED***4***REMOVED***$/)
* @result true
*
* @example $.validator.methods.pattern("BR1004",element,/^AR\d***REMOVED***4***REMOVED***$/)
* @result false
*
* @name $.validator.methods.pattern
* @type Boolean
* @cat Plugins/Validate/Methods
*/
$.validator.addMethod("pattern", function(value, element, param) ***REMOVED***
	if (this.optional(element)) ***REMOVED***
		return true;
	***REMOVED***
	if (typeof param === "string") ***REMOVED***
		param = new RegExp("^(?:" + param + ")$");
	***REMOVED***
	return param.test(value);
***REMOVED***, "Invalid format.");
