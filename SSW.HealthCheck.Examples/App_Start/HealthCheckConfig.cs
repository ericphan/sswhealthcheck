using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(SSW.HealthCheck.Mvc5.Examples.App_Start.HealthCheckConfig), "PreStart")]
namespace SSW.HealthCheck.Mvc5.Examples.App_Start 
{
	using SSW.HealthCheck.Infrastructure;
    using SSW.HealthCheck.Infrastructure.Tests;
    using SSW.HealthCheck.Mvc5;

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
            var debugTest = new NotDebugTest(1) { TestCategory = new TestCategory { Name = "Test" } };

            svc.Add(debugTest);
            svc.Add(new DbConnectionTest(2));
			svc.Add(new SmtpTest(3));
            svc.Setup<Hubs.HealthCheckHub>();
        }
    }
}