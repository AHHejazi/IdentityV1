//class constructor
DatePickerWidget = function (settings) {

    var self = this;
    //private variables setters
    //initialize class logic here
    self.containerId = settings.containerId;
    self.settings = settings;

    self.settings.lang = (self.settings.lang) ? self.settings.lang : "ar";
    self.settings.dateFormat = (self.settings.dateFormat) ? self.settings.dateFormat.toLowerCase() : "dd/mm/yyyy";
    self.settings.calendarType = (self.settings.calendarType) ? self.settings.calendarType : "ummalqura";
    self.settings.pickerYearRange = (self.settings.pickerYearRange) ? self.settings.pickerYearRange : "c-100:c+50";
    self.settings.isReadOnly = (self.settings.isReadOnly === true) ? self.settings.isReadOnly : false;

    self.picker = null;
    self.currentCalendar = null;

    self.eleContainer = $("#" + self.containerId);
    self.txtPicker = $("#" + self.containerId + " .date-picker-input");
    self.eleSelectCalType = $("#" + self.containerId + " .date-picker-select");

    //set select value only once for first init
    //var selectedCalQueryStringVal = self.getQueryStringParameterByName('calendarType');
    //if (selectedCalQueryStringVal) {
    //    selectedCalQueryStringVal = selectedCalQueryStringVal.split(',')[0];
    //}

    //self.eleSelectCalType.val(selectedCalQueryStringVal ? selectedCalQueryStringVal : self.settings.calendarType);
    self.eleSelectCalType.val(self.settings.calendarType);
    //change event for select element trigger reload for picker with different calendar type
    self.eleSelectCalType.change(function () {

        var isReadonly = self.txtPicker.attr('readonly') === 'readonly';
        
        self.reloadPicker(self.settings.lang, self.eleSelectCalType.val());
        //if selected value changed fire validation if validation library exist

        if (isReadonly) {
            self.txtPicker.attr('readonly', 'readonly').off();
        }

        if (self.txtPicker.valid) {
            self.txtPicker.valid();
        }
    });

    //this.load();

};

//class members
DatePickerWidget.prototype =
{
    //widget load entry point
    load: function () {

        var self = this;

        if (!self.eleContainer) return;

        //readonly
        if (self.settings.isReadOnly === true) {
            self.txtPicker.attr("readonly", "readonly");
        } else {
            self.txtPicker.removeAttr("readonly");
        }
        
        //init calendar obj with selected caltype and lang for localization

       // self.settings.calendarType = 'gregorian';

      //  alert(self.settings.lang + ' ' + self.settings.calendarType);

        self.currentCalendar = $.calendars.instance(self.settings.calendarType, self.settings.lang);
        self.currentCalendar.dateFormat = self.settings.dateFormat;

        $.calendarsPicker.setDefaults(self.settings.lang === 'ar'
            ? $.calendarsPicker.regionalOptions.ar
            : $.calendarsPicker.regionalOptions.en);

        //year range logic
        //var rangeString = self.settings.pickerYearRange.replace(/c/g, self.settings.currentCalendar.today()._year);
       // var yearRangeToApply = eval(rangeString.split(":")[0]) + ":" + eval(rangeString.split(":")[1]);
        
        var currentDate = new Date();

        var todayYear = currentDate.getFullYear();
        var todayMonth = currentDate.getMonth(); // month is 0 ... why? js sometimes sucks
        var todayDay = currentDate.getDate();

        var calenderRange = self.settings.calendarRange;
        var computedMinDate = "";
        var computedMaxDate = "";
        switch (calenderRange) {
        case 100://FutureOnly
            computedMinDate = self.currentCalendar.formatDate("dd/mm/yyyy", self.currentCalendar.today().add(1, 'd'));
            break;
        case 101://TodayAndFutureOnly
            computedMinDate = self.currentCalendar.formatDate("dd/mm/yyyy", self.currentCalendar.today());
            break;
        case 102://TodayAndPastOnly
            computedMaxDate = self.currentCalendar.formatDate("dd/mm/yyyy", self.currentCalendar.today());
            break;
        case 103://PastOnly
            computedMaxDate = self.currentCalendar.formatDate("dd/mm/yyyy", self.currentCalendar.today().add(-1, 'd'));
            break;
        case 104:// ViewBoth
            break;
        case 105: //SpecificDates
            computedMaxDate = self.settings.maxDate;
            computedMinDate = self.settings.minDate;
            break;
        }


        //init picker jquery plugin
        self.txtPicker.calendarsPicker({
            minDate: computedMinDate,
            maxDate: computedMaxDate,
            //yearRange: yearRangeToApply, //'c-03:c+03',
            calendar: self.currentCalendar,
            dateFormat: self.settings.dateFormat,
            firstDay: 6,
            monthsToShow: 1,
            showOtherMonths: false,
            monthsToStep: 1,
            useMouseWheel: true,
            showOnFocus: !self.settings
                .isReadOnly, //in case of readonly we can not open picker in case of element focus
            onSelect: function(date) {
                //if selected value changed fire validation if validation library exist
                if (self.txtPicker.valid) {
                    self.txtPicker.valid();
                }
            }
        });
        
    }, //load End.

    reloadPicker: function (lang, calType) {
        var self = this;

        var newDate = null;

        if (lang) {
            self.settings.lang = lang;
        }

        if (calType && self.settings.calendarType !== calType) {

            var fromCalName = self.settings.calendarType;
            var toCalName = calType;

            //a convert between dates will be needed
            var selectedDate = self.getSelectedDate();
            if (selectedDate) {
                newDate = self.convertDates(selectedDate, fromCalName, toCalName);
            }

            self.settings.calendarType = calType;
        }

        //cleanup
        self.txtPicker.calendarsPicker("destroy");
        self.txtPicker.removeClass("hasCalendarsPicker");
        self.txtPicker.val("");

        if (newDate)
            self.txtPicker.val(newDate.formatDate(self.settings.dateFormat));

        self.load(newDate);
    },

    getSelectedDate: function () {
        var self = this;
        if (self.txtPicker.length > 0 && self.txtPicker.val() !== "")
            return self.currentCalendar.parseDate(self.settings.dateFormat, self.txtPicker.val());
        else
            return null;
    },

    getSelectedDateGregString: function (targetFormat) {
        
        var self = this;

        targetFormat = targetFormat ? targetFormat : self.settings.dateFormat;//'mm/dd/yyyy';

        if (self.txtPicker.length > 0 && self.txtPicker.val() !== "") {
            var dateObj = self.currentCalendar.parseDate(self.settings.dateFormat, self.txtPicker.val());
            return dateObj.formatDate(targetFormat);
            
        } else {
            return null;

        }
    },

    convertDates: function (dateObjToParse, fromCalName, toCalName) {
        //var jd = $.calendars.instance(fromCalName).newDate(year, month, day).toJD();
        var jd = dateObjToParse.toJD();
        return $.calendars.instance(toCalName).fromJD(jd);
    },

    getQueryStringParameterByName: function (name) {
        name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
        var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
            results = regex.exec(location.search);
        return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
    }


}; //class end