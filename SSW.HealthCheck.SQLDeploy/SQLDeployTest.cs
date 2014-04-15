namespace SSW.HealthCheck.SQLDeploy
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;

    using SSW.HealthCheck.Infrastructure;
    using SSW.SQLDeploy.Core;

    public class SQLDeployTest : ITest
    {
        private readonly SqlDeployDbHelper sqlDeploy;

        private readonly string name = Titles.TestTitle;

        private readonly string description = Titles.TestDescription;

        private readonly bool isDefault = false;

        private int order;

        /// <summary>
        /// Initializes a new instance of the <see cref="SQLDeployTest" /> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="order">The order in which test will appear in the list of tests.</param>
        /// <param name="security">The security.</param>
        public SQLDeployTest(SqlDeploySettings settings, int order = 0, SqlDeploySecurity security = null)
        {
            this.sqlDeploy = new SqlDeployDbHelper(settings, security);
            this.Order = order;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SQLDeployTest" /> class.
        /// </summary>
        /// <param name="isDefault">The is default.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="order">The order in which test will appear in the list of tests.</param>
        /// <param name="security">The security.</param>
        public SQLDeployTest(bool isDefault, SqlDeploySettings settings, int order = 0, SqlDeploySecurity security = null)
        {
            this.sqlDeploy = new SqlDeployDbHelper(settings, security);
            this.isDefault = isDefault;
            this.Order = order;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SQLDeployTest" /> class.
        /// </summary>
        /// <param name="isDefault">The is default.</param>
        /// <param name="name">The test title.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="order">The order in which test will appear in the list of tests.</param>
        /// <param name="security">The security.</param>
        public SQLDeployTest(bool isDefault, string name, SqlDeploySettings settings, int order = 0, SqlDeploySecurity security = null)
        {
            this.sqlDeploy = new SqlDeployDbHelper(settings, security);
            this.isDefault = isDefault;
            this.name = name;
            this.Order = order;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SQLDeployTest" /> class.
        /// </summary>
        /// <param name="isDefault">The is default.</param>
        /// <param name="name">The test title.</param>
        /// <param name="description">The test description.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="order">The order in which test will appear in the list of tests.</param>
        /// <param name="security">The security.</param>
        public SQLDeployTest(bool isDefault, string name, string description, SqlDeploySettings settings, int order = 0, SqlDeploySecurity security = null)
        {
            this.sqlDeploy = new SqlDeployDbHelper(settings, security);
            this.isDefault = isDefault;
            this.name = name;
            this.description = description;
            this.Order = order;
        }

        /// <summary>
        /// Gets the name for the test.
        /// </summary>
        /// <value></value>
        public string Name
        {
            get
            {
                return this.name;
            }
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
            get
            {
                return this.description;
            }
        }

        /// <summary>
        /// Gets or sets the test category. Used for grouping of tests
        /// </summary>
        /// <value>The test category.</value>
        public TestCategory TestCategory { get; set; }

        /// <summary>
        /// Gets a value indicating whether test belongs to a category.
        /// </summary>
        /// <value></value>
        public bool HasCategory
        {
            get
            {
                return this.TestCategory != null;
            }
        }

        /// <summary>
        /// Gets a value that indicate if the test is to run by default.
        /// </summary>
        /// <value>Is default.</value>
        public bool IsDefault
        {
            get
            {
                return this.isDefault;
            }
        }

        public void Test(ITestContext context)
        {
            var failedChecks = new System.Collections.Concurrent.ConcurrentBag<SqlDeployResult>();
            var databasesCount = this.sqlDeploy.Settings.Databases.Count();
            var processedCount = 0;
            var databases = this.sqlDeploy.Settings.Databases.ToList();
            context.UpdateProgress(0, processedCount, databasesCount);
            Parallel.ForEach(
                databases,
                database =>
                {
                    var csBuilder = new SqlConnectionStringBuilder(database.ConnectionString);
                    var dbName = csBuilder.InitialCatalog;
                    var upToDate = database.Status.IsUpToDate;
                    var reconcileResult = this.sqlDeploy.ReconcileDatabase(database.DatabaseName);
                    if (upToDate && reconcileResult.IsSuccessful)
                    {
                        context.WriteLine(EventType.Success, Titles.DbIsUpToDate, dbName);
                    }
                    else
                    {
                        if (!upToDate)
                        {
                            context.WriteLine(EventType.Error, Titles.DbCheckFailed, dbName, Titles.SchemaNotUpToDate);
                            var result = new SqlDeployResult { IsSuccessful = false, DatabaseName = dbName, Exceptions = new List<Exception> { new Exception(Titles.SchemaNotUpToDate) } };
                            failedChecks.Add(result);
                        }
                        else
                        {
                            context.WriteLine(EventType.Error, Titles.DbCheckFailed, dbName, Titles.SchemaChanged);
                            failedChecks.Add(reconcileResult);
                        }
                    }

                    processedCount++;
                    context.UpdateProgress(0, processedCount, databasesCount);
                });
            
            if (failedChecks.Any())
            {
                var msg = string.Format(Titles.DbsNotUpToDate, string.Join(", ", failedChecks.Select(x => x.DatabaseName).ToArray()));
                Assert.Fails(msg);
            }
        }

        public void Update(ITestContext context)
        {
        }

        public IEnumerable<TestAction> TestActions
        {
            get
            {
                return new List<TestAction>
                           {
                               new TestAction
                                   {
                                       Action = this.Update, 
                                       ActionId = Guid.NewGuid(), 
                                       ActionName = Titles.Update
                                   }
                           };
            }
        }
    }
}
