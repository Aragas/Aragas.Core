using System;
using System.Collections.Generic;
using System.Reflection;

namespace Aragas.Core.Wrappers
{
    public interface IAppDomain
    {
        Assembly GetAssembly(Type type);
        Assembly[] GetAssemblies();
        Assembly LoadAssembly(byte[] assemblyData);
    }

    public static class AppDomainWrapper
    {
        private static IAppDomain _instance;
        public static IAppDomain Instance
        {
            private get
            {
                if (_instance == null)
                    throw new NotImplementedException(
                        "This instance is not implemented. You need to implement it in your main project");
                return _instance;
            }
            set { _instance = value; }
        }

        public static Assembly GetAssembly(Type type) { return Instance.GetAssembly(type); }
        public static Assembly[] GetAssemblies() { return Instance.GetAssemblies(); }
        public static Assembly LoadAssembly(byte[] assemblyData) { return Instance.LoadAssembly(assemblyData); }

        public static Type GetTypeFromNameAndInterface<T>(string className, Assembly assembly)
        {
            foreach (var typeInfo in new List<TypeInfo>(assembly.DefinedTypes))
                if (typeInfo.Name == className)
                    foreach (var type in new List<Type>(typeInfo.ImplementedInterfaces))
                        if (type == typeof (T))
                            return typeInfo.AsType();

            return null;
        }
        public static Type GetTypeFromNameAndAbstract<T>(string className, Assembly assembly)
        {
            foreach (var typeInfo in new List<TypeInfo>(assembly.DefinedTypes))
                if (typeInfo.Name == className)
                    if (typeInfo.IsSubclassOf(typeof (T)))
                        return typeInfo.AsType();

            return null;
        }
        public static Type GetTypeFromName(string className, Assembly assembly)
        {
            foreach (var typeInfo in new List<TypeInfo>(assembly.DefinedTypes))
                if (typeInfo.Name == className)
                    return typeInfo.AsType();

            return null;
        }
    }
}
