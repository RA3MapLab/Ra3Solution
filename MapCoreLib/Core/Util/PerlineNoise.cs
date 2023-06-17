using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using MapCoreLib.Util;

namespace MapCoreLib.Core.Util
{
    public class PerlineNoise
    {
		private static float[,,] noise;

		private static float[,,] smoothNoise;

		public static float elevationScale = 255f;

		public static float maxElevation = 255f;

		private static int passes;

		private static int pass;

		private static int width;

		private static int height;

		public static float[,] NoiseMap(int width, int height, int seed, int passCount)
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			PerlineNoise.width = width;
			PerlineNoise.height = height;
			passes = passCount;
			SeedNoise(width, height, seed);
			float[,] map = new float[width, height];
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					map[x, y] = PerlinNoise2D(x, y);
				}
			}
			noise = null;
			smoothNoise = null;
			GC.Collect();
			LogUtil.log($"Perlin noise map generated in {stopwatch.ElapsedMilliseconds}ms");
			return map;
		}
		private static void SeedNoise(int width, int height, int seed)
		{
			Random random = new Random(seed);
			smoothNoise = new float[passes, width + 1, height + 1];
			noise = new float[passes, width + 3, height + 3];
			for (int i = 0; i < passes; i++)
			{
				for (int x = 0; x < width + 3; x++)
				{
					for (int y = 0; y < height + 3; y++)
					{
						noise[i, x, y] = (float)random.NextDouble() * 2f - 1f;
					}
				}
			}
		}

		private static float Noise(int x, int y)
		{
			return noise[pass, x + 1, y + 1];
		}

		private static float SmoothNoise(int x, int y)
		{
			if (smoothNoise[pass, x, y] != 0f)
			{
				return smoothNoise[pass, x, y];
			}
			float corners = (noise[pass, x, y] + noise[pass, x + 2, y] + noise[pass, x, y + 2] + noise[pass, x + 2, y + 2]) / 16f;
			float sides = (noise[pass, x, y + 1] + noise[pass, x + 2, y + 1] + noise[pass, x + 1, y] + noise[pass, x + 1, y + 2]) / 8f;
			float center = noise[pass, x + 1, y + 1] / 4f;
			if (smoothNoise[pass, x, y] == 0f)
			{
				smoothNoise[pass, x, y] = corners + sides + center;
			}
			return corners + sides + center;
		}

		private static float InterpolatedNoise(float x, float y)
		{
			int intX = (int)x;
			float fracX = x - (float)intX;
			int intY = (int)y;
			float fracY = y - (float)intY;
			float v1 = SmoothNoise(intX, intY);
			float v2 = SmoothNoise(intX + 1, intY);
			float v3 = SmoothNoise(intX, intY + 1);
			float v4 = SmoothNoise(intX + 1, intY + 1);
			float i1 = v1 - fracX * (v1 - v2);
			float i2 = v3 - fracX * (v3 - v4);
			return i1 - fracY * (i1 - i2);
		}

		private static float Interpolate(float a, float b, float x)
		{
			return a - x * (a - b);
		}

		private static float PerlinNoise2D(float x, float y)
		{
			float total = 0f;
			for (int i = 0; i < passes; i++)
			{
				pass = i;
				float f = (float)Math.Pow(2.0, -i);
				float a = (float)Math.Pow(0.5, passes - 1 - i);
				total += InterpolatedNoise(x * f, y * f) * a;
			}
			return total = (total + 1f) / 2f;
		}

		private static float PerlinNoise2DFrequency(float x, float y)
		{
			float total = 0f;
			for (int i = 0; i < passes; i++)
			{
				pass = i;
				float f = (float)Math.Pow(2.0, -i);
				float a = (float)Math.Pow(0.5, passes - 1 - i);
				total += InterpolatedNoise(x * f, y * f) * a;
			}
			return (total + 1f) / 2f;
		}

		private static float PerlinNoise2DWaveLength(float x, float y)
		{
			float total = 0f;
			float p = 0.5f;
			float maxWL = Math.Max(width, height);
			for (int i = 0; i < passes; i++)
			{
				pass = i;
				float a = (float)Math.Pow(p, i);
				float wl = maxWL / (float)Math.Pow(2.0, i);
				Wave(pass, out a, out wl);
				if (wl < 1f)
				{
					break;
				}
				float j = InterpolatedNoise(x / wl, y / wl) * a;
				total += j;
			}
			return (total + 1f) / 2f;
		}

		private static void Wave(int pass, out float a, out float wl)
		{
			float maxWL = Math.Max(width, height);
			switch (pass)
			{
			case 0:
				a = 0.6f;
				wl = maxWL;
				return;
			case 1:
				a = 0.3f;
				wl = maxWL / 2f;
				return;
			case 2:
				a = 0.15f;
				wl = maxWL / 4f;
				return;
			}
			if (pass == 2)
			{
				a = 0.075f;
				wl = maxWL / 8f;
			}
			else
			{
				a = 0f;
				wl = 1f;
			}
		}

		public static Color ToColor(ushort v)
		{
			int r = 0;
			int g = 0;
			int b = 0;
			r = (((int)v % 4 == 0) ? 255 : (((int)v % 2 == 0) ? 128 : 0));
			g = v >> 8;
			b = v & 0xFF;
			return Color.FromArgb(r, g, b);
		}

		public static Color ToColor(float v)
		{
			int r = 0;
			int g = 0;
			int b = 0;
			if (v < 128f)
			{
				b = (int)(127f - v) * 2;
				g = (int)v * 2;
			}
			else
			{
				g = (int)(255f - v) * 2;
				r = (int)(v - 128f) * 2;
			}
			return Color.FromArgb(r, g, b);
		}

		public static int ToColorWater(float v)
		{
			return -16777216 | (int)((uint)(v * 160f) * 257 << 8) | 0xFF;
		}

		public static int ToColorLand(float v)
		{
			byte b = 0;
			byte g;
			byte r;
			if (v < 0.5f)
			{
				g = byte.MaxValue;
				r = (byte)(v * 2f * 255f);
			}
			else
			{
				r = byte.MaxValue;
				g = (byte)((1f - v) * 2f * 255f);
			}
			return (int)(0xFF000000u | (r << 16) | (g << 8) | b);
		}
	}
}