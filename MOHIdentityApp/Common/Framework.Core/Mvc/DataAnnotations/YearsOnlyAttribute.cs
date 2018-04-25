// --------------------------------------------------------------------------------------------------------------------
// <copyright file="YearsOnlyAttribute.cs" company="Usama Nada">
//   No Copy Rights. Free To Use and Share. Enjoy
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Framework.Core.Mvc.DataAnnotations
{
    using Framework.Core.Mvc.Helpers;
    #region

    using global::Framework.Core.Extensions;
    using global::Framework.Core.Resources;
    using Microsoft.AspNetCore.Mvc.DataAnnotations.Internal;
    using Microsoft.Extensions.Localization;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;

    #endregion

    /// <summary>
    ///     The years only attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class YearsOnlyAttribute : RegularExpressionAttribute
    {
        /// <summary>
        ///     The pattern.
        /// </summary>
        private new const string Pattern = @"^\d{4}$";

        

        /// <summary>
        ///     Initializes a new instance of the <see cref="YearsOnlyAttribute" /> class.
        /// </summary>
        public YearsOnlyAttribute()
            : base(Pattern)
        {
        }

        /// <summary>
        ///     Gets or sets the culture.
        /// </summary>
        public CalendarType CalendarType { get; set; } = CalendarType.gregorian;

        /// <summary>
        /// The format error message.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public override string FormatErrorMessage(string name)
        {
            return string.Format(Messages.YearOnlyErrorMessage, name);
        }

        /// <summary>
        /// The is valid.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool IsValid(object value)
        {
            if (string.IsNullOrEmpty(value.To<string>()))
            {
                return true;
            }

            try
            {
                var date = DateTime.ParseExact(
                    value.To<string>(),
                    "yyyy",
                    this.CalendarType == CalendarType.gregorian ? new CultureInfo("en-US") : new CultureInfo("ar-SA"));
                return date.Year != DateTime.MinValue.Year;
            }
            catch
            {
                return false;
            }
        }
    }

    public class YearsOnlyAttributeAdaptor : RegularExpressionAttributeAdapter
    {
        public YearsOnlyAttributeAdaptor(RegularExpressionAttribute attribute, IStringLocalizer stringLocalizer)
            : base(attribute, stringLocalizer)
        {
        }
    }
}