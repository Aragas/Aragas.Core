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

    public interface IDatabase
    {
        String FileExtension { get; }

        Boolean PrimitivesOnly { get; }

        IDatabase CreateDB(String databaseName);

        void CreateTable<T>() where T : DatabaseTable, new();
        void Insert<T>(T obj) where T : DatabaseTable, new();
        void Update<T>(T obj) where T : DatabaseTable, new();
        T Find<T>(Expression<Func<T, Boolean>> predicate) where T : DatabaseTable, new();
    }

    
    public static class DatabaseWrapper
    {
        private static IDatabase _instance;
        public static IDatabase Instance
        {
            private get
            {
                if (_instance == null)
                    throw new NotImplementedException("This instance is not implemented. You need to implement it in your main project");
                return _instance;
            }
            set { _instance = value; }
        }

        public static IDatabase Create(string databaseName) { return Instance.CreateDB(databaseName); }
    }
}
