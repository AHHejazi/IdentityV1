//class constructor
CaptchaWidget = function (containerId, captchaHandlrUrl) {

    var self = this;
    //private variables setters
    //initialize class logic here
    self.containerId = containerId;
    self.captchaHandlrUrl = captchaHandlrUrl;
   
    self.eleContainer = $('#' + self.containerId);
    self.txtCaptcha = $('#' + self.containerId + ' .captcha-input');
    self.imgCaptcha = $('#' + self.containerId + ' .img-captcha');
    self.ancCaptchaReload = $('#' + self.containerId + ' .img-captcha-reload');
};

//class members
CaptchaWidget.prototype =
{
    //widget load entry point
    load: function () {
        var self = this;
        if (!self.eleContainer) return;        

        self.ancCaptchaReload.click(function (e) {
            e.preventDefault();
            self.imgCaptcha.attr('src', self.captchaHandlrUrl + '?' + new Date());
        });      
    }
}; //class end