// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemSettingsRepository.cs" company="Usama Nada">
//   No Copy Rights. Free To Use and Share. Enjoy
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Framework.Core.Data.Repositories
{
    #region
    
    using System.Linq;
    using Framework.Core.Extensions;
    using Framework.Core.Model;
    using Microsoft.EntityFrameworkCore;

    #endregion

    /// <summary>
    /// The system settings repository.
    /// </summary>
    public class SystemSettingsRepository : RepositoryBase<SystemSetting>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemSettingsRepository"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public SystemSettingsRepository(DbContext context)
            : base(context)
        {
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="applicationId">
        /// The application id.
        /// </param>
        public void Delete(string key, string applicationId = null)
        {
            var setting = this.DbSet.Where(c => c.Key == key && c.ApplicationId == applicationId);
            this.Delete(setting);
        }

        /// <summary>
        /// The get value.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="applicationId">
        /// The application id.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public T GetValue<T>(string key, string applicationId = null)
        {
            var setting = this.DbSet.FirstOrDefault(
                s => s.Key == key && (s.ApplicationId == applicationId || applicationId == null));
            return setting == null ? default(T) : setting.Value.To<T>();
        }
    }
}