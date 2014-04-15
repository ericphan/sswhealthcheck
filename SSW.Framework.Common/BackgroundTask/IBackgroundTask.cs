using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSW.Framework.Common.BackgroundTask
{
    // simple interface for a background task that can report on its status and progress.
    public interface IBackgroundTask
    {

        void DoTask();


        BackgroundTaskStatus Status { get; set; }


        string StatusMessage { get; set; }


        decimal ProgressPercent { get; set;  }


        Exception Exception { get; set; }

    }


    public enum BackgroundTaskStatus
    {
        New = 0,
        Running = 1,
        Complete = 2,
        Error = 3
    }
}



