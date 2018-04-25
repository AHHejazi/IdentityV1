// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidatePhoneNumberAttribute.cs" company="Usama Nada">
//   No Copy Rights. Free To Use and Share. Enjoy
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Framework.Core.Mvc.DataAnnotations
{
    using Framework.Core.Extensions;
    #region

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Framework.Core.Utils;
    using NumberType = Helpers.NumberType;
    using Microsoft.AspNetCore.Mvc.DataAnnotations;
    using Microsoft.Extensions.Localization;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
    using System;

    #endregion

    /// <summary>
    ///     The phone number attribute.
    /// </summary>
    public class ValidatePhoneNumberAttribute : ValidationAttribute
    {
        
        /// <summary>
        ///     The default error message.
        /// </summary>
        /// <summary>
        ///     Gets or sets the country code.
        /// </summary>
        public string CountryCode { get; set; } = string.Empty;

        /// <summary>
        ///     Gets or sets the number type.
        /// </summary>
        public NumberType NumberType { get; set; } = NumberType.MOBILE;


        // public override bool
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
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var numberType = this.NumberType.ToString().ToEnum<Utils.NumberType>();

            var isValid = PhoneNumbers.IsValidNumber(value as string, numberType, this.CountryCode);

            if (isValid)
            {
                validationContext.ObjectType.GetProperty(validationContext.MemberName).SetValue(
                    validationContext.ObjectInstance,
                    PhoneNumbers.FormatPhoneNumber(value.ToString(), this.CountryCode),
                    null);
                return ValidationResult.Success;
            }

            return new ValidationResult(this.ErrorMessage);
        }
    }

    public class ValidatePhoneNumberAttributeAdapter : AttributeAdapterBase<ValidatePhoneNumberAttribute>
    {

        public ValidatePhoneNumberAttribute CurrentAttribute { get; set; }

        public ValidatePhoneNumberAttributeAdapter(ValidatePhoneNumberAttribute attribute, IStringLocalizer stringLocalizer)
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
            MergeAttribute(context.Attributes, "data-val-checkisvalidnumber", GetErrorMessage(context));
            MergeAttribute(context.Attributes, "data-val-checkisvalidnumber-countrycode", CurrentAttribute.CountryCode);
            MergeAttribute(context.Attributes, "data-val-checkisvalidnumber-numbertype", CurrentAttribute.NumberType.ToString());
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