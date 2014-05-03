// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HealthCheckTestsController.cs" company="SSW">
//   All rights reserved
// </copyright>
// <summary>
//   Health check API for tests. Allows running multiple or individual tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace $rootnamespace$.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Http;

    using SSW.HealthCheck.Infrastructure;

    /// <summary>
    /// Health check API for tests. Allows running multiple or individual tests.
    /// </summary>
    public class HealthCheckTestsController : ApiController
    {    
        /// <summary>
        /// Run all tests.
        /// Sample request: GET api/healthchecktests
        /// </summary>
        /// <returns>Collection of all test results</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public IEnumerable<TestResult> Get()
        {
            var tests = HealthCheckService.Default.GetAll();
            var results = tests.Select(test => test.Run()).ToList();
            return results;
        }
        
        /// <summary>
        /// Runs specific test.
        /// Sample request: GET api/healthchecktests/debug
        /// </summary>
        /// <param name="id">The test id.</param>
        /// <returns>Result of test run</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public TestResult Get(string id)
        {
            var test = HealthCheckService.Default.GetByKey(id);
            var result = test.Run();

            return result;
        }

        /// <summary>
        /// Get list of all test ids/keys.
        /// Sample request: GET api/healthchecktests/gettestids
        /// </summary>
        /// <returns>Collection of all test results</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public IEnumerable<string> GetTestIds()
        {
            var tests = HealthCheckService.Default.GetAll();
            var results = tests.Select(test => test.Key).ToList();
            return results;
        }
    }
}