using System;
using System.Collections.Generic;

namespace Aragas.Core.Wrappers
{
    public interface ILua
    {
        bool ReloadFile();

        Object this[string index] { get; set; }

        Object[] CallFunction(string functionName, params object[] args);
    }

    public interface ILuaTable
    {
        Object this[Object field] { get; set; }
        Object this[String field] { get; set; }

        Dictionary<Object, Object> ToDictionary();

        List<Object> ToList();
        Object[] ToArray();
    }

    public interface ILuaWrapper
    {
        ILua Create();
        ILua Create(string scriptName);
        ILuaTable Create(ILua lua, string tableName);
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

        public static ILuaTable Create(ILua lua, string tableName) { return Instance.Create(lua, tableName); }
    }
}
