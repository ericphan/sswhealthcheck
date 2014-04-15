// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITestRepository.cs" company="SSW">
//   All rights reserved
// </copyright>
// <summary>
//   Test repository
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SSW.HealthCheck.Data.Interfaces
{
    using SSW.HealthCheck.DomainModel.Entities;

    /// <summary>
    /// Test repository
    /// </summary>
    public interface ITestRepository : IRepository<Test>
    {
    }
}
