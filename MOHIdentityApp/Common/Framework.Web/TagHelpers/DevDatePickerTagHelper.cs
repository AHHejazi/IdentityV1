using System.Collections.Generic;
using System.Threading.Tasks;
using Framework.Core.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.AspNetCore.Razor.TagHelpers;

using Microsoft.AspNetCore.Mvc.TagHelpers;
using static System.String;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using Framework.Core.Resources;

namespace Framework.Web.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("dev-datepicker")]
    public class DevDatePickerTagHelper : TagHelper
    {
        private readonly IScriptManager _scriptManager;
        private readonly IHtmlGenerator _generator;
        private readonly ICssManager _cssManager;


        public DevDatePickerTagHelper(IScriptManager scriptManager, ICssManager cssManager, IHtmlGenerator generator)
        {
            _scriptManager = scriptManager;
            _cssManager = cssManager;
            _generator = generator;
        }
        
        private const string DateTimePropertyName = "date-property";
        private const string Settings = "date-picker-settings";


        [HtmlAttributeName(DateTimePropertyName)]
        public ModelExpression DateTimeProperty { get; set; }


        [HtmlAttributeName(Settings)]
        public DatePickerSettings DatePickerSettings { get; set; }
    
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            var clientId = "datePickerInput".AppendRandomString(4, true);

            DatePickerSettings = DatePickerSettings ?? new DatePickerSettings();

            if (DateTimeProperty != null)
            {
                var inputBuilder = 
                    _generator.GenerateTextBox(
                        ViewContext, 
                        DateTimeProperty.ModelExplorer,
                        DateTimeProperty.Name, 
                        DateTimeProperty.ModelExplorer.Model, 
                        null, 
                        null);

                var key = DateTimeProperty.Name.ToLower();
                var className = $"datepicker-{key}";

                inputBuilder.MergeAttribute("data-widget-settings", DatePickerSettings.ToJson());
                inputBuilder.AddCssClass("date-picker-input");


                // compy all attribute from control
                foreach (var attribute in output.Attributes)
                {
                    if (attribute.Name.ToLower().Equals("class"))
                    {
                        inputBuilder.AddCssClass(attribute.Value.ToString());
                    }
                    else
                    {
                        inputBuilder.Attributes.Add(attribute.Name, attribute.Value.ToString());
                    }
                    
                }


                // to remove taghelper from main tag
                output.Attributes.RemoveAll("class");

                MergeClassAttributes(className, context, inputBuilder);

                var sb = new StringBuilder();

                AppendDatepickerHtml(sb, className, inputBuilder);

                DatePickerSettings.ContainerId = clientId;


                //sb.AppendLine("<script type=\"text/javascript\">");
                //sb.AppendLine("$(function (){");
                //sb.AppendLine($"$('#{className}').datetimepicker({{");
                //if (!string.IsNullOrEmpty(DatePickerSettings.DateFormat))
                //{
                //    sb.AppendLine($"format: '{DatePickerSettings.DateFormat}',");
                //}
                //sb.AppendLine("});");
                //sb.AppendLine("});");
                //sb.AppendLine("</script>");

                output.Content.AppendHtml(sb.ToString());


            }


            output.PreElement.SetHtmlContent($@"<div id=""{clientId}"" class=""row datePickerContainer"">
                        <div class=""form-group col-xs-9"">");

            output.PostElement.SetHtmlContent($@"</div>                
                         <div class=""form-group col-xs-3"" style=""padding-right: 0;"">
                             <select id=""datePickerSelect_{clientId}"" class=""date-picker-select form-control"">
                                 <option value=""gregorian"">{Messages.Gregorian}</option>  
                                 <option value=""ummalqura"">{Messages.UmmAlqura}</option>   
                             </select>
                         </div>                
                   </div>");



            _cssManager.AddCss(new LinkReference("/EmbeddedResources/Widgets/v1/DatePicker/css/jquery-ui.css"));
            _cssManager.AddCss(new LinkReference("/EmbeddedResources/Widgets/v1/DatePicker/css/jquery.calendars.picker.css"));
            _cssManager.AddCss(new LinkReference("/EmbeddedResources/Widgets/v1/DatePicker/css/ui-hot-sneaks.calendars.picker.css"));
            _cssManager.AddCss(new LinkReference("/EmbeddedResources/Widgets/v1/DatePicker/css/highlight.css"));
            _scriptManager.AddScript(new ScriptReference("/EmbeddedResources/Widgets/v1/DatePicker/js/jquery.plugin.js"));
            _scriptManager.AddScript(new ScriptReference("/EmbeddedResources/Widgets/v1/DatePicker/js/jquery.calendars.js"));
            _scriptManager.AddScript(new ScriptReference("/EmbeddedResources/Widgets/v1/DatePicker/js/jquery.calendars-ar.js"));
            _scriptManager.AddScript(new ScriptReference("/EmbeddedResources/Widgets/v1/DatePicker/js/jquery.calendars.ummalqura.js"));
            _scriptManager.AddScript(new ScriptReference("/EmbeddedResources/Widgets/v1/DatePicker/js/jquery.calendars.ummalqura-ar.js"));
            _scriptManager.AddScript(new ScriptReference("/EmbeddedResources/Widgets/v1/DatePicker/js/jquery.calendars.plus.js"));
            _scriptManager.AddScript(new ScriptReference("/EmbeddedResources/Widgets/v1/DatePicker/js/jquery.calendars.picker.js"));
            _scriptManager.AddScript(new ScriptReference("/EmbeddedResources/Widgets/v1/DatePicker/js/jquery.calendars.picker-ar.js"));
            _scriptManager.AddScript(new ScriptReference("/EmbeddedResources/Widgets/v1/DatePicker/DatePickerWidget.js"));

            var initScript = $@"$(function () {{ 
                        var datePickerWidLoader_{clientId} = new DatePickerWidget({DatePickerSettings.ToJson()});
                        datePickerWidLoader_{clientId}.load(); }});";

            _scriptManager.AddScriptText(initScript);


        }
        private void MergeClassAttributes(string className, TagHelperContext context, TagBuilder inputBuilder)
        {
            Contract.Requires(context != null && inputBuilder != null && !string.IsNullOrEmpty(className));

            var classes = className;
            if (context.AllAttributes.TryGetAttribute("class", out TagHelperAttribute classTag))
            {
                classes = $"{classes} {classTag.Value}";
            }

            if (inputBuilder.Attributes.ContainsKey("class"))
            {
                inputBuilder.Attributes["class"] = $"{inputBuilder.Attributes["class"]} {classes}";
            }
            else
            {
                inputBuilder.Attributes.Add("class", classes);
            }
        }

        private void AppendDatepickerHtml(StringBuilder sb, string className, TagBuilder inputBuilder)
        {
            Contract.Requires(sb != null && inputBuilder != null && !string.IsNullOrEmpty(className));

            using (var stringWriter = new StringWriter())
            {
                inputBuilder.WriteTo(stringWriter, HtmlEncoder.Default);
                sb.AppendLine($"<div class='input-group date' id='{className}'>");
                sb.AppendLine(stringWriter.ToString());
                sb.AppendLine("<span class=\"input-group-addon\">");
                sb.AppendLine("<span class=\"glyphicon glyphicon-calendar\"></span>");
                sb.AppendLine("</span>");
                sb.AppendLine("</div>");
            }
        }

        public enum CalendarRange
        {
            /// <summary>
            ///     The FutureOnly.
            /// </summary>
            FutureOnly = 100,

            /// <summary>
            ///     The TodayAndFutureOnly.
            /// </summary>
            TodayAndFutureOnly = 101,

            /// <summary>
            ///     The TodayAndPastOnly.
            /// </summary>
            TodayAndPastOnly = 102,
            /// <summary>
            ///     The PastOnly.
            /// </summary>
            PastOnly = 103,

            /// <summary>
            ///     The ViewBoth.
            /// </summary>
            ViewBoth = 104,

            /// <summary>
            ///     The SpecificDates.
            /// </summary>
            SpecificDates = 105
        }


    }
}
