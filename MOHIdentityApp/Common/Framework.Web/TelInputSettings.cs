// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TelInputSettings.cs" company="Usama Nada">
//   No Copy Rights. Free To Use and Share. Enjoy
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Framework.Web
{
    /// <summary>
    ///     The tel input settings.
    /// </summary>
    public class TelInputSettings : WidgetSettingsBase
    {
        /// <summary>
        ///     Gets or sets the initial country.
        /// </summary>
        public string InitialCountry { get; set; } = "sa";

        /// <summary>
        ///     Gets or sets the phone number type.
        /// </summary>
        // [JsonConverter(typeof(StringEnumConverter))]
        public NumberType PhoneNumberType { get; set; } = NumberType.MOBILE;

        /// <summary>
        ///     Gets or sets the preferred countries.
        /// </summary>
        public string PreferredCountries { get; set; } = "sa,ae,kw,qa,om,bh";
    }

    /// <summary>
    ///     The number type.
    /// </summary>
    public enum NumberType
    {
        /// <summary>
        ///     The unknown.
        /// </summary>
        UNKNOWN = -1,

        /// <summary>
        ///     The fixe d_ line.
        /// </summary>
        FIXED_LINE = 0,

        /// <summary>
        ///     The mobile.
        /// </summary>
        MOBILE = 1,

        /// <summary>
        ///     The fixe d_ lin e_ o r_ mobile.
        /// </summary>
        FIXED_LINE_OR_MOBILE = 2,

        /// <summary>
        ///     The tol l_ free.
        /// </summary>
        TOLL_FREE = 3,

        /// <summary>
        ///     The premiu m_ rate.
        /// </summary>
        PREMIUM_RATE = 4,

        /// <summary>
        ///     The share d_ cost.
        /// </summary>
        SHARED_COST = 5,

        /// <summary>
        ///     The voip.
        /// </summary>
        VOIP = 6,

        /// <summary>
        ///     The persona l_ number.
        /// </summary>
        PERSONAL_NUMBER = 7,

        /// <summary>
        ///     The pager.
        /// </summary>
        PAGER = 8,

        /// <summary>
        ///     The uan.
        /// </summary>
        UAN = 9,

        /// <summary>
        ///     The voicemail.
        /// </summary>
        VOICEMAIL = 10
    }
}