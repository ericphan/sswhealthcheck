namespace SSW.HealthCheck.SQLDeploy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security;

    using SSW.SQLDeploy.Core;

    /// <summary>
    /// Custom helper for SQL Deploy tool. 
    /// </summary>
    public class SqlDeployDbHelper
    {
        public delegate void UpdateProgressHandler(object sender, SqlDeployUpdateProgressArgs e);
        public event UpdateProgressHandler UpdateProgress;

        private SqlDeploySettings _settings;
        private SqlDeploySecurity _security;

        public SqlDeploySecurity Security { get { return _security; } }
        public SqlDeploySettings Settings { get { return _settings; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDeployDbHelper" /> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="security">The security.</param>
        public SqlDeployDbHelper(SqlDeploySettings settings, SqlDeploySecurity security = null)
        {
            this._security = security;
            this._settings = settings;
        }

        /// <summary>
        /// Updates the databases.
        /// </summary>
        /// <returns>Result of each database update</returns>
        public IEnumerable<SqlDeployResult> UpdateDatabases()
        {
            if (this._security != null && !this._security.IsAllowed())
            {
                return new List<SqlDeployResult> { GetIsNotAllowedUpgradeResult() };
            }

            var updater = new SqlDeployUpdater(this.Settings.Databases);
            updater.UpdateProgress += (sender, e) =>
                {
                    if (this.UpdateProgress != null)
                    {
                        UpdateProgress(this, e);
                    }
                };

            return updater.Execute(true);
        }

        /// <summary>
        /// Updates the database.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Result of updating the database</returns>
        public SqlDeployResult UpdateDatabase(string name)
        {
            if (this._security != null && !this._security.IsAllowed())
            {
                return GetIsNotAllowedUpgradeResult();
            }

            var databaseToUpdate = this._settings.Databases.Single(d => d.DatabaseName.Equals(name, StringComparison.OrdinalIgnoreCase));
            var updater = new SqlDeployUpdater(databaseToUpdate);
            updater.UpdateProgress += (sender, e) =>
                {
                    if (this.UpdateProgress != null)
                    {
                        UpdateProgress(this, e);
                    }
                };
            return updater.Execute().Single();
        }

        /// <summary>
        /// Reconciles the databases.
        /// </summary>
        /// <returns>Result of each database reconciliation</returns>
        public IEnumerable<SqlDeployResult> ReconcileDatabases()
        {
            var updater = new SqlDeployUpdater(this.Settings.Databases);
            updater.UpdateProgress += (sender, e) =>
                {
                    if (this.UpdateProgress != null)
                    {
                        this.UpdateProgress(this, e);
                    }
                };

            return updater.Reconcile();
        }

        /// <summary>
        /// Reconciles the database.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Result of database reconciliation.</returns>
        public SqlDeployResult ReconcileDatabase(string name)
        {
            var databaseToUpdate = this._settings.Databases.Single(d => d.DatabaseName.Equals(name, StringComparison.OrdinalIgnoreCase));
            var updater = new SqlDeployUpdater(databaseToUpdate);
            updater.UpdateProgress += (sender, e) =>
                {
                    if (this.UpdateProgress != null)
                    {
                        this.UpdateProgress(this, e);
                    }
                };
            return updater.Reconcile().Single();
        }

        /// <summary>
        /// Gets the is not allowed upgrade result.
        /// </summary>
        /// <returns>Get not allowed result</returns>
        private static SqlDeployResult GetIsNotAllowedUpgradeResult()
        {
            return new SqlDeployResult 
            {
                Exceptions = new List<Exception> { new SecurityException("Not allowed to Update") },
                IsSuccessful = false
            };
        }
    }
}
