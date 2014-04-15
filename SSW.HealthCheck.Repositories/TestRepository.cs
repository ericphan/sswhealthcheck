// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestRepository.cs" company="SSW">
//   All rights reserved
// </copyright>
// <summary>
//   Test repository implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SSW.HealthCheck.Repositories
{
    using SSW.Framework.Data.EF;
    using SSW.HealthCheck.Data.Interfaces;
    using SSW.HealthCheck.DomainModel.Entities;

    /// <summary>
    /// Test repository implementation
    /// </summary>
    public class TestRepository : BaseRepository<Test>, ITestRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestRepository" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public TestRepository(IDbContextManager context) : base(context)
        {
        }
    }
}
