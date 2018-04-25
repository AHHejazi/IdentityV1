// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GlobalizationModule.cs" company="Usama Nada">
//   No Copy Rights. Free To Use and Share. Enjoy
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Framework.Core.Globalization
{
    #region

    using System;
    using System.Web;

    #endregion

    /// <summary>
    ///     The globalization module.
    /// </summary>
    //public class GlobalizationModule : IHttpModule
    //{
    //    /// <summary>
    //    /// The initialize culture.
    //    /// </summary>
    //    public static void InitializeCulture()
    //    {
    //        CultureHelper.InitializeCultureFromCookie();
    //    }

    //    /// <summary>
    //    /// The dispose.
    //    /// </summary>
    //    public void Dispose()
    //    {
    //    }

    //    /// <summary>
    //    /// The init.
    //    /// </summary>
    //    /// <param name="context">
    //    /// The context.
    //    /// </param>
    //    public void Init(HttpApplication context)
    //    {
    //        context.PreRequestHandlerExecute += this.ContextPreRequestHandlerExecute;
    //    }

    //    /// <summary>
    //    /// The context pre request handler execute.
    //    /// </summary>
    //    /// <param name="sender">
    //    /// The sender.
    //    /// </param>
    //    /// <param name="e">
    //    /// The e.
    //    /// </param>
    //    private void ContextPreRequestHandlerExecute(object sender, EventArgs e)
    //    {
    //        var application = sender as HttpApplication;
    //        var currentHandler = application?.Context.CurrentHandler;
    //        if (currentHandler is Page || currentHandler is MvcHandler)
    //        {
    //            CultureHelper.InitializeCultureFromCookie();
    //        }
    //    }
    //}
}