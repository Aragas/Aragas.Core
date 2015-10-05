using System;

using Aragas.Core.Data;
using Aragas.Core.Interfaces;

namespace Aragas.Core.Extensions
{
    public static class PacketExtensions
    {
        public static void Write(this IPacketStream stream, TimeSpan timeSpan)
        {
            stream.Write(timeSpan.Ticks);
        }
        public static TimeSpan ReadTimeSpan(this IPacketDataReader reader)
        {
            return new TimeSpan(reader.ReadLong());
        }


        public static void Write(this IPacketStream stream, DateTime dateTime)
        {
            stream.Write(dateTime.Ticks);
        }
        public static DateTime ReadDateTime(this IPacketDataReader reader)
        {
            return new DateTime(reader.ReadLong());
        }


        #region Vector3

        public static void Write_Byte(this IPacketStream stream, Vector3 vector3)
        {
            stream.Write((byte) vector3.X);
            stream.Write((byte) vector3.Y);
            stream.Write((byte) vector3.Z);
        }
        public static void Write_SByte(this IPacketStream stream, Vector3 vector3)
        {
            stream.Write((sbyte) vector3.X);
            stream.Write((sbyte) vector3.Y);
            stream.Write((sbyte) vector3.Z);
        }
        public static void Write_UShort(this IPacketStream stream, Vector3 vector3)
        {
            stream.Write((ushort) vector3.X);
            stream.Write((ushort) vector3.Y);
            stream.Write((ushort) vector3.Z);
        }
        public static void Write_Short(this IPacketStream stream, Vector3 vector3)
        {
            stream.Write((short) vector3.X);
            stream.Write((short) vector3.Y);
            stream.Write((short) vector3.Z);
        }
        public static void Write_Float(this IPacketStream stream, Vector3 vector3)
        {
            stream.Write(vector3.X);
            stream.Write(vector3.Y);
            stream.Write(vector3.Z);
        }
        public static void Write_Double(this IPacketStream stream, Vector3 vector3)
        {
            stream.Write((double) vector3.X);
            stream.Write((double) vector3.Y);
            stream.Write((double) vector3.Z);
        }
        public static void Write_SByteFixedPoint(this IPacketStream stream, Vector3 vector3)
        {
            stream.Write((sbyte) (vector3.X * 32.0f));
            stream.Write((sbyte) (vector3.Y * 32.0f));
            stream.Write((sbyte) (vector3.Z * 32.0f));
        }
        public static void Write_IntFixedPoint(this IPacketStream stream, Vector3 vector3)
        {
            stream.Write((int) (vector3.X * 32.0f));
            stream.Write((int) (vector3.Y * 32.0f));
            stream.Write((int) (vector3.Z * 32.0f));
        }

        public static Vector3 ReadVector3_Byte(this IPacketDataReader reader)
        {
            return new Vector3(
                reader.ReadByte(),
                reader.ReadByte(),
                reader.ReadByte()
            );
        }
        public static Vector3 ReadVector3_SByte(this IPacketDataReader reader)
        {
            return new Vector3(
                reader.ReadSByte(),
                reader.ReadSByte(),
                reader.ReadSByte()
            );
        }
        public static Vector3 ReadVector3_UShort(this IPacketDataReader reader)
        {
            return new Vector3(
                reader.ReadUShort(),
                reader.ReadUShort(),
                reader.ReadUShort()
            );
        }
        public static Vector3 ReadVector3_Short(this IPacketDataReader reader)
        {
            return new Vector3(
                reader.ReadShort(),
                reader.ReadShort(),
                reader.ReadShort()
            );
        }
        public static Vector3 ReadVector3_Float(this IPacketDataReader reader)
        {
            return new Vector3(
                reader.ReadFloat(),
                reader.ReadFloat(),
                reader.ReadFloat()
            );
        }
        public static Vector3 ReadVector3_Double(this IPacketDataReader reader)
        {
            return new Vector3(
                reader.ReadDouble(),
                reader.ReadDouble(),
                reader.ReadDouble()
            );
        }
        public static Vector3 ReadVector3_SByteFixedPoint(this IPacketDataReader reader)
        {
            return new Vector3(
                reader.ReadSByte() / 32.0f,
                reader.ReadSByte() / 32.0f,
                reader.ReadSByte() / 32.0f
            );
        }
        public static Vector3 ReadVector3_IntFixedPoint(this IPacketDataReader reader)
        {
            return new Vector3(
                reader.ReadInt() / 32.0f,
                reader.ReadInt() / 32.0f,
                reader.ReadInt() / 32.0f
            );
        }

        #endregion

    }
}
