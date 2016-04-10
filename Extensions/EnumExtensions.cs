using System;
using System.Linq;
using System.Reflection;

using Aragas.Core.PacketHandlers;
using Aragas.Core.Packets;
using Aragas.Core.Wrappers;

namespace Aragas.Core.Extensions
{
    public static class EnumExtensions
    {
        public static Func<TPacket>[] CreatePacketInstances<TPacket>(this Enum packetType, Assembly assembly) where TPacket : Packet
        {
            var typeNames = Enum.GetValues(packetType.GetType());
            var packets = new Func<TPacket>[typeNames.Cast<int>().Max() + 1];

            foreach (var packetName in typeNames)
            {
                var typeName = $"{packetName}Packet";
                var type = AppDomainWrapper.GetTypeFromNameAndAbstract<TPacket>(typeName, assembly);
                packets[(int) packetName] = type != null ? (Func<TPacket>) (() => (TPacket) Activator.CreateInstance(type)) : null;
            }

            return packets;
        }
        public static void CreatePacketInstancesOut<TPacket>(this Enum packetType, out Func<TPacket>[] packets, Assembly assembly) where TPacket : Packet
        {
            var typeNames = Enum.GetValues(packetType.GetType());
            packets = new Func<TPacket>[typeNames.Cast<int>().Max() + 1];

            foreach (var packetName in typeNames)
            {
                var typeName = $"{packetName}Packet";
                var type = AppDomainWrapper.GetTypeFromNameAndAbstract<TPacket>(typeName, assembly);
                packets[(int) packetName] = type != null ? (Func<TPacket>) (() => (TPacket) Activator.CreateInstance(type)) : null;
            }
        }
        public static void CreatePacketInstancesRef<TPacket>(this Enum packetType, ref Func<TPacket>[] packets, Assembly assembly) where TPacket : Packet
        {
            var typeNames = Enum.GetValues(packetType.GetType());

            var size = typeNames.Cast<int>().Max() + 1;
            if (packets == null)
                packets = new Func<TPacket>[size];
            else
                Array.Resize(ref packets, size);

            foreach (var packetName in typeNames)
            {
                var typeName = $"{packetName}Packet";
                var type = AppDomainWrapper.GetTypeFromNameAndAbstract<TPacket>(typeName, assembly);
                packets[(int) packetName] = type != null ? (Func<TPacket>) (() => (TPacket) Activator.CreateInstance(type)) : null;
            }
        }


        public static ContextFunc<TPacket>[] CreateHandlerInstances<TPacket>(this Enum packetType, Assembly assembly) where TPacket : Packet
        {
            var typeNames = Enum.GetValues(packetType.GetType());
            var packets = new ContextFunc<TPacket>[typeNames.Cast<int>().Max() + 1];

            foreach (var packetName in typeNames)
            {
                var typeName = $"{packetName}Handler";
                var type = AppDomainWrapper.GetTypeFromName(typeName, assembly);
                packets[(int) packetName] = type != null ? new ContextFunc<TPacket>((PacketHandler) Activator.CreateInstance(type)) : null;
            }

            return packets;
        }
        public static void CreateHandlerInstancesOut<TPacket>(this Enum packetType, out ContextFunc<TPacket>[] packets, Assembly assembly) where TPacket : Packet
        {
            var typeNames = Enum.GetValues(packetType.GetType());
            packets = new ContextFunc<TPacket>[typeNames.Cast<int>().Max() + 1];

            foreach (var packetName in typeNames)
            {
                var typeName = $"{packetName}Handler";
                var type = AppDomainWrapper.GetTypeFromName(typeName, assembly);
                packets[(int) packetName] = type != null ? new ContextFunc<TPacket>((PacketHandler) Activator.CreateInstance(type)) : null;
            }
        }
        public static void CreateHandlerInstancesRef<TPacket>(this Enum packetType, ref ContextFunc<TPacket>[] packets, Assembly assembly) where TPacket : Packet
        {
            var typeNames = Enum.GetValues(packetType.GetType());

            var size = typeNames.Cast<int>().Max() + 1;
            if (packets == null)
                packets = new ContextFunc<TPacket>[size];
            else
                Array.Resize(ref packets, size);

            foreach (var packetName in typeNames)
            {
                var typeName = $"{packetName}Handler";
                var type = AppDomainWrapper.GetTypeFromName(typeName, assembly);
                packets[(int) packetName] = type != null ? new ContextFunc<TPacket>((PacketHandler) Activator.CreateInstance(type)) : null;
            }
        }
    }
}