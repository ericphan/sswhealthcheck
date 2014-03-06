[assembly: WebActivatorEx.PreApplicationStartMethod(typeof($rootnamespace$.App_Start.HealthCheckConfig), "PreStart")]
namespace $rootnamespace$.App_Start 
{
	using SSW.HealthCheck.Infrastructure;
    using SSW.HealthCheck.Infrastructure.Tests;
    using SSW.HealthCheck.Mvc4;

    public static class HealthCheckConfig 
    {
        public static void PreStart() 
        {
            // Add your start logic here
            RegisterTests();
        }
        public static void RegisterTests()
        {
            RegisterTests(HealthCheckService.Default);
        }
        public static void RegisterTests(HealthCheckService svc)
        {
            svc.Add(new NotDebugTest(1));
            svc.Add(new DbConnectionTest(2));
			svc.Add(new SmtpTest(3));
            svc.Setup<$rootnamespace$.Hubs.HealthCheckHub>();
        }
    }
}