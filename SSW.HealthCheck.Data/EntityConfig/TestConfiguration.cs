namespace SSW.HealthCheck.Data.EntityConfig
{
    using System.Data.Entity.ModelConfiguration;

    using SSW.HealthCheck.DomainModel.Entities;

    public class TestConfiguration : EntityTypeConfiguration<Test>
    {
        public TestConfiguration()
        {
            HasMany(c => c.TestRuns)
                .WithRequired(a => a.Test)
                .HasForeignKey(a => a.TestId)
                .WillCascadeOnDelete(true);
        }
    }
}
