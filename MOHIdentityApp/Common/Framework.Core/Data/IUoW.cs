// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUoW.cs" company="Usama Nada">
//   No Copy Rights. Free To Use and Share. Enjoy
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Framework.Core.Data
{
    #region

    using System;

    #endregion

    /// <summary>
    ///     http://codereview.stackexchange.com/questions/19037/entity-framework-generic-repository-pattern
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// The save.
        /// </summary>
        /// <param name="userId">
        /// The user identifier.
        /// </param>
        /// <param name="validateOnSaveEnabled">
        /// The validate On Save Enabled.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        int Save(string userId = null, bool validateOnSaveEnabled = true);
    }
}