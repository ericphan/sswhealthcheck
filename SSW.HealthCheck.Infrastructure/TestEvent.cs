using System;

namespace SSW.HealthCheck.Infrastructure
{
    using System.Diagnostics;

    /// <summary>
    /// Different event types
    /// </summary>
    public enum EventType
    {
        /// <summary>
        /// The info - generic type, will be assigned by default.
        /// </summary>
        Info,

        /// <summary>
        /// The warning - will indicate warnings.
        /// </summary>
        Warning,
        
        /// <summary>
        /// The error - indicates error or exception log event
        /// </summary>
        Error,

        /// <summary>
        /// The success - will indicate successful execution or successful action
        /// </summary>
        Success
    }

    public class TestEvent
    {
        public TestEvent()
        {
            this.DateTime = DateTime.Now;
        }

        public DateTime DateTime { get; set; }

        public string Message { get; set; }

        public EventType EventType { get; set; }
    }
}
