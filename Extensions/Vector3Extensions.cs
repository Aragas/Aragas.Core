using Aragas.Core.Data;

namespace Aragas.Core.Extensions
{
    public static class Vector3Extensions
    {
        public static Vector3 FromFixedPoint(long x, long y, long z) => new Vector3(x / 32.0f, y / 32.0f, z / 32.0f);
    }
}