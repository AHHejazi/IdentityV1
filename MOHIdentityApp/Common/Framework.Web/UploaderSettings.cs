// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UploaderSettings.cs" company="Usama Nada">
//   No Copy Rights. Free To Use and Share. Enjoy
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Framework.Web
{
    /// <summary>
    ///     The uploader settings.
    /// </summary>
    public class UploaderSettings : WidgetSettingsBase
    {
        /// <summary>
        ///     Gets or sets the allowed extensions.
        /// </summary>
        public string AllowedExtensions { get; set; }

        /// <summary>
        ///     Gets or sets the download file url.
        /// </summary>
        public string DownloadFileUrl { get; set; } = "/Download/?attId=";

        /// <summary>
        ///     Gets or sets the file id.
        /// </summary>
        public object FileId { get; set; }

        /// <summary>
        ///     Gets or sets the image max height.
        /// </summary>
        public int ImageMaxHeight { get; set; }

        /// <summary>
        ///     Gets or sets the image max width.
        /// </summary>
        public int ImageMaxWidth { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether is image.
        /// </summary>
        public bool IsImage { get; set; }

        /// <summary>
        ///     Gets or sets the max size in megabytes.
        /// </summary>
        public int MaxSizeInMegabytes { get; set; }
    }
}