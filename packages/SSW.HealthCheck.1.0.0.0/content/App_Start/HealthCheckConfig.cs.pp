using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSW.HealthCheck.Tests;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof($rootnamespace$.App_Start.HealthCheckConfig), "PreStart")]
namespace $rootnamespace$.App_Start 
{
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
            svc.Add(new NotDebugTest());
            svc.Add(new DbConnectionTest());
            svc.Setup<$rootnamespace$.Hubs.HealthCheckHub>();
        }
    }
}