Before you can run health check, you need to setup SignalR routing.

Add the following code in your Startup.cs:

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            app.MapSignalR();
        }
    }


To configure tests for your website's health check, please see:

	App_StartUp/HealthCheckConfig.cs

