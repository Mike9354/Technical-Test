using Autofac;
using NetC.JuniorDeveloperExam.Web.Helper;

namespace NetC.JuniorDeveloperExam.Web.DependencyInjection
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<JsonHelper>().As<IJsonHelper>();
        }
    }
}