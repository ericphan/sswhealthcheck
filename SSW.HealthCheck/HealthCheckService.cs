using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSW.HealthCheck
{
    /// <summary>
    /// 
    /// </summary>
    public class HealthCheckService
    {
        public static readonly HealthCheckService Default = new HealthCheckService();

        private ConcurrentDictionary<string, ITest> tests = new ConcurrentDictionary<string, ITest>();
        private ConcurrentDictionary<string, TestMonitor> monitors = new ConcurrentDictionary<string, TestMonitor>();

        public event EventHandler TestStarted;
        public event EventHandler TestCompleted;
        public event EventHandler<TestEvent> TestEventReceived;
        public event EventHandler<TestProgress> TestProgressChanged;

        protected void OnTestStarted(object sender, EventArgs e)
        {
            if (TestStarted != null) 
                TestStarted(sender, e);
        }
        protected void OnTestCompleted(object sender, EventArgs e)
        {
            if (TestCompleted != null) 
                TestCompleted(sender, e);
        }
        protected void OnTestEventReceived(object sender, TestEvent e)
        {
            if (TestEventReceived != null)
                TestEventReceived(sender, e);
        }
        protected void OnProgressChanged(object sender, TestProgress p)
        {
            if (TestProgressChanged != null)
                TestProgressChanged(sender, p);
        }

        public TestMonitor Add(ITest test)
        {
            var key = test.GetType().FullName;
            var keyFormat = key + ".{0}";
            var i = 1;
            while (!tests.TryAdd(key, test))
            {
                key = string.Format(keyFormat, i);
                i++;
            }
            var monitor = monitors.GetOrAdd(key, (k) =>
            {
                var m = new TestMonitor(this, k, test);
                m.Started += OnTestStarted;
                m.Completed += OnTestCompleted;
                m.EventReceived += OnTestEventReceived;
                m.ProgressChanged += OnProgressChanged;
                return m;
            });
            return monitor;
        }

        public IEnumerable<TestMonitor> GetAll()
        {
            return monitors.Values;
        }

        public TestMonitor GetByKey(string key)
        {
            TestMonitor monitor;
            if (monitors.TryGetValue(key, out monitor))
            {
                return monitor;
            }
            else
            {
                throw new ArgumentException(string.Format("TestMonitor with key {0} cannot be found.", key));
            }
        }
    }

}
