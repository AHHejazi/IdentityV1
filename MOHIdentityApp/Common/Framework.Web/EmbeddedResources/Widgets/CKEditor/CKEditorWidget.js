//class constructor
CKEditorWidget = function (settings) {

    //private variables setters
    //initialize class logic here
    this.containerId = settings.containerId;
    this.settings = settings;

    //init elemets
    this.eleContainer = $('#' + this.containerId);
    this.eleTextArea = $('#' + this.containerId + ' .ck-text-area-input');//textarea
    this.ckEditorId = this.eleTextArea.attr('id');
    
    this.load();

};

//class members
CKEditorWidget.prototype =
{

    //widget load entry point
    load: function() {

        var self = this;

        if (!self.eleTextArea) return;

        if (CKEDITOR.env.ie && CKEDITOR.env.version < 9)
            CKEDITOR.tools.enableHtml5Elements(document);

        //CKEDITOR.config.height = 150;
        //CKEDITOR.config.width = 'auto';

        CKEDITOR.replace(self.ckEditorId, {
            language: self.settings.lang,
            height: self.settings.height,
            width: 'auto',
            uiColor: self.settings.uiColor,
            toolbar: 'full'//self.settings.toolbarMode // 'full'
        });


        self.eleTextArea.blur(function () {

            var editor = CKEDITOR.instances[self.ckEditorId];

            if (editor) {
                editor.on('blur', function(event) {

                    var ckdata = $.trim(editor.getData());

                    if (ckdata) {
                        if (self.eleTextArea.valid) {
                            self.eleTextArea.valid();
                        }
                    }
                });
            }

        });

        $(function() {
            $('input[type="submit"]').click(function() {
                var editor = CKEDITOR.instances[self.ckEditorId];
                if (editor) {
                    editor.updateElement();
                    if (self.eleTextArea.valid) {                        
                        self.eleTextArea.valid();
                    }
                }
            });

        });
    }

}; //class end

//http://stackoverflow.com/questions/24572749/mvc-how-to-enable-client-side-validation-on-hidden-fields
//to enable text area validation when it is display:none
if ($.validator) {
    $.validator.setDefaults({ ignore: null });
}