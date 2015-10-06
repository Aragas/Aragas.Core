using System;

using Aragas.Core.Data;
using Aragas.Core.Interfaces;

namespace Aragas.Core.Extensions
{
    public static class PacketExtensions
    {
        public static void Write(this IPacketStream stream, TimeSpan value)
        {
            stream.Write(value.Ticks);
        }
        public static TimeSpan Read(this IPacketDataReader reader, TimeSpan value = default(TimeSpan))
        {
            return new TimeSpan(reader.Read<long>());
        }


        public static void Write(this IPacketStream stream, DateTime value)
        {
            stream.Write(value.Ticks);
        }
        public static DateTime Read(this IPacketDataReader reader, DateTime value = default(DateTime))
        {
            return new DateTime(reader.Read<long>());
        }


        public static void Write(this IPacketStream stream, Vector3 value)
        {
            stream.Write(value.X);
            stream.Write(value.Y);
            stream.Write(value.Z);
        }
        public static Vector3 Read(this IPacketDataReader reader, Vector3 value = default(Vector3))
        {
            return new Vector3(reader.Read<float>(), reader.Read<float>(), reader.Read<float>());
        }

    }
}
