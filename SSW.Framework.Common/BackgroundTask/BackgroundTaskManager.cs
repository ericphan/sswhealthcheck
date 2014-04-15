using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using StructureMap;

namespace SSW.Framework.Common.BackgroundTask
{
    public class BackgroundTaskManager
    {

        private BackgroundTaskManager()
        {
            Tasks = new Dictionary<int, IBackgroundTask>();
        }

        private int _id = 0;

        private static BackgroundTaskManager _instance;


        /// <summary>
        /// singleton instance of task manager
        /// </summary>
        public static BackgroundTaskManager Instance
        {
            get
            {
                if (_instance == null) _instance = new BackgroundTaskManager();
                return _instance;
            }
        }



        private Dictionary<int, IBackgroundTask> Tasks
        {
            get; set;
        }


        public int AddTask(IBackgroundTask task)
        {
            int id = ++_id;
            Tasks[id] = task;

            Task.Run(() =>
            {
                ObjectFactory.BuildUp(task);
                task.DoTask();
            });

            return id;
        }


        public IBackgroundTask GetTask(int id)
        {
            if (Tasks.ContainsKey(id))
            {
                return Tasks[id];
            }
            return null;
        }


    }
}
