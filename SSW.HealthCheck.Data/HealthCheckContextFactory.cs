namespace SSW.HealthCheck.Data
{
    using System.Data.Entity;

    using SSW.Framework.Data.EF;

    public class HealthCheckContextFactory : IDbContextFactory<HealthCheckDbContext>
    {
        /// <summary>
        /// The database initializer.
        /// </summary>
        private readonly IDatabaseInitializer<HealthCheckDbContext> databaseInitializer;

        /// <summary>
        /// The flag indicating if initializer is set.
        /// </summary>
        private bool hasSetInitializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="HealthCheckContextFactory" /> class.
        /// </summary>
        /// <param name="databaseInitializer">The database initializer.</param>
        public HealthCheckContextFactory(IDatabaseInitializer<HealthCheckDbContext> databaseInitializer)
        {
            this.databaseInitializer = databaseInitializer;
        }

        /// <summary>
        /// Builds this instance.
        /// </summary>
        /// <returns>Created database context.</returns>
        public HealthCheckDbContext Build()
        {
            if (!this.hasSetInitializer)
            {
                Database.SetInitializer(this.databaseInitializer);

                this.hasSetInitializer = true;
            }

            return new HealthCheckDbContext();
        }
    }
}
