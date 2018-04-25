using Framework.Core.Extensions;
using Framework.Core.Utils;

using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Framework.Core.Mvc.DataAnnotations
{
    public enum DateRestrictionType
    {
        FutureOnly,
        FutureIncludingToday,
        PastOnly,
        PastIncludingToday
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class DateRestrictionAttribute : ValidationAttribute
    {
        public DateRestrictionType DateRestrictionType { get; set; }

        public bool ValidateClientSideOnly { get; set; }


        /// <summary>
        /// The is valid.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="validationContext">
        /// The validation context.
        /// </param>
        /// <returns>
        /// The <see cref="ValidationResult"/>.
        /// </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (this.ValidateClientSideOnly)
            {
                return ValidationResult.Success;
            }

            if (!(value is DateTime))
            {
                return ValidationResult.Success;
            }

            var selectedDate = value.To<DateTime>().Date;
            var today = DateTime.Now.Date;

            switch (DateRestrictionType)
            {
                case DateRestrictionType.FutureOnly:
                    if (selectedDate > today)
                    {
                        return ValidationResult.Success;
                    }
                    break;
                case DateRestrictionType.FutureIncludingToday:
                    if (selectedDate >= today)
                    {
                        return ValidationResult.Success;
                    }
                    break;
                case DateRestrictionType.PastOnly:
                    if (selectedDate < today)
                    {
                        return ValidationResult.Success;
                    }
                    break;
                case DateRestrictionType.PastIncludingToday:
                    if (selectedDate <= today)
                    {
                        return ValidationResult.Success;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(DateRestrictionType));
            }

            return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
        }
        
        public override string FormatErrorMessage(string currentPropDisplayName)
        {
            return string.Format(
                CultureInfo.CurrentCulture,
                this.ErrorMessageString,
                currentPropDisplayName);
        }
    }


    public class DateRestrictionAttributeAdapter : AttributeAdapterBase<DateRestrictionAttribute>
    {

        public DateRestrictionAttribute CurrentAttribute { get; set; }

        public DateRestrictionAttributeAdapter(DateRestrictionAttribute attribute, IStringLocalizer stringLocalizer)
            : base(attribute, stringLocalizer)
        {
            CurrentAttribute = attribute;
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-daterestriction", GetErrorMessage(context));
            MergeAttribute(context.Attributes, "data-todaydate", DateTimeHelper.GetGregoreanDateString(DateTime.Now.Date));
            MergeAttribute(context.Attributes, "data-daterestrictiontype", CurrentAttribute.DateRestrictionType.ToString().ToCamelCase());
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