using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSW.HealthCheck
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
}
