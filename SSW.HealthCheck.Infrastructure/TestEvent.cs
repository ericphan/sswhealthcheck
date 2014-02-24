using System;

namespace SSW.HealthCheck.Infrastructure
{
    public class TestEvent
    {
        public TestEvent()
        {
            this.DateTime = DateTime.Now;
        }

        public DateTime DateTime { get; set; }

        public string Message { get; set; }
    }
}
