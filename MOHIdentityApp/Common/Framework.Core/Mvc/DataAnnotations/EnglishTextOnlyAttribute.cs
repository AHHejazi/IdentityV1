// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnglishTextOnlyAttribute.cs" company="Usama Nada">
//   No Copy Rights. Free To Use and Share. Enjoy
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Framework.Core.Mvc.DataAnnotations
{
    #region

    using System;
    using System.ComponentModel.DataAnnotations;
    using global::Framework.Core.Resources;
    using Microsoft.AspNetCore.Mvc.DataAnnotations.Internal;
    using Microsoft.Extensions.Localization;

    #endregion

    /// <summary>
    ///     The english text only attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class EnglishTextOnlyAttribute : RegularExpressionAttribute
    {
        /// <summary>
        ///     The pattern.
        /// </summary>
        private new const string Pattern = @"^[A-Za-z0-9\s!@#$%^&*()_+=-`~\\\]\[{}|';:/.,?]*$";


        public EnglishTextOnlyAttribute()
            : base(Pattern)
        {

        }



        public override string FormatErrorMessage(string name)
        {

            return string.Format(Messages.ArabicLettersOnlyErrorMessage, name);


        }
    }

    public class EnglishTextOnlyAttributeAdaptor : RegularExpressionAttributeAdapter
    {
        public EnglishTextOnlyAttributeAdaptor(RegularExpressionAttribute attribute, IStringLocalizer stringLocalizer)
            : base(attribute, stringLocalizer)
        {
        }
    }
}
