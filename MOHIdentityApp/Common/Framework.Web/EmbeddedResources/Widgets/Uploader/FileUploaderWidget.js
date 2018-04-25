//class constructor
FileUploaderWidget = function (settings) {

    //private variables setters
    //initialize class logic here
    this.settings = settings;
   
    //init elemets
    this.eleContainer = $('#' + this.settings.containerId);
    this.eleUploader = $('#' + this.settings.containerId + ' .file-uploader-input');

    this.load();

};

//class members
FileUploaderWidget.prototype =
{

    //widget load entry point
    load: function () {

        var self = this;

        if (!self.eleUploader) return;

        self.eleUploader.change(function (e) {

            var uploader = $(this);

            if (!self.settings.isImage) {
                uploader.data('current-img-width', 0);
                uploader.data('current-img-height', 0);
                return;
            }
            
            var windowUrl = window.URL || window.webkitURL;

            var file, img;
            if ((file = uploader[0].files[0])) {
                img = new Image();
                img.onload = function() {
                    img = this;
                    uploader.data('current-img-width', img.width);
                    uploader.data('current-img-height', img.width);
                    //console.log('ONLOAD ' + img.width + " X " + img.height);
                };
                img.src = windowUrl.createObjectURL(file);
            }
        });

    } //load End.

}; //class end

//adding static methods to js class
FileUploaderWidget.isValidFile = function (element, params) {

    var fileUploader = $(element);
    //var widSettings = JSON.parse(fileUploader.attr('data-widget-settings'));
    
    if (!fileUploader.length) return true;

    var file = fileUploader[0].files[0];
    if (!file) return true;

    //file exists
    var fileName = file.name.toLowerCase();
    if (!fileName || fileUploader.val() === "") { return true; }

    var errors = [];

    //check file type
    if (params.allowedextensions) {
        var regex = new RegExp(params.allowedextensionsregex);
        if (!(regex.test(fileName))) {
            errors.push(params.filetypeerrormessage.replace('{0}', params.allowedextensions));
        }
    }

    //file size checks
    if (params.maxsizeinmegabytes && file.size) {
        var fileSize = file.size / 1024 / 1024;
        if (fileSize > params.maxsizeinmegabytes) {
            errors.push(params.filesizeerrormessage.replace('{0}', params.maxsizeinmegabytes));
        }
    }

  
    //image file checks. Is Image, width, height
    if (params.isimage === true || params.isimage === "true") {

        var width = parseInt(params.imagemaxwidth) ? parseInt(params.imagemaxwidth) : 0;
        var height = parseInt(params.imagemaxwidthimagemaxheight) ? parseInt(params.imagemaxwidthimagemaxheight) : 0;

        var currentImgWidth = fileUploader.data("current-img-width");
        var currentImgHeight = fileUploader.data("current-img-height");

        console.log('VALIDATION currentImgWidth ' + currentImgWidth + ' currentImgHeight ' + currentImgHeight);

        if ((width > 0 && currentImgWidth > width) ||
            (height > 0 && currentImgHeight > height)) {

            errors.push(params.fileimagedimensionserrormessage.replace('{0}', params.imagemaxwidth)
                .replace('{1}', params.imagemaxheight));
        }
     
    }

    if (errors.length > 0) {
        //fileUploader.val('');
        fileUploader.attr('data-error-message', errors.join(';'));
        return false;
    } else {
        return true;
    }
};

//Custom Client Side Validation for phone numbers
//these validators work when the custom server validation attribute is added to the phone number property

String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.replace(new RegExp(search, 'g'), replacement);
};

if (jQuery.validator && jQuery.validator.unobtrusive) {
    jQuery.validator.unobtrusive.adapters.add('checkisvalidfile',
        ['allowedextensions', 'allowedextensionsregex', 'isimage', 'maxsizeinmegabytes', 'imagemaxheight', 'imagemaxwidth', 'fileimagedimensionserrormessage', 'filenotvalidimageerrormessage', 'filesizeerrormessage', 'filetypeerrormessage'], function (options) {
            // simply pass the options.params here
            options.rules['checkisvalidfile'] = options.params;
            options.messages['checkisvalidfile'] = function () { return $(options.element).attr('data-error-message').replaceAll(';', '<br/>'); };//options.message;
        });

    jQuery.validator.addMethod('checkisvalidfile', function (value, element, params) {
        return FileUploaderWidget.isValidFile(element, params);
    }, '');
}