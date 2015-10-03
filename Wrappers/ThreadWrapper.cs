using System;

namespace Aragas.Core.Wrappers
{
    public delegate void WaitCallback(object state);
    public interface IThreadWrapper
    {
        Int32 StartThread(Action action, Boolean isBackground, String threadName);
        void AbortThread(Int32 id);

        Boolean IsRunning(Int32 id);

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

        public static int StartThread(Action action, bool isBackground, string threadName) { return Instance.StartThread(action, isBackground, threadName); }
        public static void AbortThread(int id) { Instance.AbortThread(id); }

        public static bool IsRunning(int id) { return Instance.IsRunning(id); }

        public static void Sleep(int milliseconds) { Instance.Sleep(milliseconds); }

        public static void QueueUserWorkItem(WaitCallback waitCallback) { Instance.QueueUserWorkItem(waitCallback); }
    }
}
