// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HealthCheckTestsController.cs" company="SSW">
//   All rights reserved
// </copyright>
// <summary>
//   Health check API for tests. Allows running multiple or individual tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SSW.HealthCheck.Mvc5.Examples.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Http;
    using System.Web.UI.WebControls.Expressions;

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
        public IEnumerable<TestRunInstance> Get()
        {
            var tests = HealthCheckService.Default.GetAll();
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
        
        /// <summary>
        /// Runs specific test.
        /// Sample request: GET api/healthchecktests/debug
        /// </summary>
        /// <param name="id">The test id.</param>
        /// <returns>Result of test run</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public TestRunInstance Get(string id)
        {
            var test = HealthCheckService.Default.GetByKey(id);
            var result = new TestRunInstance
                    {
                        Result = test.Run(), 
                        Key = test.Key,
                        Name = test.Name,
                        Description = test.Description,
                        Order = test.Order,
                        TestCategory = test.TestCategory
                    };

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