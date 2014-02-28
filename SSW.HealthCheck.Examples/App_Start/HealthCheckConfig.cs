using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(SSW.HealthCheck.Mvc5.Examples.App_Start.HealthCheckConfig), "PreStart")]
namespace SSW.HealthCheck.Mvc5.Examples.App_Start 
{
    using System.Web.Hosting;

    using SSW.HealthCheck.Infrastructure;
    using SSW.HealthCheck.Infrastructure.Tests;
    using SSW.HealthCheck.Mvc5;
    using SSW.HealthCheck.SQLDeploy;
    using SSW.SQLDeploy.Core;

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

            var configPath = HostingEnvironment.MapPath("~/App_Data/SqlDeploy.config");
            var sqlDeployConfig = SqlDeployConfigurationHelper.GetSqlDeployConfiguration(configPath);
            var settings = new SqlDeploySettings(sqlDeployConfig);
            svc.Add(new SQLDeployTest(settings));

            svc.Setup<Hubs.HealthCheckHub>();
        }
    }
}