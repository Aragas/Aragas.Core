using System;
using System.Linq;
using System.Reflection;
using Aragas.Core.Wrappers;

namespace Aragas.Core.Extensions
{
    public static class EnumExtensions
    {
        public static Func<T>[] CreatePacketInstances<T>(this Enum packetType, Assembly assembly)
        {
            var typeNames = Enum.GetValues(packetType.GetType());
            var packets = new Func<T>[typeNames.Cast<int>().Max() + 1];

            foreach (var packetName in typeNames)
            {
                var typeName = $"{packetName}Packet";
                var type = AppDomainWrapper.GetTypeFromNameAndAbstract<T>(typeName, assembly);
                packets[(int) packetName] = type != null ? (Func<T>) (() => type) : null;
            }

            return packets;
        }
        public static void CreatePacketInstancesOut<T>(this Enum packetType, out Func<T>[] packets, Assembly assembly)
        {
            var typeNames = Enum.GetValues(packetType.GetType());
            packets = new Func<T>[typeNames.Cast<int>().Max() + 1];

            foreach (var packetName in typeNames)
            {
                var typeName = $"{packetName}Packet";
                var type = AppDomainWrapper.GetTypeFromNameAndAbstract<T>(typeName, assembly);
                packets[(int) packetName] = type != null ? (Func<T>) (() => type) : null;
            }
        }
        public static void CreatePacketInstancesRef<T>(this Enum packetType, ref Func<T>[] packets, Assembly assembly)
        {
            var typeNames = Enum.GetValues(packetType.GetType());

            var size = typeNames.Cast<int>().Max() + 1;
            if (packets == null)
                packets = new Func<T>[size];
            else
                Array.Resize(ref packets, size);

            foreach (var packetName in typeNames)
            {
                var typeName = $"{packetName}Packet";
                var type = AppDomainWrapper.GetTypeFromNameAndAbstract<T>(typeName, assembly);
                packets[(int) packetName] = type != null ? (Func<T>) (() => type) : null;
            }
        }
    }
}