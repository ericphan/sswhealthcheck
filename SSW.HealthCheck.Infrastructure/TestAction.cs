using System;

namespace SSW.HealthCheck.Infrastructure
{
    public class TestAction
    {
        public Guid ActionId { get; set; }

        public string ActionName { get; set; }

        public string ButtonStyle { get; set; }

        public Action<ITestContext> Action { get; set; }
    }
}
