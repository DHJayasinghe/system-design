/*
 * The número de identidad de extranjero ( NIE )is a code used to identify the non-nationals in Spain
 */
$.validator.addMethod( "nieES", function( value ) ***REMOVED***
	"use strict";

	value = value.toUpperCase();

	// Basic format test
	if ( !value.match( "((^[A-Z]***REMOVED***1***REMOVED***[0-9]***REMOVED***7***REMOVED***[A-Z0-9]***REMOVED***1***REMOVED***$|^[T]***REMOVED***1***REMOVED***[A-Z0-9]***REMOVED***8***REMOVED***$)|^[0-9]***REMOVED***8***REMOVED***[A-Z]***REMOVED***1***REMOVED***$)" ) ) ***REMOVED***
		return false;
	***REMOVED***

	// Test NIE
	//T
	if ( /^[T]***REMOVED***1***REMOVED***/.test( value ) ) ***REMOVED***
		return ( value[ 8 ] === /^[T]***REMOVED***1***REMOVED***[A-Z0-9]***REMOVED***8***REMOVED***$/.test( value ) );
	***REMOVED***

	//XYZ
	if ( /^[XYZ]***REMOVED***1***REMOVED***/.test( value ) ) ***REMOVED***
		return (
			value[ 8 ] === "TRWAGMYFPDXBNJZSQVHLCKE".charAt(
				value.replace( "X", "0" )
					.replace( "Y", "1" )
					.replace( "Z", "2" )
					.substring( 0, 8 ) % 23
			)
		);
	***REMOVED***

	return false;

***REMOVED***, "Please specify a valid NIE number." );
