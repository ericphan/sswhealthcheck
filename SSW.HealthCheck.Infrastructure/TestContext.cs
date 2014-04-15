namespace SSW.HealthCheck.Infrastructure
{
    using System.Web;

    public interface ITestContext
    {
        void UpdateProgress(int min, int val, int max);

        void WriteLine(string message, EventType type);

        HttpContext HttpContext { get; set; }
    }

    public static class ITestContextExtensions
    {
        public static void WriteLine(this ITestContext ctx, string format, params object[] args)
        {
            ctx.WriteLine(EventType.Info, string.Format(format, args));
        }

        public static void WriteLine(this ITestContext ctx, EventType type, string format, params object[] args)
        {
            ctx.WriteLine(string.Format(format, args), type);
        }
    }
}
