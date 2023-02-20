using System.IO;
using MapCoreLib.Util;

namespace MapCoreLib.Core.Asset
{

    public class GlobalLighting : MajorAsset
    {
        public int time;
        public GlobalLightingConfiguration[] lightingConfigurations = new GlobalLightingConfiguration[4];
        public MapColorArgb shadowColor;
        public ColorRgbF noCloudFactor;

        protected override void parseData(BinaryReader binaryReader, MapDataContext context)
        {
            time = binaryReader.ReadInt32();
            for (int i = 0; i < lightingConfigurations.Length; i++)
            {
                lightingConfigurations[i] = new GlobalLightingConfiguration().fromStream(binaryReader, context);
            }

            shadowColor = new MapColorArgb().fromStream(binaryReader, context);
            noCloudFactor = binaryReader.ReadColorRgbF();
        }

        protected override void saveData(BinaryWriter binaryWriter, MapDataContext context)
        {
            binaryWriter.Write(time);
            foreach (var lightingConfiguration in lightingConfigurations)
            {
                lightingConfiguration.saveData(binaryWriter, context);
            }

            shadowColor.saveData(binaryWriter, context);
            binaryWriter.writeColorRgbF(noCloudFactor);
        }

        public override string getAssetName()
        {
            return Ra3MapConst.ASSET_GlobalLighting;
        }
    }
    
    public struct MapColorArgb
    {
        public byte A;
        public byte R;
        public byte G;
        public byte B;

        public MapColorArgb(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        public MapColorArgb fromStream(BinaryReader binaryReader, MapDataContext context)
        {
            var value = binaryReader.ReadUInt32();
            return new MapColorArgb
            {
                A = (byte) ((value >> 24) & 0xFF),
                R = (byte) ((value >> 16) & 0xFF),
                G = (byte) ((value >> 8) & 0xFF),
                B = (byte) (value & 0xFF)
            };
        }

        public override bool Equals(object obj)
        {
            return (obj is MapColorArgb) && Equals((MapColorArgb) obj);
        }

        public bool Equals(MapColorArgb other)
        {
            return A == other.A
                   && R == other.R
                   && G == other.G
                   && B == other.B;
        }

        public override string ToString()
        {
            return $"{{A:{A} R:{R} G:{G} B:{B}}}";
        }

        public void saveData(BinaryWriter binaryWriter, MapDataContext context)
        {
            var combined = (A << 24) | (R << 16) | (G << 8) | B;
            binaryWriter.Write((uint) combined);
        }
    }
    
    public readonly struct ColorRgbF
    {
        public readonly float R;
        public readonly float G;
        public readonly float B;

        public ColorRgbF(float r, float g, float b)
        {
            R = r;
            G = g;
            B = b;
        }

        public Vec3D ToVector3()
        {
            return new Vec3D(R, G, B);
        }
    }
}