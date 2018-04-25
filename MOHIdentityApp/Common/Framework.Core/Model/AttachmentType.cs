// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttachmentType.cs" company="Usama Nada">
//   No Copy Rights. Free To Use and Share. Enjoy
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Framework.Core.Model
{
    #region

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    #endregion

    /// <summary>
    /// The attachment type.
    /// </summary>
    [Table("laws.AttachmentType")]
    internal class AttachmentType
    {
        /// <summary>
        /// Gets or sets the allowed files extension.
        /// </summary>
        [StringLength(200)]
        public string AllowedFilesExtension { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        [Required]
        [StringLength(256)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the created on.
        /// </summary>
        [Column(TypeName = "datetime2")]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the image max height.
        /// </summary>
        public int? ImageMaxHeight { get; set; }

        /// <summary>
        /// Gets or sets the image max width.
        /// </summary>
        public int? ImageMaxWidth { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is image.
        /// </summary>
        public bool IsImage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is mandatory.
        /// </summary>
        public bool IsMandatory { get; set; }

        /// <summary>
        /// Gets or sets the max size in megabytes.
        /// </summary>
        public int MaxSizeInMegabytes { get; set; }

        /// <summary>
        /// Gets or sets the name ar.
        /// </summary>
        [Required]
        [StringLength(200)]
        public string NameAr { get; set; }

        /// <summary>
        /// Gets or sets the name en.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string NameEn { get; set; }

        /// <summary>
        /// Gets or sets the updated by.
        /// </summary>
        [StringLength(256)]
        public string UpdatedBy { get; set; }

        /// <summary>
        /// Gets or sets the updated on.
        /// </summary>
        [Column(TypeName = "datetime2")]
        public DateTime? UpdatedOn { get; set; }
    }
}