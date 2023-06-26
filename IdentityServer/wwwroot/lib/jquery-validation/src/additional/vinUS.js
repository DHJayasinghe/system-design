/**
 * Return true, if the value is a valid vehicle identification number (VIN).
 *
 * Works with all kind of text inputs.
 *
 * @example <input type="text" size="20" name="VehicleID" class="***REMOVED***required:true,vinUS:true***REMOVED***" />
 * @desc Declares a required input element whose value must be a valid vehicle identification number.
 *
 * @name $.validator.methods.vinUS
 * @type Boolean
 * @cat Plugins/Validate/Methods
 */
$.validator.addMethod("vinUS", function(v) ***REMOVED***
	if (v.length !== 17) ***REMOVED***
		return false;
	***REMOVED***

	var LL = [ "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M", "N", "P", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" ],
		VL = [ 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 7, 9, 2, 3, 4, 5, 6, 7, 8, 9 ],
		FL = [ 8, 7, 6, 5, 4, 3, 2, 10, 0, 9, 8, 7, 6, 5, 4, 3, 2 ],
		rs = 0,
		i, n, d, f, cd, cdv;

	for (i = 0; i < 17; i++) ***REMOVED***
		f = FL[i];
		d = v.slice(i, i + 1);
		if (i === 8) ***REMOVED***
			cdv = d;
		***REMOVED***
		if (!isNaN(d)) ***REMOVED***
			d *= f;
		***REMOVED*** else ***REMOVED***
			for (n = 0; n < LL.length; n++) ***REMOVED***
				if (d.toUpperCase() === LL[n]) ***REMOVED***
					d = VL[n];
					d *= f;
					if (isNaN(cdv) && n === 8) ***REMOVED***
						cdv = LL[n];
					***REMOVED***
					break;
				***REMOVED***
			***REMOVED***
		***REMOVED***
		rs += d;
	***REMOVED***
	cd = rs % 11;
	if (cd === 10) ***REMOVED***
		cd = "X";
	***REMOVED***
	if (cd === cdv) ***REMOVED***
		return true;
	***REMOVED***
	return false;
***REMOVED***, "The specified vehicle identification number (VIN) is invalid.");
