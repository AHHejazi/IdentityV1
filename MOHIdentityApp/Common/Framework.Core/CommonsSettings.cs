// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommonsSettings.cs" company="Usama Nada">
//   No Copy Rights. Free To Use and Share. Enjoy
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Framework.Core
{
    #region

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Configuration;
   
    #endregion

    /// <summary>
    /// The commons settings.
    /// </summary>
    public static class CommonsSettings
    {
        /// <summary>
        /// Gets the application name.
        /// </summary>
        public static string ApplicationName { get; internal set; } = "MyApplicationName";

        /// <summary>
        /// Gets the connection string name.
        /// </summary>
        public static string ConnectionStringName { get; internal set; } = "MCIRecallEntities";

        private static string connstringVal = null;
        /// <summary>
        /// Gets the connection string value.
        /// </summary>
       

        /// <summary>
        /// Gets the default culture.
        /// </summary>
        public static string DefaultCulture { get; internal set; } = "ar-SA";

        /// <summary>
        /// Gets the widgets static content version.
        /// </summary>
        public static double WidgetsStaticContentVersion { get; internal set; } = 1.0;

      
       
    }
}