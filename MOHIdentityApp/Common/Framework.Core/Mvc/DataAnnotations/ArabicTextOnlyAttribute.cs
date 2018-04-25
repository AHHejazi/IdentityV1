using Framework.Core.Resources;
using Microsoft.AspNetCore.Mvc.DataAnnotations.Internal;
using Microsoft.Extensions.Localization;
using System;
using System.ComponentModel.DataAnnotations;

namespace Framework.Core.Mvc.DataAnnotations
{


    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ArabicTextOnlyAttribute : RegularExpressionAttribute
    {
        private new const string Pattern = @"^[\u0600-\u06FF\u003A\0-9s]{0,4000}$";

        public ArabicTextOnlyAttribute()
            : base(Pattern)
        {

        }



        public override string FormatErrorMessage(string name)
        {

            return string.Format(Messages.ArabicLettersOnlyErrorMessage, name);


        }
    }

    public class ArabicTextOnlyAttributeAdaptor : RegularExpressionAttributeAdapter
    {
        public ArabicTextOnlyAttributeAdaptor(RegularExpressionAttribute attribute, IStringLocalizer stringLocalizer)
            : base(attribute, stringLocalizer)
        {
        }
    }
}
