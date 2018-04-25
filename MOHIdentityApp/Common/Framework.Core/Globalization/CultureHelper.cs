// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CultureHelper.cs" company="Usama Nada">
//   No Copy Rights. Free To Use and Share. Enjoy
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Framework.Core.Globalization
{
    using Framework.Core;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Localization;
    #region

    using System;
    using System.Globalization;
    using System.Threading;

    #endregion

    #region

    #endregion

    /// <summary>
    ///     The culture helper.
    /// </summary>
    public static class CultureHelper
    {
        /// <summary>
        /// The current culture name.
        /// </summary>
        public static string CurrentCultureName => Thread.CurrentThread.CurrentCulture.Name;

        /// <summary>
        ///     The current direction.
        /// </summary>
        public static string CurrentDirection => IsRightToLeft ? "rtl" : "ltr";

        /// <summary>
        ///     The current language.
        /// </summary>
        public static string CurrentLanguage => Thread.CurrentThread.CurrentCulture.Name.Substring(0, 2);

        /// <summary>
        ///     The is arabic.
        /// </summary>
        public static bool IsArabic => Thread.CurrentThread.CurrentCulture.LCID == 1025;

        /// <summary>
        ///     The is right to left.
        /// </summary>
        public static bool IsRightToLeft => Thread.CurrentThread.CurrentCulture.TextInfo.IsRightToLeft;

        /// <summary>
        /// The get culture info.
        /// </summary>
        /// <param name="cultureName">
        /// The culture name.
        /// </param>
        /// <returns>
        /// The <see cref="CultureInfo"/>.
        /// </returns>
        public static CultureInfo GetCultureInfo(string cultureName = "ar-SA")
        {
            // The default date in the system should be gregorian.
            var dateTimeFormat = new CultureInfo("en-GB").DateTimeFormat;

            // The default currency should be SAR
            var numberFormat = new CultureInfo("ar-SA").NumberFormat;
            numberFormat.CurrencyPositivePattern = 3;
            numberFormat.CurrencyNegativePattern = 3;

            return new CultureInfo(cultureName) { NumberFormat = numberFormat, DateTimeFormat = dateTimeFormat };
        }

        /// <summary>
        /// The initialize culture from cookie.
        /// </summary>
        public static void InitializeCultureFromCookie(HttpContext context)
        {
            var defaultCulture = CommonsSettings.DefaultCulture; // READ FROM FRAMEWORK SETTINGS
            var cookieName = "app-culture";
            var cultureCookie = context.Request.Cookies[cookieName];
            if (string.IsNullOrEmpty(cultureCookie))
            {
                context.Response.Cookies.Append("app-culture", defaultCulture,
                    new CookieOptions {Expires = DateTimeOffset.UtcNow.AddYears(1)}
                );
            }
            var clutureValFromCookie = cultureCookie ?? defaultCulture;
            var cultureToSet = GetCultureInfo(clutureValFromCookie);
            Thread.CurrentThread.CurrentCulture = cultureToSet;
            Thread.CurrentThread.CurrentUICulture = cultureToSet;
        }
    }
}