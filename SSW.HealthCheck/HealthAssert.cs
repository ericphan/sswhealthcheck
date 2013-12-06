using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSW.HealthCheck
{
    public static class Assert
    {
        public static void Fails(string message)
        {
            throw new HealthException(message);
        }
        public static void Fails(string format, params object[] args)
        {
            Fails(string.Format(format, args));
        }
    }
}
