using Aragas.Core.Data;

namespace Aragas.Core.Extensions
{
    public static class Vector2Extensions
    {
        public static Vector2 FromFixedPoint(long x, long y) => new Vector2(x / 32.0f, y / 32.0f);
    }
}