// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Configuration.cs" company="SSW">
//   All rights reserved
// </copyright>
// <summary>
//   Migrations configuration
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SSW.HealthCheck.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    /// <summary>
    /// Migrations configuration
    /// </summary>
    public sealed class Configuration : DbMigrationsConfiguration<HealthCheckDbContext>
    {
        /// <summary>
        /// Allow migrations to register seed actions to be performed after migration complete.
        /// </summary>
        private static readonly IList<Action<HealthCheckDbContext>> SeedActions = new List<Action<HealthCheckDbContext>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration" /> class.
        /// </summary>
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
        }

        /// <summary>
        /// Seeds the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        protected override void Seed(HealthCheckDbContext context)
        {
            // initial seed only
            if (!context.ClientApplications.Any())
            {
                SeedHealthCheckDb.Seed(context);
                context.SaveChanges();
            }

            // run all seed actions
            foreach (var action in SeedActions)
            {
                action(context);   
            }

            SeedActions.Clear();
        }
    }
}
