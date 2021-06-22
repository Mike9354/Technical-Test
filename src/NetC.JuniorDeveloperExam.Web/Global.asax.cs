using Autofac;
using Autofac.Integration.Mvc;
using JavaScriptEngineSwitcher.Core;
using JavaScriptEngineSwitcher.V8;
using NetC.JuniorDeveloperExam.Web.DependencyInjection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NetC.JuniorDeveloperExam.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //JsEngineSwitcherConfig.Configure(JsEngineSwitcher.Current);

            var builder = new ContainerBuilder();

            // Register MVC controllers and Autofac
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterModule<AutofacModule>();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
