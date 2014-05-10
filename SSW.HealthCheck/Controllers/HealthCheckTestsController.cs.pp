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
    public class HealthCheckStatisticsController : ApiController
    {   
        /// <summary>
        /// Gets test run statistics
        /// </summary>
        /// <returns>Statistics of running test.</returns>
        public TestRunSummary Get()
        {
            var tests = HealthCheckService.Default.GetAll();
            var results = tests.Select(test => 
                new TestRunInstance
                    {
                        Result = test.Run()
                    }).ToList();
            return new TestRunSummary
                       {
                           IsHealthy = results.All(r => r.Result.Success),
                           Failed = results.Count(r => !r.Result.Success),
                           Passed = results.Count(r => r.Result.Success && !r.Result.ShowWarning),
                           Warnings = results.Count(r => r.Result.Success && r.Result.ShowWarning),
                           Total = results.Count()
                       };
        }
    }

    /// <summary>
    /// Health check API for tests. Allows running multiple or individual tests.
    /// </summary>
    public class HealthCheckTestsController : ApiController
    {   
        /// <summary>
        /// Run all tests.
        /// Sample request: GET api/healthchecktests
        /// </summary>
        /// <param name="id">The optional test id.</param>
        /// <returns>Collection of all test results</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public IEnumerable<TestRunInstance> Get(string id = null)
        {
            var tests = HealthCheckService.Default.GetAll().Where(t => string.IsNullOrEmpty(id) || t.Key == id);
            var results = tests.Select(test => 
                new TestRunInstance
                    {
                        Result = test.Run(), 
                        Key = test.Key,
                        Name = test.Name,
                        Description = test.Description,
                        Order = test.Order,
                        TestCategory = test.TestCategory
                    }).ToList();
            return results;
        }
    }
}