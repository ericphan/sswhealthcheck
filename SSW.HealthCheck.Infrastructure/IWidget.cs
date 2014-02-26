namespace SSW.HealthCheck.Infrastructure
{
    using System.Collections.Generic;

    public interface IWidget : ITest
    {
        /// <summary>
        /// Gets the name for the test.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the description for test.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets a value that indicate if the test is to run by default.
        /// </summary>
        bool IsDefault { get; }

        /// <summary>
        /// Gets or sets the widget actions.
        /// </summary>
        /// <value>The widget actions.</value>
        IEnumerable<WidgetAction> WidgetActions { get; set; }
    }
}
