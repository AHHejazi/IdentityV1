// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatePickerSettings.cs" company="Usama Nada">
//   No Copy Rights. Free To Use and Share. Enjoy
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Framework.Web
{
    #region

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    #endregion

    /// <summary>
    ///     The date picker settings.
    /// </summary>
    public class DatePickerSettings : WidgetSettingsBase
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public PickerRangeType? RangeType { get; set; }

        /// <summary>
        ///     Gets or sets the calendar type.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public CalendarType CalendarType { get; set; } = CalendarType.gregorian;

        /// <summary>
        ///     Gets or sets the date format.
        /// </summary>
        public string DateFormat { get; set; } = "dd/MM/yyyy";

        /// <summary>
        ///     Gets or sets the picker year range.
        /// </summary>
        public string PickerYearRange { get; set; } = "c-100:c+50";
    }

    /// <summary>
    ///     The calendar type.
    /// </summary>
    public enum CalendarType
    {
        /// <summary>
        ///     The ummalqura.
        /// </summary>
        ummalqura,

        /// <summary>
        ///     The gregorian.
        /// </summary>
        gregorian
    }

    public enum PickerRangeType{
        futureOnly,
        futureIncludingToday,
        pastOnly,
        pastIncludingToday
    }
}