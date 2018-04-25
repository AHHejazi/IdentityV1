// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateModelBinder.cs" company="Usama Nada">
//   No Copy Rights. Free To Use and Share. Enjoy
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Framework.Core.Mvc
{
    #region

    using System;
    using System.Threading.Tasks;
    using global::Framework.Core.Resources;
    using global::Framework.Core.Utils;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    #endregion

    /// <summary>
    ///     The date model binder.
    /// </summary>
    public class DateTimeModelBinder : IModelBinder
    {
        /// <summary>
        /// The bind model.
        /// </summary>
        /// <param name="controllerContext">
        /// The controller context.
        /// </param>
        /// <param name="bindingContext">
        /// The binding context.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        Task IModelBinder.BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            if (bindingContext.ModelType != typeof(DateTime?) && bindingContext.ModelType != typeof(DateTime))
            {
                return Task.CompletedTask;
            }

            var dateTime = DateTime.MinValue;

            try
            {

                var modelName = GetModelName(bindingContext);

                var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
                if (valueProviderResult == ValueProviderResult.None)
                {
                    return Task.CompletedTask;
                }

                bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

                var dateToParse = valueProviderResult.FirstValue;

                if (string.IsNullOrEmpty(dateToParse))
                {
                    return Task.CompletedTask;
                }
                
                if (DateTimeHelper.IsUmAlqura(dateToParse))
                {
                    dateTime = DateTimeHelper.ParseUmAlquraDate(dateToParse).Value;
                }
                else if (DateTimeHelper.IsGregorian(dateToParse))
                {
                    dateTime = DateTimeHelper.ParseGregorianDate(dateToParse).Value;
                }

                if (dateTime == DateTime.MinValue)
                {
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, Messages.InvalidDateTime);
                    return null;
                }

                bindingContext.Result = ModelBindingResult.Success(dateTime);

                return Task.CompletedTask;
                
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string GetModelName(ModelBindingContext bindingContext)
        {
            //The "Name" property of the ModelBinder attribute can be used to specify the
            //route parameter name when the action parameter name is different from the route parameter name.
            //For instance, when the route is /api/{birthDate} and the action parameter name is "date"
            //we need to say [DateTimeModelBinder(Name ="birthDate")]
            //The bindingContext.BinderModelName contains the "Name" property of the ModelBinder attribute
            if (string.IsNullOrEmpty(bindingContext.BinderModelName))
            {
                return bindingContext.ModelName;
            }

            return bindingContext.BinderModelName;
        }

    }


    public class DateTimeModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(DateTime?) || context.Metadata.ModelType == typeof(DateTime))
            {
                return new DateTimeModelBinder();
            }

            return null;
        }
    }
}