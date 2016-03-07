using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Aragas.Core.Wrappers
{
    public class CustomTypeInfo
    {
        public Type CustomType { get; set; }
        public int Count { get; set; }
    }

    public class DatabaseTableInfo
    {
        public Type BaseType { get; }
        public List<CustomTypeInfo> CustomTypes { get; } = new List<CustomTypeInfo>();

        public DatabaseTableInfo(Type baseType) { BaseType = baseType; }
    }

    public abstract class DatabaseTable
    {
        private static List<DatabaseTableInfo> CustomTableTypes { get; } = new List<DatabaseTableInfo>();


        public static DatabaseTableInfo GetCustomTableType<TBaseType>(TBaseType baseType = default(TBaseType)) where TBaseType : Type
        {
            return CustomTableTypes.FirstOrDefault(table => table.BaseType == baseType);
        }
        public static void AddCustomTableType<TBaseType>(TBaseType baseType = default(TBaseType)) where TBaseType : Type
        {
            if (GetCustomTableType(baseType) == null)
                CustomTableTypes.Add(new DatabaseTableInfo(baseType));
        }

        public static void AddCustomType<TBaseType, TCustomType>(TBaseType baseType, TCustomType customType, int count) where TBaseType : Type where TCustomType : Type
        {
            if (GetCustomTableType(baseType) == null)
                CustomTableTypes.Add(new DatabaseTableInfo(baseType));

            GetCustomTableType(baseType).CustomTypes.Add(new CustomTypeInfo { CustomType = customType, Count = count });
        }

		public int Id { get; set; }
    }

    public abstract class Database
    {
        public abstract string FileExtension { get; }


        public abstract Database Create(string databaseName);

        public abstract void CreateTable<T>() where T : DatabaseTable, new();
        public abstract void Insert<T>(T obj) where T : DatabaseTable, new();
        public abstract void Update<T>(T obj) where T : DatabaseTable, new();
        public abstract T Find<T>(Expression<Func<T, bool>> predicate) where T : DatabaseTable, new();
    }

    
    public static class DatabaseWrapper
    {
        private static Database _instance;
        public static Database Instance
        {
            private get
            {
                if (_instance == null)
                    throw new NotImplementedException("This instance is not implemented. You need to implement it in your main project");
                return _instance;
            }
            set { _instance = value; }
        }

        public static Database Create(string databaseName) { return Instance.Create(databaseName); }
    }
}
