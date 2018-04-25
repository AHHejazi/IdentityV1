// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitOfWorkBase.cs" company="Usama Nada">
//   No Copy Rights. Free To Use and Share. Enjoy
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Framework.Core.Data
{

    #region
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Framework.Core.Extensions;
    using System;
    using System.Linq;
    using System.Reflection;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Framework.Core.LogProvider;


    #endregion

    /// <summary>
    /// The unit of work base.
    /// </summary>
    public class UnitOfWorkBase
    {

        private static IHttpContextAccessor httpContextAccessor;
        public static void SetHttpContextAccessor(IHttpContextAccessor accessor)
        {
            httpContextAccessor = accessor;
        }

        /// <summary>
        ///     The Logger
        /// </summary>
        private static readonly LogHelper Logger = new LogHelper();

        /// <summary>
        /// The context.
        /// </summary>
        protected DbContext context;

        /// <summary>
        /// Calling SaveChanges does create a DB transaction so
        ///     every query executed against the DB will be rollbacked is something goes wrong.
        /// </summary>
        /// <param name="userId">
        /// The user Id.
        /// </param>
        /// <param name="validateOnSaveEnabled">
        /// The validate On Save Enabled.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        

        public  int Save(string userId = null, bool validateOnSaveEnabled = true)
        {
            var changeTracker = new ChangeTracker(this.context);
            var entities = (from entry in changeTracker.Entries()
                            where entry.State == EntityState.Modified || entry.State == EntityState.Added
                            select entry.Entity);

            var validationResults = new List<ValidationResult>();
            foreach (var entity in entities)
            {
                String validationErrors = null;
                if (!Validator.TryValidateObject(entity, new ValidationContext(entity), validationResults))
                {
                    var exception = new ValidationException();
                    
                    foreach (var item in validationResults)
                    {
                        validationErrors +=$"Entity: {entity} - Property: {item.MemberNames} - Error: {item.ErrorMessage} \n ";
                        Logger.InsertLog(validationErrors);
                    }
                    throw new ValidationException(validationErrors);
                }
            }
         //   this.UpdatePropertiesBeforeSave(userId);
            return this.context.SaveChanges();
        }

        /// <summary>
        /// Updates the properties before save.
        /// </summary>
        /// <param name="userId">
        /// The user identifier.
        /// </param>
        private void UpdatePropertiesBeforeSave(string userId = null)
        {


            if (string.IsNullOrEmpty(userId)  && httpContextAccessor.HttpContext.User != null
                && httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                userId = httpContextAccessor.HttpContext.User.Identity.Name;
            }

            const string CreatedOnProperty = "CreatedOn";
            const string ModifiedOnPropery = "UpdatedOn";

            // const string IsActiveProperty = "IsActive";
            const string IdProperty = "Id";

            var entitiesWithCreatedOn = this.context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added && e.Entity.GetType().GetProperty(CreatedOnProperty) != null);
            foreach (var entry in entitiesWithCreatedOn)
            {
                entry.Property(CreatedOnProperty).CurrentValue = DateTime.Now;
            }

            // IEnumerable<DbEntityEntry> entitiesWithStateCode =
            // this.context.ChangeTracker.Entries()
            // .Where(
            // e => e.State == EntityState.Added && e.Entity.GetType().GetProperty(IsActiveProperty) != null);
            // foreach (DbEntityEntry entry in entitiesWithStateCode)
            // {
            // entry.Property(IsActiveProperty).CurrentValue = true;
            // }
            var entitiesWithId = this.context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added && e.Entity.GetType().GetProperty(IdProperty) != null);
            foreach (var entry in entitiesWithId)
            {
                Guid id;
                if (Guid.TryParse(entry.Property(IdProperty).CurrentValue.ToString(), out id) && id == Guid.Empty)
                {
                    entry.Property(IdProperty).CurrentValue = Guid.NewGuid().AsSequentialGuid();
                }
            }

            var entitiesWithModifiedOn = this.context.ChangeTracker.Entries()
                .Where(
                    e => e.State == EntityState.Modified && e.Entity.GetType().GetProperty(ModifiedOnPropery) != null);
            foreach (var entry in entitiesWithModifiedOn)
            {
                entry.Property(ModifiedOnPropery).CurrentValue = DateTime.Now;
            }

            if (!string.IsNullOrEmpty(userId))
            {
                const string CreatedByPropery = "CreatedBy";
                const string ModifiedByPropery = "UpdatedBy";
                var entitiesWithCreatedBy = this.context.ChangeTracker.Entries()
                    .Where(
                        e => e.State == EntityState.Added && e.Entity.GetType().GetProperty(CreatedByPropery) != null);
                foreach (var entry in entitiesWithCreatedBy)
                {
                    entry.Property(CreatedByPropery).CurrentValue = userId;
                }

                var entitiesWithModifiedBy = this.context.ChangeTracker.Entries()
                    .Where(
                        e => e.State == EntityState.Modified
                             && e.Entity.GetType().GetProperty(ModifiedByPropery) != null);
                foreach (var entry in entitiesWithModifiedBy)
                {
                    entry.Property(ModifiedByPropery).CurrentValue = userId;
                }
            }
        }
    }
}