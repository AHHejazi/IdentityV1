// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WidgetSettingsBase.cs" company="Usama Nada">
//   No Copy Rights. Free To Use and Share. Enjoy
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Framework.Core.Mvc
{
    #region

    using Framework.Core.Globalization;

    #endregion

    /// <summary>
    ///     The widget settings base.
    /// </summary>
    public class WidgetSettingsBase
    {
        /// <summary>
        ///     Gets or sets the container id.
        /// </summary>
        public string ContainerId { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether is read only.
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        ///     Gets or sets the lang.
        /// </summary>
        public string Lang { get; set; } = CultureHelper.CurrentLanguage;
    }
}