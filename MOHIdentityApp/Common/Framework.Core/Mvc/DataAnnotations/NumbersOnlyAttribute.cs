// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NumbersOnlyAttribute.cs" company="Usama Nada">
//   No Copy Rights. Free To Use and Share. Enjoy
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Framework.Core.Mvc.DataAnnotations
{
    using Framework.Core.Extensions;
    #region

    using global::Framework.Core.Resources;
    using Microsoft.AspNetCore.Mvc.DataAnnotations.Internal;
    using Microsoft.Extensions.Localization;
    using System;
    using System.ComponentModel.DataAnnotations;

    #endregion

    /// <summary>
    /// The numbers only attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class NumbersOnlyAttribute : RegularExpressionAttribute
    {
        /// <summary>
        ///     The pattern.
        /// </summary>
        private new const string Pattern = @"^[0-9]*$";

        private int? Minimum { get; set; }

        private int? Maximum { get; set; }
        

        /// <summary>
        /// Initializes a new instance of the <see cref="NumbersOnlyAttribute"/> class. 
        ///     Initializes a new instance of the <see cref="ArabicTextOnlyAttribute"/> class.
        ///     Initializes a new instance of the <see cref="DisableScriptsAttribute"/>
        ///     class.
        /// </summary>
        public NumbersOnlyAttribute()
            : base(Pattern)
        {
        }

        public NumbersOnlyAttribute(int? minimum, int? maximum)
            : base(Pattern)
        {
            this.Minimum = minimum;
            this.Maximum = maximum;
        }
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
            return string.Format(Messages.NumberOnlyErrorMessage, name);
        }

        public override bool IsValid(object value)
        {
            var obj = value.To<int>();
            if (Maximum != null && Minimum != null)
            {
                return obj < Maximum && obj > Minimum;
            }
            if (Minimum != null)
            {
                return  obj > Minimum;
            }
            if (Maximum != null)
            {
                return obj > Minimum;
            }
            return base.IsValid(value);
        }
    }

    public class NumbersOnlyAttributeAdaptor : RegularExpressionAttributeAdapter
    {
        public NumbersOnlyAttributeAdaptor(RegularExpressionAttribute attribute, IStringLocalizer stringLocalizer)
            : base(attribute, stringLocalizer)
        {
        }
    }
}