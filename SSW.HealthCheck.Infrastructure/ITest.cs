namespace SSW.HealthCheck.Infrastructure
{
    using System.Collections.Generic;

    public interface ITest
    {
        /// <summary>
        /// Gets the name for the test.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the description for test.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets a value indicating whether the test is to run by default.
        /// </summary>
        /// <value>The is default.</value>
        bool IsDefault { get; }

        /// <summary>
        /// Run the health check.
        /// </summary>
        /// <param name="context">The context.</param>
        void Test(ITestContext context);
    }
}
