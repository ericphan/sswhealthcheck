// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlExceptionExtensions.cs" company="SSW">
//   All rights reserved
// </copyright>
// <summary>
//   Exceptions extensions
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SSW.HealthCheck.Data
{
    using System;
    using System.Data.SqlClient;

    using SSW.Framework.Common;
    using SSW.Framework.Common.Exceptions;

    /// <summary>
    /// Exceptions extensions
    /// </summary>
    public static class SqlExceptionExtensions
    {
        /// <summary>
        /// Convert to the data operation exception.
        /// </summary>
        /// <param name="databaseUpdateException">The database update exception.</param>
        /// <returns>Data operation exception.</returns>
        public static DataOperationException ToDataOperationException(this System.Data.Entity.Infrastructure.DbUpdateException databaseUpdateException)
        {
            DataOperationException result;
            
            Exception innerMostException = databaseUpdateException.InnerMostException();
            var sqlException = innerMostException as SqlException; // if (innerMostException is SqlException) if (innerMostException.GetType().IsAssignableFrom(typeof(SqlException)))
            if (sqlException != null)
            {
                result = sqlException.ToDataOperationException();
            }
            else
            {
                result = new DataOperationException("Unknown Data Error", databaseUpdateException);
            }

            return result;
        }

        /// <summary>
        /// Convert to the data operation exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>Data operation exception</returns>
        public static DataOperationException ToDataOperationException(this SqlException ex)
        {
            DataOperationException dataException = null;

            // Assume the interesting stuff is in the first error
            if (ex.Errors.Count > 0) 
            {
                switch (ex.Errors[0].Number)
                {
                    case 547: // Foreign Key violation
                        dataException = new DataOperationException("This record is in use.");
                        break;
                    case 2601: // Primary key violation
                        dataException = new DataOperationException("Primary key violation.");
                        break;
                    default:
                        dataException = new DataOperationException("Unknown data exception.");
                        break;
                }
            }

            return dataException;
        }
    }
}
