using System;

namespace SSW.HealthCheck.Infrastructure.Tests
{
    /// <summary>
    /// Generic test class that can be extended
    /// </summary>
    public class GenericTest : ITest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericTest" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="isDefault">The is default.</param>
        /// <param name="testMethod">The test method.</param>
        public GenericTest(string name, string description, bool isDefault, Action<ITestContext> testMethod)
        {
            this.Name = name;
            this.Description = description;
            this.IsDefault = isDefault;
            this.Method = testMethod;
        }

        /// <summary>
        /// Gets the name for the test.
        /// </summary>
        /// <value></value>
        public string Name { get;private set; }

        /// <summary>
        /// Gets the description for test.
        /// </summary>
        /// <value></value>
        public string Description { get;private set; }

        /// <summary>
        /// Gets a value that indicate if the test is to run by default.
        /// </summary>
        /// <value></value>
        public bool IsDefault { get;private set; }

        /// <summary>
        /// Gets or sets the method.
        /// </summary>
        /// <value>The method.</value>
        protected Action<ITestContext> Method { get; set; }

        /// <summary>
        /// Run the health check.
        /// </summary>
        /// <param name="context"></param>
        public void Test(ITestContext context)
        {
            Method(context);
        }
    }
}
