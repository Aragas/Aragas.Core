using System;

namespace Aragas.Core.Wrappers
{
    public delegate void ThreadStart();
    public delegate void ParameterizedThreadStart(object obj);
    public interface IThread
    {
        string Name { get; set; }
        bool IsBackground { get; set; }

        bool IsRunning { get; }

        void Start();
        void Start(object obj);

        void Abort();
    }

    public delegate void WaitCallback(object state);
    public interface IThreadWrapper
    {
        //IThread CreateThread(Action action);
        IThread CreateThread(ThreadStart action);
        IThread CreateThread(ParameterizedThreadStart action);

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

        //public static IThread CreateThread(Action action) {  return Instance.CreateThread(action); }
        public static IThread CreateThread(ThreadStart action) { return Instance.CreateThread(action); }
        public static IThread CreateThread(ParameterizedThreadStart action) { return Instance.CreateThread(action); }

        public static void Sleep(int milliseconds) { Instance.Sleep(milliseconds); }

        public static void QueueUserWorkItem(WaitCallback waitCallback) { Instance.QueueUserWorkItem(waitCallback); }
    }
}
