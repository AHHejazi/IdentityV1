var localResources = {
	ar: {
		CharactersRemaining: "أحرف متبقية",
		DeleteConfirm: "هل أنت متأكد انك تريد الحذف؟",
		ErrorOccured: "لقد حدث خطأ",
		UnExpectedError: "لقد حدث خطأ غير متوقع برجاء المحاولة مرة أخرى",
		ChangeLangDescription: "حول إلى اللغة العربية",
		Arabic: "عربي",
		Yes: 'نعم',
		No: 'لا',
		English: "إنجليزي",
		LoadingText: "جارى تحميل البيانات الخاصة بك.",
		Ok: "موافق",
		Cancel: "إلغاء",
		Close: "إغلاق"
	},
	en: {
		CharactersRemaining: "characters remaining",
		DeleteConfirm: "Are you sure you want to delete?",
		ErrorOccured: "Error Occured",
		UnExpectedError: "Unexpected Error Occured. Please try again later.",
		ChangeLangDescription: "Switch to English language",
		Arabic: "Arabic",
		English: "English",
		LoadingText: "Loading ...",
		Yes: 'Yes',
		No: 'No',
		Ok: "Ok",
		Cancel: "Cancel",
		Close: "Close"
	}
};

var $commons = (function () {


// ------------- Dates ------------------------------
	function parseDate(dateString, calendarType, dateFormat, lang){

		calendarType = calendarType || 'gregorian';
		dateFormat = (dateFormat || 'dd/mm/yyyy').toLowerCase();
		lang = lang || getCurrentLang();

		if(!$.calendars){
			return;
		}

		 var calendar = $.calendars.instance(calendarType, lang);
		 return calendar.parseDate(dateFormat, dateString);
	}

	function tryParseDate(dateString, dateFormat, calendarType, lang) {
		
		dateFormat = (dateFormat || 'dd/mm/yyyy').toLowerCase();
		lang = lang || getCurrentLang();

		if (!$.calendars) {
			return;
		}

		var parsedDate = null;
		var parseSuccess = false;
		
		try {
			var gregorianCalendar = $.calendars.instance(calendarType, lang);
			parsedDate = gregorianCalendar.parseDate(dateFormat, dateString);
			parseSuccess = true;
		} catch (e) {
		}


		return parsedDate;
	}

	function convertDates (dateObjToParse, fromCalName, toCalName) {
		var jd = dateObjToParse.toJD();
		return $.calendars.instance(toCalName).fromJD(jd);
	}
	
	function diffDates(startDateString, startDateWidgetSettings, endDateString, endDateWidgetSettings){

		var startDate = parseDate(startDateString, startDateWidgetSettings.calendarType, startDateWidgetSettings.dateFormat, startDateWidgetSettings.lang);
		var endDate = parseDate(endDateString, endDateWidgetSettings.calendarType, endDateWidgetSettings.dateFormat, endDateWidgetSettings.lang);

		var oneDay = 24 * 60 * 60 * 1000; // hours*minutes*seconds*milliseconds
		var diffDays = Math.round((endDate.toJSDate().getTime() - startDate.toJSDate().getTime())/(oneDay));

		return diffDays;

	}

	function tryDiffDates(startDateString, startDateCalType, endDateString, endDateCalType) {

		var startDate = tryParseDate(startDateString, 'dd/mm/yyyy', startDateCalType);
		var endDate = tryParseDate(endDateString, 'dd/mm/yyyy', endDateCalType);

		if (startDate == null) {
			throw "unable to parse start date";
		}

		if (endDate == null) {
			throw "unable to parse end date";
		}

		var oneDay = 24 * 60 * 60 * 1000; // hours*minutes*seconds*milliseconds
		var diffDays = Math.round((endDate.toJSDate().getTime() - startDate.toJSDate().getTime()) / (oneDay));

		return diffDays;

	}


	// ------------------------------- Localization and Culture Related Code -----------------------------

	function isArabic() {
		return getCurrentLang() === 'ar';
	}

	function isRtl() {
		return isArabic();
	}

	var cultureCookieName = 'app-culture';

	function getCurrentLang() {
		var lang = 'ar';
		var cultureFromCookie = readCookie(cultureCookieName);
		var langFromHtmlTag = $('html').attr('lang');

		if (cultureFromCookie !== undefined && cultureFromCookie !== "") {
			lang = (cultureFromCookie === 'ar-SA') ? 'ar' : 'en';
		} else if (langFromHtmlTag !== undefined && langFromHtmlTag !== "") {
			lang = langFromHtmlTag;
		}

		return lang;
	};

	function getLocalString(key) {
		return localResources[getCurrentLang()][key];
	};

	function getTimeSuffix(time) {

		if (isArabic())
			return time.replace(/am/g, " ص ").replace(/pm/g, " م ");
		else
			return time;
	};

	// ------------------- Application Behavior  --------------------------------------------------------------------

	function applyAutoPostBackAttribute() {
		$('[autopostback=true]').change(function () {
			$(this).closest('form').submit();
		});
	};

	function setCurrentCulture(cultureName) {
		if (cultureName) {
			setCookie(cultureCookieName, cultureName);
		}
	};

	function initLangSwitcher(elementId) {

		elementId = elementId || "lang-switcher";
		var langSwitcherAnchor = $("#" + elementId);
		var isIcon = false;

		if (!langSwitcherAnchor.length) {
			elementId = "lang-switcher-icon";
			langSwitcherAnchor = $("#" + elementId);
			isIcon = langSwitcherAnchor.length !== 0;
		}

		langSwitcherAnchor.attr("title",
			isArabic()
				? localResources.en.ChangeLangDescription
				: localResources.ar.ChangeLangDescription);

		if (!isIcon) {
			langSwitcherAnchor.text(isArabic()
				? localResources.en.English
				: localResources.ar.Arabic);
		}
		
		langSwitcherAnchor.click(function (e) {
			if (isArabic()) {
				setCookie(cultureCookieName, "en-US");
			} else {
				setCookie(cultureCookieName, "ar-SA");
			}

			window.location = window.location.href;
			e.preventDefault();
		});
	};

	//allpy max length automatically from mvc vaidation attribute string
	function applyMaxLengthAttributes() {

		// for string values with stringlength attribute added
		$("input[data-val-length-max], textarea[data-val-length-max]").each(function () {
			var element = $(this);
			element.attr("maxlength", element.data().valLengthMax);
		});

		//for number values with number with range attribute added
		$("input[data-val-range-max][data-val-range-max], textarea[data-val-range-max][data-val-range-max]").each(function () {
			var element = $(this);
			element.attr("maxlength", element.data().valRangeMax.toString().length - 1);
		});
	   
	};

	//character countdown in textarea
	function applyTextAreasCharCountDown() {
		$('textarea').keyup(function () {

			var textArea = $(this);

			var maxLength = textArea.attr('maxlength');
			if (!maxLength) {
				return;
			}

			//JavaScript is treating a newline as one character rather than two so when I try to insert a "max length" string into the database I get an error.
			//Detect how many newlines are in the textarea, then be sure to count them twice as part of the length of the input.
			var newlines = (textArea.val().match(/\n/g) || []).length;
			if (textArea.val().length + newlines > maxLength) {
				textArea.val(textArea.val().substring(0, maxLength - newlines));
			}

			var length = textArea.val().length;
			length = maxLength - length - newlines;

			if (!textArea.next('.textarea-countdown').length) {
				$('<span class="textarea-countdown help-block" style="direction:' +
					(isRtl() ? 'rtl' : 'ltr') +
					';"><span>').insertAfter(textArea);
			}

			textArea.next('.textarea-countdown').text(length + ' ' + getLocalString('CharactersRemaining'));
		});
	};

	function initSubmitButtonOverlay() {
		$(':submit').click(function (evt) {

			if ($(this).hasClass("disable-overlay")) {
				return;
			}

			var form = $(this).parents('form:first');

			if (form && form.length && form.valid && form.valid()) {
				showOverlay(' ');
			} else {
				hideOverlay();
			}
		});

		$('a.enable-overlay').click(function (evt) {
			showOverlay(' ');
		});

	};

	function initRequiredFieldsMark() {

		$("[data-val-required][name], [data-val-requiredif][name]")
			.each(function () {
				$('label[for="' + $(this).attr("name").replace(".", "_") + '"]').not(":contains('*')")
					.append('<span class="text-danger">*</span>');

			});

		//$('[data-val-required], [data-val-requiredif][name]')
		//    .each(function () {
		//        $('label[for="' + $(this).attr('name').replace(".", "_") + '"]:not(:has(>span))')
		//            .append('<span class="text-danger">*</span>');
		//    });


	};

	function printPdf(pdfFileUrl) {

		if ($("#frmPrint").length) {
			$("#frmPrint").remove();
		}

		if (!pdfFileUrl) {
			return false;
		}

		var html =
			'<iframe id="frmPrint" width="0" height="0" frameborder="0" name="frmPrint" src="' +
			pdfFileUrl +
			'"></iframe>';

		$('body').after(html);

		var iframe = document.frames
			? window.frames.frames['frmPrint']
			: document.getElementById('frmPrint');

		var ifWin = iframe.contentWindow || iframe;

		try {
			ifWin.focus();
			ifWin.print();
		} catch (e) {
			window.print(false);
			//or when you get Invalid calling object error for IE9 and above
			//set the browser into IE8 compatibility mode will work
		}

		return false;
	};

	function refreshFormValidation(selector) {
		var form = $(selector ? selector : 'form')
			.removeData("validator") // added by the raw jquery.validate plugin /
			.removeData("unobtrusiveValidation"); /* added by the jquery unobtrusive plugin*/
		$.validator.unobtrusive.parse(form);
	};

	function disableAllInputs(containerSelector) {
		if (containerSelector) {

			$(containerSelector).find('input[data-val-required],input[data-val-requiredif],select[data-val-required],select[data-val-requiredif]')
				.removeAttr('data-val-required');
			$(containerSelector).find('.text-danger').hide();
			$(containerSelector).find("input,textarea,select").not(".btn-default").not('.btn-primary').attr("disabled", "disabled");
			$('div.panel-heading-btn > input').hide();
		} else {

			$('input[data-val-required],input[data-val-requiredif],select[data-val-required],select[data-val-requiredif]')
				.removeAttr('data-val-required');
			$('.text-danger').hide();
			$("input,textarea,select").not(".btn-default").not('.btn-primary').attr("disabled", "disabled");
			$('div.panel-heading-btn > input').hide();
		}
		$(".date-picker-select").removeAttr("disabled");
	}

	function disableInputs(containerSelector) {
		disableAllInputs(containerSelector);
		$(document).ajaxStop(function () {
			disableAllInputs(containerSelector);
		});  
	}

	var InlineAlertCssClasses =
		{
			danger: "alert-danger",
			information: "alert-info",
			success: "alert-success",
			warning: "alert-warning"
		}

	function showMessageInlineAlert(containerSelector, messageType, message) {
		$(containerSelector).html('<div class="alert ' + messageType + '">' + message + '</div>');
	}

	function showSuccessInlineAlert(containerSelector, message) {
		showMessageInlineAlert(containerSelector, InlineAlertCssClasses.success, message);
	}

	function showErrorInlineAlert(containerSelector, message) {
		showMessageInlineAlert(containerSelector, InlineAlertCssClasses.danger, message);
	}

	function showInformationInlineAlert(containerSelector, message) {
		showMessageInlineAlert(containerSelector, InlineAlertCssClasses.information, message);
	}

	function showWarningInlineAlert(containerSelector, message) {
		showMessageInlineAlert(containerSelector, InlineAlertCssClasses.warning, message);
	}

	function initSelect(selector, data, selectedVal) {
		var ddl = $(selector);
		var selectItemOption = ddl.find('option:first');
		ddl.empty();
		if (selectItemOption.length) {
			ddl.append(selectItemOption);
		}

		ddl.attr('disabled', 'disabled');

		if (data && data.length) {
			ddl.removeAttr('disabled');
			$.each(data,
				function (index, d) {
					ddl.append($("<option />").val(d.value).text(d.text));
				}
			);
		}

		if (selectedVal) {
			ddl.val(selectedVal);
		}

	}

	function initCheckBoxList(selector, data, propName) {
		var container = $(selector);

		container.empty();

		$.each(data,
			function (iteration, item) {
				if (data && data.length) {
					container.append(
						$(document.createElement('label')).attr({
							'for': 'chk-' + iteration,
							'class': 'checkbox-inline'
						})
							.append(
							$(document.createElement("input")).attr({
								type: 'checkbox',
								id: propName,
								name: propName,
								value: item.value
							}),
							item.text
							));
				}
			});

	}

	function bindCheckBoxList(selector, data) {
		var container = $(selector);

		if (data && data.length) {
			$.each(data,
				function (i, e) {
					$('input[type=checkbox][value=' + e + ']').prop('checked', true);
				});
		}
	}

	function showOverlay(loadingText) {

		loadingText = loadingText ? loadingText : getLocalString("LoadingText");

		if ($("#divOverlay").length) {
			$("#divOverlay").show();
			return;
		}

		var divOverlay = '<div id="divOverlay" style="display:none">' +
			'<div class="cube_loader_overlay">'+
			'<div class="Cube panelLoad">' +
			'<div class="cube-face cube-face-front"></div>' +
			'<div class="cube-face cube-face-back"></div>' +
			'<div class="cube-face cube-face-left"></div>' +
			'<div class="cube-face cube-face-right"></div>' +
			'<div class="cube-face cube-face-bottom"></div>' +
			'<div class="cube-face cube-face-top"></div>' +
			'</div>' +
			' </div>' +
			'</div>';

		$('body').after(divOverlay);
		$("#divOverlay").show();
	};

	function hideOverlay() {
		if ($("#divOverlay").length) {
			$("#divOverlay").hide();
		}
	};

	function showAlert(title, text, callback) {

		hideOverlay();

		var html =
			'<div id="divAlertModalMessage" class="modal fade">' +
			'<div class="modal-dialog">' +
			'<div class="modal-content">' +
			'<div class="modal-header">' +
			'<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>' +
			'<h4 class="modal-title">' +
			title +
			'</h4>' +
			'</div>' +
			'<div class="modal-body">' +
			text +
			'</div>' +
			'<div class="modal-footer">' +
			'<div class="text-left"><button type="button" class="btn btn-ok btn-default btn-lg btn-nor-primary">' +
			getLocalString("Close") +
			'</button></div>' +
			'</div>' +
			'</div>' +
			'</div>' +
			'</div>';

		$('body').after(html);

		$("#divAlertModalMessage button.btn-ok").click(function (e) {
			$("#divAlertModalMessage").modal('hide');

			if (isFunction(callback)) {
				callback();
			}
		});

		$("#divAlertModalMessage").on('show.bs.modal', function () {
			// z-index fix.
			// if you open modal alert from another modal dialogue.

			var zIndex = 1050 + (10 * $('.modal:visible').length);
			$(this).css('z-index', zIndex);

			setTimeout(function () {
				//$("#divAlertModalMessage").find('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
				if ($("#divAlertModalMessage").length) {
					$("#divAlertModalMessage").css('z-index', zIndex - 1);
				}
			}, 1);

		});

		$("#divAlertModalMessage").on("hidden.bs.modal",
			function () {
				$(this).data('bs.modal', null);
				$("#divAlertModalMessage").remove();
			});

		$("#divAlertModalMessage").modal('show');
	};

	function confirm(title, text, callback, okText, cancelText) {

		hideOverlay();

		if ($("#divConfirmMessage").length) {
			$(this).data('bs.modal', null);
			$("#divConfirmMessage").remove();
		}

		var html =
			'<div id="divConfirmMessage" class="modal fade">' +
			'<div class="modal-dialog">' +
			'<div class="modal-content">' +
			'<div class="modal-header">' +
			'<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>' +
			'<h4 class="modal-title">' +
			title +
			'</h4>' +
			'</div>' +
			'<div class="modal-body">' +
			text +
			'</div>' +
			'<div class="modal-footer">' +
			'<div class="text-left"><button type="button" class="btn btn-ok btn-lg btn-nor-primary">' +
			(okText || getLocalString("Ok")) +
			'</button><button type="button" class="btn btn-cancel btn-default btn-lg btn-nor-primary">' +
			(cancelText || getLocalString("Cancel")) +
			'</button></div>' +
			'</div>' +
			'</div>' +
			'</div>' +
			'</div>';

		$('body').after(html);

		$("#divConfirmMessage").modal('show');

		$("#divConfirmMessage button.btn-ok").on("click",
			function (e) {
				$("#divConfirmMessage").modal('hide');
				if (isFunction(callback)) {
					callback();
				}
			});

		$("#divConfirmMessage button.btn-cancel").on("click",
			function (e) {
				$("#divConfirmMessage").modal('hide');
			});

		$("#divConfirmMessage").on('show.bs.modal', function () {
			// z-index fix.
			// if you open modal alert from another modal dialogue.

			var zIndex = 1050 + (10 * $('.modal:visible').length);
			$(this).css('z-index', zIndex);

			setTimeout(function () {
				//$("#divAlertModalMessage").find('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
				if ($("#divAlertModalMessage").length) {
					$("#divAlertModalMessage").css('z-index', zIndex - 1);
				}
				
			}, 1);

		});

		$("#divConfirmMessage").on("hidden.bs.modal",
			function () {
				$(this).data('bs.modal', null);
				$("#divConfirmMessage").remove();
			});

	};

	function confirmYesNo(title, text, callBack) {

		hideOverlay();

		var html =
			'<div id="divConfirmMessage" class="modal fade">' +
			'<div class="modal-dialog">' +
			'<div class="modal-content">' +
			'<div class="modal-header">' +
			'<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>' +
			'<h4 class="modal-title">' +
			title +
			'</h4>' +
			'</div>' +
			'<div class="modal-body">' +
			text +
			'</div>' +
			'<div class="modal-footer">' +
			'<div class="text-left">' +
			'<button type="button" class="btn btn-yes btn-lg btn-nor-primary">' +
			getLocalString("Yes") +
			'</button>' +
			'<button type="button" class="btn btn-no btn-lg btn-nor-primary">' +
			getLocalString("No") +
			'</button>' +
			'<button type="button" class="btn btn-cancel btn-default btn-lg btn-nor-primary">' +
			getLocalString("Cancel") +
			'</button>' +
			'</div></div>' +
			'</div>' +
			'</div>' +
			'</div>' +
			'</div>';

		$('body').after(html);

		$("#divConfirmMessage").modal('show');

		$("#divConfirmMessage button.btn-yes").on("click",
			function (e) {
				$("#divConfirmMessage").modal('hide');
				if (isFunction(callBack)) {
					callBack(true);
				}
			});

		$("#divConfirmMessage button.btn-no").on("click",
			function (e) {
				$("#divConfirmMessage").modal('hide');
				if (isFunction(callBack)) {
					callBack(false);
				}
			});

		$("#divConfirmMessage button.btn-cancel").on("click",
			function (e) {
				$("#divConfirmMessage").modal('hide');
			});

		$("#divConfirmMessage").on('show.bs.modal', function () {
			// z-index fix.
			// if you open modal alert from another modal dialogue.

			var zIndex = 1050 + (10 * $('.modal:visible').length);
			$(this).css('z-index', zIndex);

			setTimeout(function () {
				//$("#divAlertModalMessage").find('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
				if ($("#divAlertModalMessage").length) {
					$("#divAlertModalMessage").css('z-index', zIndex - 1);
				}
			}, 1);

		});

		$("#divConfirmMessage").on("hidden.bs.modal",
			function () {
				$(this).data('bs.modal', null);
				$("#divConfirmMessage").remove();
			});

	};

	function showModal(title, text, okButtonText, cancelButtonText, callback, hideOnOk) {

		hideOverlay();

		var cancelButtonHtml = cancelButtonText && $.trim(cancelButtonText) !== ''
			? '<button type="button" class="btn btn-default btn-lg btn-nor-primary btn-primary btn-cancel">' + cancelButtonText
			: '';
		var html =
			'<div id="divModalContainer" class="modal fade">' +
			'<div class="modal-dialog">' +
			'<div class="modal-content">' +
			'<div class="modal-header">' +
			'<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>' +
			'<h4 class="modal-title">' +
			title +
			'</h4>' +
			'</div>' +
			'<div class="modal-body">' +
			text +
			'</div>' +
			'<div class="modal-footer">' +
			'<div class="text-left"><button type="button" class="btn-ok btn btn-lg btn-nor-primary">' +
			okButtonText +
			'</button>' +
			cancelButtonHtml +
			'</div>' +
			'</div>' +
			'</div>' +
			'</div>' +
			'</div>';

		$('body').after(html);

		$("#divModalContainer").modal('show');

		//refreshFormValidation('#frmDeActivateUserContent');

		$("#divModalContainer button.btn-ok").on("click",
			function (e) {

				if (hideOnOk === true) {
					$("#divModalContainer").modal('hide');
				}

				if (isFunction(callback)) {
					callback();
				}
			});

		$("#divModalContainer button.btn-cancel").on("click",
			function (e) {
				$("#divModalContainer").modal('hide');
			});

		$("#divModalContainer").on('show.bs.modal', function () {
			// z-index fix.
			// if you open modal alert from another modal dialogue.

			var zIndex = 1040 + (10 * $('.modal:visible').length);
			$(this).css('z-index', zIndex);

			setTimeout(function () {
				//$("#divAlertModalMessage").find('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
				if ($("#divAlertModalMessage").length) {
					$("#divAlertModalMessage").css('z-index', zIndex - 1);
				}
			}, 1);

		});

		$("#divModalContainer").on("hidden.bs.modal",
			function () {

				// Scrollbar fix
				// If you have a modal on your page that exceeds the browser height, then you can't scroll in it when closing an second modal.
				$('.modal:visible').length && $(document.body).addClass('modal-open');

				$(this).data('bs.modal', null);
				$("#divModalContainer").remove();

			});

	};

	function hideModal() {
		$("#divModalContainer").modal('hide');
		$("#divModalContainer").on("hidden.bs.modal",
			function () {

				// Scrollbar fix
				// If you have a modal on your page that exceeds the browser height, then you can't scroll in it when closing an second modal.
				$('.modal:visible').length && $(document.body).addClass('modal-open');

				$(this).data('bs.modal', null);
				$("#divModalContainer").remove();

			});
	};

	function confirmWithAjaxPost(title,
		confirmMsg,
		defaultSuccessMsg,
		defaultErrorMsg,
		url,
		dataObject,
		successCallBack) {

		hideOverlay();

		confirm(title,
			confirmMsg,
			function() {
				showOverlay(title);
				$.post(url,
					dataObject,
					function(data) {
						hideOverlay();
						if (data) {
							if (data.success === true) {
								if (data.successMessage || defaultSuccessMsg) {
									showAlert(title,
										data.successMessage || defaultSuccessMsg,
										function() {
											//showOverlay();
											if (isFunction(successCallBack)) {
												successCallBack(data);
											}
										});
								} else if (isFunction(successCallBack)) {
									successCallBack(data);
								}

							} else if (data.errorMessage || defaultErrorMsg) {

								showAlert(title, data.errorMessage || defaultErrorMsg);

							}
						}
					});
			});
	}

	function showModalWithPartial(title, url, successCallback, closeCallback) {
		
		hideModal();
		hideOverlay();

		var html =
			'<div id="divModalContainer" class="modal fade">' +
			'<div class="modal-dialog">' +
			'<div class="modal-content">' +
			'<div class="modal-header">' +
			'<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>' +
			'<h4 class="modal-title">' +
			title +
			'</h4>' +
			'</div>' +
			'<div class="modal-body">' +
			'</div>' +
			'</div>' +
			'</div>' +
			'</div>';

		$('body').after(html);

		$(".modal-body").load(url,
			function () {

				hideOverlay();

				//$("#divModalContainer").modal('show');
				$("#divModalContainer").modal({
					backdrop: 'static',
					keyboard: false
				});

				initMvcFormPage();
				$commons.refreshFormValidation('#divModalContainer')

				var partialScripts = $.trim($(".modal-body").find("script[type='text/javascript']").last().html());

				if (partialScripts) {
					var script = "<script type=\"text/javascript\"> " + partialScripts + " </script>";                    
					$('body').append(script);//incorporates and executes inmediatelly
				}

				executePartialViewsScriptTags();
			});

		$(document).off('submit').on('submit', '#divModalContainer form',
			function (e) {

				e.preventDefault();

				var form = this;
				var formData = new FormData(form);
				
				if(!$(form).valid()){

					animateToFirstValidationError();
					return;
				}

				$.ajax({    
					url: form.action,
					type: form.method,
					//dataType: 'json',
					processData: false,
					contentType: false,
					cache : false,
					data: formData,
					success: function (result) {

						hideOverlay();

						$(form).unbind();

						if (result.success === true) {
							$('#divModalContainer').modal('hide');
							if (result.successMessage) {
								showAlert(title,
									result.successMessage,
									function () {
										if (isFunction(successCallback)) {
											successCallback(result);
										}
									});
							} else {
								if (isFunction(successCallback)) {
									successCallback(result);
								}
							}

						} else if (result.success === false) {
							if (result.errorMessage && result.errorMessage.length) {
								result.errorMessage = stringReplaceAll(result.errorMessage, ",", "</br>");
								$('.modal-body').html(result.errorMessage);

							} else {
								$('.modal-body').html(getLocalString('ErrorOccured'));
							}

						} else {

							$('.modal-body').html(result);
						}
					},
					error: function (result) {
						$('.modal-body').html(getLocalString('ErrorOccured'));
					}
				});
				return false;
			});

		$(document).on('click',
			'.btnCancel',
			function (e) {
				$("#divModalContainer").modal('hide');
			});

		$("#divModalContainer a.btn-cancel").on("click",
			function (e) {
				$("#divModalContainer").modal('hide');
			});

		$("#divModalContainer").on('show.bs.modal', function () {
			// z-index fix.
			// if you open modal alert from another modal dialogue.

			var zIndex = 1040 + (10 * $('.modal:visible').length);
			$(this).css('z-index', zIndex);

			setTimeout(function () {
				//$("#divAlertModalMessage").find('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
				if ($("#divAlertModalMessage").length) {
					$("#divAlertModalMessage").css('z-index', zIndex - 1);
				}
			}, 1);

		});

		$("#divModalContainer").on('hidden.bs.modal',
			function () {

				if (isFunction(closeCallback)) {
					closeCallback();
				}

				if ($("#divModalContainer").find('.select2').length) {
					var select2ddls = $("#divModalContainer").find('.select2-hidden-accessible');
					select2ddls.each(function (i, item) {
						$(item).select2("destroy");
					});
				}

				// Scrollbar fix
				// If you have a modal on your page that exceeds the browser height, then you can't scroll in it when closing an second modal.
				$('.modal:visible').length && $(document.body).addClass('modal-open');

				$(this).data('bs.modal', null);
				$("#divModalContainer").remove();
			});

	};
	
	function ajaxSubmitForm(formCssSelector, formTitle, successCallback, isControllerActionReturnViewHtml, skipFormValidation)
	{
		return new Promise(function(resolve, reject) {

			var form = $(formCssSelector);

			if ($(form).length === 0) {
				return;
			}

			if(!skipFormValidation){
				refreshFormValidation(formCssSelector);
			
				if(form.valid() === false){
					
					animateToFirstValidationError();
					return;
				}
			}

			showOverlay();

			var formData = new FormData(form[0]); //$(this).serialize()        

			$.ajax({    
				url: form[0].action,
				type: form[0].method,
				processData: false,
				contentType: false,
				cache : false,
				data: formData,
				success: function (result) {

					hideOverlay();

					$(form).unbind();

					if(isControllerActionReturnViewHtml === true){
						form.replaceWith(result);
						if (isFunction(successCallback)) {
							successCallback(result);
						}
						resolve(result);
						return;
					}
					
					if (result.success === true) {
		   
						if (result.successMessage) {
							showAlert(formTitle,
								result.successMessage,
								function () {
									if (isFunction(successCallback)) {
										successCallback(result);
									}
								});
						} else {
							if (isFunction(successCallback)) {
								successCallback(result);
							}
						}
						
					} else if (result.success === false) {
						if (result.errorMessage && result.errorMessage.length) {
							result.errorMessage = stringReplaceAll(result.errorMessage, ",", "</br>");
								//$(form).find('#CustomValidationSummary').html(result.errorMessage)
							showAlert(formTitle, result.errorMessage);

						} else {
							//$(form).find('#CustomValidationSummary').html(result.errorMessage)
							showAlert(formTitle, result.errorMessage);
						}

					} else {
						showAlert(formTitle, result);

					}
					
					resolve(result);
				},
				error: function (result) {
					hideOverlay();
					showAlert(formTitle, getLocalString('ErrorOccured'));
				}
			});
		});		
	}

	function animateTo(cssSelector) {

		if (cssSelector && $(cssSelector).length) {
			$('html, body').animate({
				scrollTop: ($(cssSelector).offset().top - 300)
			}, 1000);
		}
	}

	function animateToFirstValidationError() {
		animateTo('.field-validation-error');
	}

	function redirectToUrl(url, appendReturnUrlParam) {
		if (!url) {
			return;
		}

		if ($.trim(url) === '') {
			return;
		}

		appendReturnUrlParam = appendReturnUrlParam === true;

		showOverlay();

		document.location.href =
			!appendReturnUrlParam ? url : updateQueryStringParameter('returnUrl', document.location.href, url);
	}


	function ajaxAppendPartial(containerSelector, url, successCallback) {

		if ($(containerSelector).length == 0) {
			return;
		}

		showOverlay();

		$(containerSelector).load(url,
			function (data) {
				
				hideOverlay();

				initMvcFormPage();

				var partialScripts = $.trim($(containerSelector).find("script[type='text/javascript']").last().html());
				if (partialScripts) {
					var script = "<script type=\"text/javascript\"> " + partialScripts + " </script>";
					$('body').append(script);//incorporates and executes inmediatelly
				}

				executePartialViewsScriptTags();

				if (isFunction(successCallback)) {
					successCallback();
				}
			});
	};

	function initBtnBack() {
		var btnBack = $('#btnBack');
		var returnUrl = getQueryStringParameterByName('returnUrl');

		if (btnBack.length && btnBack.attr('href') === '#') {

			if (returnUrl && $.trim(returnUrl) !== '') {
				btnBack.click(function (e) {
					e.preventDefault();
					redirectToUrl(decodeURI(returnUrl));
				});
			} else {
				btnBack.text(getLocalString('Close'));
				btnBack.click(function (e) {
					e.preventDefault();
					window.close();
				});
			}

		}


	}
	

	function initBootstrapTabs(selector) {
		selector = selector ? selector : '.tabsContainer li a';
		$(selector).click(function (e) {
			e.preventDefault();
			$(this).tab('show');
		});
	}

	function activateBootstrapTab(tabId) {
		$('.nav-tabs a[href="#' + tabId + '"]').tab('show');
	};

	function initUploaderInfoTooltip() {
		$('.upload-terms-icn').on('mouseenter',
			function() {
				$(this).parent().find(".upload-terms").show();
			}).on('mouseleave',
			function() {
				$(this).parent().find(".upload-terms").hide();
			});
		//    $('.upload-terms-icn').off('hover').on('hover', function () {
		//        $(this).parent().find(".upload-terms").fadeToggle(100);
		//    });
	}

	function executePartialViewsScriptTags() {
		////$("script[data-partial-view-script='true']").each(function (index) {
		////    var partialViewScript = "<script type=\"text/javascript\"> " + $(this).html() + " </script>";
		////    $('body').append(partialViewScript);//incorporates and executes inmediatelly
		////});
	}

	// ------------------- Common Helper Functions ------------------------------------------------------------------

	function getRandomString() {
		var text = "";
		var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

		for (var i = 0; i < 5; i++)
			text += possible.charAt(Math.floor(Math.random() * possible.length));

		return text;
	}

	function swapElements(obj1, obj2) {
		// create marker element and insert it where obj1 is
		var temp = document.createElement("div");
		obj1.parentNode.insertBefore(temp, obj1);

		// move obj1 to right before obj2
		obj2.parentNode.insertBefore(obj1, obj2);

		// move obj2 to right before where obj1 used to be
		temp.parentNode.insertBefore(obj2, temp);

		// remove temporary marker node
		temp.parentNode.removeChild(temp);
	}


	function moveTableRow(tr, direction) {

		var selectedOption = $(tr);

		if (direction === 'down') {

			var nextOption = $(tr).next("tr");
			if ($(nextOption).text() !== "") {
				$(selectedOption).remove();
				$(nextOption).after($(selectedOption));
			}
		}

		if (direction === 'up') {

			var prevOption = $(tr).prev("tr");
			if ($(prevOption).text() !== "") {
				$(selectedOption).remove();
				$(prevOption).before($(selectedOption));
			}
		}

	}

	function stringReplaceAll(string, search, replacement) {
		return string.replace(new RegExp(search, 'g'), replacement);
	}

	function getQueryStringParameterByName(name) {
		name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
		var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
			results = regex.exec(location.search);
		return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
	};

	function updateQueryStringParameter(key, value, url) {
		var uri = url || document.location.href;
		var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
		var separator = uri.indexOf('?') !== -1 ? "&" : "?";
		if (uri.match(re)) {
			return uri.replace(re, '$1' + key + "=" + value + '$2');
		}
		else {
			return uri + separator + key + "=" + value;
		}
	}

	function htmlEncode(value) {
		//create a in-memory div, set it's inner text(which jQuery automatically encodes)
		//then grab the encoded contents back out.  The div never exists on the page.
		return $('<div/>').text(value).html();
	};

	function htmlDecode(value) {
		return $('<div/>').html(value).text();
	};

	function truncateString(string, count, suffix) {
		if (string === undefined || string === null)
			return "";
		if (string.length > count)
			return string.substring(0, string.substring(0, count).lastIndexOf(" ")) + suffix;
		else
			return string;
	};

	function isStringHasScriptTag(string) {
		var patt = new RegExp("<(.|\n)*?>");
		return patt.test(string);
	};

	function isFunction(functionToCheck) {
		var getType = {};
		return functionToCheck && getType.toString.call(functionToCheck) === '[object Function]';
	};

	function setCookie(name, value, days) {
		var expires;
		if (days) {
			var date = new Date();
			date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
			expires = "; expires=" + date.toGMTString();
		} else {
			expires = "";
		}
		document.cookie = name + "=" + value + expires + "; path=/";
	};

	function readCookie(name) {
		var nameEq = name + "=";
		var ca = document.cookie.split(';');
		for (var i = 0; i < ca.length; i++) {
			var c = ca[i];
			while (c.charAt(0) === ' ') c = c.substring(1, c.length);
			if (c.indexOf(nameEq) === 0) return c.substring(nameEq.length, c.length);
		}
		return null;
	};

	function eraseCookie(name) {
		setCookie(name, "", -1);
	}

	// ---------------------- Return Public Accessible Functions ---------------------------------------------------


	function initMvcFormPage() {
		$commons.applyMaxLengthAttributes();
		$commons.applyTextAreasCharCountDown();
		$commons.applyAutoPostBackAttribute();
		$commons.initLangSwitcher();
		$commons.initRequiredFieldsMark();
		$commons.initSubmitButtonOverlay();
		$commons.initUploaderInfoTooltip();
		$commons.initBootstrapTabs();
		$commons.executePartialViewsScriptTags();
		$commons.initBtnBack();
		$commons.refreshFormValidation();
	}

	var toReturn = {
		isArabic: isArabic,
		isRtl: isRtl,
		getTimeSuffix: getTimeSuffix,
		applyAutoPostBackAttribute: applyAutoPostBackAttribute,
		initLangSwitcher: initLangSwitcher,
		applyMaxLengthAttributes: applyMaxLengthAttributes,
		applyTextAreasCharCountDown: applyTextAreasCharCountDown,
		initSubmitButtonOverlay: initSubmitButtonOverlay,
		initRequiredFieldsMark: initRequiredFieldsMark,
		printPdf: printPdf,
		refreshFormValidation: refreshFormValidation,
		disableInputs: disableInputs,
		stringReplaceAll: stringReplaceAll,
		getQueryStringParameterByName: getQueryStringParameterByName,
		updateQueryStringParameter: updateQueryStringParameter,
		initSelect: initSelect,
		showAlert: showAlert,
		confirm: confirm,
		hideModal: hideModal,
		showModal: showModal,
		confirmWithAjaxPost: confirmWithAjaxPost,
		showModalWithPartial: showModalWithPartial,
		confirmYesNo: confirmYesNo,
		showOverlay: showOverlay,
		hideOverlay: hideOverlay,
		bindCheckBoxList: bindCheckBoxList,
		initCheckBoxList: initCheckBoxList,
		htmlEncode: htmlEncode,
		htmlDecode: htmlDecode,
		truncateString: truncateString,
		isStringHasScriptTag: isStringHasScriptTag,
		isFunction: isFunction,
		setCookie: setCookie,
		readCookie: readCookie,
		eraseCookie: eraseCookie,
		initBootstrapTabs: initBootstrapTabs,
		activateBootstrapTab: activateBootstrapTab,
		executePartialViewsScriptTags: executePartialViewsScriptTags,
		getCurrentLang: getCurrentLang,
		moveTableRow: moveTableRow,
		swapElements: swapElements,
		ajaxAppendPartial: ajaxAppendPartial,
		initUploaderInfoTooltip: initUploaderInfoTooltip,
		setCurrentCulture: setCurrentCulture,
		showSuccessInlineAlert: showSuccessInlineAlert,
		showErrorInlineAlert: showErrorInlineAlert,
		showInformationInlineAlert: showInformationInlineAlert,
		showWarningInlineAlert: showWarningInlineAlert,
		animateTo: animateTo,
		animateToFirstValidationError: animateToFirstValidationError,
		ajaxSubmitForm: ajaxSubmitForm,
		redirectToUrl: redirectToUrl,
		initBtnBack: initBtnBack,
		initMvcFormPage: initMvcFormPage,
		diffDates: diffDates,
		tryDiffDates: tryDiffDates,
		getRandomString: getRandomString
	};
	return toReturn;
})();


