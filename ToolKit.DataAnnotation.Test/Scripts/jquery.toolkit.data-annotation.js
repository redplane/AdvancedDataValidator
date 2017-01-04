/**
 * Prefix of validator.
 */
var validatorPrefixes = {
    startWith: 'startswith',
    endsWith: 'endswith',
    contains: 'contains'
}

/**
 * Check whether string starts with a property.
 */
$.validator.addMethod(validatorPrefixes.startWith, function (value, element, params) {
    try {
        // Value of the field is null.
        if (value == null)
            return false;

        // Find the target property value.
        var propertyValue = $('input[name="' + params + '"]').val();

        // Not the first element.
        if (value.indexOf(propertyValue) !== 0)
            return false;

        return true;
    } catch (exception) {
        return false;
    }
});

/**
 * Check whether string ends with a property's value.
 */
$.validator.addMethod(validatorPrefixes.endsWith, function (value, element, params) {

    try {

        // Value of the field is null.
        if (value == null)
            return false;

        // Find the target property value.
        var propertyValue = $('input[name="' + params + '"]').val();

        // Not the first element.
        return value.endsWith(propertyValue);

    } catch (exception) {
        return false;
    }
});

/**
 * Check whether string contains a property.
 */
$.validator.addMethod(validatorPrefixes.contains, function (value, element, params) {
    try {
        // Value of the field is null.
        if (value == null)
            return false;

        // Find the target property value.
        var propertyValue = $('input[name="' + params + '"]').val();

        // Not the first element.
        if (value.indexOf(propertyValue) === -1)
            return false;

        return true;
    } catch (exception) {
        return false;
    }
});

/**
 * Client-side validator of StartsWithPropertyAttribute.
 */
$.validator.unobtrusive.adapters.add(validatorPrefixes.startWith, function (options) {
    options.messages[validatorPrefixes.startWith] = options.message;
    options.rules[validatorPrefixes.startWith] = $(options.element).attr('data-val-startswith-property');
});

/**
 * Client-side validator of EndsWithPropertyAttribute
 */
$.validator.unobtrusive.adapters.add(validatorPrefixes.endsWith, function (options) {
    options.messages[validatorPrefixes.endsWith] = options.message;
    options.rules[validatorPrefixes.endsWith] = $(options.element).attr('data-val-endswith-property');
});

/**
 * Client-side validator of ContainsPropertyAttribute
 */
$.validator.unobtrusive.adapters.add(validatorPrefixes.contains, function (options) {
    options.messages[validatorPrefixes.contains] = options.message;
    options.rules[validatorPrefixes.contains] = $(options.element).attr('data-val-contains-property');
});