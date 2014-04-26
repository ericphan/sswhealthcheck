using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSW.HealthCheck.Infrastructure
{
    public class TestGroup
    {
        public string Name { get; set; }

        public IEnumerable<TestMonitor> TestMonitors { get; set; }
    }
}