// ---------------------------------- Validation Attributes for custom validations ------------------------------------------- //



//if (jQuery.validator && jQuery.validator.unobtrusive) {

//	// enable validaitions on specific input hidden fields too!

//	$.validator.setDefaults({ ignore: ':hidden:not(.do-not-ignore-validation)' });

//	//must be true attribute client validation
//	if (jQuery.validator && jQuery.validator.unobtrusive) {
//		//must be true attribute
//		jQuery.validator.unobtrusive.adapters.addBool("mustbetrue");

//		jQuery.validator.addMethod('mustbetrue', function (value, element, params) {
//			return element.checked;
//		}, '');
//	}

//	// date validation to support ummalqura and also gregorian dates
//	jQuery.validator.methods.date = function (value, element) {

//		var ele = $(element);
	   
//		var widSettings = {
//			dateFormat: "dd/mm/yyyy",
//			lang: "ar",
//			calendarType: ele.parents('.datePickerContainer').find('select').val() || 'gregorian'
//		};        

//		var calendar = $.calendars.instance(widSettings.calendarType, widSettings.lang);
//		calendar.dateFormat = widSettings.dateFormat;

//		try {
//			calendar.parseDate(widSettings.dateFormat, ele.val());
//			return true;
//		} catch (e) {
//			return false;
//		}
//	};


