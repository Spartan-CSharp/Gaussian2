/**
 * Client-side validation for RequireAtLeastOne attribute
 * Validates that at least one of the specified properties has a non-empty value
 */

// Unobtrusive validation adapter for requireatleastone
$.validator.addMethod( "requireatleastone", function ( value, element, params ) {
	// Check if the current element has a value
	if ( value && (value || '').trim() !== '' ) {
		return true;
	}

	// Check other properties
	const otherProperties = params.otherproperties.split( ',' );
	const form = $( element ).closest( 'form' );

	for ( let i = 0; i < otherProperties.length; i++ ) {
		const propertyName = otherProperties[ i ];
		const propertyElement = form.find( `[name="${ propertyName }"]` );

		if ( propertyElement.length > 0 ) {
			const propertyValue = propertyElement.val();

			// Check if value is not null, undefined, empty, or only whitespace
			if ( propertyValue && (propertyValue || '').trim() !== '' ) {
				return true;
			}
		}
	}

	return false;
} );

// Register the adapter with jQuery unobtrusive validation
$.validator.unobtrusive.adapters.add( "requireatleastone", [ "otherproperties" ], function ( options ) {
	options.rules[ "requireatleastone" ] = {
		otherproperties: options.params.otherproperties
	};
	options.messages[ "requireatleastone" ] = options.message;
} );