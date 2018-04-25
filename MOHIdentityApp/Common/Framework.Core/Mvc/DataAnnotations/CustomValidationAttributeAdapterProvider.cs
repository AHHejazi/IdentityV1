// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;

namespace Framework.Core.Mvc.DataAnnotations
{
    public class CustomValidationAttributeAdapterProvider
        : ValidationAttributeAdapterProvider, IValidationAttributeAdapterProvider
    {
        public CustomValidationAttributeAdapterProvider()
        {
        }

        IAttributeAdapter IValidationAttributeAdapterProvider.GetAttributeAdapter(
            ValidationAttribute attribute,
            IStringLocalizer stringLocalizer)
        {
            var adapter = base.GetAttributeAdapter(attribute, stringLocalizer);

            if (adapter == null)
            {
                var minLengthSix = attribute as MinLengthSixAttribute;
                if (minLengthSix != null)
                {
                    adapter = new MinLengthSixAttributeAdapter(minLengthSix, stringLocalizer);
                }

                var arabictextOnly = attribute as ArabicTextOnlyAttribute;
                if (arabictextOnly != null)
                {
                    return new ArabicTextOnlyAttributeAdaptor(arabictextOnly, stringLocalizer);
                }


                var englishtextOnly = attribute as EnglishTextOnlyAttribute;
                if (englishtextOnly != null)
                {
                    return new EnglishTextOnlyAttributeAdaptor(englishtextOnly, stringLocalizer);
                }

                var locationLatLon = attribute as LocationLatLonAttribute;
                if (locationLatLon != null)
                {
                    return new LocationLatLonAttributeAdaptor(locationLatLon, stringLocalizer);
                }


                var numbersOnly = attribute as NumbersOnlyAttribute;
                if (numbersOnly != null)
                {
                    return new NumbersOnlyAttributeAdaptor(numbersOnly, stringLocalizer);
                }

                var yearsonly = attribute as YearsOnlyAttribute;
                if (yearsonly != null)
                {
                    return new YearsOnlyAttributeAdaptor(numbersOnly, stringLocalizer);
                }

                var mustbetrue = attribute as MustBeTrueAttribute;
                if (mustbetrue != null)
                {
                    return new MustBeTrueAttributeAdapter(mustbetrue, stringLocalizer);
                }

                var requirednospace = attribute as RequiredNoSpaceAttribute;
                if (requirednospace != null)
                {
                    return new RequiredNoSpaceAttributeAdapter(requirednospace, stringLocalizer);
                }


                var greaterThan = attribute as GreaterThanAttribute;
                if (greaterThan != null)
                {
                    return new GreaterThanAttributeAdapter(greaterThan, stringLocalizer);
                }

                var isdateafter = attribute as IsDateAfterAttribute;
                if (isdateafter != null)
                {
                    return new IsDateAfterAttributeAdapter(isdateafter, stringLocalizer);
                }

                var daterestriction = attribute as DateRestrictionAttribute;
                if (daterestriction != null)
                {
                    return new DateRestrictionAttributeAdapter(daterestriction, stringLocalizer);
                }

                var validatephonenumber = attribute as ValidatePhoneNumberAttribute;
                if (validatephonenumber != null)
                {
                    return new ValidatePhoneNumberAttributeAdapter(validatephonenumber, stringLocalizer);
                }

                var validatefileupload = attribute as ValidateFileUploadAttribute;
                if (validatefileupload != null)
                {
                    return new ValidateFileUploadAttributeAdapter(validatefileupload, stringLocalizer);
                }

            }

            return adapter;
        }
    }
}