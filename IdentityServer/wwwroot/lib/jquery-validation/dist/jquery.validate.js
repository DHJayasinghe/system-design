/*!
 * jQuery Validation Plugin v1.14.0
 *
 * http://jqueryvalidation.org/
 *
 * Copyright (c) 2015 Jörn Zaefferer
 * Released under the MIT license
 */
(function( factory ) ***REMOVED***
	if ( typeof define === "function" && define.amd ) ***REMOVED***
		define( ["jquery"], factory );
	***REMOVED*** else ***REMOVED***
		factory( jQuery );
	***REMOVED***
***REMOVED***(function( $ ) ***REMOVED***

$.extend($.fn, ***REMOVED***
	// http://jqueryvalidation.org/validate/
	validate: function( options ) ***REMOVED***

		// if nothing is selected, return nothing; can't chain anyway
		if ( !this.length ) ***REMOVED***
			if ( options && options.debug && window.console ) ***REMOVED***
				console.warn( "Nothing selected, can't validate, returning nothing." );
			***REMOVED***
			return;
		***REMOVED***

		// check if a validator for this form was already created
		var validator = $.data( this[ 0 ], "validator" );
		if ( validator ) ***REMOVED***
			return validator;
		***REMOVED***

		// Add novalidate tag if HTML5.
		this.attr( "novalidate", "novalidate" );

		validator = new $.validator( options, this[ 0 ] );
		$.data( this[ 0 ], "validator", validator );

		if ( validator.settings.onsubmit ) ***REMOVED***

			this.on( "click.validate", ":submit", function( event ) ***REMOVED***
				if ( validator.settings.submitHandler ) ***REMOVED***
					validator.submitButton = event.target;
				***REMOVED***

				// allow suppressing validation by adding a cancel class to the submit button
				if ( $( this ).hasClass( "cancel" ) ) ***REMOVED***
					validator.cancelSubmit = true;
				***REMOVED***

				// allow suppressing validation by adding the html5 formnovalidate attribute to the submit button
				if ( $( this ).attr( "formnovalidate" ) !== undefined ) ***REMOVED***
					validator.cancelSubmit = true;
				***REMOVED***
			***REMOVED***);

			// validate the form on submit
			this.on( "submit.validate", function( event ) ***REMOVED***
				if ( validator.settings.debug ) ***REMOVED***
					// prevent form submit to be able to see console output
					event.preventDefault();
				***REMOVED***
				function handle() ***REMOVED***
					var hidden, result;
					if ( validator.settings.submitHandler ) ***REMOVED***
						if ( validator.submitButton ) ***REMOVED***
							// insert a hidden input as a replacement for the missing submit button
							hidden = $( "<input type='hidden'/>" )
								.attr( "name", validator.submitButton.name )
								.val( $( validator.submitButton ).val() )
								.***REMOVED***endTo( validator.currentForm );
						***REMOVED***
						result = validator.settings.submitHandler.call( validator, validator.currentForm, event );
						if ( validator.submitButton ) ***REMOVED***
							// and clean up afterwards; thanks to no-block-scope, hidden can be referenced
							hidden.remove();
						***REMOVED***
						if ( result !== undefined ) ***REMOVED***
							return result;
						***REMOVED***
						return false;
					***REMOVED***
					return true;
				***REMOVED***

				// prevent submit for invalid forms or custom submit handlers
				if ( validator.cancelSubmit ) ***REMOVED***
					validator.cancelSubmit = false;
					return handle();
				***REMOVED***
				if ( validator.form() ) ***REMOVED***
					if ( validator.pendingRequest ) ***REMOVED***
						validator.formSubmitted = true;
						return false;
					***REMOVED***
					return handle();
				***REMOVED*** else ***REMOVED***
					validator.focusInvalid();
					return false;
				***REMOVED***
			***REMOVED***);
		***REMOVED***

		return validator;
	***REMOVED***,
	// http://jqueryvalidation.org/valid/
	valid: function() ***REMOVED***
		var valid, validator, errorList;

		if ( $( this[ 0 ] ).is( "form" ) ) ***REMOVED***
			valid = this.validate().form();
		***REMOVED*** else ***REMOVED***
			errorList = [];
			valid = true;
			validator = $( this[ 0 ].form ).validate();
			this.each( function() ***REMOVED***
				valid = validator.element( this ) && valid;
				errorList = errorList.concat( validator.errorList );
			***REMOVED***);
			validator.errorList = errorList;
		***REMOVED***
		return valid;
	***REMOVED***,

	// http://jqueryvalidation.org/rules/
	rules: function( command, argument ) ***REMOVED***
		var element = this[ 0 ],
			settings, staticRules, existingRules, data, param, filtered;

		if ( command ) ***REMOVED***
			settings = $.data( element.form, "validator" ).settings;
			staticRules = settings.rules;
			existingRules = $.validator.staticRules( element );
			switch ( command ) ***REMOVED***
			case "add":
				$.extend( existingRules, $.validator.normalizeRule( argument ) );
				// remove messages from rules, but allow them to be set separately
				delete existingRules.messages;
				staticRules[ element.name ] = existingRules;
				if ( argument.messages ) ***REMOVED***
					settings.messages[ element.name ] = $.extend( settings.messages[ element.name ], argument.messages );
				***REMOVED***
				break;
			case "remove":
				if ( !argument ) ***REMOVED***
					delete staticRules[ element.name ];
					return existingRules;
				***REMOVED***
				filtered = ***REMOVED******REMOVED***;
				$.each( argument.split( /\s/ ), function( index, method ) ***REMOVED***
					filtered[ method ] = existingRules[ method ];
					delete existingRules[ method ];
					if ( method === "required" ) ***REMOVED***
						$( element ).removeAttr( "aria-required" );
					***REMOVED***
				***REMOVED***);
				return filtered;
			***REMOVED***
		***REMOVED***

		data = $.validator.normalizeRules(
		$.extend(
			***REMOVED******REMOVED***,
			$.validator.classRules( element ),
			$.validator.attributeRules( element ),
			$.validator.dataRules( element ),
			$.validator.staticRules( element )
		), element );

		// make sure required is at front
		if ( data.required ) ***REMOVED***
			param = data.required;
			delete data.required;
			data = $.extend( ***REMOVED*** required: param ***REMOVED***, data );
			$( element ).attr( "aria-required", "true" );
		***REMOVED***

		// make sure remote is at back
		if ( data.remote ) ***REMOVED***
			param = data.remote;
			delete data.remote;
			data = $.extend( data, ***REMOVED*** remote: param ***REMOVED***);
		***REMOVED***

		return data;
	***REMOVED***
***REMOVED***);

// Custom selectors
$.extend( $.expr[ ":" ], ***REMOVED***
	// http://jqueryvalidation.org/blank-selector/
	blank: function( a ) ***REMOVED***
		return !$.trim( "" + $( a ).val() );
	***REMOVED***,
	// http://jqueryvalidation.org/filled-selector/
	filled: function( a ) ***REMOVED***
		return !!$.trim( "" + $( a ).val() );
	***REMOVED***,
	// http://jqueryvalidation.org/unchecked-selector/
	unchecked: function( a ) ***REMOVED***
		return !$( a ).prop( "checked" );
	***REMOVED***
***REMOVED***);

// constructor for validator
$.validator = function( options, form ) ***REMOVED***
	this.settings = $.extend( true, ***REMOVED******REMOVED***, $.validator.defaults, options );
	this.currentForm = form;
	this.init();
***REMOVED***;

// http://jqueryvalidation.org/jQuery.validator.format/
$.validator.format = function( source, params ) ***REMOVED***
	if ( arguments.length === 1 ) ***REMOVED***
		return function() ***REMOVED***
			var args = $.makeArray( arguments );
			args.unshift( source );
			return $.validator.format.***REMOVED***ly( this, args );
		***REMOVED***;
	***REMOVED***
	if ( arguments.length > 2 && params.constructor !== Array  ) ***REMOVED***
		params = $.makeArray( arguments ).slice( 1 );
	***REMOVED***
	if ( params.constructor !== Array ) ***REMOVED***
		params = [ params ];
	***REMOVED***
	$.each( params, function( i, n ) ***REMOVED***
		source = source.replace( new RegExp( "\\***REMOVED***" + i + "\\***REMOVED***", "g" ), function() ***REMOVED***
			return n;
		***REMOVED***);
	***REMOVED***);
	return source;
***REMOVED***;

$.extend( $.validator, ***REMOVED***

	defaults: ***REMOVED***
		messages: ***REMOVED******REMOVED***,
		groups: ***REMOVED******REMOVED***,
		rules: ***REMOVED******REMOVED***,
		errorClass: "error",
		validClass: "valid",
		errorElement: "label",
		focusCleanup: false,
		focusInvalid: true,
		errorContainer: $( [] ),
		errorLabelContainer: $( [] ),
		onsubmit: true,
		ignore: ":hidden",
		ignoreTitle: false,
		onfocusin: function( element ) ***REMOVED***
			this.lastActive = element;

			// Hide error label and remove error class on focus if enabled
			if ( this.settings.focusCleanup ) ***REMOVED***
				if ( this.settings.unhighlight ) ***REMOVED***
					this.settings.unhighlight.call( this, element, this.settings.errorClass, this.settings.validClass );
				***REMOVED***
				this.hideThese( this.errorsFor( element ) );
			***REMOVED***
		***REMOVED***,
		onfocusout: function( element ) ***REMOVED***
			if ( !this.checkable( element ) && ( element.name in this.submitted || !this.optional( element ) ) ) ***REMOVED***
				this.element( element );
			***REMOVED***
		***REMOVED***,
		onkeyup: function( element, event ) ***REMOVED***
			// Avoid revalidate the field when pressing one of the following keys
			// Shift       => 16
			// Ctrl        => 17
			// Alt         => 18
			// Caps lock   => 20
			// End         => 35
			// Home        => 36
			// Left arrow  => 37
			// Up arrow    => 38
			// Right arrow => 39
			// Down arrow  => 40
			// Insert      => 45
			// Num lock    => 144
			// AltGr key   => 225
			var excludedKeys = [
				16, 17, 18, 20, 35, 36, 37,
				38, 39, 40, 45, 144, 225
			];

			if ( event.which === 9 && this.elementValue( element ) === "" || $.inArray( event.keyCode, excludedKeys ) !== -1 ) ***REMOVED***
				return;
			***REMOVED*** else if ( element.name in this.submitted || element === this.lastElement ) ***REMOVED***
				this.element( element );
			***REMOVED***
		***REMOVED***,
		onclick: function( element ) ***REMOVED***
			// click on selects, radiobuttons and checkboxes
			if ( element.name in this.submitted ) ***REMOVED***
				this.element( element );

			// or option elements, check parent select in that case
			***REMOVED*** else if ( element.parentNode.name in this.submitted ) ***REMOVED***
				this.element( element.parentNode );
			***REMOVED***
		***REMOVED***,
		highlight: function( element, errorClass, validClass ) ***REMOVED***
			if ( element.type === "radio" ) ***REMOVED***
				this.findByName( element.name ).addClass( errorClass ).removeClass( validClass );
			***REMOVED*** else ***REMOVED***
				$( element ).addClass( errorClass ).removeClass( validClass );
			***REMOVED***
		***REMOVED***,
		unhighlight: function( element, errorClass, validClass ) ***REMOVED***
			if ( element.type === "radio" ) ***REMOVED***
				this.findByName( element.name ).removeClass( errorClass ).addClass( validClass );
			***REMOVED*** else ***REMOVED***
				$( element ).removeClass( errorClass ).addClass( validClass );
			***REMOVED***
		***REMOVED***
	***REMOVED***,

	// http://jqueryvalidation.org/jQuery.validator.setDefaults/
	setDefaults: function( settings ) ***REMOVED***
		$.extend( $.validator.defaults, settings );
	***REMOVED***,

	messages: ***REMOVED***
		required: "This field is required.",
		remote: "Please fix this field.",
		email: "Please enter a valid email address.",
		url: "Please enter a valid URL.",
		date: "Please enter a valid date.",
		dateISO: "Please enter a valid date ( ISO ).",
		number: "Please enter a valid number.",
		digits: "Please enter only digits.",
		creditcard: "Please enter a valid credit card number.",
		equalTo: "Please enter the same value again.",
		maxlength: $.validator.format( "Please enter no more than ***REMOVED***0***REMOVED*** characters." ),
		minlength: $.validator.format( "Please enter at least ***REMOVED***0***REMOVED*** characters." ),
		rangelength: $.validator.format( "Please enter a value between ***REMOVED***0***REMOVED*** and ***REMOVED***1***REMOVED*** characters long." ),
		range: $.validator.format( "Please enter a value between ***REMOVED***0***REMOVED*** and ***REMOVED***1***REMOVED***." ),
		max: $.validator.format( "Please enter a value less than or equal to ***REMOVED***0***REMOVED***." ),
		min: $.validator.format( "Please enter a value greater than or equal to ***REMOVED***0***REMOVED***." )
	***REMOVED***,

	autoCreateRanges: false,

	prototype: ***REMOVED***

		init: function() ***REMOVED***
			this.labelContainer = $( this.settings.errorLabelContainer );
			this.errorContext = this.labelContainer.length && this.labelContainer || $( this.currentForm );
			this.containers = $( this.settings.errorContainer ).add( this.settings.errorLabelContainer );
			this.submitted = ***REMOVED******REMOVED***;
			this.valueCache = ***REMOVED******REMOVED***;
			this.pendingRequest = 0;
			this.pending = ***REMOVED******REMOVED***;
			this.invalid = ***REMOVED******REMOVED***;
			this.reset();

			var groups = ( this.groups = ***REMOVED******REMOVED*** ),
				rules;
			$.each( this.settings.groups, function( key, value ) ***REMOVED***
				if ( typeof value === "string" ) ***REMOVED***
					value = value.split( /\s/ );
				***REMOVED***
				$.each( value, function( index, name ) ***REMOVED***
					groups[ name ] = key;
				***REMOVED***);
			***REMOVED***);
			rules = this.settings.rules;
			$.each( rules, function( key, value ) ***REMOVED***
				rules[ key ] = $.validator.normalizeRule( value );
			***REMOVED***);

			function delegate( event ) ***REMOVED***
				var validator = $.data( this.form, "validator" ),
					eventType = "on" + event.type.replace( /^validate/, "" ),
					settings = validator.settings;
				if ( settings[ eventType ] && !$( this ).is( settings.ignore ) ) ***REMOVED***
					settings[ eventType ].call( validator, this, event );
				***REMOVED***
			***REMOVED***

			$( this.currentForm )
				.on( "focusin.validate focusout.validate keyup.validate",
					":text, [type='password'], [type='file'], select, textarea, [type='number'], [type='search'], " +
					"[type='tel'], [type='url'], [type='email'], [type='datetime'], [type='date'], [type='month'], " +
					"[type='week'], [type='time'], [type='datetime-local'], [type='range'], [type='color'], " +
					"[type='radio'], [type='checkbox']", delegate)
				// Support: Chrome, oldIE
				// "select" is provided as event.target when clicking a option
				.on("click.validate", "select, option, [type='radio'], [type='checkbox']", delegate);

			if ( this.settings.invalidHandler ) ***REMOVED***
				$( this.currentForm ).on( "invalid-form.validate", this.settings.invalidHandler );
			***REMOVED***

			// Add aria-required to any Static/Data/Class required fields before first validation
			// Screen readers require this attribute to be present before the initial submission http://www.w3.org/TR/WCAG-TECHS/ARIA2.html
			$( this.currentForm ).find( "[required], [data-rule-required], .required" ).attr( "aria-required", "true" );
		***REMOVED***,

		// http://jqueryvalidation.org/Validator.form/
		form: function() ***REMOVED***
			this.checkForm();
			$.extend( this.submitted, this.errorMap );
			this.invalid = $.extend(***REMOVED******REMOVED***, this.errorMap );
			if ( !this.valid() ) ***REMOVED***
				$( this.currentForm ).triggerHandler( "invalid-form", [ this ]);
			***REMOVED***
			this.showErrors();
			return this.valid();
		***REMOVED***,

		checkForm: function() ***REMOVED***
			this.prepareForm();
			for ( var i = 0, elements = ( this.currentElements = this.elements() ); elements[ i ]; i++ ) ***REMOVED***
				this.check( elements[ i ] );
			***REMOVED***
			return this.valid();
		***REMOVED***,

		// http://jqueryvalidation.org/Validator.element/
		element: function( element ) ***REMOVED***
			var cleanElement = this.clean( element ),
				checkElement = this.validationTargetFor( cleanElement ),
				result = true;

			this.lastElement = checkElement;

			if ( checkElement === undefined ) ***REMOVED***
				delete this.invalid[ cleanElement.name ];
			***REMOVED*** else ***REMOVED***
				this.prepareElement( checkElement );
				this.currentElements = $( checkElement );

				result = this.check( checkElement ) !== false;
				if ( result ) ***REMOVED***
					delete this.invalid[ checkElement.name ];
				***REMOVED*** else ***REMOVED***
					this.invalid[ checkElement.name ] = true;
				***REMOVED***
			***REMOVED***
			// Add aria-invalid status for screen readers
			$( element ).attr( "aria-invalid", !result );

			if ( !this.numberOfInvalids() ) ***REMOVED***
				// Hide error containers on last error
				this.toHide = this.toHide.add( this.containers );
			***REMOVED***
			this.showErrors();
			return result;
		***REMOVED***,

		// http://jqueryvalidation.org/Validator.showErrors/
		showErrors: function( errors ) ***REMOVED***
			if ( errors ) ***REMOVED***
				// add items to error list and map
				$.extend( this.errorMap, errors );
				this.errorList = [];
				for ( var name in errors ) ***REMOVED***
					this.errorList.push(***REMOVED***
						message: errors[ name ],
						element: this.findByName( name )[ 0 ]
					***REMOVED***);
				***REMOVED***
				// remove items from success list
				this.successList = $.grep( this.successList, function( element ) ***REMOVED***
					return !( element.name in errors );
				***REMOVED***);
			***REMOVED***
			if ( this.settings.showErrors ) ***REMOVED***
				this.settings.showErrors.call( this, this.errorMap, this.errorList );
			***REMOVED*** else ***REMOVED***
				this.defaultShowErrors();
			***REMOVED***
		***REMOVED***,

		// http://jqueryvalidation.org/Validator.resetForm/
		resetForm: function() ***REMOVED***
			if ( $.fn.resetForm ) ***REMOVED***
				$( this.currentForm ).resetForm();
			***REMOVED***
			this.submitted = ***REMOVED******REMOVED***;
			this.lastElement = null;
			this.prepareForm();
			this.hideErrors();
			var i, elements = this.elements()
				.removeData( "previousValue" )
				.removeAttr( "aria-invalid" );

			if ( this.settings.unhighlight ) ***REMOVED***
				for ( i = 0; elements[ i ]; i++ ) ***REMOVED***
					this.settings.unhighlight.call( this, elements[ i ],
						this.settings.errorClass, "" );
				***REMOVED***
			***REMOVED*** else ***REMOVED***
				elements.removeClass( this.settings.errorClass );
			***REMOVED***
		***REMOVED***,

		numberOfInvalids: function() ***REMOVED***
			return this.objectLength( this.invalid );
		***REMOVED***,

		objectLength: function( obj ) ***REMOVED***
			/* jshint unused: false */
			var count = 0,
				i;
			for ( i in obj ) ***REMOVED***
				count++;
			***REMOVED***
			return count;
		***REMOVED***,

		hideErrors: function() ***REMOVED***
			this.hideThese( this.toHide );
		***REMOVED***,

		hideThese: function( errors ) ***REMOVED***
			errors.not( this.containers ).text( "" );
			this.addWr***REMOVED***er( errors ).hide();
		***REMOVED***,

		valid: function() ***REMOVED***
			return this.size() === 0;
		***REMOVED***,

		size: function() ***REMOVED***
			return this.errorList.length;
		***REMOVED***,

		focusInvalid: function() ***REMOVED***
			if ( this.settings.focusInvalid ) ***REMOVED***
				***REMOVED*** ***REMOVED***
					$( this.findLastActive() || this.errorList.length && this.errorList[ 0 ].element || [])
					.filter( ":visible" )
					.focus()
					// manually trigger focusin event; without it, focusin handler isn't called, findLastActive won't have anything to find
					.trigger( "focusin" );
				***REMOVED*** catch ( e ) ***REMOVED***
					// ignore IE throwing errors when focusing hidden elements
				***REMOVED***
			***REMOVED***
		***REMOVED***,

		findLastActive: function() ***REMOVED***
			var lastActive = this.lastActive;
			return lastActive && $.grep( this.errorList, function( n ) ***REMOVED***
				return n.element.name === lastActive.name;
			***REMOVED***).length === 1 && lastActive;
		***REMOVED***,

		elements: function() ***REMOVED***
			var validator = this,
				rulesCache = ***REMOVED******REMOVED***;

			// select all valid inputs inside the form (no submit or reset buttons)
			return $( this.currentForm )
			.find( "input, select, textarea" )
			.not( ":submit, :reset, :image, :disabled" )
			.not( this.settings.ignore )
			.filter( function() ***REMOVED***
				if ( !this.name && validator.settings.debug && window.console ) ***REMOVED***
					console.error( "%o has no name assigned", this );
				***REMOVED***

				// select only the first element for each name, and only those with rules specified
				if ( this.name in rulesCache || !validator.objectLength( $( this ).rules() ) ) ***REMOVED***
					return false;
				***REMOVED***

				rulesCache[ this.name ] = true;
				return true;
			***REMOVED***);
		***REMOVED***,

		clean: function( selector ) ***REMOVED***
			return $( selector )[ 0 ];
		***REMOVED***,

		errors: function() ***REMOVED***
			var errorClass = this.settings.errorClass.split( " " ).join( "." );
			return $( this.settings.errorElement + "." + errorClass, this.errorContext );
		***REMOVED***,

		reset: function() ***REMOVED***
			this.successList = [];
			this.errorList = [];
			this.errorMap = ***REMOVED******REMOVED***;
			this.toShow = $( [] );
			this.toHide = $( [] );
			this.currentElements = $( [] );
		***REMOVED***,

		prepareForm: function() ***REMOVED***
			this.reset();
			this.toHide = this.errors().add( this.containers );
		***REMOVED***,

		prepareElement: function( element ) ***REMOVED***
			this.reset();
			this.toHide = this.errorsFor( element );
		***REMOVED***,

		elementValue: function( element ) ***REMOVED***
			var val,
				$element = $( element ),
				type = element.type;

			if ( type === "radio" || type === "checkbox" ) ***REMOVED***
				return this.findByName( element.name ).filter(":checked").val();
			***REMOVED*** else if ( type === "number" && typeof element.validity !== "undefined" ) ***REMOVED***
				return element.validity.badInput ? false : $element.val();
			***REMOVED***

			val = $element.val();
			if ( typeof val === "string" ) ***REMOVED***
				return val.replace(/\r/g, "" );
			***REMOVED***
			return val;
		***REMOVED***,

		check: function( element ) ***REMOVED***
			element = this.validationTargetFor( this.clean( element ) );

			var rules = $( element ).rules(),
				rulesCount = $.map( rules, function( n, i ) ***REMOVED***
					return i;
				***REMOVED***).length,
				dependencyMismatch = false,
				val = this.elementValue( element ),
				result, method, rule;

			for ( method in rules ) ***REMOVED***
				rule = ***REMOVED*** method: method, parameters: rules[ method ] ***REMOVED***;
				***REMOVED*** ***REMOVED***

					result = $.validator.methods[ method ].call( this, val, element, rule.parameters );

					// if a method indicates that the field is optional and therefore valid,
					// don't mark it as valid when there are no other rules
					if ( result === "dependency-mismatch" && rulesCount === 1 ) ***REMOVED***
						dependencyMismatch = true;
						continue;
					***REMOVED***
					dependencyMismatch = false;

					if ( result === "pending" ) ***REMOVED***
						this.toHide = this.toHide.not( this.errorsFor( element ) );
						return;
					***REMOVED***

					if ( !result ) ***REMOVED***
						this.formatAndAdd( element, rule );
						return false;
					***REMOVED***
				***REMOVED*** catch ( e ) ***REMOVED***
					if ( this.settings.debug && window.console ) ***REMOVED***
						console.log( "Exception occurred when checking element " + element.id + ", check the '" + rule.method + "' method.", e );
					***REMOVED***
					if ( e instanceof TypeError ) ***REMOVED***
						e.message += ".  Exception occurred when checking element " + element.id + ", check the '" + rule.method + "' method.";
					***REMOVED***

					throw e;
				***REMOVED***
			***REMOVED***
			if ( dependencyMismatch ) ***REMOVED***
				return;
			***REMOVED***
			if ( this.objectLength( rules ) ) ***REMOVED***
				this.successList.push( element );
			***REMOVED***
			return true;
		***REMOVED***,

		// return the custom message for the given element and validation method
		// specified in the element's HTML5 data attribute
		// return the generic message if present and no method specific message is present
		customDataMessage: function( element, method ) ***REMOVED***
			return $( element ).data( "msg" + method.charAt( 0 ).toUpperCase() +
				method.substring( 1 ).toLowerCase() ) || $( element ).data( "msg" );
		***REMOVED***,

		// return the custom message for the given element name and validation method
		customMessage: function( name, method ) ***REMOVED***
			var m = this.settings.messages[ name ];
			return m && ( m.constructor === String ? m : m[ method ]);
		***REMOVED***,

		// return the first defined argument, allowing empty strings
		findDefined: function() ***REMOVED***
			for ( var i = 0; i < arguments.length; i++) ***REMOVED***
				if ( arguments[ i ] !== undefined ) ***REMOVED***
					return arguments[ i ];
				***REMOVED***
			***REMOVED***
			return undefined;
		***REMOVED***,

		defaultMessage: function( element, method ) ***REMOVED***
			return this.findDefined(
				this.customMessage( element.name, method ),
				this.customDataMessage( element, method ),
				// title is never undefined, so handle empty string as undefined
				!this.settings.ignoreTitle && element.title || undefined,
				$.validator.messages[ method ],
				"<strong>Warning: No message defined for " + element.name + "</strong>"
			);
		***REMOVED***,

		formatAndAdd: function( element, rule ) ***REMOVED***
			var message = this.defaultMessage( element, rule.method ),
				theregex = /\$?\***REMOVED***(\d+)\***REMOVED***/g;
			if ( typeof message === "function" ) ***REMOVED***
				message = message.call( this, rule.parameters, element );
			***REMOVED*** else if ( theregex.test( message ) ) ***REMOVED***
				message = $.validator.format( message.replace( theregex, "***REMOVED***$1***REMOVED***" ), rule.parameters );
			***REMOVED***
			this.errorList.push(***REMOVED***
				message: message,
				element: element,
				method: rule.method
			***REMOVED***);

			this.errorMap[ element.name ] = message;
			this.submitted[ element.name ] = message;
		***REMOVED***,

		addWr***REMOVED***er: function( toToggle ) ***REMOVED***
			if ( this.settings.wr***REMOVED***er ) ***REMOVED***
				toToggle = toToggle.add( toToggle.parent( this.settings.wr***REMOVED***er ) );
			***REMOVED***
			return toToggle;
		***REMOVED***,

		defaultShowErrors: function() ***REMOVED***
			var i, elements, error;
			for ( i = 0; this.errorList[ i ]; i++ ) ***REMOVED***
				error = this.errorList[ i ];
				if ( this.settings.highlight ) ***REMOVED***
					this.settings.highlight.call( this, error.element, this.settings.errorClass, this.settings.validClass );
				***REMOVED***
				this.showLabel( error.element, error.message );
			***REMOVED***
			if ( this.errorList.length ) ***REMOVED***
				this.toShow = this.toShow.add( this.containers );
			***REMOVED***
			if ( this.settings.success ) ***REMOVED***
				for ( i = 0; this.successList[ i ]; i++ ) ***REMOVED***
					this.showLabel( this.successList[ i ] );
				***REMOVED***
			***REMOVED***
			if ( this.settings.unhighlight ) ***REMOVED***
				for ( i = 0, elements = this.validElements(); elements[ i ]; i++ ) ***REMOVED***
					this.settings.unhighlight.call( this, elements[ i ], this.settings.errorClass, this.settings.validClass );
				***REMOVED***
			***REMOVED***
			this.toHide = this.toHide.not( this.toShow );
			this.hideErrors();
			this.addWr***REMOVED***er( this.toShow ).show();
		***REMOVED***,

		validElements: function() ***REMOVED***
			return this.currentElements.not( this.invalidElements() );
		***REMOVED***,

		invalidElements: function() ***REMOVED***
			return $( this.errorList ).map(function() ***REMOVED***
				return this.element;
			***REMOVED***);
		***REMOVED***,

		showLabel: function( element, message ) ***REMOVED***
			var place, group, errorID,
				error = this.errorsFor( element ),
				elementID = this.idOrName( element ),
				describedBy = $( element ).attr( "aria-describedby" );
			if ( error.length ) ***REMOVED***
				// refresh error/success class
				error.removeClass( this.settings.validClass ).addClass( this.settings.errorClass );
				// replace message on existing label
				error.html( message );
			***REMOVED*** else ***REMOVED***
				// create error element
				error = $( "<" + this.settings.errorElement + ">" )
					.attr( "id", elementID + "-error" )
					.addClass( this.settings.errorClass )
					.html( message || "" );

				// Maintain reference to the element to be placed into the DOM
				place = error;
				if ( this.settings.wr***REMOVED***er ) ***REMOVED***
					// make sure the element is visible, even in IE
					// actually showing the wr***REMOVED***ed element is handled elsewhere
					place = error.hide().show().wrap( "<" + this.settings.wr***REMOVED***er + "/>" ).parent();
				***REMOVED***
				if ( this.labelContainer.length ) ***REMOVED***
					this.labelContainer.***REMOVED***end( place );
				***REMOVED*** else if ( this.settings.errorPlacement ) ***REMOVED***
					this.settings.errorPlacement( place, $( element ) );
				***REMOVED*** else ***REMOVED***
					place.insertAfter( element );
				***REMOVED***

				// Link error back to the element
				if ( error.is( "label" ) ) ***REMOVED***
					// If the error is a label, then associate using 'for'
					error.attr( "for", elementID );
				***REMOVED*** else if ( error.parents( "label[for='" + elementID + "']" ).length === 0 ) ***REMOVED***
					// If the element is not a child of an associated label, then it's necessary
					// to explicitly ***REMOVED***ly aria-describedby

					errorID = error.attr( "id" ).replace( /(:|\.|\[|\]|\$)/g, "\\$1");
					// Respect existing non-error aria-describedby
					if ( !describedBy ) ***REMOVED***
						describedBy = errorID;
					***REMOVED*** else if ( !describedBy.match( new RegExp( "\\b" + errorID + "\\b" ) ) ) ***REMOVED***
						// Add to end of list if not already present
						describedBy += " " + errorID;
					***REMOVED***
					$( element ).attr( "aria-describedby", describedBy );

					// If this element is grouped, then assign to all elements in the same group
					group = this.groups[ element.name ];
					if ( group ) ***REMOVED***
						$.each( this.groups, function( name, testgroup ) ***REMOVED***
							if ( testgroup === group ) ***REMOVED***
								$( "[name='" + name + "']", this.currentForm )
									.attr( "aria-describedby", error.attr( "id" ) );
							***REMOVED***
						***REMOVED***);
					***REMOVED***
				***REMOVED***
			***REMOVED***
			if ( !message && this.settings.success ) ***REMOVED***
				error.text( "" );
				if ( typeof this.settings.success === "string" ) ***REMOVED***
					error.addClass( this.settings.success );
				***REMOVED*** else ***REMOVED***
					this.settings.success( error, element );
				***REMOVED***
			***REMOVED***
			this.toShow = this.toShow.add( error );
		***REMOVED***,

		errorsFor: function( element ) ***REMOVED***
			var name = this.idOrName( element ),
				describer = $( element ).attr( "aria-describedby" ),
				selector = "label[for='" + name + "'], label[for='" + name + "'] *";

			// aria-describedby should directly reference the error element
			if ( describer ) ***REMOVED***
				selector = selector + ", #" + describer.replace( /\s+/g, ", #" );
			***REMOVED***
			return this
				.errors()
				.filter( selector );
		***REMOVED***,

		idOrName: function( element ) ***REMOVED***
			return this.groups[ element.name ] || ( this.checkable( element ) ? element.name : element.id || element.name );
		***REMOVED***,

		validationTargetFor: function( element ) ***REMOVED***

			// If radio/checkbox, validate first element in group instead
			if ( this.checkable( element ) ) ***REMOVED***
				element = this.findByName( element.name );
			***REMOVED***

			// Always ***REMOVED***ly ignore filter
			return $( element ).not( this.settings.ignore )[ 0 ];
		***REMOVED***,

		checkable: function( element ) ***REMOVED***
			return ( /radio|checkbox/i ).test( element.type );
		***REMOVED***,

		findByName: function( name ) ***REMOVED***
			return $( this.currentForm ).find( "[name='" + name + "']" );
		***REMOVED***,

		getLength: function( value, element ) ***REMOVED***
			switch ( element.nodeName.toLowerCase() ) ***REMOVED***
			case "select":
				return $( "option:selected", element ).length;
			case "input":
				if ( this.checkable( element ) ) ***REMOVED***
					return this.findByName( element.name ).filter( ":checked" ).length;
				***REMOVED***
			***REMOVED***
			return value.length;
		***REMOVED***,

		depend: function( param, element ) ***REMOVED***
			return this.dependTypes[typeof param] ? this.dependTypes[typeof param]( param, element ) : true;
		***REMOVED***,

		dependTypes: ***REMOVED***
			"boolean": function( param ) ***REMOVED***
				return param;
			***REMOVED***,
			"string": function( param, element ) ***REMOVED***
				return !!$( param, element.form ).length;
			***REMOVED***,
			"function": function( param, element ) ***REMOVED***
				return param( element );
			***REMOVED***
		***REMOVED***,

		optional: function( element ) ***REMOVED***
			var val = this.elementValue( element );
			return !$.validator.methods.required.call( this, val, element ) && "dependency-mismatch";
		***REMOVED***,

		startRequest: function( element ) ***REMOVED***
			if ( !this.pending[ element.name ] ) ***REMOVED***
				this.pendingRequest++;
				this.pending[ element.name ] = true;
			***REMOVED***
		***REMOVED***,

		stopRequest: function( element, valid ) ***REMOVED***
			this.pendingRequest--;
			// sometimes synchronization fails, make sure pendingRequest is never < 0
			if ( this.pendingRequest < 0 ) ***REMOVED***
				this.pendingRequest = 0;
			***REMOVED***
			delete this.pending[ element.name ];
			if ( valid && this.pendingRequest === 0 && this.formSubmitted && this.form() ) ***REMOVED***
				$( this.currentForm ).submit();
				this.formSubmitted = false;
			***REMOVED*** else if (!valid && this.pendingRequest === 0 && this.formSubmitted ) ***REMOVED***
				$( this.currentForm ).triggerHandler( "invalid-form", [ this ]);
				this.formSubmitted = false;
			***REMOVED***
		***REMOVED***,

		previousValue: function( element ) ***REMOVED***
			return $.data( element, "previousValue" ) || $.data( element, "previousValue", ***REMOVED***
				old: null,
				valid: true,
				message: this.defaultMessage( element, "remote" )
			***REMOVED***);
		***REMOVED***,

		// cleans up all forms and elements, removes validator-specific events
		destroy: function() ***REMOVED***
			this.resetForm();

			$( this.currentForm )
				.off( ".validate" )
				.removeData( "validator" );
		***REMOVED***

	***REMOVED***,

	classRuleSettings: ***REMOVED***
		required: ***REMOVED*** required: true ***REMOVED***,
		email: ***REMOVED*** email: true ***REMOVED***,
		url: ***REMOVED*** url: true ***REMOVED***,
		date: ***REMOVED*** date: true ***REMOVED***,
		dateISO: ***REMOVED*** dateISO: true ***REMOVED***,
		number: ***REMOVED*** number: true ***REMOVED***,
		digits: ***REMOVED*** digits: true ***REMOVED***,
		creditcard: ***REMOVED*** creditcard: true ***REMOVED***
	***REMOVED***,

	addClassRules: function( className, rules ) ***REMOVED***
		if ( className.constructor === String ) ***REMOVED***
			this.classRuleSettings[ className ] = rules;
		***REMOVED*** else ***REMOVED***
			$.extend( this.classRuleSettings, className );
		***REMOVED***
	***REMOVED***,

	classRules: function( element ) ***REMOVED***
		var rules = ***REMOVED******REMOVED***,
			classes = $( element ).attr( "class" );

		if ( classes ) ***REMOVED***
			$.each( classes.split( " " ), function() ***REMOVED***
				if ( this in $.validator.classRuleSettings ) ***REMOVED***
					$.extend( rules, $.validator.classRuleSettings[ this ]);
				***REMOVED***
			***REMOVED***);
		***REMOVED***
		return rules;
	***REMOVED***,

	normalizeAttributeRule: function( rules, type, method, value ) ***REMOVED***

		// convert the value to a number for number inputs, and for text for backwards compability
		// allows type="date" and others to be compared as strings
		if ( /min|max/.test( method ) && ( type === null || /number|range|text/.test( type ) ) ) ***REMOVED***
			value = Number( value );

			// Support Opera Mini, which returns NaN for undefined minlength
			if ( isNaN( value ) ) ***REMOVED***
				value = undefined;
			***REMOVED***
		***REMOVED***

		if ( value || value === 0 ) ***REMOVED***
			rules[ method ] = value;
		***REMOVED*** else if ( type === method && type !== "range" ) ***REMOVED***

			// exception: the jquery validate 'range' method
			// does not test for the html5 'range' type
			rules[ method ] = true;
		***REMOVED***
	***REMOVED***,

	attributeRules: function( element ) ***REMOVED***
		var rules = ***REMOVED******REMOVED***,
			$element = $( element ),
			type = element.getAttribute( "type" ),
			method, value;

		for ( method in $.validator.methods ) ***REMOVED***

			// support for <input required> in both html5 and older browsers
			if ( method === "required" ) ***REMOVED***
				value = element.getAttribute( method );

				// Some browsers return an empty string for the required attribute
				// and non-HTML5 browsers might have required="" markup
				if ( value === "" ) ***REMOVED***
					value = true;
				***REMOVED***

				// force non-HTML5 browsers to return bool
				value = !!value;
			***REMOVED*** else ***REMOVED***
				value = $element.attr( method );
			***REMOVED***

			this.normalizeAttributeRule( rules, type, method, value );
		***REMOVED***

		// maxlength may be returned as -1, 2147483647 ( IE ) and 524288 ( safari ) for text inputs
		if ( rules.maxlength && /-1|2147483647|524288/.test( rules.maxlength ) ) ***REMOVED***
			delete rules.maxlength;
		***REMOVED***

		return rules;
	***REMOVED***,

	dataRules: function( element ) ***REMOVED***
		var rules = ***REMOVED******REMOVED***,
			$element = $( element ),
			type = element.getAttribute( "type" ),
			method, value;

		for ( method in $.validator.methods ) ***REMOVED***
			value = $element.data( "rule" + method.charAt( 0 ).toUpperCase() + method.substring( 1 ).toLowerCase() );
			this.normalizeAttributeRule( rules, type, method, value );
		***REMOVED***
		return rules;
	***REMOVED***,

	staticRules: function( element ) ***REMOVED***
		var rules = ***REMOVED******REMOVED***,
			validator = $.data( element.form, "validator" );

		if ( validator.settings.rules ) ***REMOVED***
			rules = $.validator.normalizeRule( validator.settings.rules[ element.name ] ) || ***REMOVED******REMOVED***;
		***REMOVED***
		return rules;
	***REMOVED***,

	normalizeRules: function( rules, element ) ***REMOVED***
		// handle dependency check
		$.each( rules, function( prop, val ) ***REMOVED***
			// ignore rule when param is explicitly false, eg. required:false
			if ( val === false ) ***REMOVED***
				delete rules[ prop ];
				return;
			***REMOVED***
			if ( val.param || val.depends ) ***REMOVED***
				var keepRule = true;
				switch ( typeof val.depends ) ***REMOVED***
				case "string":
					keepRule = !!$( val.depends, element.form ).length;
					break;
				case "function":
					keepRule = val.depends.call( element, element );
					break;
				***REMOVED***
				if ( keepRule ) ***REMOVED***
					rules[ prop ] = val.param !== undefined ? val.param : true;
				***REMOVED*** else ***REMOVED***
					delete rules[ prop ];
				***REMOVED***
			***REMOVED***
		***REMOVED***);

		// evaluate parameters
		$.each( rules, function( rule, parameter ) ***REMOVED***
			rules[ rule ] = $.isFunction( parameter ) ? parameter( element ) : parameter;
		***REMOVED***);

		// clean number parameters
		$.each([ "minlength", "maxlength" ], function() ***REMOVED***
			if ( rules[ this ] ) ***REMOVED***
				rules[ this ] = Number( rules[ this ] );
			***REMOVED***
		***REMOVED***);
		$.each([ "rangelength", "range" ], function() ***REMOVED***
			var parts;
			if ( rules[ this ] ) ***REMOVED***
				if ( $.isArray( rules[ this ] ) ) ***REMOVED***
					rules[ this ] = [ Number( rules[ this ][ 0 ]), Number( rules[ this ][ 1 ] ) ];
				***REMOVED*** else if ( typeof rules[ this ] === "string" ) ***REMOVED***
					parts = rules[ this ].replace(/[\[\]]/g, "" ).split( /[\s,]+/ );
					rules[ this ] = [ Number( parts[ 0 ]), Number( parts[ 1 ] ) ];
				***REMOVED***
			***REMOVED***
		***REMOVED***);

		if ( $.validator.autoCreateRanges ) ***REMOVED***
			// auto-create ranges
			if ( rules.min != null && rules.max != null ) ***REMOVED***
				rules.range = [ rules.min, rules.max ];
				delete rules.min;
				delete rules.max;
			***REMOVED***
			if ( rules.minlength != null && rules.maxlength != null ) ***REMOVED***
				rules.rangelength = [ rules.minlength, rules.maxlength ];
				delete rules.minlength;
				delete rules.maxlength;
			***REMOVED***
		***REMOVED***

		return rules;
	***REMOVED***,

	// Converts a simple string to a ***REMOVED***string: true***REMOVED*** rule, e.g., "required" to ***REMOVED***required:true***REMOVED***
	normalizeRule: function( data ) ***REMOVED***
		if ( typeof data === "string" ) ***REMOVED***
			var transformed = ***REMOVED******REMOVED***;
			$.each( data.split( /\s/ ), function() ***REMOVED***
				transformed[ this ] = true;
			***REMOVED***);
			data = transformed;
		***REMOVED***
		return data;
	***REMOVED***,

	// http://jqueryvalidation.org/jQuery.validator.addMethod/
	addMethod: function( name, method, message ) ***REMOVED***
		$.validator.methods[ name ] = method;
		$.validator.messages[ name ] = message !== undefined ? message : $.validator.messages[ name ];
		if ( method.length < 3 ) ***REMOVED***
			$.validator.addClassRules( name, $.validator.normalizeRule( name ) );
		***REMOVED***
	***REMOVED***,

	methods: ***REMOVED***

		// http://jqueryvalidation.org/required-method/
		required: function( value, element, param ) ***REMOVED***
			// check if dependency is met
			if ( !this.depend( param, element ) ) ***REMOVED***
				return "dependency-mismatch";
			***REMOVED***
			if ( element.nodeName.toLowerCase() === "select" ) ***REMOVED***
				// could be an array for select-multiple or a string, both are fine this way
				var val = $( element ).val();
				return val && val.length > 0;
			***REMOVED***
			if ( this.checkable( element ) ) ***REMOVED***
				return this.getLength( value, element ) > 0;
			***REMOVED***
			return value.length > 0;
		***REMOVED***,

		// http://jqueryvalidation.org/email-method/
		email: function( value, element ) ***REMOVED***
			// From https://html.spec.whatwg.org/multipage/forms.html#valid-e-mail-address
			// Retrieved 2014-01-14
			// If you have a problem with this implementation, report a bug against the above spec
			// Or use custom methods to implement your own email validation
			return this.optional( element ) || /^[a-zA-Z0-9.!#$%&'*+\/=?^_`***REMOVED***|***REMOVED***~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]***REMOVED***0,61***REMOVED***[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]***REMOVED***0,61***REMOVED***[a-zA-Z0-9])?)*$/.test( value );
		***REMOVED***,

		// http://jqueryvalidation.org/url-method/
		url: function( value, element ) ***REMOVED***

			// Copyright (c) 2010-2013 Diego Perini, MIT licensed
			// https://gist.github.com/dperini/729294
			// see also https://mathiasbynens.be/demo/url-regex
			// modified to allow protocol-relative URLs
			return this.optional( element ) || /^(?:(?:(?:https?|ftp):)?\/\/)(?:\S+(?::\S*)?@)?(?:(?!(?:10|127)(?:\.\d***REMOVED***1,3***REMOVED***)***REMOVED***3***REMOVED***)(?!(?:169\.254|192\.168)(?:\.\d***REMOVED***1,3***REMOVED***)***REMOVED***2***REMOVED***)(?!172\.(?:1[6-9]|2\d|3[0-1])(?:\.\d***REMOVED***1,3***REMOVED***)***REMOVED***2***REMOVED***)(?:[1-9]\d?|1\d\d|2[01]\d|22[0-3])(?:\.(?:1?\d***REMOVED***1,2***REMOVED***|2[0-4]\d|25[0-5]))***REMOVED***2***REMOVED***(?:\.(?:[1-9]\d?|1\d\d|2[0-4]\d|25[0-4]))|(?:(?:[a-z\u00a1-\uffff0-9]-*)*[a-z\u00a1-\uffff0-9]+)(?:\.(?:[a-z\u00a1-\uffff0-9]-*)*[a-z\u00a1-\uffff0-9]+)*(?:\.(?:[a-z\u00a1-\uffff]***REMOVED***2,***REMOVED***)).?)(?::\d***REMOVED***2,5***REMOVED***)?(?:[/?#]\S*)?$/i.test( value );
		***REMOVED***,

		// http://jqueryvalidation.org/date-method/
		date: function( value, element ) ***REMOVED***
			return this.optional( element ) || !/Invalid|NaN/.test( new Date( value ).toString() );
		***REMOVED***,

		// http://jqueryvalidation.org/dateISO-method/
		dateISO: function( value, element ) ***REMOVED***
			return this.optional( element ) || /^\d***REMOVED***4***REMOVED***[\/\-](0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])$/.test( value );
		***REMOVED***,

		// http://jqueryvalidation.org/number-method/
		number: function( value, element ) ***REMOVED***
			return this.optional( element ) || /^(?:-?\d+|-?\d***REMOVED***1,3***REMOVED***(?:,\d***REMOVED***3***REMOVED***)+)?(?:\.\d+)?$/.test( value );
		***REMOVED***,

		// http://jqueryvalidation.org/digits-method/
		digits: function( value, element ) ***REMOVED***
			return this.optional( element ) || /^\d+$/.test( value );
		***REMOVED***,

		// http://jqueryvalidation.org/creditcard-method/
		// based on http://en.wikipedia.org/wiki/Luhn_algorithm
		creditcard: function( value, element ) ***REMOVED***
			if ( this.optional( element ) ) ***REMOVED***
				return "dependency-mismatch";
			***REMOVED***
			// accept only spaces, digits and dashes
			if ( /[^0-9 \-]+/.test( value ) ) ***REMOVED***
				return false;
			***REMOVED***
			var nCheck = 0,
				nDigit = 0,
				bEven = false,
				n, cDigit;

			value = value.replace( /\D/g, "" );

			// Basing min and max length on
			// http://developer.ean.com/general_info/Valid_Credit_Card_Types
			if ( value.length < 13 || value.length > 19 ) ***REMOVED***
				return false;
			***REMOVED***

			for ( n = value.length - 1; n >= 0; n--) ***REMOVED***
				cDigit = value.charAt( n );
				nDigit = parseInt( cDigit, 10 );
				if ( bEven ) ***REMOVED***
					if ( ( nDigit *= 2 ) > 9 ) ***REMOVED***
						nDigit -= 9;
					***REMOVED***
				***REMOVED***
				nCheck += nDigit;
				bEven = !bEven;
			***REMOVED***

			return ( nCheck % 10 ) === 0;
		***REMOVED***,

		// http://jqueryvalidation.org/minlength-method/
		minlength: function( value, element, param ) ***REMOVED***
			var length = $.isArray( value ) ? value.length : this.getLength( value, element );
			return this.optional( element ) || length >= param;
		***REMOVED***,

		// http://jqueryvalidation.org/maxlength-method/
		maxlength: function( value, element, param ) ***REMOVED***
			var length = $.isArray( value ) ? value.length : this.getLength( value, element );
			return this.optional( element ) || length <= param;
		***REMOVED***,

		// http://jqueryvalidation.org/rangelength-method/
		rangelength: function( value, element, param ) ***REMOVED***
			var length = $.isArray( value ) ? value.length : this.getLength( value, element );
			return this.optional( element ) || ( length >= param[ 0 ] && length <= param[ 1 ] );
		***REMOVED***,

		// http://jqueryvalidation.org/min-method/
		min: function( value, element, param ) ***REMOVED***
			return this.optional( element ) || value >= param;
		***REMOVED***,

		// http://jqueryvalidation.org/max-method/
		max: function( value, element, param ) ***REMOVED***
			return this.optional( element ) || value <= param;
		***REMOVED***,

		// http://jqueryvalidation.org/range-method/
		range: function( value, element, param ) ***REMOVED***
			return this.optional( element ) || ( value >= param[ 0 ] && value <= param[ 1 ] );
		***REMOVED***,

		// http://jqueryvalidation.org/equalTo-method/
		equalTo: function( value, element, param ) ***REMOVED***
			// bind to the blur event of the target in order to revalidate whenever the target field is updated
			// TODO find a way to bind the event just once, avoiding the unbind-rebind overhead
			var target = $( param );
			if ( this.settings.onfocusout ) ***REMOVED***
				target.off( ".validate-equalTo" ).on( "blur.validate-equalTo", function() ***REMOVED***
					$( element ).valid();
				***REMOVED***);
			***REMOVED***
			return value === target.val();
		***REMOVED***,

		// http://jqueryvalidation.org/remote-method/
		remote: function( value, element, param ) ***REMOVED***
			if ( this.optional( element ) ) ***REMOVED***
				return "dependency-mismatch";
			***REMOVED***

			var previous = this.previousValue( element ),
				validator, data;

			if (!this.settings.messages[ element.name ] ) ***REMOVED***
				this.settings.messages[ element.name ] = ***REMOVED******REMOVED***;
			***REMOVED***
			previous.originalMessage = this.settings.messages[ element.name ].remote;
			this.settings.messages[ element.name ].remote = previous.message;

			param = typeof param === "string" && ***REMOVED*** url: param ***REMOVED*** || param;

			if ( previous.old === value ) ***REMOVED***
				return previous.valid;
			***REMOVED***

			previous.old = value;
			validator = this;
			this.startRequest( element );
			data = ***REMOVED******REMOVED***;
			data[ element.name ] = value;
			$.ajax( $.extend( true, ***REMOVED***
				mode: "abort",
				port: "validate" + element.name,
				dataType: "json",
				data: data,
				context: validator.currentForm,
				success: function( response ) ***REMOVED***
					var valid = response === true || response === "true",
						errors, message, submitted;

					validator.settings.messages[ element.name ].remote = previous.originalMessage;
					if ( valid ) ***REMOVED***
						submitted = validator.formSubmitted;
						validator.prepareElement( element );
						validator.formSubmitted = submitted;
						validator.successList.push( element );
						delete validator.invalid[ element.name ];
						validator.showErrors();
					***REMOVED*** else ***REMOVED***
						errors = ***REMOVED******REMOVED***;
						message = response || validator.defaultMessage( element, "remote" );
						errors[ element.name ] = previous.message = $.isFunction( message ) ? message( value ) : message;
						validator.invalid[ element.name ] = true;
						validator.showErrors( errors );
					***REMOVED***
					previous.valid = valid;
					validator.stopRequest( element, valid );
				***REMOVED***
			***REMOVED***, param ) );
			return "pending";
		***REMOVED***
	***REMOVED***

***REMOVED***);

// ajax mode: abort
// usage: $.ajax(***REMOVED*** mode: "abort"[, port: "uniqueport"]***REMOVED***);
// if mode:"abort" is used, the previous request on that port (port can be undefined) is aborted via XMLHttpRequest.abort()

var pendingRequests = ***REMOVED******REMOVED***,
	ajax;
// Use a prefilter if available (1.5+)
if ( $.ajaxPrefilter ) ***REMOVED***
	$.ajaxPrefilter(function( settings, _, xhr ) ***REMOVED***
		var port = settings.port;
		if ( settings.mode === "abort" ) ***REMOVED***
			if ( pendingRequests[port] ) ***REMOVED***
				pendingRequests[port].abort();
			***REMOVED***
			pendingRequests[port] = xhr;
		***REMOVED***
	***REMOVED***);
***REMOVED*** else ***REMOVED***
	// Proxy ajax
	ajax = $.ajax;
	$.ajax = function( settings ) ***REMOVED***
		var mode = ( "mode" in settings ? settings : $.ajaxSettings ).mode,
			port = ( "port" in settings ? settings : $.ajaxSettings ).port;
		if ( mode === "abort" ) ***REMOVED***
			if ( pendingRequests[port] ) ***REMOVED***
				pendingRequests[port].abort();
			***REMOVED***
			pendingRequests[port] = ajax.***REMOVED***ly(this, arguments);
			return pendingRequests[port];
		***REMOVED***
		return ajax.***REMOVED***ly(this, arguments);
	***REMOVED***;
***REMOVED***

***REMOVED***));