//	// RequiredAttribute with AllowEmptyString=true in ASP.NET MVC unobtrusive validation

//	jQuery.validator.methods.oldRequired = jQuery.validator.methods.required;

//	jQuery.validator.addMethod("required", function (value, element, param) {
//			if ($(element).attr('data-val-required-allowempty') === 'true') {
//				return true;
//			} else if ($.trim(value) === '') {
//				return false;
//			}

//			return jQuery.validator.methods.oldRequired.call(this, value, element, param);
//		},
//		jQuery.validator.messages.required // use default message
//	);


//	// IsDateAfterAttribute Attribute

//	jQuery.validator.unobtrusive.adapters.add('isdateafter',
//		['propertytested', 'allowequaldates'], function (options) {
//			// simply pass the options.params here
//			options.rules['isdateafter'] = options.params;
//			options.messages['isdateafter'] = function () { return $(options.element).attr('data-val-isdateafter'); };//options.message;
//		});

//	jQuery.validator.addMethod('isdateafter', function (value, element, params) {

//		var parts = element.name.split(".");
//		var prefix = "";

//		if (parts.length > 1)
//			prefix = parts[0] + ".";

//		var startDateElement =  $('input[name="' + prefix + params.propertytested + '"]');
//		var endDateElement = $(element);

//		var startDateWidgetSettings = JSON.parse(startDateElement.attr('data-widget-settings'));
//		var endDateWidgetSettings = JSON.parse(endDateElement.attr('data-widget-settings'));

