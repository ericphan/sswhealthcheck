using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using SSW.HealthCheck;

namespace $rootnamespace$.Controllers
{
    public class zsValidateController : Controller
    {
        private static readonly JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            DateFormatHandling = DateFormatHandling.IsoDateFormat
        };

        public ActionResult Index()
        {
            var tests = HealthCheckService.Default.GetAll();
            var tasks = tests.Where(x => x.IsDefault).Select(x => x.RunAsync()).ToArray();
            Task.WaitAll(tasks);

            var failed = tests.Any(t => t.Result != null && !t.Result.Success);
            if (failed)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            return View(tests.OrderBy(x => x.Name));
        }

        public ActionResult Check(string key)
        {
            var m = HealthCheckService.Default.GetByKey(key);
            m.RunAsync();
            var json = JsonConvert.SerializeObject(m, settings);
            return this.Content(json, "application/json");
        }
    }
}