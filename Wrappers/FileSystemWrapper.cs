using System;
using System.IO;

using Newtonsoft.Json;

using PCLStorage;

namespace Aragas.Core.Wrappers
{
    public interface IFileSystem
    {
        IFolder UsersFolder { get;  }
        IFolder SettingsFolder { get; }
        IFolder LogFolder { get; }
        IFolder CrashLogFolder { get; }
        IFolder LuaFolder { get; }
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

        public static IFolder UsersFolder => Instance.UsersFolder;
        public static IFolder SettingsFolder => Instance.SettingsFolder;
        public static IFolder LogFolder => Instance.LogFolder;
        public static IFolder CrashLogFolder => Instance.CrashLogFolder;
        public static IFolder LuaFolder => Instance.LuaFolder;

        static readonly JsonConverter[] Converters = {
        };

        public static bool LoadSettings<T>(string filename, T value)
        {
            using (var stream = Instance.SettingsFolder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists).Result.OpenAsync(FileAccess.ReadAndWrite).Result)
            using (var reader = new StreamReader(stream))
            using (var writer = new StreamWriter(stream))
            {
                var file = reader.ReadToEnd();
                if (!string.IsNullOrEmpty(file))
                {
                    try
                    {
                        JsonConvert.PopulateObject(file, value, new JsonSerializerSettings { Converters = Converters });
                        stream.SetLength(0);
                        writer.Write(JsonConvert.SerializeObject(value, Formatting.Indented, Converters));
                    }
                    catch (JsonReaderException e)
                    {
                        stream.SetLength(0);
                        writer.Write(JsonConvert.SerializeObject(value, Formatting.Indented, Converters));
                        return false;
                    }
                    catch (JsonWriterException e) { return false; }
                }
                else
                {
                    try
                    {
                        stream.SetLength(0);
                        writer.Write(JsonConvert.SerializeObject(value, Formatting.Indented, Converters));
                    }
                    catch (JsonWriterException e) { return false; }
                }
            }

            return true;
        }
        public static bool SaveSettings<T>(string filename, T defaultValue = default(T))
        {
            using (var stream = Instance.SettingsFolder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists).Result.OpenAsync(FileAccess.ReadAndWrite).Result)
            using (var writer = new StreamWriter(stream))
            {
                try { writer.Write(JsonConvert.SerializeObject(defaultValue, Formatting.Indented, Converters)); }
                catch (JsonWriterException e) { return false; }
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
