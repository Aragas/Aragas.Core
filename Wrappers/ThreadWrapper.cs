using System;

namespace Aragas.Core.Wrappers
{
    public delegate void ThreadStart();
    public delegate void ParameterizedThreadStart(object obj);
    public interface IThread
    {
        String Name { get; set; }
        Boolean IsBackground { get; set; }

        Boolean IsRunning { get; }

        void Start();
        void Start(Object obj);

        void Abort();
    }

    public delegate void WaitCallback(object state);
    public interface IThreadWrapper
    {
        int Threads { get; }

        //IThread CreateThread(Action action);
        IThread Create(ThreadStart action);
        IThread Create(ParameterizedThreadStart action);

        void Sleep(Int32 milliseconds);

        void QueueUserWorkItem(WaitCallback waitCallback);
    }

    /// <summary>
    /// Exception handling in Task? Never heard of that.
    /// </summary>
    public static class ThreadWrapper
    {
        private static IThreadWrapper _instance;
        public static IThreadWrapper Instance
        {
            private get
            {
                if (_instance == null)
                    throw new NotImplementedException("This instance is not implemented. You need to implement it in your main project");
                return _instance;
            }
            set { _instance = value; }
        }

        public static int Threads => _instance.Threads;

        public static IThread Create(ThreadStart action) { return Instance.Create(action); }
        public static IThread Create(ParameterizedThreadStart action) { return Instance.Create(action); }

        public static void Sleep(int milliseconds) { Instance.Sleep(milliseconds); }

        public static void QueueUserWorkItem(WaitCallback waitCallback) { Instance.QueueUserWorkItem(waitCallback); }
    }
}
