using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Framework.Core.Mvc.DataAnnotations
{
    public class RequiredNoSpaceAttribute : ValidationAttribute
    {
        public RequiredNoSpaceAttribute()
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name);
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(value.ToString()))
            {
                return false;
            }

            if (value.GetType() != typeof(string))
            {
                throw new InvalidOperationException("can only be used on boolean properties.");
            }

            return (bool)value;
        }
    }

    public class RequiredNoSpaceAttributeAdapter : AttributeAdapterBase<RequiredNoSpaceAttribute>
    {
        public RequiredNoSpaceAttributeAdapter(RequiredNoSpaceAttribute attribute, IStringLocalizer stringLocalizer)
            : base(attribute, stringLocalizer)
        {
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-required", GetErrorMessage(context));
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            if (validationContext == null)
            {
                throw new ArgumentNullException(nameof(validationContext));
            }

            return GetErrorMessage(validationContext.ModelMetadata, validationContext.ModelMetadata.GetDisplayName());
        }
    }
}