//		var startdatevalue = startDateElement.val();
//		if (!value || !startdatevalue)
//			return true;
		
//		var isValid = true;
		
//		if(params.allowequaldates === 'True') {
//			//isValid = Date.parse(startdatevalue) <= Date.parse(value);
//		   var dateDiffInDays = $commons.diffDates(startdatevalue, startDateWidgetSettings, value, endDateWidgetSettings);
//		   isValid = dateDiffInDays >= 0;

//		}  else{
//			//isValid = Date.parse(startdatevalue) < Date.parse(value);
//			var dateDiffInDays = $commons.diffDates(startdatevalue, startDateWidgetSettings, value, endDateWidgetSettings);
//			isValid = dateDiffInDays > 0;
//		}

//		return isValid;
			

//	}, '');

//// DateRestrictionAttribute

//	jQuery.validator.unobtrusive.adapters.add('daterestriction',
//		['todaydate', 'daterestrictiontype'], function (options) {
//			// simply pass the options.params here
//			options.rules['daterestriction'] = options.params;
//			options.messages['daterestriction'] = function () { return $(options.element).attr('data-val-daterestriction'); };//options.message;
//		});

//	jQuery.validator.addMethod('daterestriction', function (value, element, params) {

//		var todayDate = params.todaydate;
//		var selectedDate = value;
		
