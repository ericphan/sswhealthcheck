using System;

namespace SSW.HealthCheck.Infrastructure
{
    [Serializable]
    public class HealthException : Exception
    {
        public HealthException() { }
        public HealthException(string message) : base(message) { }
        public HealthException(string message, Exception inner) : base(message, inner) { }
        protected HealthException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [Serializable]
    public class PassedWithWarningException : Exception
    {
        public PassedWithWarningException() { }
        public PassedWithWarningException(string message) : base(message) { }
        public PassedWithWarningException(string message, Exception inner) : base(message, inner) { }
        protected PassedWithWarningException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
