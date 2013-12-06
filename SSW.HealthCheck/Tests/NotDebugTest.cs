using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace SSW.HealthCheck.Tests
{
    public class NotDebugTest : ITest
    {
        public string Name
        {
            get { return "Debug Mode should be Off"; }
        }

        public string Description
        {
            get { return "Verify that debug mode is off on production."; }
        }

        public bool IsDefault
        {
            get { return true; }
        }

        public void Test(ITestContext context)
        {
            var config = WebConfigurationManager.OpenWebConfiguration("~/");
            var compilationSection = config.GetSection("system.web/compilation") as CompilationSection;
            if (compilationSection != null)
            {
                if (compilationSection.Debug)
                {
                    Assert.Fails("Debug mode is true.");
                }
            }
        }
    }
}
