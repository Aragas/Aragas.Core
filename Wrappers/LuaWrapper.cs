using System;

namespace Aragas.Core.Wrappers
{
    public interface ILua
    {
        Object this[string index] { get; set; }

        Object[] CallFunction(string functionName, params object[] args);
    }

    public interface ILuaWrapper
    {
        ILua Create();
        ILua Create(string scriptName);
    }

    public static class LuaWrapper
    {
        private static ILuaWrapper _instance;
        public static ILuaWrapper Instance
        {
            private get
            {
                if (_instance == null)
                    throw new NotImplementedException("This instance is not implemented. You need to implement it in your main project");
                return _instance;
            }
            set { _instance = value; }
        }

        public static ILua Create() { return Instance.Create(); }
        public static ILua Create(string scriptName) { return Instance.Create(scriptName); }
    }
}
