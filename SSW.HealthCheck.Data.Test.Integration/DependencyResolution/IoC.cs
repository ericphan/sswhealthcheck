// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IoC.cs" company="SSW">
//   All rights reserved
// </copyright>
// <summary>
//   Defines the IoC type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SSW.HealthCheck.Data.Test.Integration.DependencyResolution
{
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Net.NetworkInformation;

    using SSW.Framework.Data.EF;
    using SSW.SqlVerify.Core;

    using StructureMap;
    using StructureMap.Graph;
    using StructureMap.Pipeline;
    using StructureMap.Web;

    /// <summary>
    /// Inversion of control helper class
    /// </summary>
    public static class IoC
    {
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <returns>IoC container.</returns>
        public static IContainer Initialize()
        {
            ObjectFactory.Initialize(x =>
            {
                x.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });

                // HealthCheckDbContext - Last DBContext is the default
                x.For<DbMigrationsConfiguration<HealthCheckDbContext>>().Use<Migrations.Configuration>();
                x.For<IDatabaseInitializer<HealthCheckDbContext>>().Use<UpdateMigrationsInitializer<HealthCheckDbContext>>();

                x.For<IDbContextFactory<HealthCheckDbContext>>().Use<HealthCheckContextFactory>();
                x.For<IDbContextManager<HealthCheckDbContext>>().HybridHttpOrThreadLocalScoped().Use<DbContextManager<HealthCheckDbContext>>();


                // wire SqlVerify to HealthCheckDbContext
                x.For<ISqlVerify>().Use<SqlVerify>();
                x.For<ISqlVerifyConfiguration>().Use<SqlVerifyConfiguration>();
                x.For<IDbConnectionFactory>().Use<SqlConnectionFactory>().Ctor<string>().Is(System.Configuration.ConfigurationManager.ConnectionStrings["HealthCheckEntities"].ConnectionString);
            });
            
            return ObjectFactory.Container;
        }
    }
}
