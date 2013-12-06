using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSW.HealthCheck
{
    public class TestProgress
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public int Val { get; set; }

        public double Percent
        {
            get
            {
                var cur = (Val - Min);
                var total = (Max - Min);
                if (total == 0)
                {
                    return 1;
                }
                else
                {
                    return cur / total;
                }
            }
        }
    }
}
