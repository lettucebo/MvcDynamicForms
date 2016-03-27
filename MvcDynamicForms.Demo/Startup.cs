using Microsoft.Owin;

using MvcDynamicForms.Demo;

[assembly: OwinStartup(typeof(Startup))]
namespace MvcDynamicForms.Demo
{
    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
        }
    }
}
