using System;
using System.Runtime.InteropServices;

namespace Aragas.Core.Data
{
    /// <summary>
    /// Represents the location of an object in 2D space (float).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Vector2 : IEquatable<Vector2>
    {
        public readonly float X;
        public readonly float Y;

        public Vector2(float value) { X = Y = value; }

        public Vector2(float x, float y) { X = x; Y = y; }

        public Vector2(double x, double y) { X = (float) x; Y = (float) y; }

        public Vector2(Vector2 vector2) { X = vector2.X; Y = vector2.Y; }


        /// <summary>
        /// Converts this Vector2 to a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"X: {X}, Y: {Y}";

        #region Math

        public static Vector2 FromFixedPoint(int x, int y) => new Vector2(x / 32.0f, y / 32.0f);
        

        public static Vector2 Floor(Vector2 vector2) => new Vector2(Math.Floor(vector2.X), Math.Floor(vector2.Y));
        public Vector2 Floor() => Floor(this);

        public static Vector2 Ceiling(Vector2 vector2) => new Vector2(Math.Ceiling(vector2.X), Math.Ceiling(vector2.Y));
        public Vector2 Ceiling() => Ceiling(this);


        private static double Square(double num) => num * num;

        /// <summary>
        /// Calculates the distance between two Vector2 objects.
        /// </summary>
        public double DistanceTo(Vector2 other) => Math.Sqrt(Square(other.X - X) + Square(other.Y - Y));

        /// <summary>
        /// Finds the distance of this vector from Vector2.Zero
        /// </summary>
        public double Distance() => DistanceTo(Zero);

        public static Vector2 Min(Vector2 value1, Vector2 value2) => new Vector2(Math.Min(value1.X, value2.X), Math.Min(value1.Y, value2.Y));
        public Vector2 Min(Vector2 value2) => new Vector2(Math.Min(X, value2.X), Math.Min(Y, value2.Y));

        public static Vector2 Max(Vector2 value1, Vector2 value2) => new Vector2(Math.Max(value1.X, value2.X), Math.Max(value1.Y, value2.Y));
        public Vector2 Max(Vector2 value2) => new Vector2(Math.Max(X, value2.X), Math.Max(Y, value2.Y));

        public static Vector2 Delta(Vector2 firstLocation, Vector2 secondLocation) => new Vector2(firstLocation.X - secondLocation.X, firstLocation.Y - secondLocation.Y);
        public Vector2 Delta(Vector2 secondLocation) => new Vector2(X - secondLocation.X, Y - secondLocation.Y);

        #endregion

        #region Operators

        public static Vector2 operator -(Vector2 a) => new Vector2(-a.X, -a.Y);
        public static Vector2 operator ++(Vector2 a) => new Vector2(a.X, a.Y) + 1.0;
        public static Vector2 operator --(Vector2 a) => new Vector2(a.X, a.Y) - 1.0;


        public static bool operator !=(Vector2 a, Vector2 b) => !a.Equals(b);
        public static bool operator ==(Vector2 a, Vector2 b) => a.Equals(b);
        public static bool operator >(Vector2 a, Vector2 b) => a.X > b.X && a.Y > b.Y;
        public static bool operator <(Vector2 a, Vector2 b) => a.X < b.X && a.Y < b.Y;
        public static bool operator >=(Vector2 a, Vector2 b) => a.X >= b.X && a.Y >= b.Y;
        public static bool operator <=(Vector2 a, Vector2 b) => a.X <= b.X && a.Y <= b.Y;

        public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.X + b.X, a.Y + b.Y);
        public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a.X - b.X, a.Y - b.Y);
        public static Vector2 operator *(Vector2 a, Vector2 b) => new Vector2(a.X * b.X, a.Y * b.Y);
        public static Vector2 operator /(Vector2 a, Vector2 b) => new Vector2(a.X / b.X, a.Y / b.Y);
        public static Vector2 operator %(Vector2 a, Vector2 b) => new Vector2(a.X % b.X, a.Y % b.Y);

        public static Vector2 operator +(Vector2 a, double b) => new Vector2(a.X + b, a.Y + b);
        public static Vector2 operator -(Vector2 a, double b) => new Vector2(a.X - b, a.Y - b);
        public static Vector2 operator *(Vector2 a, double b) => new Vector2(a.X * b, a.Y * b);
        public static Vector2 operator /(Vector2 a, double b) => new Vector2(a.X / b, a.Y / b);
        public static Vector2 operator %(Vector2 a, double b) => new Vector2(a.X % b, a.Y % b);

        public static Vector2 operator +(double a, Vector2 b) => new Vector2(a + b.X, a + b.Y);
        public static Vector2 operator -(double a, Vector2 b) => new Vector2(a - b.X, a - b.Y);
        public static Vector2 operator *(double a, Vector2 b) => new Vector2(a * b.X, a * b.Y);
        public static Vector2 operator /(double a, Vector2 b) => new Vector2(a / b.X, a / b.Y);
        public static Vector2 operator %(double a, Vector2 b) => new Vector2(a % b.X, a % b.Y);

        #endregion

        #region Constants

        public static readonly Vector2 Zero = new Vector2(0, 0);
        public static readonly Vector2 One = new Vector2(1, 1);

        #endregion

        public bool Equals(Vector2 other) => other.X.Equals(X) && other.Y.Equals(Y);
        public bool Equals(float other) => other.Equals(X) && other.Equals(Y);

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((Vector2)obj);
        }

        public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode();
    }
}
