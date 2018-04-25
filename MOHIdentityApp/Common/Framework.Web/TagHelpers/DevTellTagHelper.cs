// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using Framework.Core.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Framework.Web.TagHelpers
{
    /// <summary>
    /// <see cref="ITagHelper"/> implementation targeting &lt;input&gt; elements with an <c>asp-for</c> attribute.
    /// </summary>
    [HtmlTargetElement("dev-telinputfor", Attributes = ForAttributeName)]
    public class DevTellTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-for";
        private const string Settings = "tel-settings";
       // private const string HTMLattributesName = "html-attributes";


        /// <summary>
        /// Creates a new <see cref="InputTagHelper"/>.
        /// </summary>
        /// <param name="generator">The <see cref="IHtmlGenerator"/>.</param>
        public DevTellTagHelper(IScriptManager scriptManager, IHtmlGenerator generator, ICssManager cssManager)
        {
            _scriptManager = scriptManager;
            _cssManager = cssManager;
            Generator = generator;
        }


        private readonly ICssManager _cssManager;
        private readonly IScriptManager _scriptManager;
        protected IHtmlGenerator Generator { get; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        ///// <summary>
        ///// An expression to be evaluated against the current model.
        ///// </summary>
        //[HtmlAttributeName(HTMLattributesName)]
        //public IDictionary<string, object> HTMLAttributes { get; set; } = null;

        /// <summary>
        /// An expression to be evaluated against the current model.
        /// </summary>
        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }


        /// <summary>
        /// The name of the &lt;input&gt; element.
        /// </summary>
        /// <remarks>
        /// Passed through to the generated HTML in all cases. Also used to determine whether <see cref="For"/> is
        /// valid with an empty <see cref="ModelExpression.Name"/>.
        /// </remarks>
        public string Name { get; set; }

        [HtmlAttributeName(Settings)]
        public TelInputSettings TellSettings { get; set; }

        /// <inheritdoc />
        /// <remarks>Does nothing if <see cref="For"/> is <c>null</c>.</remarks>
        /// <exception cref="InvalidOperationException">
        /// Thrown if <see cref="Format"/> is non-<c>null</c> but <see cref="For"/> is <c>null</c>.
        /// </exception>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            TellSettings = TellSettings ?? new TelInputSettings();
            var modelExplorer = For.ModelExplorer;

            // to read Tell setting
            if (TellSettings.PreferredCountries != null && TellSettings.PreferredCountries.Contains(","))
            {
                var countries = TellSettings.PreferredCountries.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();
                TellSettings.PreferredCountries = countries.ToJson();
            }

            // to link files with control
            _cssManager.AddCss(new LinkReference("/EmbeddedResources/Widgets/v1/TelInput/css/intlTelInput.css"));
            _scriptManager.AddScript(new ScriptReference("/EmbeddedResources/Widgets/v1/TelInput/js/intlTelInput.min.js"));
            _scriptManager.AddScript(new ScriptReference("/EmbeddedResources/Widgets/v1/TelInput/js/libphonenumber.utils.js"));
            _scriptManager.AddScript(new ScriptReference("/EmbeddedResources/Widgets/v1/TelInput/IntTelInputWidget.js"));


            // Ensure Generator does not throw due to empty "fullName" if user provided a name attribute.
            IDictionary<string, object> htmlAttributes = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            // compy all attribute from control
            foreach (var attribute in output.Attributes)
                htmlAttributes.Add(attribute.Name, attribute.Value);

            
            TagBuilder tagBuilder; 

            // genrate hidden field with his attribute
            tagBuilder = GenerateHidden(modelExplorer, htmlAttributes);
            var sb = new StringBuilder();
            var clientId = "phoneNumInput".AppendRandomString(4, true);
            TellSettings.ContainerId = clientId;
            tagBuilder.AddCssClass("do-not-ignore-validation");
            AppendTell2Html(sb, clientId, tagBuilder);


            // genrate Text field with his attribute
            tagBuilder = GenerateTextBox(modelExplorer, htmlAttributes);
            tagBuilder.MergeAttribute("data-widget-settings", TellSettings.ToJson());
            tagBuilder.MergeAttribute("value", string.Empty);
            // In the international telephone network, the format of telephone numbers is standardized by ITU-T recommendation E.164. This code specifies that the entire number should be 15 digits or shorter
            // max length of any international number 15
            tagBuilder.MergeAttribute("maxlength", "15");
            tagBuilder.MergeAttribute("style", "direction:ltr;", false);
            tagBuilder.AddCssClass("phone-number-input");
            tagBuilder.Attributes.Remove("name");



            // control continer
            output.PreElement.SetHtmlContent($@"<div id=""{clientId}"" class=""phoneInputContainer"" style=""direction:ltr;"">");
            output.PostElement.SetHtmlContent($@"</div>");

            // to remove taghelper from main tag
            output.Attributes.RemoveAll("class");


            // to append text box to string builder
            AppendTell2Html(sb, clientId, tagBuilder);
            

            output.Content.AppendHtml(sb.ToString());

            var initScript =
                  $@" var telInputWidLoader_{clientId} = new IntTelInputWidget({TellSettings.ToJson()});
                   $(function () {{ telInputWidLoader_{clientId}.setNumber('{For.ModelExplorer.Model}'); }}); ";

            _scriptManager.AddScriptText(initScript);

        }

        /// <summary>
        /// to create string builder
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="className"></param>
        /// <param name="inputBuilder"></param>
        private void AppendTell2Html(StringBuilder sb, string className, TagBuilder inputBuilder)
        {
            Contract.Requires(sb != null && inputBuilder != null && !string.IsNullOrEmpty(className));

            using (var stringWriter = new StringWriter())
            {
                inputBuilder.WriteTo(stringWriter, HtmlEncoder.Default);
                sb.AppendLine(stringWriter.ToString());
            }
        }



        // Imitate Generator.GenerateHidden() using Generator.GenerateTextBox(). This adds support for asp-format that
        // is not available in Generator.GenerateHidden().
        private TagBuilder GenerateTextBox(
            ModelExplorer modelExplorer, IDictionary<string, object> htmlAttributes)
        {
            var value = For.Model;
            if (value is byte[] byteArrayValue)
            {
                value = Convert.ToBase64String(byteArrayValue);
            }

            if (htmlAttributes == null)
            {
                htmlAttributes = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            }

            // In DefaultHtmlGenerator(), GenerateTextBox() calls GenerateInput() _almost_ identically to how
            // GenerateHidden() does and the main switch inside GenerateInput() handles InputType.Text and
            // InputType.Hidden identically. No behavior differences at all when a type HTML attribute already exists.
            htmlAttributes.Remove("type");
            htmlAttributes.Add("type", "tel");
            return Generator.GenerateTextBox(
                 ViewContext,
                 modelExplorer,
                 For.Name,
                 modelExplorer.Model,
                 modelExplorer.Metadata.EditFormatString,
                 htmlAttributes);
        }

        /// <summary>
        /// Generate Hidden
        /// </summary>
        /// <param name="modelExplorer"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        private TagBuilder GenerateHidden(
            ModelExplorer modelExplorer,
            
            IDictionary<string, object> htmlAttributes)
        {


            if (htmlAttributes == null)
            {
                htmlAttributes = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            }
            htmlAttributes.Remove("type");
            htmlAttributes["type"] = "hidden";
            return Generator.GenerateHidden(
                 ViewContext,
                 modelExplorer,
                 For.Name,
                 modelExplorer.Model,
                 true,
                 htmlAttributes);
        }


    }
}