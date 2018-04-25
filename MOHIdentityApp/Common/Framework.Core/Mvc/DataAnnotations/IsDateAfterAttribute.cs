// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompareDatesAttribute.cs" company="Usama Nada">
//   No Copy Rights. Free To Use and Share. Enjoy
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Framework.Core.Mvc.DataAnnotations
{
    using Microsoft.AspNetCore.Mvc.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
    using Microsoft.Extensions.Localization;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    #region

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    #endregion

    // <summary>
    // The compare dates attribute.
    // </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class IsDateAfterAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IsDateAfterAttribute"/> class.
        /// </summary>
        /// <param name="from">
        /// The from.
        /// </param>
        public IsDateAfterAttribute(string from, Type fromDisplayType,string fromResourceName)
        {
            this.Start = from;
            this.FromDisplayType = fromDisplayType;
            this.FromResourceName = fromResourceName;
            
        }

        private void GetResourceValue()
        {
            if (this.FromDisplayType != null && !string.IsNullOrEmpty(this.FromResourceName))
            {
                var property = this.FromDisplayType.GetProperty(this.FromResourceName, BindingFlags.Public | BindingFlags.Static);

                if (property != null && property.PropertyType != typeof(string))
                {

                    this.FromDisplayName = string.Empty;
                }
                else
                {
                    this.FromDisplayName = property.GetValue(null, null).ToString();
                }
            }
        }



        /// <summary>
        /// Gets the start.
        /// </summary>
        public string Start { get; }

        private string FromResourceName { get; set; }
        private string FromDisplayName { get; set; }
        private Type FromDisplayType { get; set; }

        public bool AllowEqualDates { get; set; }

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

            var propertyInfo = validationContext.ObjectType.GetProperty(this.Start);
            if (propertyInfo == null)
            {
                return new ValidationResult($"unknown property {this.Start}");
            }

            var startDateValue = propertyInfo.GetValue(validationContext.ObjectInstance, null);

            if (string.IsNullOrEmpty(this.FromDisplayName))
            {
                GetResourceValue();
            }
            


            if (!(value is DateTime) || !(startDateValue is DateTime))
            {
                return ValidationResult.Success;
            }

            // Compare values
            if ((DateTime)value >= (DateTime)startDateValue)
            {
                if (this.AllowEqualDates)
                {
                    return ValidationResult.Success;
                }

                if ((DateTime)value > (DateTime)startDateValue)
                {
                    return ValidationResult.Success;
                }
            }
           
            //var provider = new DataAnnotationsModelMetadataProvider();
            //var otherMetaData = provider.GetMetadataForProperty(() => model, type, this.Start);

            //this.StartDisplayName = otherMetaData.DisplayName;

            return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
        }

       

        public override string FormatErrorMessage(string currentPropDisplayName)
        {
            if (string.IsNullOrEmpty(this.FromDisplayName))
            {
                GetResourceValue();
            }


            return string.Format(
                CultureInfo.CurrentCulture,
                this.ErrorMessageString,
                currentPropDisplayName,
                this.FromDisplayName);
        }
    }


    public class IsDateAfterAttributeAdapter : AttributeAdapterBase<IsDateAfterAttribute>
    {

        public IsDateAfterAttribute CurrentAttribute { get; set; }

        public IsDateAfterAttributeAdapter(IsDateAfterAttribute attribute, IStringLocalizer stringLocalizer)
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
            MergeAttribute(context.Attributes, "data-val-isdateafter", GetErrorMessage(context));
            MergeAttribute(context.Attributes, "data-propertytested", CurrentAttribute.Start);
            MergeAttribute(context.Attributes, "data-allowequalvalues", CurrentAttribute.AllowEqualDates.ToString());
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