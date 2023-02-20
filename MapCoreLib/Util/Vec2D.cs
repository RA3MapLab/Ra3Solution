using System;
using System.IO;

namespace MapCoreLib.Util
{
    public class Vec2D
    {
        public float x;

		public float y;

		public Vec2D()
		{
		}

		public Vec2D(float X, float Y)
		{
			x = X;
			y = Y;
		}

		public Vec2D(double X, double Y)
		{
			x = (float)X;
			y = (float)Y;
		}

		public Vec2D(BinaryReader br)
		{
			x = br.ReadSingle();
			y = br.ReadSingle();
		}

		public override string ToString()
		{
			return $"{x:F2}, {y:F2}";
		}

		public string ToString(string format)
		{
			return string.Format(format, x, y);
		}

		public bool IsZero()
		{
			if (x == 0f)
			{
				return y == 0f;
			}
			return false;
		}

		public void Save(BinaryWriter bw)
		{
			bw.Write(x);
			bw.Write(y);
		}

		public float Distance(Vec2D v)
		{
			return (float)Math.Sqrt((x - v.x) * (x - v.x) + (y - v.y) * (y - v.y));
		}

		public Vec2D ReflectX(float X)
		{
			return new Vec2D(X + (X - x), y);
		}

		public Vec2D ReflectY(float Y)
		{
			return new Vec2D(x, Y + (Y - y));
		}

		public static float operator *(Vec2D left, Vec2D right)
		{
			return left.x * right.x + left.y * right.y;
		}

		public static Vec2D operator *(Vec2D left, float right)
		{
			return new Vec2D(left.x * right, left.y * right);
		}

		public static Vec2D operator *(float left, Vec2D right)
		{
			return new Vec2D(right.x * left, right.y * left);
		}

		public static Vec2D operator /(Vec2D left, float right)
		{
			return new Vec2D(left.x / right, left.y / right);
		}

		public static Vec2D operator /(float left, Vec2D right)
		{
			return new Vec2D(left / right.x, left / right.y);
		}

		public static Vec2D operator +(Vec2D left, Vec2D right)
		{
			return new Vec2D(left.x + right.x, left.y + right.y);
		}

		public static Vec2D operator -(Vec2D left, Vec2D right)
		{
			return new Vec2D(left.x - right.x, left.y - right.y);
		}

		public float Length()
		{
			return (float)Math.Sqrt(x * x + y * y);
		}

		public Vec2D Project(Vec2D target)
		{
			return this * target / (target * target) * target;
		}

		public Vec2D Rotate(double a)
		{
			float sin = (float)Math.Sin(a);
			float cos = (float)Math.Cos(a);
			return new Vec2D(cos * x - sin * y, sin * x + cos * y);
		}

		public void Normalize()
		{
			float len = Length();
			x /= len;
			y /= len;
		}

		public static explicit operator Vec3D(Vec2D v)
		{
			return new Vec3D(v.x, v.y, 0f);
		}
    }
}