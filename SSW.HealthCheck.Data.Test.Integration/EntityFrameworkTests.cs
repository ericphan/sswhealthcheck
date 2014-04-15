namespace SSW.HealthCheck.Data.Test.Integration
{
    using System;
    using System.Data.SqlClient;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using SSW.Framework.Data.EF;
    using SSW.HealthCheck.DomainModel.Entities;

    [TestClass]
    public class EntityFrameworkTests
    {
        private static StructureMap.IContainer container;
        private static HealthCheckDbContext dbContext;
        private static bool dbCreated = false;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            container = DependencyResolution.IoC.Initialize();
            var contextFactory = container.GetInstance<IDbContextFactory<HealthCheckDbContext>>();
            dbContext = contextFactory.Build();

            Assert.IsNotNull(dbContext);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            dbContext.Dispose();

            if (!dbCreated)
            {
                return;
            }

            var builder =
                new SqlConnectionStringBuilder(
                    System.Configuration.ConfigurationManager.ConnectionStrings["HealthCheckEntities"].ConnectionString);

            using (var conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["HealthCheckEntities"].ConnectionString))
            {
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText =
                    string.Format("use master; alter database {0} set single_user with rollback immediate; drop database {0}", builder.InitialCatalog);
                command.ExecuteNonQuery();
            }

            container = null;
        }

        [TestMethod]
        public void Test01ConnectionString()
        {
            dbCreated = false;
            Assert.IsNotNull(System.Configuration.ConfigurationManager.ConnectionStrings["HealthCheckEntities"], "Cannot find HealthCheckCMS connection string");
        }

        [TestMethod]
        public void Test02DbContext()
        {
            dbCreated = false;
            Assert.IsNotNull(dbContext);
        }

        [TestMethod]
        public void Test03TestOnDelete()
        {
            dbCreated = true;
            var test = new Test
            {
                Name = "Test",
                ClientApplication = new ClientApplication
                                        {
                                            Name = "Test app",
                                            Key = Guid.NewGuid(),
                                            IsActive = true
                                        },
                IsActive = true,
                Key = "Test"
            };
            dbContext.Tests.Add(test);

            // save and refresh context
            dbContext.SaveChanges();
            dbContext.Dispose();
            var contextFactory = container.GetInstance<IDbContextFactory<HealthCheckDbContext>>();
            dbContext = contextFactory.Build();

            var beforeDelete = dbContext.Tests.Count();
            Assert.IsTrue(beforeDelete > 0, "No tests after save.");

            var test2 = dbContext.Tests.First(g => g.Id == test.Id); // reload from new context
            dbContext.Tests.Remove(test2);
            dbContext.SaveChanges();

            int afterDelete = dbContext.Tests.Count();

            Assert.AreEqual(beforeDelete - 1, afterDelete, "Tests should be deleted.");
        }
    }
}
