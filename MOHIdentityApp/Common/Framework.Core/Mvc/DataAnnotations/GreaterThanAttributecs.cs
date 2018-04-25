// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateGreaterThanTodayAttribute.cs" company="Usama Nada">
//   No Copy Rights. Free To Use and Share. Enjoy
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Framework.Core.Mvc.DataAnnotations
{
    using Microsoft.AspNetCore.Mvc.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
    using Microsoft.Extensions.Localization;
    #region

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;

    #endregion

    /// <summary>
    ///     The date greater than today.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class GreaterThanAttribute : ValidationAttribute
    {
        private string MinDisplayName { get; set; }

        public bool AllowEqualValues { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GreaterThanAttribute"/> class.
        /// </summary>
        /// <param name="minPropertyName">
        /// The other property.
        /// </param>
        public GreaterThanAttribute(string minPropertyName)
        {
            this.MinPropertyName = minPropertyName;
        }

        /// <summary>
        /// Gets or sets the other property.
        /// </summary>
        public string MinPropertyName { get; set; }


        /// <summary>
        /// The get second comparable.
        /// </summary>
        /// <param name="validationContext">
        /// The validation context.
        /// </param>
        /// <returns>
        /// The <see cref="IComparable"/>.
        /// </returns>
        protected IComparable GetSecondComparable(ValidationContext validationContext)
        {
            var propertyInfo = validationContext.ObjectType.GetProperty(this.MinPropertyName);
            if (propertyInfo != null)
            {
                var secondValue = propertyInfo.GetValue(validationContext.ObjectInstance, null);
                return secondValue as IComparable;
            }

            return null;
        }

        /// <summary>
        /// The is valid.
        /// </summary>
        /// <param name="firstValue">
        /// The first value.
        /// </param>
        /// <param name="validationContext">
        /// The validation context.
        /// </param>
        /// <returns>
        /// The <see cref="ValidationResult"/>.
        /// </returns>
        protected override ValidationResult IsValid(object firstValue, ValidationContext validationContext)
        {
            var propertyInfo = validationContext.ObjectType.GetProperty(this.MinPropertyName);
            if (propertyInfo == null)
            {
                return new ValidationResult($"unknown property {this.MinPropertyName}");
            }

            var firstComparable = firstValue as IComparable;
            var secondComparable = this.GetSecondComparable(validationContext);

            if (firstComparable == null || secondComparable == null)
            {
                return ValidationResult.Success;
            }

            if (this.AllowEqualValues && firstComparable.CompareTo(secondComparable) == 0)
            {
                return ValidationResult.Success;
            }

            if (firstComparable.CompareTo(secondComparable) < 0)
            {
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }

       

        public override string FormatErrorMessage(string currentPropDisplayName)
        {
            return string.Format(
                CultureInfo.CurrentCulture,
                this.ErrorMessageString,
                currentPropDisplayName,
                this.MinDisplayName);
        }
    }


    public class GreaterThanAttributeAdapter : AttributeAdapterBase<GreaterThanAttribute>
    {

        public GreaterThanAttribute CurrentAttribute { get; set; }

        public GreaterThanAttributeAdapter(GreaterThanAttribute attribute, IStringLocalizer stringLocalizer)
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
            MergeAttribute(context.Attributes, "data-val-greaterthan", GetErrorMessage(context));
             MergeAttribute(context.Attributes, "data-propertytested", CurrentAttribute.MinPropertyName);
            MergeAttribute(context.Attributes, "data-allowequalvalues", CurrentAttribute.AllowEqualValues.ToString());
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