/*
 * Lets you say "at least X inputs that match selector Y must be filled."
 *
 * The end result is that neither of these inputs:
 *
 *	<input class="productinfo" name="partnumber">
 *	<input class="productinfo" name="description">
 *
 *	...will validate unless at least one of them is filled.
 *
 * partnumber:	***REMOVED***require_from_group: [1,".productinfo"]***REMOVED***,
 * description: ***REMOVED***require_from_group: [1,".productinfo"]***REMOVED***
 *
 * options[0]: number of fields that must be filled in the group
 * options[1]: CSS selector that defines the group of conditionally required fields
 */
$.validator.addMethod("require_from_group", function(value, element, options) ***REMOVED***
	var $fields = $(options[1], element.form),
		$fieldsFirst = $fields.eq(0),
		validator = $fieldsFirst.data("valid_req_grp") ? $fieldsFirst.data("valid_req_grp") : $.extend(***REMOVED******REMOVED***, this),
		isValid = $fields.filter(function() ***REMOVED***
			return validator.elementValue(this);
		***REMOVED***).length >= options[0];

	// Store the cloned validator for future validation
	$fieldsFirst.data("valid_req_grp", validator);

	// If element isn't being validated, run each require_from_group field's validation rules
	if (!$(element).data("being_validated")) ***REMOVED***
		$fields.data("being_validated", true);
		$fields.each(function() ***REMOVED***
			validator.element(this);
		***REMOVED***);
		$fields.data("being_validated", false);
	***REMOVED***
	return isValid;
***REMOVED***, $.validator.format("Please fill at least ***REMOVED***0***REMOVED*** of these fields."));
