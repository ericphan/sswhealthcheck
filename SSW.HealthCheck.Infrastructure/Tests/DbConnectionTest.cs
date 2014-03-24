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
        private readonly string name = Labels.DbTestTitle;
        private readonly string description = Labels.DbTestDescription;
        private readonly bool isDefault = true;
        private int order;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbConnectionTest" /> class.
        /// </summary>
        public DbConnectionTest(int order = 0)
        {
            this.Order = order;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbConnectionTest" /> class.
        /// </summary>
        /// <param name="name">The test name.</param>
        /// <param name="order">The order in which test will appear in the list.</param>
        public DbConnectionTest(string name, int order = 0)
        {
            this.name = name;
            this.order = order;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbConnectionTest" /> class.
        /// </summary>
        /// <param name="name">The test name.</param>
        /// <param name="description">The test description.</param>
        /// <param name="order">The order in which test will appear in the list.</param>
        public DbConnectionTest(string name, string description, int order = 0)
        {
            this.name = name;
            this.description = description;
            this.order = order;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbConnectionTest" /> class.
        /// </summary>
        /// <param name="isDefault">Run test by default.</param>
        /// <param name="order">The order in which test will appear in the list.</param>
        public DbConnectionTest(bool isDefault, int order = 0)
        {
            this.isDefault = isDefault;
            this.order = order;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbConnectionTest" /> class.
        /// </summary>
        /// <param name="name">The test name.</param>
        /// <param name="description">The test description.</param>
        /// <param name="isDefault">
        /// Flag indicating if test will be run when page loads. 
        /// True - test will run everytime page is loaded, False - test will be triggered manually by user
        /// </param>
        /// <param name="order">The order in which test will appear in the list.</param>
        public DbConnectionTest(string name, string description, bool isDefault, int order = 0)
        {
            this.name = name;
            this.description = description;
            this.isDefault = isDefault;
            this.order = order;
        }

        /// <summary>
        /// Gets the name for the test.
        /// </summary>
        /// <value></value>
        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// Gets or sets the order in which test appears.
        /// </summary>
        /// <value>The order.</value>
        public int Order
        {
            get
            {
                return this.order;
            }

            set
            {
                this.order = value;
            }
        }

        /// <summary>
        /// Gets the description for test.
        /// </summary>
        /// <value></value>
        public string Description
        {
            get { return this.description; }
        }

        /// <summary>
        /// Gets a value that indicate if the test is to run by default.
        /// </summary>
        /// <value></value>
        public bool IsDefault
        {
            get { return this.isDefault; }
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
            var settings = ConfigurationManager.ConnectionStrings.OfType<ConnectionStringSettings>().ToList();

            var failedSettings = new System.Collections.Concurrent.ConcurrentBag<ConnectionStringSettings>();
            var settingsCount = settings.Count();
            var processedCount = 0;
            ctx.UpdateProgress(0, processedCount, settingsCount);
            Parallel.ForEach(
                settings.ToList(),
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
                            if (cnn == null)
                            {
                                ctx.WriteLine(EventType.Error, Errors.TestFailed, setting.Name, Errors.CannotCreateConnection);
                                failedSettings.Add(setting);
                            }
                            else
                            {
                                cnn.ConnectionString = csBuilder.ToString();
                                cnn.Open();
                            }
                        }

                        ctx.WriteLine(EventType.Success, Labels.ConnectionSuccessful, setting.Name);
                    }
                    catch (Exception ex)
                    {
                        ctx.WriteLine(EventType.Error, Errors.TestFailed, setting.Name, ex.Message);
                        failedSettings.Add(setting);
                    }
                    processedCount++;
                    ctx.UpdateProgress(0, processedCount, settingsCount);
                });

            if (failedSettings.Count > 0)
            {
                var msg = string.Format(Errors.CannotOpenConnection, string.Join(", ", failedSettings.Select(x => x.Name).ToArray()));
                Assert.Fails(msg);
            }
        }
    }
}
