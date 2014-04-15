// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITestRunRepository.cs" company="SSW">
//   All rights reserved
// </copyright>
// <summary>
//   Test run repository
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SSW.HealthCheck.Data.Interfaces
{
    using SSW.HealthCheck.DomainModel.Entities;

    /// <summary>
    /// Test run repository
    /// </summary>
    public interface ITestRunRepository : IRepository<TestRun>
    {
    }
}
