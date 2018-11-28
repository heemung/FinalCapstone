using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace GC_Car_Dealership
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            //remove the xml default for so it will be json
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            //this line will stop the infinite loop caused by the navigation properties
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling
                = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
