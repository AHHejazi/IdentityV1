// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompareDatesAttribute.cs" company="Usama Nada">
//   No Copy Rights. Free To Use and Share. Enjoy
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Framework.Core.Mvc.DataAnnotations
{
    #region

    using System;
    using System.ComponentModel.DataAnnotations;

    #endregion

    /// <summary>
    /// The compare dates attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class CompareDatesAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompareDatesAttribute"/> class.
        /// </summary>
        /// <param name="from">
        /// The from.
        /// </param>
        public CompareDatesAttribute(string from)
        {
            this.Start = from;
        }

        /// <summary>
        /// Gets the start.
        /// </summary>
        private string Start { get; }

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

            var dateTo = (DateTime)value;

            var propertyInfo = validationContext.ObjectType.GetProperty(this.Start);
            if (propertyInfo == null)
            {
                return ValidationResult.Success;
            }

            var dateFrom = (DateTime)propertyInfo.GetValue(validationContext.ObjectInstance, null);

            return dateTo < dateFrom
                       ? new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName))
                       : ValidationResult.Success;
        }
    }
}