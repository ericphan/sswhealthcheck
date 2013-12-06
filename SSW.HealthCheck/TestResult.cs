using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSW.HealthCheck
{
    public class TestResult
    {
        /// <summary>
        /// Gets or sets a value that indicate if the test passed.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets a message that describe the result of the test.
        /// </summary>
        public string Message { get; set; }
    }
}
