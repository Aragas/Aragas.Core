using System;

using Aragas.Core.Data;
using Aragas.Core.Interfaces;

namespace Aragas.Core.Extensions
{
    public static class PacketExtensions
    {
        public static void WriteTimeSpan(this IPacketStream stream, TimeSpan timeSpan)
        {
            stream.WriteLong(timeSpan.Ticks);
        }
        public static TimeSpan ReadTimeSpan(this IPacketDataReader reader)
        {
            return new TimeSpan(reader.ReadLong());
        }


        public static void WriteDateTime(this IPacketStream stream, DateTime dateTime)
        {
            stream.WriteLong(dateTime.Ticks);
        }
        public static DateTime ReadDateTime(this IPacketDataReader reader)
        {
            return new DateTime(reader.ReadLong());
        }


        #region Vector3

        public static void WriteVector3_Byte(this IPacketStream stream, Vector3 vector3)
        {
            stream.WriteByte((byte) vector3.X);
            stream.WriteByte((byte) vector3.Y);
            stream.WriteByte((byte) vector3.Z);
        }
        public static void WriteVector3_SByte(this IPacketStream stream, Vector3 vector3)
        {
            stream.WriteSByte((sbyte) vector3.X);
            stream.WriteSByte((sbyte) vector3.Y);
            stream.WriteSByte((sbyte) vector3.Z);
        }
        public static void WriteVector3_UShort(this IPacketStream stream, Vector3 vector3)
        {
            stream.WriteUShort((ushort) vector3.X);
            stream.WriteUShort((ushort) vector3.Y);
            stream.WriteUShort((ushort) vector3.Z);
        }
        public static void WriteVector3_Short(this IPacketStream stream, Vector3 vector3)
        {
            stream.WriteShort((short) vector3.X);
            stream.WriteShort((short) vector3.Y);
            stream.WriteShort((short) vector3.Z);
        }
        public static void WriteVector3_Float(this IPacketStream stream, Vector3 vector3)
        {
            stream.WriteFloat(vector3.X);
            stream.WriteFloat(vector3.Y);
            stream.WriteFloat(vector3.Z);
        }
        public static void WriteVector3_Double(this IPacketStream stream, Vector3 vector3)
        {
            stream.WriteDouble(vector3.X);
            stream.WriteDouble(vector3.Y);
            stream.WriteDouble(vector3.Z);
        }
        public static void WriteVector3_SByteFixedPoint(this IPacketStream stream, Vector3 vector3)
        {
            stream.WriteSByte((sbyte) (vector3.X * 32.0f));
            stream.WriteSByte((sbyte) (vector3.Y * 32.0f));
            stream.WriteSByte((sbyte) (vector3.Z * 32.0f));
        }
        public static void WriteVector3_IntFixedPoint(this IPacketStream stream, Vector3 vector3)
        {
            stream.WriteInt((int) (vector3.X * 32.0f));
            stream.WriteInt((int) (vector3.Y * 32.0f));
            stream.WriteInt((int) (vector3.Z * 32.0f));
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
