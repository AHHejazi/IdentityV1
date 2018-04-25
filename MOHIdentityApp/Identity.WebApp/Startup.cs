using Framework.Core.Globalization;
using Framework.Core.LogProvider;
using Framework.Core.Mvc;
using Framework.Core.Mvc.DataAnnotations;
using Framework.Web;
using Framework.Web.EmbeddedResources;
using Identity.WebApp.Code;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NToastNotify;
using System;
using System.Globalization;

namespace Identity.WebApp
{
    public class Startup
    {

       

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // to map IUnitOfWork with MOHWSO2UoW

          
            services.AddScoped<IScriptManager, ScriptManager>(); // lifetime of http request 
            services.AddScoped<ICssManager, CssManager>();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // add for using in pagging
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            // for client validation
            services.AddSingleton<IValidationAttributeAdapterProvider, CustomValidationAttributeAdapterProvider>();
            

            // for notification
            services.AddMvc().AddNToastNotifyToastr(new ToastrOptions()
            {
                PositionClass = ToastPositions.TopCenter,
                ProgressBar = true
            });


            // Add the localization services to the services container
            //services.AddSingleton<LocService>();

            services.AddLocalization();


            services.AddMvc().AddMvcOptions(option =>
            {
                // add our custom binder to beginning of collection
                option.ModelBinderProviders.Insert(0, new DateTimeModelBinderProvider());
            })
               // Add support for finding localized views, based on file name suffix, e.g. Index.fr.cshtml
               .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
               // Add support for localizing strings in data annotations (e.g. validation messages) via the
               // IStringLocalizer abstractions.
               .AddDataAnnotationsLocalization();

            // Configure supported cultures and localization options
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-GB"),
                    new CultureInfo("fr"),
                    new CultureInfo("ar-SA")
                };

                // State what the default culture for your application is. This will be used if no specific culture
                // can be determined for a given request.
                options.DefaultRequestCulture = new RequestCulture(culture: "en-GB", uiCulture: "en-GB");

                // You must explicitly state which cultures your application supports.
                // These are the cultures the app supports for formatting numbers, dates, etc.
                options.SupportedCultures = supportedCultures;

                // These are the cultures the app supports for UI strings, i.e. we have localized resources for.
                options.SupportedUICultures = supportedCultures;
            });

            var serviceProvider = services.BuildServiceProvider();
         

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            env.EnvironmentName = EnvironmentName.Development;

            app.UseGlobalization();
            app.UseStaticFiles();
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));

            WebHelper.ServiceProvider = serviceProvider;

            loggerFactory.AddContext(LogLevel.Information, Configuration.GetConnectionString("DefaultConnection"));

           
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }


            app.UseRouter(EmbeddedResourcesConfigRouter.ConfigRouteBuilder(app, RoutePath.Widgets));
            app.UseRouter(EmbeddedResourcesConfigRouter.ConfigRouteBuilder(app, RoutePath.Scripts));
            app.UseNToastNotify();
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
           
            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            // for identity
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=home}/{action=index}/{id?}");
            });

            
        }
    }
}
