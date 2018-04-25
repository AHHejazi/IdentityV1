// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisableScriptsAttribute.cs" company="Usama Nada">
//   No Copy Rights. Free To Use and Share. Enjoy
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Framework.Core.Mvc.DataAnnotations
{
    using Framework.Core.Resources;
    using Microsoft.AspNetCore.Mvc.DataAnnotations.Internal;
    using Microsoft.Extensions.Localization;
    #region

    using System;
    using System.ComponentModel.DataAnnotations;

    #endregion

    /// <summary>
    ///     The disable scripts attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DisableScriptsAttribute : RegularExpressionAttribute
    {
        /// <summary>
        ///     The pattern.
        /// </summary>
        private new const string Pattern = @"^[^<>{}]+$";
        
        /// <summary>
        ///     Initializes a new instance of the <see cref="DisableScriptsAttribute" /> class.
        /// </summary>
        public DisableScriptsAttribute()
            : base(Pattern)
        {
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
            return string.Format(Messages.ScriptsNotAllowedErrorMessage, name);
        }


    }

    public class DisableScriptsAttributeAdaptor : RegularExpressionAttributeAdapter
    {
        public DisableScriptsAttributeAdaptor(RegularExpressionAttribute attribute, IStringLocalizer stringLocalizer)
            : base(attribute, stringLocalizer)
        {
        }
    }
}