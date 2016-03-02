using System;
using System.Collections.Generic;

namespace Aragas.Core.Wrappers
{
    public abstract class LuaScript
    {
        public abstract bool ReloadFile();

        public abstract object this[string index] { get; set; }

        public abstract object[] CallFunction(string functionName, params object[] args);
    }

    public abstract class LuaTable
    {
        public abstract object this[object field] { get; set; }
        public abstract object this[string field] { get; set; }

        public abstract object[] CallFunction(string functionName, params object[] args);

        public abstract Dictionary<object, object> ToDictionary();

        public abstract List<object> ToList();
        public abstract object[] ToArray();
    }

    public interface ILuaWrapper
    {
        LuaScript CreateLuaScript(string luaScriptName = "");

        LuaTable CreateTable(LuaScript luaScript, string tableName);

        LuaTable ToLuaTable(object obj);
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

        public static LuaScript CreateLuaScript(string luaScriptName = "") => Instance.CreateLuaScript(luaScriptName);

        public static LuaTable CreateTable(LuaScript luaScript, string tableName) => Instance.CreateTable(luaScript, tableName);

        public static LuaTable ToLuaTable(object obj) => Instance.ToLuaTable(obj);
    }
}
