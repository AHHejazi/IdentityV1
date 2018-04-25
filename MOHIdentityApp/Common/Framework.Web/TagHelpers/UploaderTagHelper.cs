// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using Framework.Core.Extensions;
using Framework.Core.Resources;
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
    [HtmlTargetElement("dev-uploaderinputfor", Attributes = ForAttributeName)]
    public class UploaderTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-for";
        private const string FileIdName = "fileId";
        private const string FileURL = "fileurl";

        // Mapping from datatype names and data annotation hints to values for the <input/> element's "type" attribute.
        private static readonly Dictionary<string, string> _defaultInputTypes =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { nameof(IFormFile), "file" },
                { TemplateRenderer.IEnumerableOfIFormFileName, "file" },
            };

        /// <summary>
        /// Creates a new <see cref="InputTagHelper"/>.
        /// </summary>
        /// <param name="generator">The <see cref="IHtmlGenerator"/>.</param>
        public UploaderTagHelper(IScriptManager scriptManager, IHtmlGenerator generator, ICssManager cssManager)
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

        /// <summary>
        /// An expression to be evaluated against the current model.
        /// </summary>
        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        [HtmlAttributeName("type")]
        public string InputTypeName { get; set; } 

        [HtmlAttributeName(FileURL)]
        public string DownloadFileUrl { get; set; } = "/Files/Download/?attId=";

        /// <summary>
        /// The name of the &lt;input&gt; element.
        /// </summary>
        /// <remarks>
        /// Passed through to the generated HTML in all cases. Also used to determine whether <see cref="For"/> is
        /// valid with an empty <see cref="ModelExpression.Name"/>.
        /// </remarks>
        public string Name { get; set; }

        /// <summary>
        /// The value of the &lt;input&gt; element.
        /// </summary>
        /// <remarks>
        /// Passed through to the generated HTML in all cases. Also used to determine the generated "checked" attribute
        /// if <see cref="InputTypeName"/> is "radio". Must not be <c>null</c> in that case.
        /// </remarks>
        public string Value { get; set; }


        [HtmlAttributeName(FileIdName)]
        public object FileId { get; set; }

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
            output.TagName = null;
            var clientId = "fileUploadInput".AppendRandomString(4, true);
            var containerId = clientId;

            if (FileId.IsNullOrDefault<string>() || FileId.IsNullOrDefault<int>() || FileId.IsNullOrDefault<Guid>())
            {
                FileId = null;
            }


            // Pass through attributes that are also well-known HTML attributes. Must be done prior to any copying
            // from a TagBuilder.
            if (InputTypeName != null)
            {
                output.CopyHtmlAttribute("type", context);
            }

            if (Name != null)
            {
                output.CopyHtmlAttribute(nameof(Name), context);
            }

            if (Value != null)
            {
                output.CopyHtmlAttribute(nameof(Value), context);
            }

            // Note null or empty For.Name is allowed because TemplateInfo.HtmlFieldPrefix may be sufficient.
            // IHtmlGenerator will enforce name requirements.
            var metadata = For.Metadata;
            var modelExplorer = For.ModelExplorer;
            if (metadata == null)
            {
                throw new InvalidOperationException();
            }

            string inputType;
            string inputTypeHint;
            if (string.IsNullOrEmpty(InputTypeName))
            {
                // Note GetInputType never returns null.
                inputType = GetInputType(modelExplorer, out inputTypeHint);
            }
            else
            {
                inputType = InputTypeName.ToLowerInvariant();
                inputTypeHint = null;
            }

            // inputType may be more specific than default the generator chooses below.
            if (!output.Attributes.ContainsName("type"))
            {
                output.Attributes.SetAttribute("type", inputType);
            }

            // Ensure Generator does not throw due to empty "fullName" if user provided a name attribute.
            IDictionary<string, object> htmlAttributes = null;
            if (string.IsNullOrEmpty(For.Name) &&
                string.IsNullOrEmpty(ViewContext.ViewData.TemplateInfo.HtmlFieldPrefix) &&
                !string.IsNullOrEmpty(Name))
            {
                htmlAttributes = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                {
                    { "name", Name },
                };
            }

            TagBuilder tagBuilder;

            tagBuilder = GenerateTextBox(modelExplorer, inputTypeHint, inputType, htmlAttributes);
            tagBuilder.GenerateId("id", context.UniqueId);
            tagBuilder.AddCssClass("file-uploader-input");
            tagBuilder.AddCssClass("form-control");

            // var safeValidationAttrCollection = validationAttributes
            var isImage = tagBuilder.Attributes.GetValueOrDefault("data-val-checkisvalidfile-isimage").To<bool>();
            var imageMaxHeight = tagBuilder.Attributes.GetValueOrDefault("data-val-checkisvalidfile-imagemaxheight")
                .To<int>();
            var imageMaxWidth = tagBuilder.Attributes.GetValueOrDefault("data-val-checkisvalidfile-imagemaxwidth")
                .To<int>();
            var allowedExtensions = tagBuilder.Attributes.GetValueOrDefault("data-val-checkisvalidfile-allowedextensions").To<string>();
            var maxSizeInMegabytes = tagBuilder.Attributes.GetValueOrDefault("data-val-checkisvalidfile-maxsizeinmegabytes").To<int>();

            var uploaderSettings = new UploaderSettings
            {
                IsImage = isImage,
                ImageMaxWidth = imageMaxWidth,
                ImageMaxHeight = imageMaxHeight,
                MaxSizeInMegabytes = maxSizeInMegabytes,
                AllowedExtensions = allowedExtensions,
                ContainerId = containerId,
                DownloadFileUrl = DownloadFileUrl,
                FileId = FileId
            };

            _scriptManager.AddScript(new ScriptReference("/EmbeddedResources/Widgets/v1/Uploader/FileUploaderWidget.js"));
            

            var initScript =
               $@" var fileUploadWidLoader_{clientId} = new FileUploaderWidget({uploaderSettings.ToJson()});
                    $(function () {{ fileUploadWidLoader_{clientId}.load();}}); ";

            _scriptManager.AddScriptText(initScript);

            var imgMsg = !isImage
                             ? string.Empty
                             : $@"<p><span class=""icon-angle-arrow-down"" aria-hidden=""true""></span>{
                                     Messages.MaxImageHeight
                                 }<b><span>{imageMaxHeight}</span></b> px</p>
                            <p>
                                <span class=""icon-angle-arrow-down"" aria-hidden=""true""></span>{
                                     Messages.MaxImageWidth
                                 }<b><span>{imageMaxWidth}</span></b> px
                            </p>";

            var uploaderOptionalMsg = FileId == null
                                          ? string.Empty
                                          : $@"<p class=""UploaderNotes"">{Messages.UploaderOptionalInEditMode}</p>";

            var downloadFileLink = FileId == null
                                       ? string.Empty
                                       : $@"<p><a title=""{Messages.DownloadFile}"" href=""{DownloadFileUrl}{FileId}"">{
                                               Messages.DownloadFile
                                           }</a></p>";

            var msgHtml = $@"
                    <p>
                        <span class=""icon-angle-arrow-down"" aria-hidden=""true""></span>
                                {Messages.AllowedExtensions} <b><span>{allowedExtensions}</span></b>
                    </p>
                    <p>
                        <span class=""icon-angle-arrow-down"" aria-hidden=""true""></span>
                                {Messages.MaxFileSize} <b><span>{maxSizeInMegabytes}</span></b> {Messages.MB}
                     </p>
                    {imgMsg}";

            
            
            var sb = new StringBuilder();


            output.PreElement.SetHtmlContent($@"<span class=""icon-help upload-terms-icn""></span>");

            output.PostElement.SetHtmlContent($@"
                <div id=""{clientId}"" class=""uploaderInputContainer upload-terms"">{msgHtml}</div>
                <div class=""download-file-container"">{uploaderOptionalMsg}{downloadFileLink}</div>");

            // to remove taghelper from main tag
            output.Attributes.RemoveAll("class");


            // to append text box to string builder
            AppendTell2Html(sb, clientId, tagBuilder);


            output.Content.AppendHtml(sb.ToString());
            
        }

        /// <summary>
        /// Gets an &lt;input&gt; element's "type" attribute value based on the given <paramref name="modelExplorer"/>
        /// or <see cref="InputType"/>.
        /// </summary>
        /// <param name="modelExplorer">The <see cref="ModelExplorer"/> to use.</param>
        /// <param name="inputTypeHint">When this method returns, contains the string, often the name of a
        /// <see cref="ModelMetadata.ModelType"/> base class, used to determine this method's return value.</param>
        /// <returns>An &lt;input&gt; element's "type" attribute value.</returns>
        protected string GetInputType(ModelExplorer modelExplorer, out string inputTypeHint)
        {
            foreach (var hint in GetInputTypeHints(modelExplorer))
            {
                if (_defaultInputTypes.TryGetValue(hint, out var inputType))
                {
                    inputTypeHint = hint;
                    return inputType;
                }
            }

            inputTypeHint = InputType.Text.ToString().ToLowerInvariant();
            return inputTypeHint;
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



        private TagBuilder GenerateTextBox(
            ModelExplorer modelExplorer,
            string inputTypeHint,
            string inputType,
            IDictionary<string, object> htmlAttributes)
        {
            
            var format = GetFormat(modelExplorer);

            if (htmlAttributes == null)
            {
                htmlAttributes = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            }

            htmlAttributes["type"] = inputType;
            if (string.Equals(inputType, "file") &&
                string.Equals(
                    inputTypeHint,
                    TemplateRenderer.IEnumerableOfIFormFileName,
                    StringComparison.OrdinalIgnoreCase))
            {
                htmlAttributes["multiple"] = "multiple";
            }

            return Generator.GenerateTextBox(
                ViewContext,
                modelExplorer,
                For.Name,
                modelExplorer.Model,
                format,
                htmlAttributes);
        }


        // Get a fall-back format based on the metadata.
        private string GetFormat(ModelExplorer modelExplorer)
        {
            string format;
            // Otherwise use EditFormatString, if any.
            format = modelExplorer.Metadata.EditFormatString;
            return format;
        }

        // A variant of TemplateRenderer.GetViewNames(). Main change relates to bool? handling.
        private static IEnumerable<string> GetInputTypeHints(ModelExplorer modelExplorer)
        {
            if (!string.IsNullOrEmpty(modelExplorer.Metadata.TemplateHint))
            {
                yield return modelExplorer.Metadata.TemplateHint;
            }

            if (!string.IsNullOrEmpty(modelExplorer.Metadata.DataTypeName))
            {
                yield return modelExplorer.Metadata.DataTypeName;
            }

            // In most cases, we don't want to search for Nullable<T>. We want to search for T, which should handle
            // both T and Nullable<T>. However we special-case bool? to avoid turning an <input/> into a <select/>.
            var fieldType = modelExplorer.ModelType;
            if (typeof(bool?) != fieldType)
            {
                fieldType = modelExplorer.Metadata.UnderlyingOrModelType;
            }

            foreach (var typeName in TemplateRenderer.GetTypeNames(modelExplorer.Metadata, fieldType))
            {
                yield return typeName;
            }
        }
    }
}