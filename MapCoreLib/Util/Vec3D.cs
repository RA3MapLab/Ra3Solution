using System;

namespace MapCoreLib.Util
{
    public class Vec3D
    {
        public float x;

        public float y;

        public float z;

        public Vec3D()
        {
        }

        public Vec3D(float X, float Y)
        {
            x = X;
            y = Y;
            z = 0f;
        }

        public Vec3D(double X, double Y)
        {
            x = (float)X;
            y = (float)Y;
            z = 0f;
        }

        public Vec3D(float X, float Y, float Z)
        {
            x = X;
            y = Y;
            z = Z;
        }

        public Vec3D(double X, double Y, double Z)
        {
            x = (float)X;
            y = (float)Y;
            z = (float)Z;
        }

        public override string ToString()
        {
            return $"({x},{y},{z})";
        }

        public static Vec3D fromXmlStr(string value)
        {
            if (value.StartsWith("(") && value.EndsWith(")"))
            {
                var coords = value.Substring(1, value.Length - 2).Split(',');
                if (coords.Length != 3)
                {
                    throw new Exception("坐标格式有错误");
                }
                else
                {
                    return new Vec3D(Convert.ToSingle(coords[0]), Convert.ToSingle(coords[1]),
                        Convert.ToSingle(coords[2]));
                }
            }

            throw new Exception("坐标格式有错误");
        }
    }
}