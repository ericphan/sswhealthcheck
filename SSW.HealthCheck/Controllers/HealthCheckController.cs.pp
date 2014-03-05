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
            var tests = HealthCheckService.Default.GetAll();
            return View(tests.OrderBy(x => x.Name));
        }

        public ActionResult Check(string key)
        {
            var m = HealthCheckService.Default.GetByKey(key);
            m.Run();
            var json = JsonConvert.SerializeObject(m, settings);
            return this.Content(json, "application/json");
        }
    }
}