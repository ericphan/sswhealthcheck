using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSW.HealthCheck.Mvc5
{
    using SSW.HealthCheck.Infrastructure;

    /// <summary>
    /// Send health check changes and events to a clients via SignalR hub.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HealthCheckHubNotifier<T> where T : Microsoft.AspNet.SignalR.Hubs.IHub
    {
        private HealthCheckService service;

        public HealthCheckHubNotifier(HealthCheckService svc)
        {
            service = svc;
            service.TestCompleted += service_TestCompleted;
            service.TestStarted += service_TestStarted;
            service.TestEventReceived += service_TestEventReceived;
            service.TestProgressChanged += service_TestProgressChanged;
        }

        void service_TestStarted(object sender, EventArgs e)
        {
            OnTestStarted((TestMonitor)sender);
        }
        void service_TestCompleted(object sender, EventArgs e)
        {
            OnTestCompleted((TestMonitor)sender);
        }
        void service_TestEventReceived(object sender, TestEvent e)
        {
            OnTestEvent((TestMonitor)sender, e);
        }
        void service_TestProgressChanged(object sender, TestProgress p)
        {
            OnTestProgressChanged((TestMonitor)sender, p);
        }

        protected IHubContext GetHub()
        {
            return GlobalHost.ConnectionManager.GetHubContext<T>();
        }

        protected void OnTestStarted(TestMonitor test)
        {
            var hub = GetHub();
            hub.Clients.All.testStarted(new
            {
                test.Key,
                test.IsRunning,
                test.Result
            });
        }
        protected void OnTestCompleted(TestMonitor test)
        {
            var hub = GetHub();
            hub.Clients.All.testCompleted(new
            {
                test.Key,
                test.IsRunning,
                test.Result
            });
        }
        protected void OnTestProgressChanged(TestMonitor test, TestProgress progress)
        {
            var hub = GetHub();
            hub.Clients.All.testProgress(new
            {
                Key = test.Key,
                Progress = progress
            });
        }
        protected void OnTestEvent(TestMonitor test, TestEvent ev)
        {
            var hub = GetHub();
            hub.Clients.All.testEvent(new
            {
                Key = test.Key,
                Event = ev
            });
        }
    }

    public static class HealthCheckHubNotifierExtensions
    {
        public static HealthCheckHubNotifier<T> Setup<T>(this HealthCheckService svc) where T : Microsoft.AspNet.SignalR.Hubs.IHub
        {
            return new HealthCheckHubNotifier<T>(svc);
        }
    }
}
