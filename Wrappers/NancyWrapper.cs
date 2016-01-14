using System;
using System.Collections.Generic;

namespace Aragas.Core.Wrappers
{
    public class NancyData
    {
        public class PageAction
        {
            public string Page { get; }
            public Func<dynamic, dynamic> Action { get; }

            public PageAction(string page, Func<dynamic, dynamic> action) { Page = page; Action = action; }
        }
        public List<PageAction> List { get; } = new List<PageAction>();
        
        public void Add(string page, Func<dynamic, dynamic> action) { List.Add(new PageAction(page, action));}
    }

    public interface INancyCreatorWrapper
    {
        void SetDataApi(NancyData data);

        void Start(String url, UInt16 port);
        void Stop();
    }

    public static class NancyWrapper
    {
        private static INancyCreatorWrapper _instance;
        public static INancyCreatorWrapper Instance
        {
            private get
            {
                if (_instance == null)
                    throw new NotImplementedException("This instance is not implemented. You need to implement it in your main project");
                return _instance;
            }
            set { _instance = value; }
        }
        
        public static void SetDataApi(NancyData data) { Instance.SetDataApi(data); }
        public static void Start(string url, ushort port) { Instance.Start(url, port); }
        public static void Stop() { Instance.Stop(); }
    }
}
