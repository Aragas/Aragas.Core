using System;

namespace Aragas.Core.Wrappers
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ConfigIgnoreAttribute : Attribute { }

    public class ConfigException : Exception
    {
        public ConfigException() { }
        public ConfigException(string message): base(message) { }
        public ConfigException(string message, Exception innerException): base(message, innerException) { }
    }
    public class ConfigSerializingException : Exception
    {
        public ConfigSerializingException() { }
        public ConfigSerializingException(string message) : base(message) { }
        public ConfigSerializingException(string message, Exception innerException) : base(message, innerException) { }
    }
    public class ConfigDeserializingException : Exception
    {
        public ConfigDeserializingException() { }
        public ConfigDeserializingException(string message) : base(message) { }
        public ConfigDeserializingException(string message, Exception innerException) : base(message, innerException) { }
    }

    public interface IConfigWrapper
    {
        string FileExtension { get; }

        string Serialize<T>(T target);
        T Deserialize<T>(string value);
        void PopulateObject<T>(string value, T target);

    }

    public interface IConfigFactory
    {
        IConfigWrapper Create();
    }

    public static class ConfigWrapper
    {
        private static IConfigFactory _instance;
        public static IConfigFactory Instance
        {
            private get
            {
                if (_instance == null)
                    throw new NotImplementedException("This instance is not implemented. You need to implement it in your main project");
                return _instance;
            }
            set { _instance = value; }
        }


        public static IConfigWrapper Create() { return Instance.Create(); }
    }
}
