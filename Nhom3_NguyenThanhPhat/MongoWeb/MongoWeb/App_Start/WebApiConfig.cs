using System.Web.Http;

namespace MongoWeb
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Kích hoạt routing theo attribute
            config.MapHttpAttributeRoutes();

            // Route mặc định
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