//		if (!value || !todayDate)
//			return true;

//		var isValid = false;
//		var calendarType = $(element).parents('.datePickerContainer').find('select').val() || 'gregorian';
//		var dateDiffInDays = $commons.tryDiffDates(todayDate, 'gregorian', selectedDate, calendarType);

//		switch (params.daterestrictiontype) {

//			case "futureOnly":                
//				isValid = dateDiffInDays > 0;
//				break;

//			case "futureIncludingToday":
//				if (dateDiffInDays >= 0) {
//					isValid = true; 
//				}
//				break;

//			case "pastOnly":
//				isValid = dateDiffInDays < 0;
//				break;

//			case "pastIncludingToday":
//				if (dateDiffInDays <= 0) {
//					isValid = true;
//				}
//				break;

//			default:
//				isValid = true;
//				break;
//		}
		
//		return isValid;


//	}, '');


//// GreaterThanAttribute Attribute

//jQuery.validator.unobtrusive.adapters.add('greaterthan',
//['propertytested', 'allowequalvalues'], function (options) {
//	// simply pass the options.params here
//	options.rules['greaterthan'] = options.params;
//	options.messages['greaterthan'] = function () { return $(options.element).attr('data-val-greaterthan'); };//options.message;
//});

//jQuery.validator.addMethod('greaterthan', function (value, element, params) {

