using System;
using System.IO;

using PCLStorage;

namespace Aragas.Core.Wrappers
{
    public interface IFileSystem
    {
        IFolder SettingsFolder { get; }
        IFolder LogFolder { get; }
        IFolder CrashLogFolder { get; }
        IFolder LuaFolder { get; }
        IFolder AssemblyFolder { get; }
        IFolder DatabaseFolder { get; }
        IFolder ContentFolder { get; }
        IFolder OutputFolder { get; }
    }

    public static class FileSystemWrapper
    {
        private static IFileSystem _instance;
        public static IFileSystem Instance
        {
            private get
            {
                if (_instance == null)
                    throw new NotImplementedException("This instance is not implemented. You need to implement it in your main project");
                return _instance;
            }
            set { _instance = value; }
        }

        public static IFolder SettingsFolder => Instance.SettingsFolder;
        public static IFolder LogFolder => Instance.LogFolder;
        public static IFolder CrashLogFolder => Instance.CrashLogFolder;
        public static IFolder LuaFolder => Instance.LuaFolder;
        public static IFolder AssemblyFolder => Instance.AssemblyFolder;
        public static IFolder DatabaseFolder => Instance.DatabaseFolder;
        public static IFolder ContentFolder => Instance.ContentFolder;
        public static IFolder OutputFolder => Instance.ContentFolder;

        public static bool LoadSettings<T>(string filename, T value)
        {
            var config = ConfigWrapper.CreateConfig();

            using (var stream = Instance.SettingsFolder.CreateFileAsync($"{filename}.{config.FileExtension}", CreationCollisionOption.OpenIfExists).Result.OpenAsync(FileAccess.ReadAndWrite).Result)
            using (var reader = new StreamReader(stream))
            using (var writer = new StreamWriter(stream))
            {
                var file = reader.ReadToEnd();
                if (!string.IsNullOrEmpty(file))
                {
                    try
                    {
                        if (value == null)
                        {
                            value = config.Deserialize<T>(file);
                        }
                        else
                        {
                            config.PopulateObject(file, value);
                            stream.SetLength(0);
                            writer.Write(config.Serialize(value));
                        }
                    }
                    catch (ConfigDeserializingException)
                    {
                        stream.SetLength(0);
                        writer.Write(config.Serialize(value));
                        return false;
                    }
                    catch (ConfigSerializingException) { return false; }
                }
                else
                {
                    try
                    {
                        stream.SetLength(0);
                        writer.Write(config.Serialize(value));
                    }
                    catch (ConfigSerializingException) { return false; }
                }
            }

            return true;
        }
        public static bool SaveSettings<T>(string filename, T defaultValue = default(T))
        {
            var config = ConfigWrapper.CreateConfig();

            using (var stream = Instance.SettingsFolder.CreateFileAsync($"{filename}.{config.FileExtension}", CreationCollisionOption.OpenIfExists).Result.OpenAsync(FileAccess.ReadAndWrite).Result)
            using (var writer = new StreamWriter(stream))
            {
                try { writer.Write(config.Serialize(defaultValue)); }
                catch (ConfigSerializingException) { return false; }
            }

            return true;
        }

        public static bool LoadLog(string filename, out string content)
        {
            content = string.Empty;

            using (var stream = Instance.LogFolder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists).Result.OpenAsync(FileAccess.ReadAndWrite).Result)
            using (var writer = new StreamWriter(stream))
            {
                try { writer.Write(content); }
                catch (IOException) { return false; }
            }

            return true;
        }
        public static bool SaveLog(string filename, string content)
        {
            using (var stream = Instance.LogFolder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists).Result.OpenAsync(FileAccess.ReadAndWrite).Result)
            using (var writer = new StreamWriter(stream))
            {
                try { writer.Write(content); }
                catch (IOException) { return false; }
            }

            return true;
        }
    }
}
