$(function () {
	jQuery.validator.unobtrusive.adapters.add('numeric', ['milestone', 'mode'], function (options) {
		var params = {
			milestone: options.params.milestone,
			mode: options.params.mode
		};

		options.rules['numeric'] = params;
		if (options.message) {
			options.messages['numeric'] = options.message;
		}
	});

	jQuery.validator.addMethod("numeric", function (value, element, param) {

        // Value is invalid.
	    if (value == null)
	        return false;

	    // Convert string to integer.
	    param.mode = parseInt(param.mode);
	    param.milestone = parseFloat(param.milestone);
	    value = parseFloat(value);

	    switch (param.mode) {
	        case 0:
	            if (value >= param.milestone)
	                return false;
	            break;

	        case 1:
	            if (value > param.milestone)
	                return false;
	            break;

	        case 2:
	            if (value === param.milestone)
	                return false;
	            break;

	        case 3:
	            if (value < param.milestone)
	                return false;
	            break;

	        case 4:
	            if (value <= param.milestone)
	                return false;
	            break;
	    }

	    return true;
	});

}(jQuery));