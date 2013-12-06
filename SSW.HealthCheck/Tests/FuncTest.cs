using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSW.HealthCheck.Tests
{
    public class FuncTest : ITest
    {
        public FuncTest(string name, string description, bool isDefault, Action<ITestContext> testMethod)
        {
            this.Name = name;
            this.Description = description;
            this.IsDefault = isDefault;
            this.Method = testMethod;
        }

        public string Name
        {
            get;
            private set;
        }

        public string Description
        {
            get;
            private set;
        }

        public bool IsDefault
        {
            get;
            private set;
        }

        protected Action<ITestContext> Method { get; set; }

        public void Test(ITestContext context)
        {
            Method(context);
        }
    }
}
