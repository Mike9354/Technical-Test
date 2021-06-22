using System.Web.Mvc;
using System.Web.Routing;

namespace NetC.JuniorDeveloperExam.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Default",
                url: "",
                defaults: new { controller = "Home", action = "Index" }
            );
            routes.MapRoute(
                name: "Comments",
                url: "comments/{id}",
                defaults: new { controller = "Home", action = "Comments", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Blog",
                url: "blog/{id}",
                defaults: new { controller = "Home", action = "Blog", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "AddComment",
                url: "addComment",
                defaults: new { controller = "Home", action = "AddComment" }
            );
        }
    }
}