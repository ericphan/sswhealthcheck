using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SSW.HealthCheck
{
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
        /// Gets a value that indicate if the test is to run by default.
        /// </summary>
        bool IsDefault { get; }

        /// <summary>
        /// Run the health check.
        /// </summary>
        void Test(ITestContext context);
    }
}
