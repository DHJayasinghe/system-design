/**
 * Return true, if the value is a valid date, also making this formal check dd/mm/yyyy.
 *
 * @example $.validator.methods.date("01/01/1900")
 * @result true
 *
 * @example $.validator.methods.date("01/13/1990")
 * @result false
 *
 * @example $.validator.methods.date("01.01.1900")
 * @result false
 *
 * @example <input name="pippo" class="***REMOVED***dateITA:true***REMOVED***" />
 * @desc Declares an optional input element whose value must be a valid date.
 *
 * @name $.validator.methods.dateITA
 * @type Boolean
 * @cat Plugins/Validate/Methods
 */
$.validator.addMethod("dateITA", function(value, element) ***REMOVED***
	var check = false,
		re = /^\d***REMOVED***1,2***REMOVED***\/\d***REMOVED***1,2***REMOVED***\/\d***REMOVED***4***REMOVED***$/,
		adata, gg, mm, aaaa, xdata;
	if ( re.test(value)) ***REMOVED***
		adata = value.split("/");
		gg = parseInt(adata[0], 10);
		mm = parseInt(adata[1], 10);
		aaaa = parseInt(adata[2], 10);
		xdata = new Date(Date.UTC(aaaa, mm - 1, gg, 12, 0, 0, 0));
		if ( ( xdata.getUTCFullYear() === aaaa ) && ( xdata.getUTCMonth () === mm - 1 ) && ( xdata.getUTCDate() === gg ) ) ***REMOVED***
			check = true;
		***REMOVED*** else ***REMOVED***
			check = false;
		***REMOVED***
	***REMOVED*** else ***REMOVED***
		check = false;
	***REMOVED***
	return this.optional(element) || check;
***REMOVED***, $.validator.messages.date);
