// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitOfWork.cs" company="SSW">
//   All rights reserved
// </copyright>
// <summary>
//   Exceptions extensions
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SSW.HealthCheck.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    using SSW.Framework.Common;
    using SSW.Framework.Data.EF;
    using SSW.HealthCheck.Data.Interfaces;

    /// <summary>
    /// Unit of work implementation
    /// </summary>
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        /// <summary>
        /// The context list.
        /// </summary>
        private readonly IEnumerable<IDbContextManager> contextList;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork" /> class.
        /// </summary>
        /// <param name="contextList">The context list.</param>
        /// <param name="logger">The logger.</param>
        public UnitOfWork(IEnumerable<IDbContextManager> contextList, ILogger logger)
        {
            this.contextList = contextList;
            this.logger = logger;
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        public void SaveChanges()
        {
            try
            {
                foreach (var contextManager in this.contextList)
                {
                    if (contextManager.HasContext)
                    {
                        contextManager.Context.SaveChanges();
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                this.logger.Error(sqlEx.Message, sqlEx);
                sqlEx.ToDataOperationException();
                throw;
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException due)
            {
                this.logger.Error(due.Message, due);
                due.ToDataOperationException();
                throw;
            }
            catch (Exception ex)
            {
                this.logger.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            foreach (var contextManager in this.contextList)
            {
                contextManager.Dispose();
            }
        }
    }
}
