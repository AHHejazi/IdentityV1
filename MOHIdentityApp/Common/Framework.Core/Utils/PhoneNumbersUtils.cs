// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PhoneNumbersUtils.cs" company="Usama Nada">
//   No Copy Rights. Free To Use and Share. Enjoy
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Framework.Core.Utils
{
    #region

    using global::PhoneNumbers;

    #endregion

    /// <summary>
    ///     The number type.
    /// </summary>
    public enum NumberType
    {
        /// <summary>
        ///     The fixe d_ line.
        /// </summary>
        FIXED_LINE,

        /// <summary>
        ///     The mobile.
        /// </summary>
        MOBILE,

        /// <summary>
        ///     The fixe d_ lin e_ o r_ mobile.
        /// </summary>
        FIXED_LINE_OR_MOBILE,

        /// <summary>
        ///     The tol l_ free.
        /// </summary>
        TOLL_FREE,

        /// <summary>
        ///     The premiu m_ rate.
        /// </summary>
        PREMIUM_RATE,

        /// <summary>
        ///     The share d_ cost.
        /// </summary>
        SHARED_COST,

        /// <summary>
        ///     The voip.
        /// </summary>
        VOIP,

        /// <summary>
        ///     The persona l_ number.
        /// </summary>
        PERSONAL_NUMBER,

        /// <summary>
        ///     The pager.
        /// </summary>
        PAGER,

        /// <summary>
        ///     The uan.
        /// </summary>
        UAN,

        /// <summary>
        ///     The voicemail.
        /// </summary>
        VOICEMAIL,

        /// <summary>
        ///     The unknown.
        /// </summary>
        UNKNOWN
    }

    /// <summary>
    ///     The phone numbers.
    /// </summary>
    public static class PhoneNumbers
    {
        /// <summary>
        /// Formats the phone number.
        /// </summary>
        /// <param name="phoneNumber">
        /// The phone number.
        /// </param>
        /// <param name="countryCode">
        /// The country Code.
        /// </param>
        /// <returns>
        /// The formatted phone number in E164 international format.
        /// </returns>
        /// <author>Ahmed Magdy (amagdy@sure.com.sa)</author>
        /// <created>07/09/2015</created>
        public static string FormatPhoneNumber(string phoneNumber, string countryCode)
        {
            countryCode = countryCode.ToUpper();
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();

            var isPossibleNumber = phoneNumberUtil.IsPossibleNumber(phoneNumber, countryCode);

            if (!isPossibleNumber)
            {
                return string.Empty;
            }

            var number = phoneNumberUtil.ParseAndKeepRawInput(phoneNumber, countryCode);
            var isValidNumber = phoneNumberUtil.IsValidNumber(number);
            if (!isValidNumber)
            {
                return string.Empty;
            }

            var formattedNumber = phoneNumberUtil.Format(number, PhoneNumberFormat.E164);
            return formattedNumber;
        }

        /// <summary>
        /// Determines whether [is valid mobile number] [the specified phone number].
        /// </summary>
        /// <param name="phoneNumber">
        /// The phone number.
        /// </param>
        /// <param name="numType">
        /// The num Type.
        /// </param>
        /// <param name="countryCode">
        /// The country Code. value is empty string if not validating aganist specific country
        /// </param>
        /// <returns>
        /// True if the provider number is a valid mobile phone; otherwise false.
        /// </returns>
        /// <author>Usama Nada (unada@sure.com.sa)</author>
        public static bool IsValidNumber(string phoneNumber, NumberType numType, string countryCode = "")
        {
            countryCode = countryCode.ToUpper();
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();

            var isPossibleNumber = phoneNumberUtil.IsPossibleNumber(phoneNumber, countryCode);
            if (!isPossibleNumber)
            {
                return false;
            }

            var number = phoneNumberUtil.ParseAndKeepRawInput(phoneNumber, countryCode);
            var isValidNumber = phoneNumberUtil.IsValidNumber(number);
            if (!isValidNumber)
            {
                return false;
            }

            var numberType = phoneNumberUtil.GetNumberType(number);

            if ((numberType == PhoneNumberType.FIXED_LINE || numberType == PhoneNumberType.MOBILE)
                && numType == NumberType.FIXED_LINE_OR_MOBILE)
            {
                return true;
            }

            return numberType.ToString().Equals(numType.ToString());
        }
    }
}