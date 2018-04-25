// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LookupsHelper.cs" company="SURE International Technology">
//   Copyright © 2017 All Right Reserved
// </copyright>
// --------------------------------------------------------------------------------------------------------------------



namespace Identity.Web.Code
{

    #region Usings
    using Identity.Loclization;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;

    #endregion

    /// <summary>
    ///     The lookups helper.
    /// </summary>
    public static class LookupsHelper
    {
        /// <summary>
        ///     The activation status.
        /// </summary>
        public static List<SelectListItem> ActivationStatus => new List<SelectListItem>
        {
            new SelectListItem
            {
                Text = SharedResources.Active,
                Value = "true"
            },
            new SelectListItem
            {
                Text = SharedResources.Inactive,
                Value = "false"
            }
        };

        public static List<SelectListItem> YesNo => new List<SelectListItem>
        {
            new SelectListItem
            {
                Text = SharedResources.Yes,
                Value = "true"
            },
            new SelectListItem
            {
                Text = SharedResources.No,
                Value = "false"
            }
        };

      
    }  
}