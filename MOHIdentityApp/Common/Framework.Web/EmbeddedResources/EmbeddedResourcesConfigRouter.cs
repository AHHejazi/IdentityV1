using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Framework.Core.EmbeddedResources;
using Framework.Core.Extensions;
using Framework.Core.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Framework.Web.EmbeddedResources
{
    public static class EmbeddedResourcesConfigRouter
    {
        //routePath must be Scripts or Widgets
        public static IRouter ConfigRouteBuilder(IApplicationBuilder app, RoutePath routePath)
        {

            // from Usama Nada
            var routeBuilder = new RouteBuilder(app);

            // Routing For Widgets Embedded Resources
            routeBuilder.MapGet("EmbeddedResources/" + routePath.ToString() + "/v{versionNumber}/{*filepath}", httpContext =>
                {
                    var filePath = httpContext.Request.Path;
                    var extension = Path.GetExtension(filePath);

                    var fileContents = EmbeddedResourceReader.Read(filePath);

                    if (fileContents == null)
                    {
                        httpContext.Response.StatusCode = 404;
                        return Task.FromResult<object>(null);
                    }

                     httpContext.Response.Headers.Add("content-type",MimeTypeMap.GetMimeType(extension));

                    if (!CommonsExtensions.IsImageExtension(extension))
                    {
                        httpContext.Response.Headers.Add("charset", "utf-8");
                    }

                return httpContext.Response.Body.WriteAsync(fileContents, 0, fileContents.Length);
                });

            return routeBuilder.Build();
        }
    }

    public enum RoutePath
    {
        Scripts,
        Widgets
    }
}
