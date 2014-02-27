using System.Web.Configuration;

namespace SSW.HealthCheck.Infrastructure.Tests
{
    using System.Collections.Generic;

    /// <summary>
    /// Check if system is in debug mode
    /// </summary>
    public class NotDebugTest : ITest
    {
        /// <summary>
        /// Gets the name for the test.
        /// </summary>
        /// <value></value>
        public string Name
        {
            get { return "Debug Mode should be Off"; }
        }

        /// <summary>
        /// Gets the description for test.
        /// </summary>
        /// <value></value>
        public string Description
        {
            get { return "Verify that debug mode is off on production."; }
        }

        /// <summary>
        /// Gets the widget actions.
        /// </summary>
        /// <value>The test actions.</value>
        public IEnumerable<TestAction> TestActions
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a value that indicate if the test is to run by default.
        /// </summary>
        /// <value></value>
        public bool IsDefault
        {
            get { return true; }
        }

        /// <summary>
        /// Run the health check.
        /// </summary>
        /// <param name="context">Test context</param>
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
