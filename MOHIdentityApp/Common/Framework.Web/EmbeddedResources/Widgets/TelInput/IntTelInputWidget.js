//class constructor
IntTelInputWidget = function (settings) {

    //private variables setters
    //initialize class logic here
    this.containerId = settings.containerId;
    this.settings = settings;

    //init elemets
    this.eleContainer = $('#' + this.containerId);
    this.eleTelInput = $('#' + this.containerId + ' .phone-number-input');
    this.eleValidMsg = $('#' + this.containerId + ' .phone-valid-msg');
    this.eleHdPhoneNumber = $('#' + this.containerId + ' input[type=hidden]');

    this.load();

};

//class members
IntTelInputWidget.prototype =
{


    //widget load entry point
    load: function () {

        var self = this;

        if (!self.eleTelInput) return;

        var valCountry = self.eleHdPhoneNumber.data('val-checkisvalidnumber-countrycode');
        valCountry = valCountry ? (valCountry.split(',')) : undefined;


        self.eleTelInput.intlTelInput({
            //utilsScript: "", //no need
            allowExtensions: false,
            //autoFormat: true, //deprecated and removed
            excludeCountries: ['il'],
            autoHideDialCode: true,
            autoPlaceholder: true,
            initialCountry: self.settings.initialCountry,
            nationalMode: true,
            numberType: self.settings.phoneNumberType,
            onlyCountries: valCountry,//["us", "gb"]
            preferredCountries: self.settings.preferredCountries,//["qa","ae","kw","bh","us","uk","fr","sa","eg"]
            geoIpLookup: function (callback) {
                $.get("http://ipinfo.io", function () { }, "jsonp").always(function (resp) {
                    var countryCode = (resp && resp.country) ? resp.country : "";
                    callback(countryCode);
                });
            }
        });

        self.eleTelInput.blur(function () {
            var num = $.trim(self.eleTelInput.val());
            if (num) {
                num = num.replace(/\D/g, '');

                if (num.indexOf('00') === 0) {
                    num = num.replace('00', '+');
                }

                self.setNumber(num); //for auto formatting number in input while end user is typing                

                var intlFormattedNum = self.eleTelInput.intlTelInput("getNumber", intlTelInputUtils.numberFormat.E164);
                self.eleHdPhoneNumber.val(intlFormattedNum);//.replace("+", "00"));
                //self.eleTelInput.attr('value', intlFormattedNum);

                //if selected value changed fire validation if validation library exist
                if (self.eleTelInput.valid) {
                    self.eleTelInput.valid();
                }

                // validation now is on hidden field
                if (self.eleHdPhoneNumber.valid) {
                    self.eleHdPhoneNumber.valid();
                }
            }
        });

        self.eleTelInput.keyup(function (e) {
            var intlFormattedNum = self.eleTelInput.intlTelInput("getNumber", intlTelInputUtils.numberFormat.E164);
            self.eleHdPhoneNumber.val(intlFormattedNum);//.replace("+", "00"));
            //self.eleTelInput.attr('value', intlFormattedNum);
        });

    }, //load End.

    getNumber: function () {
        var self = this;
        return self.eleTelInput.intlTelInput("getNumber", intlTelInputUtils.numberFormat.E164); ///.replace("+", "00");
    },

    getNumberType: function () {
        var self = this;
        return self.eleTelInput.intlTelInput("getNumberType");
    },

    setNumber: function (number, country) {
        var self = this;
        number = $.trim(number);
        country = $.trim(country);
        if (number !== '') {
            self.eleTelInput.intlTelInput("setNumber", number);

            // var phoneUtil = i18n.phonenumbers.PhoneNumberUtil.getInstance();            
            //var number = phoneUtil.parseAndKeepRawInput(number, regionCode);
            var num = self.getNumber();
            self.eleHdPhoneNumber.val(num);
            if (country !== '') {
                self.eleTelInput.intlTelInput("selectCountry", country);
            }
        }
    },

    getValidationError: function () {
        var self = this;
        return self.eleTelInput.intlTelInput("getValidationError");
    },

    isValidNumber: function () {
        var self = this;
        return self.eleTelInput.intlTelInput("isValidNumber");
    }

}; //class end

//adding static methods to js class
IntTelInputWidget.isValidNumber = function (element, numType, countryCode) {
    var eleTelInput = $(element);

    //we use val to validate with countrycode availiable
    //we use eleTelInput.attr('value'); when the country code is not passed
    //console.log(eleTelInput.val() + ' --- ' + eleTelInput.parents('.phoneInputContainer').find('input[type=hidden]').val());
    var num = countryCode ? eleTelInput.val() : eleTelInput.parents('.phoneInputContainer').find('input[type=hidden]').val(); //..val get incorrect num
    if (num && num !== '') {
        return intlTelInputUtils.isValidNumber(num, countryCode);
    }

    return true;
    //console.log(num + ' ' + countryCode + ' ' + intlTelInputUtils.isValidNumber(num, countryCode));

};

//Custom Client Side Validation for phone numbers
//these validators work when the custom server validation attribute is added to the phone number property
//if (jQuery.validator && jQuery.validator.unobtrusive) {
jQuery.validator.unobtrusive.adapters.add('checkisvalidnumber', ['countrycode', 'numbertype'], function (options) {
    // simply pass the options.params here
    options.rules['checkisvalidnumber'] = options.params;
    options.messages['checkisvalidnumber'] = options.message;
});

jQuery.validator.addMethod('checkisvalidnumber', function (value, element, params) {
    //alert('addMethod');
    return IntTelInputWidget.isValidNumber(element, params.numbertype, params.countrycode);
}, '');
//}