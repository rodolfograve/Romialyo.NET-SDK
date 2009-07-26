using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;

namespace Romialyo
{
    public class WorkerThread
    {
        public delegate void WorkerActionDelegate();

        /// <summary>
        /// Creates an instance of WorkerThread, ready to be Started.
        /// </summary>
        /// <param name="id">Id of this WorkerThread. Useful for logging and debugging.</param>
        /// <param name="work">Action to run in a cycle until Stop is requested.</param>
        public WorkerThread(string id, WorkerActionDelegate work)
        {
            Id = id;
            Work = work;
            StopHandle = new AutoResetEvent(false);
        }

        public readonly string Id;

        protected readonly WorkerActionDelegate Work;
        protected readonly AutoResetEvent StopHandle;

        protected BackgroundWorker Worker;
        protected bool StopRequested;

        public event EventHandler<RunWorkerCompletedEventArgs> Stopped;

        /// <summary>
        /// Blocks until a new thread running 'Work' in a loop is started.
        /// The thread will run until Stop is called.
        /// </summary>
        public void Start()
        {
            Worker = new BackgroundWorker();
            Worker.DoWork += (x, e) =>
            {
                RunActionInLoop(e);
            };

            Worker.RunWorkerCompleted += (x, e) =>
            {
                RaiseStopped(e.Error);
            };
            Worker.RunWorkerAsync();
        }

        protected void RunActionInLoop(DoWorkEventArgs e)
        {
            try
            {
                while (!StopRequested)
                {
                    Work();
                }
            }
            finally
            {
                IsStopped = true;
                StopHandle.Set();
            }
        }

        protected void RaiseStopped(Exception ex)
        {
            if (Stopped != null)
            {
                Stopped(this, new RunWorkerCompletedEventArgs(null, ex, false));
            }
        }

        public bool IsStopped { get; protected set; }

        /// <summary>
        /// Blocks until stopped
        /// </summary>
        public void Stop()
        {
            if (!IsStopped)
            {
                StopRequested = true;
                StopHandle.WaitOne();
                IsStopped = true;
                RaiseStopped(null);
            }
        }

    }
}
