using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SSW.HealthCheck.Examples.Startup))]
namespace SSW.HealthCheck.Examples
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            app.MapSignalR();
        }
    }
}
