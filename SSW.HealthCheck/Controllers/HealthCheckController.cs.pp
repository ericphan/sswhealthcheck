using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace $rootnamespace$.Controllers
{
	using SSW.HealthCheck.Infrastructure;

    public class HealthCheckController : Controller
    {
        private static readonly JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            DateFormatHandling = DateFormatHandling.IsoDateFormat
        };

        public ActionResult Index()
        {
            if (this.HttpContext != null)
            {
                HealthCheckService.Default.HttpContext = this.HttpContext.ApplicationInstance.Context;
            }

            var tests = HealthCheckService
                            .Default
                            .GetAll()
                            .GroupBy(t => new { Name = t.TestCategory == null ? "Default tests" : t.TestCategory.Name, Order = t.TestCategory == null ? 0 : t.TestCategory.Order })
                            .OrderBy(g => g.Key.Order)
                            .ThenBy(g => g.Key.Name)
                            .Select(g => new TestGroup
                                            {
                                                Name = g.Key.Name,
                                                TestMonitors = g.OrderBy(i => i.Order).ThenBy(i => i.Key)
                                            });
                            
            return this.View(tests);
        }

        public ActionResult Check(string key)
        {
            if (this.HttpContext != null)
            {
                HealthCheckService.Default.HttpContext = this.HttpContext.ApplicationInstance.Context;
            }

            var m = HealthCheckService.Default.GetByKey(key);
            m.Run();
            var json = JsonConvert.SerializeObject(m, settings);
            return this.Content(json, "application/json");
        }
    }
}