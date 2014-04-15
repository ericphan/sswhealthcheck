// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IClientApplicationRepository.cs" company="SSW">
//   All rights reserved
// </copyright>
// <summary>
//   Client application repository
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SSW.HealthCheck.Data.Interfaces
{
    using SSW.HealthCheck.DomainModel.Entities;

    /// <summary>
    /// Client application repository
    /// </summary>
    public interface IClientApplicationRepository : IRepository<ClientApplication>
    {
    }
}
