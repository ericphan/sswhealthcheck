using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSW.HealthCheck
{
    public interface ITestContext
    {
        void UpdateProgress(int min, int val, int max);
        void WriteLine(string message);
    }

    public static class ITestContextExtensions
    {
        public static void WriteLine(this ITestContext ctx, string format, params object[] args)
        {
            ctx.WriteLine(string.Format(format, args));
        }
    }

}
