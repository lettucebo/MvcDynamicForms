using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Creatidea.Library.Web.DynamicForms.Demo.Startup))]
namespace Creatidea.Library.Web.DynamicForms.Demo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
