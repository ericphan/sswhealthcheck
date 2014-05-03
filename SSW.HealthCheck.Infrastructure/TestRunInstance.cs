namespace SSW.HealthCheck.Infrastructure
{
    public class TestRunInstance
    {
        /// <summary>
        /// Gets the name for the test.
        /// </summary>
        /// <value></value>
        public string Key { get; set; }

        /// <summary>
        /// Gets the name for the test.
        /// </summary>
        /// <value></value>
        public string Name { get; set; }

        /// <summary>
        /// Gets the description for test.
        /// </summary>
        /// <value></value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the order in which test appears.
        /// </summary>
        /// <value>The order.</value>
        public int Order { get; set; }

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
        /// <value></value>
        public bool IsDefault { get; private set; }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        public TestResult Result { get; set; }
    }
}
