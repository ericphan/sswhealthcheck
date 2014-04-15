namespace SSW.HealthCheck.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration;
    using System.Data.Entity.ModelConfiguration.Configuration;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using System.Web;

    using SSW.Framework.Common;
    using SSW.HealthCheck.DomainModel.Entities;

    public class HealthCheckDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HealthCheckDbContext" /> class.
        /// </summary>
        public HealthCheckDbContext() : base("name=HealthCheckEntities")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HealthCheckDbContext" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public HealthCheckDbContext(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// Gets or sets the client applications.
        /// </summary>
        /// <value>The client applications.</value>
        public IDbSet<ClientApplication> ClientApplications { get; set; }

        /// <summary>
        /// Gets or sets the tests.
        /// </summary>
        /// <value>The tests.</value>
        public IDbSet<Test> Tests { get; set; }

        /// <summary>
        /// Gets or sets the test runs.
        /// </summary>
        /// <value>The test runs.</value>
        public IDbSet<TestRun> TestRuns { get; set; }

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateException">An error
        /// occurred sending updates to the database.</exception>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        /// A database command did not affect the expected number of rows. This usually indicates
        /// an optimistic
        /// concurrency violation; that is, a row has been changed in the database since
        /// it was queried.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Validation.DbEntityValidationException">
        /// The save was aborted because validation of entity property values failed.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        /// An attempt was made to use unsupported behavior such as executing multiple asynchronous
        /// commands concurrently
        /// on the same context instance.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The context or connection
        /// have been disposed.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// Some error occurred attempting to process entities in the context either before
        /// or after sending commands
        /// to the database.
        /// </exception>
        /// <returns>The number of objects written to the underlying database.</returns>
        public override int SaveChanges()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            var currentUsername = HttpContext.Current != null && HttpContext.Current.User != null
                ? HttpContext.Current.User.Identity.Name
                : "HealthCheck";

            foreach (var entity in entities)
            {
                var baseEntity = (BaseEntity)entity.Entity;
                if (entity.State == EntityState.Added)
                {
                    baseEntity.DateCreated = DateTime.Now;
                    baseEntity.UserCreated = currentUsername;
                    baseEntity.IsActive = true;
                }
                
                baseEntity.DateModified = DateTime.Now;
                baseEntity.UserModified = currentUsername;
            }

            var result = 0;
            try
            {
                result = base.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// This method is called when the model for a derived context has been initialized,
        /// but
        /// before the model has been locked down and used to initialize the context.  The
        /// default
        /// implementation of this method does nothing, but it can be overridden in a derived
        /// class
        /// such that the model can be further configured before it is locked down.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context
        /// being created.</param>
        /// <remarks>
        /// Typically, this method is called only once when the first instance of a derived
        /// context
        /// is created.  The model for that context is then cached and is for all further
        /// instances of
        /// the context in the app domain.  This caching can be disabled by setting the ModelCaching
        /// property on the given ModelBuilder, but note that this can seriously degrade
        /// performance.
        /// More control over caching is provided through use of the DbModelBuilder and DbContextFactory
        /// classes directly.
        /// </remarks>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            var addMethod = typeof(ConfigurationRegistrar)
                .GetMethods()
                .Single(m =>
                    m.Name == "Add" &&
                    m.GetGenericArguments().Any(a => a.Name == "TEntityType"));
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Namespace == "SSW.HealthCheck.Data.EntityConfig"))
            {
                if (type.IsGenericTypeOf(typeof(EntityTypeConfiguration<>)))
                {
                    if (type.BaseType != null)
                    {
                        var entityType = type.BaseType.GetGenericArguments().Single();
                        var entityConfig = Activator.CreateInstance(type);

                        addMethod.MakeGenericMethod(entityType).Invoke(modelBuilder.Configurations, new[] { entityConfig });
                    }
                }
            }
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
