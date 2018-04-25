//class constructor
PdfViewerWidget = function (settings) {

    //private variables setters
    //initialize class logic here
    var self = this;

    self.settings = settings;

    //set default settings values
    if (settings) {
        self.settings.height = self.settings.height ? self.settings.height + "px" : '500px';
        self.settings.width = self.settings.width ? self.settings.width + "px" : '100%';
        self.settings.enableNavPans = (self.settings.enableNavPans === true) ? 1 : 0;
        self.settings.enableToolbar = (self.settings.enableToolbar === true) ? 1 : 0;
        self.settings.enableStatusBar = (self.settings.enableStatusBar === true) ? 1 : 0;
    }

    //init elemets
    self.eleContainer = $('#' + self.settings.containerId);

    self.load();

};

//class members
PdfViewerWidget.prototype =
{

    //widget load entry point
    load: function() {

        var self = this;

        if (!self.eleContainer && !self.eleContainer.length) {
            console.log('PdfViewerWidget Container element not found');
            return;
        }


        if (!self.settings && !self.settings.pdfFilePath) {
            console.log('PdfViewerWidget No Valid settings passed');
            return;
        }

        var options = {
            height: self.settings.height,
            width: self.settings.width,
            page: self.settings.pageNumber,
            forcePDFJS: self.settings.forcePdfJs,
            PDFJS_URL: self.settings.pdfJsUrl,

            pdfOpenParams: {                
                view: self.settings.view,
                page: self.settings.pageNumber,
                pagemode: self.settings.pageMode,
                navpanes: self.settings.enableNavPans,
                toolbar: self.settings.enableToolbar,
                statusbar: self.settings.enableStatusBar,
                zoom: self.settings.zoom,
                search: self.settings.search
            }
        };
        
        self.eleContainer.css('height', self.settings.height).css('width', self.settings.width);
        
        PDFObject.embed(self.settings.pdfFilePath, "#" + self.settings.containerId, options);

        var iframe = self.eleContainer.find('iframe');
        iframe.addClass('embed-responsive-item');
        iframe.load(function () {            
            iframe[0].contentWindow.mozL10n.setLanguage(self.settings.lang);

            iframe[0].contentWindow.document.getElementById('print').style.display = "none";
            iframe[0].contentWindow.document.getElementById('openFile').style.display = "none";
            iframe[0].contentWindow.document.getElementById('download').style.display = "none";
            iframe[0].contentWindow.document.getElementById('viewBookmark').style.display = "none";
        });
    }

}; //class end