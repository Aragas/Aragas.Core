using System;

using Aragas.Core.Data;
using Aragas.Core.Interfaces;

using static Aragas.Core.Interfaces.PacketDataReader;

namespace Aragas.Core.Extensions
{
    public static class PacketExtensions
    {
        public static void Init()
        {
            ExtendRead(new ExtendReadInfo(typeof(TimeSpan), ReadTimeSpan));
            ExtendRead(new ExtendReadInfo(typeof(DateTime), ReadDateTime));
            ExtendRead(new ExtendReadInfo(typeof(Vector3), ReadVector3));
        }

        public static void Write(this IPacketStream stream, TimeSpan value)
        {
            stream.Write(value.Ticks);
        }
        private static object ReadTimeSpan(PacketDataReader reader, int length = 0)
        {
            return new TimeSpan(reader.Read<long>());
        }

        public static void Write(this IPacketStream stream, DateTime value)
        {
            stream.Write(value.Ticks);
        }
        private static object ReadDateTime(PacketDataReader reader, int length = 0)
        {
            return new DateTime(reader.Read<long>());
        }
        
        public static void Write(this IPacketStream stream, Vector3 value)
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
