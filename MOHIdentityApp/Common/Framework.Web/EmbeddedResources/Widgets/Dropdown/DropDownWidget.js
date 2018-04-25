//class constructor
DropDownWidget = function (selectId, optionLabel, disabled, selectedVal) {

    var self = this;
    //private variables setters
    //initialize class logic here
    self.selectId = selectId;
    self.disabled = (disabled === true) ? disabled : false;
    self.selectedVal = selectedVal;

    self.eleSelect = $('#' + selectId);
    self.optionLabel = optionLabel;
};

//class members
DropDownWidget.prototype =
    {
        //widget load entry point
        load: function () {

            var self = this;

            if (!self.eleSelect) return;
            
            self.eleSelect.select2({
                theme: 'bootstrap4'
                , containerCssClass: ':all:'
                , placeholder: self.optionLabel
                , allowClear: true
            });
            
            //set select value only once for first init
            if (self.selectedVal) {
                self.eleSelect.val(self.selectedVal);
            }

            self.eleSelect.select2();

            //if selected value changed fire validation if validation library exist
            $(document.body).on("change", "#" + self.selectId, function () {
                if (self.eleSelect.valid) {
                    self.eleSelect.valid();
                }
            });

        }//load End.


    }//class end

