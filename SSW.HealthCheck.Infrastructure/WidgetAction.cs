using System;

namespace SSW.HealthCheck.Infrastructure
{
    public class WidgetAction
    {
        Guid ActionId { get; set; }

        Guid ActionName { get; set; }

        string ButtonStyle { get; set; }

        Action<ITestContext> Action { get; set; }
    }
}
