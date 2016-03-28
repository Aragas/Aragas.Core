using System;
using System.Linq.Expressions;

namespace Aragas.Core.Wrappers
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKeyAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public class AutoIncrementAttribute : Attribute { }


    public abstract class DatabaseTable { }
    public abstract class DatabaseTable<TKeyType> : DatabaseTable
    {
        [PrimaryKey]
		public abstract TKeyType Id { get; protected set; }
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
