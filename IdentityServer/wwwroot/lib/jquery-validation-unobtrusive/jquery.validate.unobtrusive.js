/*!
** Unobtrusive validation support library for jQuery and jQuery Validate
** Copyright (C) Microsoft Corporation. All rights reserved.
*/

/*jslint white: true, browser: true, onevar: true, undef: true, nomen: true, eqeqeq: true, plusplus: true, bitwise: true, regexp: true, newcap: true, immed: true, strict: false */
/*global document: false, jQuery: false */

(function ($) ***REMOVED***
    var $jQval = $.validator,
        adapters,
        data_validation = "unobtrusiveValidation";

    function setValidationValues(options, ruleName, value) ***REMOVED***
        options.rules[ruleName] = value;
        if (options.message) ***REMOVED***
            options.messages[ruleName] = options.message;
        ***REMOVED***
    ***REMOVED***

    function splitAndTrim(value) ***REMOVED***
        return value.replace(/^\s+|\s+$/g, "").split(/\s*,\s*/g);
    ***REMOVED***

    function escapeAttributeValue(value) ***REMOVED***
        // As mentioned on http://api.jquery.com/category/selectors/
        return value.replace(/([!"#$%&'()*+,./:;<=>?@\[\\\]^`***REMOVED***|***REMOVED***~])/g, "\\$1");
    ***REMOVED***

    function getModelPrefix(fieldName) ***REMOVED***
        return fieldName.substr(0, fieldName.lastIndexOf(".") + 1);
    ***REMOVED***

    function ***REMOVED***endModelPrefix(value, prefix) ***REMOVED***
        if (value.indexOf("*.") === 0) ***REMOVED***
            value = value.replace("*.", prefix);
        ***REMOVED***
        return value;
    ***REMOVED***

    function onError(error, inputElement) ***REMOVED***  // 'this' is the form element
        var container = $(this).find("[data-valmsg-for='" + escapeAttributeValue(inputElement[0].name) + "']"),
            replaceAttrValue = container.attr("data-valmsg-replace"),
            replace = replaceAttrValue ? $.parseJSON(replaceAttrValue) !== false : null;

        container.removeClass("field-validation-valid").addClass("field-validation-error");
        error.data("unobtrusiveContainer", container);

        if (replace) ***REMOVED***
            container.empty();
            error.removeClass("input-validation-error").***REMOVED***endTo(container);
        ***REMOVED***
        else ***REMOVED***
            error.hide();
        ***REMOVED***
    ***REMOVED***

    function onErrors(event, validator) ***REMOVED***  // 'this' is the form element
        var container = $(this).find("[data-valmsg-summary=true]"),
            list = container.find("ul");

        if (list && list.length && validator.errorList.length) ***REMOVED***
            list.empty();
            container.addClass("validation-summary-errors").removeClass("validation-summary-valid");

            $.each(validator.errorList, function () ***REMOVED***
                $("<li />").html(this.message).***REMOVED***endTo(list);
        ***REMOVED***;
        ***REMOVED***
    ***REMOVED***

    function onSuccess(error) ***REMOVED***  // 'this' is the form element
        var container = error.data("unobtrusiveContainer");

        if (container) ***REMOVED***
            var replaceAttrValue = container.attr("data-valmsg-replace"),
                replace = replaceAttrValue ? $.parseJSON(replaceAttrValue) : null;

            container.addClass("field-validation-valid").removeClass("field-validation-error");
            error.removeData("unobtrusiveContainer");

            if (replace) ***REMOVED***
                container.empty();
            ***REMOVED***
        ***REMOVED***
    ***REMOVED***

    function onReset(event) ***REMOVED***  // 'this' is the form element
        var $form = $(this),
            key = '__jquery_unobtrusive_validation_form_reset';
        if ($form.data(key)) ***REMOVED***
            return;
        ***REMOVED***
        // Set a flag that indicates we're currently resetting the form.
        $form.data(key, true);
        ***REMOVED*** ***REMOVED***
            $form.data("validator").resetForm();
        ***REMOVED*** ***REMOVED*** ***REMOVED***
            $form.removeData(key);
        ***REMOVED***

        $form.find(".validation-summary-errors")
            .addClass("validation-summary-valid")
            .removeClass("validation-summary-errors");
        $form.find(".field-validation-error")
            .addClass("field-validation-valid")
            .removeClass("field-validation-error")
            .removeData("unobtrusiveContainer")
            .find(">*")  // If we were using valmsg-replace, get the underlying error
                .removeData("unobtrusiveContainer");
    ***REMOVED***

    function validationInfo(form) ***REMOVED***
        var $form = $(form),
            result = $form.data(data_validation),
            onResetProxy = $.proxy(onReset, form),
            defaultOptions = $jQval.unobtrusive.options || ***REMOVED******REMOVED***,
            execInContext = function (name, args) ***REMOVED***
                var func = defaultOptions[name];
                func && $.isFunction(func) && func.***REMOVED***ly(form, args);
            ***REMOVED***

        if (!result) ***REMOVED***
            result = ***REMOVED***
                options: ***REMOVED***  // options structure passed to jQuery Validate's validate() method
                    errorClass: defaultOptions.errorClass || "input-validation-error",
                    errorElement: defaultOptions.errorElement || "span",
                    errorPlacement: function () ***REMOVED***
                        onError.***REMOVED***ly(form, arguments);
                        execInContext("errorPlacement", arguments);
                    ***REMOVED***,
                    invalidHandler: function () ***REMOVED***
                        onErrors.***REMOVED***ly(form, arguments);
                        execInContext("invalidHandler", arguments);
                    ***REMOVED***,
                    messages: ***REMOVED******REMOVED***,
                    rules: ***REMOVED******REMOVED***,
                    success: function () ***REMOVED***
                        onSuccess.***REMOVED***ly(form, arguments);
                        execInContext("success", arguments);
                    ***REMOVED***
                ***REMOVED***,
                attachValidation: function () ***REMOVED***
                    $form
                        .off("reset." + data_validation, onResetProxy)
                        .on("reset." + data_validation, onResetProxy)
                        .validate(this.options);
                ***REMOVED***,
                validate: function () ***REMOVED***  // a validation function that is called by unobtrusive Ajax
                    $form.validate();
                    return $form.valid();
                ***REMOVED***
    ***REMOVED***
            $form.data(data_validation, result);
        ***REMOVED***

        return result;
    ***REMOVED***

    $jQval.unobtrusive = ***REMOVED***
        adapters: [],

        parseElement: function (element, skipAttach) ***REMOVED***
            /// <summary>
            /// Parses a single HTML element for unobtrusive validation attributes.
            /// </summary>
            /// <param name="element" domElement="true">The HTML element to be parsed.</param>
            /// <param name="skipAttach" type="Boolean">[Optional] true to skip attaching the
            /// validation to the form. If parsing just this single element, you should specify true.
            /// If parsing several elements, you should specify false, and manually attach the validation
            /// to the form when you are finished. The default is false.</param>
            var $element = $(element),
                form = $element.parents("form")[0],
                valInfo, rules, messages;

            if (!form) ***REMOVED***  // Cannot do client-side validation without a form
                return;
            ***REMOVED***

            valInfo = validationInfo(form);
            valInfo.options.rules[element.name] = rules = ***REMOVED******REMOVED***;
            valInfo.options.messages[element.name] = messages = ***REMOVED******REMOVED***;

            $.each(this.adapters, function () ***REMOVED***
                var prefix = "data-val-" + this.name,
                    message = $element.attr(prefix),
                    paramValues = ***REMOVED******REMOVED***;

                if (message !== undefined) ***REMOVED***  // Compare against undefined, because an empty message is legal (and falsy)
                    prefix += "-";

                    $.each(this.params, function () ***REMOVED***
                        paramValues[this] = $element.attr(prefix + this);
                ***REMOVED***;

                    this.adapt(***REMOVED***
                        element: element,
                        form: form,
                        message: message,
                        params: paramValues,
                        rules: rules,
                        messages: messages
                ***REMOVED***;
                ***REMOVED***
        ***REMOVED***;

            $.extend(rules, ***REMOVED*** "__dummy__": true ***REMOVED***);

            if (!skipAttach) ***REMOVED***
                valInfo.attachValidation();
            ***REMOVED***
        ***REMOVED***,

        parse: function (selector) ***REMOVED***
            /// <summary>
            /// Parses all the HTML elements in the specified selector. It looks for input elements decorated
            /// with the [data-val=true] attribute value and enables validation according to the data-val-*
            /// attribute values.
            /// </summary>
            /// <param name="selector" type="String">Any valid jQuery selector.</param>

            // $forms includes all forms in selector's DOM hierarchy (parent, children and self) that have at least one
            // element with data-val=true
            var $selector = $(selector),
                $forms = $selector.parents()
                                  .addBack()
                                  .filter("form")
                                  .add($selector.find("form"))
                                  .has("[data-val=true]");

            $selector.find("[data-val=true]").each(function () ***REMOVED***
                $jQval.unobtrusive.parseElement(this, true);
        ***REMOVED***;

            $forms.each(function () ***REMOVED***
                var info = validationInfo(this);
                if (info) ***REMOVED***
                    info.attachValidation();
                ***REMOVED***
        ***REMOVED***;
        ***REMOVED***
    ***REMOVED***;

    adapters = $jQval.unobtrusive.adapters;

    adapters.add = function (adapterName, params, fn) ***REMOVED***
        /// <summary>Adds a new adapter to convert unobtrusive HTML into a jQuery Validate validation.</summary>
        /// <param name="adapterName" type="String">The name of the adapter to be added. This matches the name used
        /// in the data-val-nnnn HTML attribute (where nnnn is the adapter name).</param>
        /// <param name="params" type="Array" optional="true">[Optional] An array of parameter names (strings) that will
        /// be extracted from the data-val-nnnn-mmmm HTML attributes (where nnnn is the adapter name, and
        /// mmmm is the parameter name).</param>
        /// <param name="fn" type="Function">The function to call, which adapts the values from the HTML
        /// attributes into jQuery Validate rules and/or messages.</param>
        /// <returns type="jQuery.validator.unobtrusive.adapters" />
        if (!fn) ***REMOVED***  // Called with no params, just a function
            fn = params;
            params = [];
        ***REMOVED***
        this.push(***REMOVED*** name: adapterName, params: params, adapt: fn ***REMOVED***);
        return this;
    ***REMOVED***;

    adapters.addBool = function (adapterName, ruleName) ***REMOVED***
        /// <summary>Adds a new adapter to convert unobtrusive HTML into a jQuery Validate validation, where
        /// the jQuery Validate validation rule has no parameter values.</summary>
        /// <param name="adapterName" type="String">The name of the adapter to be added. This matches the name used
        /// in the data-val-nnnn HTML attribute (where nnnn is the adapter name).</param>
        /// <param name="ruleName" type="String" optional="true">[Optional] The name of the jQuery Validate rule. If not provided, the value
        /// of adapterName will be used instead.</param>
        /// <returns type="jQuery.validator.unobtrusive.adapters" />
        return this.add(adapterName, function (options) ***REMOVED***
            setValidationValues(options, ruleName || adapterName, true);
    ***REMOVED***;
    ***REMOVED***;

    adapters.addMinMax = function (adapterName, minRuleName, maxRuleName, minMaxRuleName, minAttribute, maxAttribute) ***REMOVED***
        /// <summary>Adds a new adapter to convert unobtrusive HTML into a jQuery Validate validation, where
        /// the jQuery Validate validation has three potential rules (one for min-only, one for max-only, and
        /// one for min-and-max). The HTML parameters are expected to be named -min and -max.</summary>
        /// <param name="adapterName" type="String">The name of the adapter to be added. This matches the name used
        /// in the data-val-nnnn HTML attribute (where nnnn is the adapter name).</param>
        /// <param name="minRuleName" type="String">The name of the jQuery Validate rule to be used when you only
        /// have a minimum value.</param>
        /// <param name="maxRuleName" type="String">The name of the jQuery Validate rule to be used when you only
        /// have a maximum value.</param>
        /// <param name="minMaxRuleName" type="String">The name of the jQuery Validate rule to be used when you
        /// have both a minimum and maximum value.</param>
        /// <param name="minAttribute" type="String" optional="true">[Optional] The name of the HTML attribute that
        /// contains the minimum value. The default is "min".</param>
        /// <param name="maxAttribute" type="String" optional="true">[Optional] The name of the HTML attribute that
        /// contains the maximum value. The default is "max".</param>
        /// <returns type="jQuery.validator.unobtrusive.adapters" />
        return this.add(adapterName, [minAttribute || "min", maxAttribute || "max"], function (options) ***REMOVED***
            var min = options.params.min,
                max = options.params.max;

            if (min && max) ***REMOVED***
                setValidationValues(options, minMaxRuleName, [min, max]);
            ***REMOVED***
            else if (min) ***REMOVED***
                setValidationValues(options, minRuleName, min);
            ***REMOVED***
            else if (max) ***REMOVED***
                setValidationValues(options, maxRuleName, max);
            ***REMOVED***
    ***REMOVED***;
    ***REMOVED***;

    adapters.addSingleVal = function (adapterName, attribute, ruleName) ***REMOVED***
        /// <summary>Adds a new adapter to convert unobtrusive HTML into a jQuery Validate validation, where
        /// the jQuery Validate validation rule has a single value.</summary>
        /// <param name="adapterName" type="String">The name of the adapter to be added. This matches the name used
        /// in the data-val-nnnn HTML attribute(where nnnn is the adapter name).</param>
        /// <param name="attribute" type="String">[Optional] The name of the HTML attribute that contains the value.
        /// The default is "val".</param>
        /// <param name="ruleName" type="String" optional="true">[Optional] The name of the jQuery Validate rule. If not provided, the value
        /// of adapterName will be used instead.</param>
        /// <returns type="jQuery.validator.unobtrusive.adapters" />
        return this.add(adapterName, [attribute || "val"], function (options) ***REMOVED***
            setValidationValues(options, ruleName || adapterName, options.params[attribute]);
    ***REMOVED***;
    ***REMOVED***;

    $jQval.addMethod("__dummy__", function (value, element, params) ***REMOVED***
        return true;
***REMOVED***;

    $jQval.addMethod("regex", function (value, element, params) ***REMOVED***
        var match;
        if (this.optional(element)) ***REMOVED***
            return true;
        ***REMOVED***

        match = new RegExp(params).exec(value);
        return (match && (match.index === 0) && (match[0].length === value.length));
***REMOVED***;

    $jQval.addMethod("nonalphamin", function (value, element, nonalphamin) ***REMOVED***
        var match;
        if (nonalphamin) ***REMOVED***
            match = value.match(/\W/g);
            match = match && match.length >= nonalphamin;
        ***REMOVED***
        return match;
***REMOVED***;

    if ($jQval.methods.extension) ***REMOVED***
        adapters.addSingleVal("accept", "mimtype");
        adapters.addSingleVal("extension", "extension");
    ***REMOVED*** else ***REMOVED***
        // for backward compatibility, when the 'extension' validation method does not exist, such as with versions
        // of JQuery Validation plugin prior to 1.10, we should use the 'accept' method for
        // validating the extension, and ignore mime-type validations as they are not supported.
        adapters.addSingleVal("extension", "extension", "accept");
    ***REMOVED***

    adapters.addSingleVal("regex", "pattern");
    adapters.addBool("creditcard").addBool("date").addBool("digits").addBool("email").addBool("number").addBool("url");
    adapters.addMinMax("length", "minlength", "maxlength", "rangelength").addMinMax("range", "min", "max", "range");
    adapters.addMinMax("minlength", "minlength").addMinMax("maxlength", "minlength", "maxlength");
    adapters.add("equalto", ["other"], function (options) ***REMOVED***
        var prefix = getModelPrefix(options.element.name),
            other = options.params.other,
            fullOtherName = ***REMOVED***endModelPrefix(other, prefix),
            element = $(options.form).find(":input").filter("[name='" + escapeAttributeValue(fullOtherName) + "']")[0];

        setValidationValues(options, "equalTo", element);
***REMOVED***;
    adapters.add("required", function (options) ***REMOVED***
        // jQuery Validate equates "required" with "mandatory" for checkbox elements
        if (options.element.tagName.toUpperCase() !== "INPUT" || options.element.type.toUpperCase() !== "CHECKBOX") ***REMOVED***
            setValidationValues(options, "required", true);
        ***REMOVED***
***REMOVED***;
    adapters.add("remote", ["url", "type", "additionalfields"], function (options) ***REMOVED***
        var value = ***REMOVED***
            url: options.params.url,
            type: options.params.type || "GET",
            data: ***REMOVED******REMOVED***
        ***REMOVED***,
            prefix = getModelPrefix(options.element.name);

        $.each(splitAndTrim(options.params.additionalfields || options.element.name), function (i, fieldName) ***REMOVED***
            var paramName = ***REMOVED***endModelPrefix(fieldName, prefix);
            value.data[paramName] = function () ***REMOVED***
                var field = $(options.form).find(":input").filter("[name='" + escapeAttributeValue(paramName) + "']");
                // For checkboxes and radio buttons, only pick up values from checked fields.
                if (field.is(":checkbox")) ***REMOVED***
                    return field.filter(":checked").val() || field.filter(":hidden").val() || '';
                ***REMOVED***
                else if (field.is(":radio")) ***REMOVED***
                    return field.filter(":checked").val() || '';
                ***REMOVED***
                return field.val();
    ***REMOVED***
    ***REMOVED***;

        setValidationValues(options, "remote", value);
***REMOVED***;
    adapters.add("password", ["min", "nonalphamin", "regex"], function (options) ***REMOVED***
        if (options.params.min) ***REMOVED***
            setValidationValues(options, "minlength", options.params.min);
        ***REMOVED***
        if (options.params.nonalphamin) ***REMOVED***
            setValidationValues(options, "nonalphamin", options.params.nonalphamin);
        ***REMOVED***
        if (options.params.regex) ***REMOVED***
            setValidationValues(options, "regex", options.params.regex);
        ***REMOVED***
***REMOVED***;

    $(function () ***REMOVED***
        $jQval.unobtrusive.parse(document);
***REMOVED***;
***REMOVED***(jQuery));