//	if (value.length === 0 && this.optional(element)) {
//		return true;
//	}

//	if (isNaN(parseInt(value))) {
//		amountNumberValidationError = $sharedResources.NumberOnlyErrorMessage;
//		return false;
//	}

//	var parts = element.name.split(".");
//	var prefix = "";

//	if (parts.length > 1)
//		prefix = parts[0] + ".";

//	var minValElemnet =  $('input[name="' + prefix + params.propertytested + '"]');
//	var maxValElement = $(element);

//	var minValue = minValElemnet.val();

//	if (!value || !minValue)
//		return true;

//	var isValid = true;

//	if(params.allowequalvalues === 'True') {
//	isValid = value >= minValue;

//	}  else{
//		isValid = value > minValue;
//	}

//	return isValid;
	

//}, '');


//}

$(function () {
	$commons.initMvcFormPage();

	if ($("#divOverlay").length) {
		$("#divOverlay").remove();
	}

	//hide empty notifications
	//workaround!
	if ($('.alert-danger ul li').length && $('.alert-danger ul li').html() === '') {
		$('.alert-danger').hide();
	}
});

window.parseBoolean = function (string) {

    var bool = (function () {
		switch (false) {
			case string.toLowerCase() !== 'true':
				return true;
			case string.toLowerCase() !== 'false':
				return false;
		}
	})();
	if (typeof bool === "boolean") {
		return bool;
	}
	return void 0;
};