using System.Web.Configuration;

namespace SSW.HealthCheck.Infrastructure.Tests
{
    using System.Collections.Generic;

    /// <summary>
    /// Check if system is in debug mode
    /// </summary>
    public class NotDebugTest : ITest
    {
        private readonly string name = Labels.DebugModeTestTitle;
        private readonly string description = Labels.DebugModeTestDescription;
        private readonly bool isDefault = true;
        private int order;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotDebugTest" /> class.
        /// </summary>
        /// <param name="order">The order in which test will appear in the list.</param>
        public NotDebugTest(int order = 0)
        { 
            this.Order = order;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotDebugTest" /> class.
        /// </summary>
        /// <param name="name">The test name.</param>
        /// <param name="order">The order in which test will appear in the list.</param>
        public NotDebugTest(string name, int order = 0)
        {
            this.name = name;
            this.Order = order;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotDebugTest" /> class.
        /// </summary>
        /// <param name="name">The test name.</param>
        /// <param name="description">The test description.</param>
        /// <param name="order">The order in which test will appear in the list.</param>
        public NotDebugTest(string name, string description, int order = 0)
        {
            this.name = name;
            this.description = description;
            this.Order = order;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotDebugTest" /> class.
        /// </summary>
        /// <param name="isDefault">Run test by default.</param>
        /// <param name="order">The order in which test will appear in the list.</param>
        public NotDebugTest(bool isDefault, int order = 0)
        {
            this.isDefault = isDefault;
            this.Order = order;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotDebugTest" /> class.
        /// </summary>
        /// <param name="name">The test name.</param>
        /// <param name="description">The test description.</param>
        /// <param name="isDefault">
        /// Flag indicating if test will be run when page loads. 
        /// True - test will run everytime page is loaded, False - test will be triggered manually by user
        /// </param>
        /// <param name="order">The order in which test will appear in the list.</param>
        public NotDebugTest(string name, string description, bool isDefault, int order = 0)
        {
            this.name = name;
            this.description = description;
            this.isDefault = isDefault;
            this.Order = order;
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
        /// Gets the description for test.
        /// </summary>
        /// <value></value>
        public string Description
        {
            get { return this.description; }
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
            get { return this.isDefault; }
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
                    Assert.Fails(Errors.DebugModeIsTrue);
                }
            }
        }
    }
}
