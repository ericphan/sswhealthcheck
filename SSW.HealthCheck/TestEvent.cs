using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSW.HealthCheck
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
