namespace SSW.HealthCheck.Data
{
    using SSW.HealthCheck.DomainModel.ObjectMothers;

    public static class SeedHealthCheckDb
    {
        public static void Seed(HealthCheckDbContext context)
        {
            var clientApplications = ClientApplicationsObjectMother.Build();
            clientApplications.ForEach(a => context.ClientApplications.Add(a));

            var tests = TestsObjectMother.Build();
            tests.ForEach(a => context.Tests.Add(a));

            var testRuns = TestRunsObjectMother.Build();
            testRuns.ForEach(a => context.TestRuns.Add(a));
        }
    }
}
