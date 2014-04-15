using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace SSW.HealthCheck.Mvc5.Examples.Controllers
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

            var tests = HealthCheckService.Default.GetAll();
            return View(tests);
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