namespace SSW.HealthCheck.Infrastructure
{
    public class TestRunSummary
    {
        public bool IsHealthy { get; set; }

        public int Failed { get; set; }

        public int Passed { get; set; }

        public int Warnings { get; set; }

        public int Total { get; set; }
    }
}
