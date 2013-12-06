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
    public class DbConnectionTest : ITest
    {
        public DbConnectionTest()
        {
        }

        public string Name
        {
            get { return "Database Tests"; }
        }

        public string Description
        {
            get { return "Check that web application have connection to all the databases."; }
        }

        public bool IsDefault
        {
            get { return false; }
        }

        public string[] Include { get; set; }
        public string[] Exclude { get; set; }

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
            Parallel.ForEach(settings, setting =>
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
