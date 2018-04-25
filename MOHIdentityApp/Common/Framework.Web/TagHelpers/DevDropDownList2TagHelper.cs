
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
    [HtmlTargetElement("dev-dropdownlist2for")]
    public class DevDropDownList2TagHelper : TagHelper
    {
        private readonly IScriptManager _scriptManager;
        private readonly IHtmlGenerator _generator;
        private readonly ICssManager _cssManager;


        public DevDropDownList2TagHelper(IScriptManager scriptManager, ICssManager cssManager, IHtmlGenerator generator)
        {
            _scriptManager = scriptManager;
            _cssManager = cssManager;
            _generator = generator;
        }
        
        private const string SelectPropertyName = "select-property";
        private const string Settings = "select-settings";
        private const string SelectList = "select-list";
        private const string OptionLabel = "option-label";
        private const string IsMulti = "Is-Multi";

        [HtmlAttributeName(SelectPropertyName)]
        public ModelExpression SelectProperty { get; set; }


        [HtmlAttributeName(SelectList)]
        public IEnumerable<SelectListItem> selectList { get; set; }

        [HtmlAttributeName(OptionLabel)]
        public string OptionLabelProperty { get; set; }

        [HtmlAttributeName(IsMulti)]
        public bool IsMultiProperty { get; set; } = false;

        [HtmlAttributeName(Settings)]
        public DropDownWidgetSettings Select2Settings { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;
            var clientId = "select2".AppendRandomString(4, true);

            Select2Settings = Select2Settings ?? new DropDownWidgetSettings();

            if (SelectProperty != null)
            {
                
                var inputBuilder =
                    _generator.GenerateSelect(
                        ViewContext,
                        SelectProperty.ModelExplorer,
                        OptionLabelProperty,
                        SelectProperty.Name,
                        selectList,
                        IsMultiProperty,
                        null);

                var key = SelectProperty.Name.ToLower();
                var className = $"simple-select2 w-100";

                Select2Settings.SelectId = clientId;

                //inputBuilder.AddCssClass("select2-selection__rendered");

                MergeClassAttributes(className, context, inputBuilder);

                if (inputBuilder.Attributes.ContainsKey("id"))
                {
                    inputBuilder.Attributes["id"] = clientId;
                }
                else
                {
                    inputBuilder.Attributes.Add("id", clientId);
                }

               

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

                var sb = new StringBuilder();

                AppendSelect2Html(sb, clientId, inputBuilder);
                

                output.Content.AppendHtml(sb.ToString());


            }

            

            _cssManager.AddCss(new LinkReference("/EmbeddedResources/Widgets/v1/Dropdown/css/select2.min.css"));
            _cssManager.AddCss(new LinkReference("/EmbeddedResources/Widgets/v1/Dropdown/css/select2-bootstrap4.min.css"));
            _scriptManager.AddScript(new ScriptReference("/EmbeddedResources/Widgets/v1/Dropdown/js/select2.full.min.js"));
            _scriptManager.AddScript(new ScriptReference("/EmbeddedResources/Widgets/v1/Dropdown/DropDownWidget.js"));

            var initScript =
               $@"$(function () {{ 
                        var select2WidLoader_{clientId} = new DropDownWidget('{
                   clientId},'+'{OptionLabelProperty}');
                        select2WidLoader_{clientId
                   }.load(); 
                    }});";
            
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

        private void AppendSelect2Html(StringBuilder sb, string className, TagBuilder inputBuilder)
        {
            Contract.Requires(sb != null && inputBuilder != null && !string.IsNullOrEmpty(className));

            using (var stringWriter = new StringWriter())
            {
                inputBuilder.WriteTo(stringWriter, HtmlEncoder.Default);
                sb.AppendLine(stringWriter.ToString());
            }
        }

      

    }
}

