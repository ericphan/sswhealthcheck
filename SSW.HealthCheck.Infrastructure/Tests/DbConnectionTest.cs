namespace SSW.HealthCheck.Infrastructure.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Configuration;

    /// <summary>
    /// Database connection test
    /// </summary>
    public class DbConnectionTest : ITest
    {
        /// <summary>
        /// Gets the name for the test.
        /// </summary>
        /// <value></value>
        public string Name
        {
            get { return "Database Tests"; }
        }

        /// <summary>
        /// Gets the description for test.
        /// </summary>
        /// <value></value>
        public string Description
        {
            get { return "Check that web application have connection to all the databases."; }
        }

        /// <summary>
        /// Gets a value that indicate if the test is to run by default.
        /// </summary>
        /// <value></value>
        public bool IsDefault
        {
            get { return false; }
        }

        /// <summary>
        /// Gets or sets the include.
        /// </summary>
        /// <value>The include.</value>
        public string[] Include { get; set; }

        /// <summary>
        /// Gets or sets the exclude.
        /// </summary>
        /// <value>The exclude.</value>
        public string[] Exclude { get; set; }

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
        /// Run the health check.
        /// </summary>
        /// <param name="ctx">Test context</param>
        public void Test(ITestContext ctx)
        {
            var config = WebConfigurationManager.OpenWebConfiguration("~/");
            var settings = config.ConnectionStrings.ConnectionStrings.OfType<ConnectionStringSettings>()
                .Where(x =>
                    (this.Exclude == null || this.Exclude.Length == 0 || !this.Exclude.Contains(x.Name)) &&
                    (this.Include == null || this.Include.Length == 0 || this.Include.Contains(x.Name)));

            var failedSettings = new System.Collections.Concurrent.ConcurrentBag<ConnectionStringSettings>();
            var settingsCount = settings.Count();
            var processedCount = 0;
            ctx.UpdateProgress(0, processedCount, settingsCount);
            Parallel.ForEach(
                settings,
                setting =>
                {
                    try
                    {
                        var factory = DbProviderFactories.GetFactory(setting.ProviderName);
                        var csBuilder = new DbConnectionStringBuilder();
                        csBuilder.ConnectionString = setting.ConnectionString;
                        csBuilder["Connection Timeout"] = 5;

                        using (var cnn = factory.CreateConnection())
                        {
                            cnn.ConnectionString = csBuilder.ToString();
                            cnn.Open();
                        }

                        ctx.WriteLine("{0} connected successfully.");
                    }
                    catch (Exception ex)
                    {
                        ctx.WriteLine("{0} failed: {1}", setting.Name, ex.Message);
                        failedSettings.Add(setting);
                    }
                    processedCount++;
                    ctx.UpdateProgress(0, processedCount, settingsCount);
                });

            if (failedSettings.Count > 0)
            {
                var msg = string.Format("Cannot open database connection to: {0}", string.Join(", ", failedSettings.Select(x => x.Name).ToArray()));
                Assert.Fails(msg);
            }
        }
    }
}
