(function () {
    $("#selectLanguage select").change(function () {
        $(this).parent().submit();

    });


}
());



$(function () {
    if (jQuery.validator && jQuery.validator.unobtrusive) {

        // enable validaitions on specific input hidden fields too!

        $.validator.setDefaults({ ignore: ':hidden:not(.do-not-ignore-validation)' });

        //must be true attribute client validation
        if (jQuery.validator && jQuery.validator.unobtrusive) {
            //must be true attribute
            jQuery.validator.unobtrusive.adapters.addBool("mustbetrue");

            jQuery.validator.addMethod('mustbetrue', function (value, element, params) {
                return element.checked;
            }, '');
        };


        // date validation to support ummalqura and also gregorian dates
        jQuery.validator.methods.date = function (value, element) {

            var ele = $(element);

            var widSettings = {
                dateFormat: "dd/mm/yyyy",
                lang: "ar",
                calendarType: ele.parents('.datePickerContainer').find('select').val() || 'gregorian'
            };

            var calendar = $.calendars.instance(widSettings.calendarType, widSettings.lang);
            calendar.dateFormat = widSettings.dateFormat;

            try {
                calendar.parseDate(widSettings.dateFormat, ele.val());
                return true;
            } catch (e) {
                return false;
            }
        };

        // RequiredAttribute with AllowEmptyString=true in ASP.NET MVC unobtrusive validation

        jQuery.validator.methods.oldRequired = jQuery.validator.methods.required;

        jQuery.validator.addMethod("required", function (value, element, param) {
            if ($(element).attr('data-val-required-allowempty') === 'true') {
                return true;
            } else if ($.trim(value) === '') {
                return false;
            }

            return jQuery.validator.methods.oldRequired.call(this, value, element, param);
        },
            jQuery.validator.messages.required // use default message
        );



        // DateRestrictionAttribute

        jQuery.validator.unobtrusive.adapters.add('daterestriction',
            ['todaydate', 'daterestrictiontype'], function (options) {
                // simply pass the options.params here
                options.rules['daterestriction'] = options.params;
                options.messages['daterestriction'] = function () { return $(options.element).attr('data-val-daterestriction'); };//options.message;
            });

        jQuery.validator.addMethod('daterestriction', function (value, element, params) {

            var todayDate = $(element).data("todaydate");
            var selectedDate = value;

            if (!value || !todayDate)
                return true;

            var isValid = false;
            var calendarType = $(element).parents('.datePickerContainer').find('select').val() || 'gregorian';
            var dateDiffInDays = $commons.tryDiffDates(todayDate, 'gregorian', selectedDate, calendarType);

            switch ($(element).data("daterestrictiontype")) {

                case "futureOnly":
                    isValid = dateDiffInDays > 0;
                    break;

                case "futureIncludingToday":
                    if (dateDiffInDays >= 0) {
                        isValid = true;
                    }
                    break;

                case "pastOnly":
                    isValid = dateDiffInDays < 0;
                    break;

                case "pastIncludingToday":
                    if (dateDiffInDays <= 0) {
                        isValid = true;
                    }
                    break;

                default:
                    isValid = true;
                    break;
            }

            return isValid;


        }, '');



        // Date 
        // IsDateAfterAttribute Attribute

        jQuery.validator.unobtrusive.adapters.add('isdateafter',
            ['propertytested', 'allowequaldates'], function (options) {
                // simply pass the options.params here
                options.rules['isdateafter'] = options.params;
                options.messages['isdateafter'] = function () { return $(options.element).attr('data-val-isdateafter'); };//options.message;
            });

        jQuery.validator.addMethod('isdateafter', function (value, element, params) {

            var parts = element.name.split(".");
            var prefix = "";

            if (parts.length > 1)
                prefix = parts[0] + ".";

            var startDateElement = $('input[name="' + $(element).data("propertytested") + '"]');
            var endDateElement = $(element);

            var startDateWidgetSettings = JSON.parse(startDateElement.attr('data-widget-settings'));
            var endDateWidgetSettings = JSON.parse(endDateElement.attr('data-widget-settings'));

            var startdatevalue = startDateElement.val();
            if (!value || !startdatevalue)
                return true;

            var isValid = true;

            if (params.allowequaldates === 'True') {
                //isValid = Date.parse(startdatevalue) <= Date.parse(value);
                var dateDiffInDays = $commons.diffDates(startdatevalue, startDateWidgetSettings, value, endDateWidgetSettings);
                isValid = dateDiffInDays >= 0;

            } else {
                //isValid = Date.parse(startdatevalue) < Date.parse(value);
                var dateDiffInDays = $commons.diffDates(startdatevalue, startDateWidgetSettings, value, endDateWidgetSettings);
                isValid = dateDiffInDays > 0;
            }

            return isValid;


        }, '');

       

        // GreaterThanAttribute Attribute

        jQuery.validator.unobtrusive.adapters.add('greaterthan',
            ['propertytested', 'allowequalvalues'], function (options) {
                // simply pass the options.params here
                options.rules['greaterthan'] = options.params;
                options.messages['greaterthan'] = function () { return $(options.element).attr('data-val-greaterthan'); };//options.message;
            });

        jQuery.validator.addMethod('greaterthan', function (value, element, params) {

            if (value.length === 0 && this.optional(element)) {
                return true;
            }

            if (isNaN(parseInt(value))) {
                amountNumberValidationError = $sharedResources.NumberOnlyErrorMessage;
                return false;
            }

            var parts = element.name.split(".");
            var prefix = "";

            if (parts.length > 1)
                prefix = parts[0] + ".";

            var minValElemnet = $('input[name="' + $(element).data("propertytested") + '"]');
            var maxValElement = $(element);

            var minValue = minValElemnet.val();

            if (!value || !minValue)
                return true;

            var isValid = true;

            if (params.allowequalvalues === 'True') {
                isValid = value >= minValue;

            } else {
                isValid = value > minValue;
            }

            return isValid;


        }, '');

        // GreaterThanAttribute Attribute

        jQuery.validator.unobtrusive.adapters.add('greaterthan',
            ['propertytested', 'allowequalvalues'], function (options) {
                // simply pass the options.params here
                options.rules['greaterthan'] = options.params;
                options.messages['greaterthan'] = function () { return $(options.element).attr('data-val-greaterthan'); };//options.message;
            });

        jQuery.validator.addMethod('greaterthan', function (value, element, params) {

            if (value.length === 0 && this.optional(element)) {
                return true;
            }

            if (isNaN(parseInt(value))) {
                amountNumberValidationError = $sharedResources.NumberOnlyErrorMessage;
                return false;
            }

            var parts = element.name.split(".");
            var prefix = "";

            if (parts.length > 1)
                prefix = parts[0] + ".";

            var minValElemnet = $('input[name="' + $(element).data("propertytested") + '"]');
            var maxValElement = $(element);

            var minValue = minValElemnet.val();

            if (!value || !minValue)
                return true;

            var isValid = true;

            if (params.allowequalvalues === 'True') {
                isValid = value >= minValue;

            } else {
                isValid = value > minValue;
            }

            return isValid;


        }, '');


    }


});


