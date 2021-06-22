using JavaScriptEngineSwitcher.V8;
using JavaScriptEngineSwitcher.Core;
using React;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NetC.JuniorDeveloperExam.Web.ReactConfig), "Configure")]

namespace NetC.JuniorDeveloperExam.Web
{
    public static class ReactConfig
    {
        public static void Configure()
        {
            ReactSiteConfiguration.Configuration
                .AddScript("~/Assets/Scripts/React/Home.jsx")
                .AddScript("~/Assets/Scripts/React/Blog.jsx");
            JsEngineSwitcher.Current.DefaultEngineName = V8JsEngine.EngineName;
            JsEngineSwitcher.Current.EngineFactories.AddV8();
        }
    }
}