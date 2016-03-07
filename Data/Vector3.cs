using System;
using System.Runtime.InteropServices;

namespace Aragas.Core.Data
{
    /// <summary>
    /// Represents the location of an object in 3D space (float).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Vector3 : IEquatable<Vector3>
    {
        public readonly float X;
        public readonly float Y;
        public readonly float Z;

        public Vector3(float value) { X = Y = Z = value; }

        public Vector3(float yaw, float pitch)
        {
            var yawRadians = ToRadians(yaw);
            var pitchRadians = ToRadians(pitch);

            var sinYaw = Math.Sin(yawRadians);
            var cosYaw = Math.Cos(yawRadians);
            var sinPitch = Math.Sin(pitchRadians);
            var cosPitch = Math.Cos(pitchRadians);

            X = (float)(-cosPitch * sinYaw);
            Y = (float)(-sinPitch);
            Z = (float)(cosPitch * cosYaw);

            //X = (float) (-Math.Cos(pitch) * Math.Sin(yaw));
            //Y = (float) -Math.Sin(pitch);
            //Z = (float) (Math.Cos(pitch) * Math.Cos(yaw));

            //X = (float) (Math.Cos(yaw)*Math.Cos(pitch));
            //Y = (float) Math.Sin(yaw);
            //Z = (float) (Math.Cos(yaw) * Math.Sin(pitch));
        }

        public Vector3(float x, float y, float z) { X = x; Y = y; Z = z; }

        public Vector3(double x, double y, double z) { X = (float) x; Y = (float) y; Z = (float) z; }

        public Vector3(Vector3 Vector3) { X = Vector3.X; Y = Vector3.Y; Z = Vector3.Z; }


        /// <summary>
        /// Converts this Vector3 to a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"X: {X}, Y: {Y}, Z: {Z}";

        #region Math

        public static Vector3 Floor(Vector3 Vector3) => new Vector3(Math.Floor(Vector3.X), Math.Floor(Vector3.Y), Math.Floor(Vector3.Z));
        public Vector3 Floor() => Floor(this);

        public static Vector3 Ceiling(Vector3 Vector3) => new Vector3(Math.Ceiling(Vector3.X), Math.Ceiling(Vector3.Y), Math.Ceiling(Vector3.Z));
        public Vector3 Ceiling() => Ceiling(this);


        private static float Square(float num) => num * num;
        public static float DistanceTo(Vector3 value1, Vector3 value2) => value1.DistanceTo(value2);
        public float DistanceTo(Vector3 other) => (float) Math.Sqrt(Square(other.X - X) + Square(other.Y - Y) + Square(other.Z - Z));

        public float Distance() => DistanceTo(Zero);

        public static Vector3 Min(Vector3 value1, Vector3 value2) => new Vector3(Math.Min(value1.X, value2.X), Math.Min(value1.Y, value2.Y), Math.Min(value1.Z, value2.Z));
        public Vector3 Min(Vector3 value2) => new Vector3(Math.Min(X, value2.X), Math.Min(Y, value2.Y), Math.Min(Z, value2.Z));

        public static Vector3 Max(Vector3 value1, Vector3 value2) => new Vector3(Math.Max(value1.X, value2.X), Math.Max(value1.Y, value2.Y), Math.Max(value1.Z, value2.Z));
        public Vector3 Max(Vector3 value2) => new Vector3(Math.Max(X, value2.X), Math.Max(Y, value2.Y), Math.Max(Z, value2.Z));

        public static Vector3 Delta(Vector3 firstLocation, Vector3 secondLocation) => new Vector3(firstLocation.X - secondLocation.X, firstLocation.Y - secondLocation.Y, firstLocation.Z - secondLocation.Z);
        public Vector3 Delta(Vector3 secondLocation) => new Vector3(X - secondLocation.X, Y - secondLocation.Y, Z - secondLocation.Z);

        public static Vector3 Normalize(Vector3 value)
        {
            var factor = DistanceTo(value, Zero);
            factor = 1f / factor;
            return new Vector3(value.X * factor, value.Y * factor, value.Z * factor);
        }
        public Vector3 Normalize() => Normalize(this);

        public static Vector3 Reflect(Vector3 vector, Vector3 normal)
        {
            var dotProduct = ((vector.X * normal.X) + (vector.Y * normal.Y)) + (vector.Z * normal.Z);
            return new Vector3(
                vector.X - (2.0f * normal.X) * dotProduct,
                vector.Y - (2.0f * normal.Y) * dotProduct,
                vector.Z - (2.0f * normal.Z) * dotProduct);
        }
        public Vector3 Reflect(Vector3 normal) => Reflect(this, normal);

        public static float Dot(Vector3 value1, Vector3 value2) => value1.X * value2.X + value1.Y * value2.Y + value1.Z * value2.Z;
        public float Dot(Vector3 value2) => Dot(this, value2);

        public static Vector3 Cross(Vector3 vector1, Vector3 vector2) => new Vector3(vector1.Y * vector2.Z - vector2.Y * vector1.Z, -(vector1.X * vector2.Z - vector2.X * vector1.Z), vector1.X * vector2.Y - vector2.X * vector1.Y);
        public Vector3 Cross(Vector3 vector2) => Cross(this, vector2);


        public static float ToYaw(Vector3 position, Vector3 look)
        {
            var delta = Delta(look, position);

            return (float) Math.Atan2(delta.Z, delta.X);
        }
        public static Vector3 Yaw(Vector3 look, float angle)
        {
            var x = (look.Z * -Math.Sin(angle)) + (look.X * Math.Cos(angle));
            var y = look.Y;
            var z = (look.Z * Math.Cos(angle)) - (look.X * -Math.Sin(angle));

            return new Vector3(x, y, z);
        }
        public Vector3 Yaw(float angle) => Yaw(this, angle);

        public static float ToPitch(Vector3 position, Vector3 look)
        {
            var delta = Delta(look, position);

            return (float)(Math.Atan2(Math.Sqrt(Square(delta.Z) + Square(delta.X)), delta.Y) + Math.PI);
        }
        public static Vector3 Pitch(Vector3 look, float angle)
        {
            var x = look.X;
            var y = (look.Y * Math.Cos(angle)) - (look.Z * Math.Sin(angle));
            var z = (look.Y * Math.Sin(angle)) + (look.Z * Math.Cos(angle));

            return new Vector3(x, y, z);
        }
        public Vector3 Pitch(float angle) => Pitch(this, angle);

        public static float ToRoll(Vector3 position, Vector3 look)
        {
            return 0.0f;
        }
        public static Vector3 Roll(Vector3 look, float angle)
        {
            var x = (look.X * Math.Cos(angle)) - (look.Y * Math.Sin(angle));
            var y = (look.X * Math.Sin(angle)) + (look.Y * Math.Cos(angle));
            var z = look.Z;

            return new Vector3(x, y, z);
        }
        public Vector3 Roll(float angle) => Roll(this, angle);


        public static float ToRadians(float val) => (float) (val * Math.PI / 180.0f);

        #endregion

        #region Operators

        public static Vector3 operator -(Vector3 a) => new Vector3(-a.X, -a.Y, -a.Z);
        public static Vector3 operator ++(Vector3 a) => new Vector3(a.X, a.Y, a.Z) + One;
        public static Vector3 operator --(Vector3 a) => new Vector3(a.X, a.Y, a.Z) - One;

        public static bool operator !=(Vector3 a, Vector3 b) => !a.Equals(b);
        public static bool operator ==(Vector3 a, Vector3 b) => a.Equals(b);
        public static bool operator >(Vector3 a, Vector3 b) => a.X > b.X && a.Y > b.Y && a.Z > b.Z;
        public static bool operator <(Vector3 a, Vector3 b) => a.X < b.X && a.Y < b.Y && a.Z < b.Z;
        public static bool operator >=(Vector3 a, Vector3 b) => a.X >= b.X && a.Y >= b.Y && a.Z >= b.Z;
        public static bool operator <=(Vector3 a, Vector3 b) => a.X <= b.X && a.Y <= b.Y && a.Z <= b.Z;

        public static Vector3 operator +(Vector3 a, Vector3 b) => new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Vector3 operator -(Vector3 a, Vector3 b) => new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static Vector3 operator *(Vector3 a, Vector3 b) => new Vector3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        public static Vector3 operator /(Vector3 a, Vector3 b) => new Vector3(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
        public static Vector3 operator %(Vector3 a, Vector3 b) => new Vector3(a.X % b.X, a.Y % b.Y, a.Z % b.Z);

        public static Vector3 operator +(Vector3 a, float b) => new Vector3(a.X + b, a.Y + b, a.Z + b);
        public static Vector3 operator -(Vector3 a, float b) => new Vector3(a.X - b, a.Y - b, a.Z - b);
        public static Vector3 operator *(Vector3 a, float b) => new Vector3(a.X * b, a.Y * b, a.Z * b);
        public static Vector3 operator /(Vector3 a, float b) => new Vector3(a.X / b, a.Y / b, a.Z / b);
        public static Vector3 operator %(Vector3 a, float b) => new Vector3(a.X % b, a.Y % b, a.Z % b);

        public static Vector3 operator +(float a, Vector3 b) => new Vector3(a + b.X, a + b.Y, a + b.Z);
        public static Vector3 operator -(float a, Vector3 b) => new Vector3(a - b.X, a - b.Y, a - b.Z);
        public static Vector3 operator *(float a, Vector3 b) => new Vector3(a * b.X, a * b.Y, a * b.Z);
        public static Vector3 operator /(float a, Vector3 b) => new Vector3(a / b.X, a / b.Y, a / b.Z);
        public static Vector3 operator %(float a, Vector3 b) => new Vector3(a % b.X, a % b.Y, a % b.Z);

        #endregion

        #region Constants

        public static readonly Vector3 Zero = new Vector3(0, 0, 0);
        public static readonly Vector3 One = new Vector3(1, 1, 1);

        public static readonly Vector3 Up = new Vector3(0, 1, 0);
        public static readonly Vector3 Down = new Vector3(0, -1, 0);
        public static readonly Vector3 Left = new Vector3(-1, 0, 0);
        public static readonly Vector3 Right = new Vector3(1, 0, 0);
        public static readonly Vector3 Backwards = new Vector3(0, 0, -1);
        public static readonly Vector3 Forwards = new Vector3(0, 0, 1);

        public static readonly Vector3 UnitX = new Vector3(1f, 0f, 0f);
        public static readonly Vector3 UnitY = new Vector3(0f, 1f, 0f);
        public static readonly Vector3 UnitZ = new Vector3(0f, 0f, 1f);

        #endregion

        public bool Equals(Vector3 other) => other.X.Equals(X) && other.Y.Equals(Y) && other.Z.Equals(Z);
        public bool Equals(float other) => other.Equals(X) && other.Equals(Y) && other.Equals(Z);

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((Vector3)obj);
        }

        public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
    }
}
