using System.Web.Mvc;
using System.Web.Routing;

namespace frontend_app
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Sign Up route
            routes.MapRoute(name: "Sign Up",
                            url: "SignUp",
                            defaults: new { controller = "Authentication", action = "SignUp" });

            // Log In route
            routes.MapRoute(name: "Log In",
                            url: "LogIn",
                            defaults: new { controller = "Authentication", action = "LogIn" });

            // Default route
            routes.MapRoute(name: "Default",
                            url: "{controller}/{action}/{id}",
                            defaults: new { controller = "Authentication", action = "SignUp", id = UrlParameter.Optional });
        }
    }
}