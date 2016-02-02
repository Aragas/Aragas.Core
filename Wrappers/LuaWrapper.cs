using System;
using System.Collections.Generic;

namespace Aragas.Core.Wrappers
{
    public interface ILua
    {
        Boolean ReloadFile();

        Object this[String index] { get; set; }

        Object[] CallFunction(String functionName, params Object[] args);
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
        ILua CreateLua();
        ILua CreateLua(String scriptName);
        ILuaTable CreateTable(ILua lua, String tableName);
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

        public static ILua CreateLua() { return Instance.CreateLua(); }
        public static ILua CreateLua(string scriptName) { return Instance.CreateLua(scriptName); }

        public static ILuaTable CreateTable(ILua lua, string tableName) { return Instance.CreateTable(lua, tableName); }
    }
}
