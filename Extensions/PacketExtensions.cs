using System;

using Aragas.Core.Data;
using Aragas.Core.IO;

using static Aragas.Core.IO.PacketDataReader;

namespace Aragas.Core.Extensions
{
    public static class PacketExtensions
    {
        public static void Init()
        {
            ExtendRead<TimeSpan>(ReadTimeSpan);
            ExtendRead<DateTime>(ReadDateTime);
            ExtendRead<Vector2>(ReadVector2);
            ExtendRead<Vector3>(ReadVector3);
        }

        public static void Write(this PacketStream stream, TimeSpan value)
        {
            stream.Write(value.Ticks);
        }
        private static object ReadTimeSpan(PacketDataReader reader, int length = 0)
        {
            return new TimeSpan(reader.Read<long>());
        }

        public static void Write(this PacketStream stream, DateTime value)
        {
            stream.Write(value.Ticks);
        }
        private static object ReadDateTime(PacketDataReader reader, int length = 0)
        {
            return new DateTime(reader.Read<long>());
        }

        public static void Write(this PacketStream stream, Vector2 value)
        {
            stream.Write(value.X);
            stream.Write(value.Y);
        }
        private static object ReadVector2(PacketDataReader reader, int length = 0)
        {
            return new Vector2(reader.Read<float>(), reader.Read<float>());
        }

        public static void Write(this PacketStream stream, Vector3 value)
        {
            stream.Write(value.X);
            stream.Write(value.Y);
            stream.Write(value.Z);
        }
        private static object ReadVector3(PacketDataReader reader, int length = 0)
        {
            return new Vector3(reader.Read<float>(), reader.Read<float>(), reader.Read<float>());
        }
    }
}
