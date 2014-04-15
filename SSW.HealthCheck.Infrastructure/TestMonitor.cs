namespace SSW.HealthCheck.Infrastructure
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;

    using Newtonsoft.Json;

    public class TestMonitor : ITestContext
    {
        private CancellationToken cancelToken = new CancellationToken();
        private ITest test;
        private Task<TestResult> task;

        [JsonIgnore]
        public HttpContext HttpContext { get; set; }

        public string Key { get; private set; }

        public string Name
        {
            get { return this.test.Name; }
        }

        public string Description
        {
            get { return this.test.Description; }
        }

        public int Order
        {
            get { return this.test.Order; }
        }

        public bool IsDefault { get { return this.test.IsDefault; } }
        public bool IsRunning
        {
            get
            {
                return task != null;
            }
        }

        public event EventHandler Started;
        public event EventHandler Completed;
        public event EventHandler<TestEvent> EventReceived;
        public event EventHandler<TestProgress> ProgressChanged;

        /// <summary>
        /// Get the current test progress if it's running. Returns null if test is not running.
        /// </summary>
        public TestProgress Progress { get; private set; }

        public List<TestEvent> Events { get; private set; }

        public TestResult Result { get; private set; }

        public TestMonitor(HealthCheckService service, string key, ITest test)
        {
            this.Key = key;
            this.test = test;
            this.Result = null;
            this.Events = new List<TestEvent>();
            this.cancelToken = new CancellationToken();
        }

        private TestResult RunCore()
        {
            this.Events.Clear();
            //while (this.Events.Any())
            //{
            //    TestEvent result;
            //    if (!this.Events.TryDequeue(out result))
            //    {
            //        break;
            //    }
            //}

            this.Result = null;
            this.OnStarted();

            var r = new TestResult();
            try
            {
                test.Test(this);
                r.Message = string.Empty;
                r.Success = true;
            }
            catch (Exception ex)
            {
                r.Message = ex.Message;
                r.Success = false;
            }
            return r;
        }

        private TestResult RunComplete(Task<TestResult> t)
        {
            this.Result = t.Result;
            this.task = null;
            this.OnCompleted();
            return t.Result;
        }

        public TestResult Run()
        {
            var watch = Stopwatch.StartNew();
            var t = this.RunAsync();
            t.Wait();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            var result = t.Result;
            result.RunningTime = elapsedMs;

            return result;
        }

        public Task<TestResult> RunAsync()
        {
            lock (this)
            {
                if (task == null)
                {
                    task = Task.Run<TestResult>((Func<TestResult>)this.RunCore, this.cancelToken).ContinueWith<TestResult>(this.RunComplete);
                }

                return task;
            }
        }

        protected void OnStarted()
        {
            if (Started != null) Started(this, EventArgs.Empty);
        }

        protected void OnCompleted()
        {
            if (Completed != null) Completed(this, EventArgs.Empty);
        }

        protected void OnProgressChanged(TestProgress p)
        {
            this.Progress = p;

            if (ProgressChanged != null)
            {
                ProgressChanged(this, p);
            }
        }
        protected void OnTestEvent(TestEvent e)
        {
            this.Events.Add(e);
            //this.Events.Enqueue(e);

            if (EventReceived != null)
            {
                EventReceived(this, e);
            }
        }

        #region ITestContext
        
        public void UpdateProgress(int min, int val, int max)
        {
            OnProgressChanged(new TestProgress()
            {
                Min = min,
                Val = val,
                Max = max
            });
        }

        public void WriteLine(string message, EventType type = EventType.Info)
        {
            this.OnTestEvent(new TestEvent()
            {
                DateTime = DateTime.Now,
                Message = message,
                EventType = type
            });
        }

        #endregion
    }
}
