using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using SSW.Framework.Common;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;

namespace SSW.Framework.Data.EF
{
    public class MigrationsZsValidateProvider : IZsValidateProvider
    {
        private readonly string _connectionName;
        private readonly DbMigrationsConfiguration _migrationsConfiguration;
        private readonly string _label;

        public MigrationsZsValidateProvider(string connectionName, DbMigrationsConfiguration migrationsConfiguration, string label)
        {
            _connectionName = connectionName;
            _migrationsConfiguration = migrationsConfiguration;
            _label = label;
        }

        public ZsValidateItem Validate()
        {
            var result = new ZsValidateItem() { Title = _label };
            try
            {
                _migrationsConfiguration.TargetDatabase = new DbConnectionInfo(_connectionName);

                var migrator = new DbMigrator(_migrationsConfiguration);


                if (migrator.GetPendingMigrations().Any())
                {
                    result.State = ZsValidateState.Fail;
                    result.Description = "<b>Pending Migrations Found</b><br/>";
                    result.Description += "<ul>";
                    foreach (var migration in migrator.GetPendingMigrations().OrderBy(b => b))
                    {
                        result.Description += "<li>" + migration + "</li>";
                    }
                    result.Description += "</ul>";

                }
                else
                {
                    result.State = ZsValidateState.Ok;
                    result.Description = "<b>Migrations Up to Date</b><br/>";
                    result.Description += "<ul>";
                    foreach (var migration in migrator.GetDatabaseMigrations().OrderBy(b => b))
                    {
                        result.Description += "<li>" + migration + "</li>";
                    }
                    result.Description += "</ul>";
                }

            }
            catch (Exception ex)
            {
                result.State = ZsValidateState.Fail;
                result.Description = "Exception occurred: " + ex.Message;
            }
            return result;
        }
    }
}
