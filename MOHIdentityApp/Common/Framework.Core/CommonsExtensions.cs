#region

using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

////using CacheManager.Core;

#endregion

namespace Framework.Core
{
    public static class CommonsExtensions
    {
        private static readonly string[] _validExtensions = {"jpg", "bmp", "gif", "png"}; //  etc

        //public static IApplicationBuilder UseCommonsFramework(this IApplicationBuilder app, IConfigurationRoot conf, IHostingEnvironment env, ILoggerFactory loggerFactory)
        //{
        //    // Logging
        //    loggerFactory.AddConsole(conf.GetSection("Logging"));
         

        //    loggerFactory.AddNLog();
        //    app.AddNLogWeb();

        //    //add NLog.Web
        //    app.AddNLogWeb();

        //    var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
        //    HttpContextHelper.Configure(httpContextAccessor, env);

        //    CommonsSettings.ConfigureSettings(conf.GetSection("CommonsFramework"));

        //    //USAMA Localization and Globalization
        //    var supportedCultures = new[]
        //    {
        //        CultureHelper.GetCultureInfo("ar-SA"),
        //        CultureHelper.GetCultureInfo("en-US")
        //    };

        //    app.UseRequestLocalization(new RequestLocalizationOptions
        //    {
        //        DefaultRequestCulture = new RequestCulture(CommonsSettings.DefaultCulture),
        //        // Formatting numbers, dates, etc.
        //        SupportedCultures = supportedCultures,
        //        // UI strings that we have localized.
        //        SupportedUICultures = supportedCultures
        //    });

        //    var routeBuilder = new RouteBuilder(app);

        //    routeBuilder.MapGet("Captcha/{*rand}", httpContext =>
        //    {
        //        var captchaImage = new CaptchaImage
        //        {
        //            Width = 200,
        //            NoiseFactor = 45,
        //            LinesFactor = 34,
        //            FontSize = 26,
        //            Length = 6,
        //            CharacterSet = "1234567890"
        //        };

        //        var cache = new MemoryCacheManager();
        //        var captchaValidationId = Guid.NewGuid().ToString();
        //        HttpContextHelper.Current.Response.Cookies.Append("cap-valid-id", Guid.NewGuid().ToString(), new CookieOptions
        //        {
        //            Expires = DateTimeOffset.UtcNow.AddMinutes(20)
        //        });
        //        cache.Set(captchaValidationId, captchaImage.Text);
                
        //        var imageBytes = captchaImage.Image.ToByteArray();
        //        httpContext.Response.Headers.Add("content-type",
        //            MimeTypeMap.GetMimeType("jpg"));
        //        return httpContext.Response.Body.WriteAsync(imageBytes, 0, imageBytes.Length);
        //    });

        //    // Routing For Widgets Embedded Resources
        //    routeBuilder.MapGet("EmbeddedResources/Widgets/v{versionNumber}/{*filepath}", httpContext =>
        //    {
        //        var filePath = httpContext.Request.Path;
        //        var extension = Path.GetExtension(filePath);

        //        var fileContents = EmbeddedResourceReader.Read(filePath);

        //        if (fileContents == null)
        //        {
        //            httpContext.Response.StatusCode = 404;
        //            return Task.FromResult<object>(null);
        //        }

        //        httpContext.Response.Headers.Add("content-type",
        //            MimeTypeMap.GetMimeType(extension));

        //        if (!IsImageExtension(extension))
        //        {
        //            httpContext.Response.Headers.Add("charset", "utf-8");
        //        }

        //        return httpContext.Response.Body.WriteAsync(fileContents, 0, fileContents.Length);
        //    });

        //    var routes = routeBuilder.Build();
        //    app.UseRouter(routes);

        //    //Caching

        //    //DbInit
        //    DbInitializer.Initialize();

        //     app.UseMiddleware<RenderRegsteredScriptMiddleware>();
        //    return app.UseMiddleware<CommonsMiddleware>();
        //}

        public static bool IsImageExtension(string ext)
        {
            return _validExtensions.Contains(ext);
        }

        public static void AddCommonsFramework(this IServiceCollection services, IConfigurationRoot conf)
        {
            services.AddSingleton(conf); // IConfigurationRoot
            services.AddSingleton<IConfiguration>(conf); // IConfiguration explicitly
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            //Caching
            services.AddMemoryCache();
            //services.AddDistributedMemoryCache();

            // using the new overload which adds a singleton of the configuration to services and the configure method to add logging

            ////services.AddCacheManagerConfiguration(conf);

            ////// uses a refined configurastion (this will not log, as we added the MS Logger only to the configuration above
            ////services.AddCacheManager<int>(conf, configure: builder => builder.WithJsonSerializer());

            ////// creates a completely new configuration for this instance (also not logging)
            ////services.AddCacheManager<DateTime>(inline => inline.WithDictionaryHandle());

            ////// any other type will be this. Configurastion used will be the one defined by AddCacheManagerConfiguration earlier.
            ////services.AddCacheManager();

            

            ////commonsSettingsConf.Get<CommonsSettings>()

            ////services.Configure<CommonsSettings>(conf.GetSection("CommonsFramework"));

            //conf.GetValue<string>("MySettings:ApplicationName");
        }
    }
}