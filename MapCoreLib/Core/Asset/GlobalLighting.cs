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

        public override short getVersion()
        {
            return 11;
        }

        public static GlobalLighting newInstance(MapDataContext context)
        {
            var globalLighting = new GlobalLighting();
            globalLighting.name = Ra3MapConst.ASSET_GlobalLighting;
            globalLighting.id = context.MapStruct.RegisterString(globalLighting.name);
            globalLighting.version = globalLighting.getVersion();
            globalLighting.time = 1;
            globalLighting.shadowColor = new MapColorArgb(127, 160, 160, 160);
            globalLighting.noCloudFactor = new ColorRgbF(1, 1, 1);
            globalLighting.lightingConfigurations = new[]
            {
                new GlobalLightingConfiguration()
                {
                    TerrainAccent1 = new GlobalLight()
                    {
                        Ambient = new Vec3D(0,0,0),
                        Color = new Vec3D(0.6901961,0.6666667,0.6901961),
                        Direction = new Vec3D(0.8070462,0.5863534,-0.06975648)
                    },
                    TerrainAccent2 = new GlobalLight()
                    {
                        Ambient = new Vec3D(0,0,0),
                        Color = new Vec3D(0.7450981,0.8313726,0.8941177),
                        Direction = new Vec3D(0.328771,-0.9032905,-0.2756374)
                    },
                    TerrainSun = new GlobalLight()
                    {
                        Ambient = new Vec3D(0.1411765,0.1333333,0.1254902),
                        Color = new Vec3D(1.247059,1.207843,1.043137),
                        Direction = new Vec3D(-0.6291487,0.3487428,-0.6946585)
                    }
                },
                new GlobalLightingConfiguration()
                {
                    TerrainAccent1 = new GlobalLight()
                    {
                        Ambient = new Vec3D(0,0,0),
                        Color = new Vec3D(0.345098,0.6117647,0.7764706),
                        Direction = new Vec3D(-0.7697511,-0.5389856,-0.3420202)
                    },
                    TerrainAccent2 = new GlobalLight()
                    {
                        Ambient = new Vec3D(0,0,0),
                        Color = new Vec3D(0.4470588,0.6666667,0.5803922),
                        Direction = new Vec3D(0.4995398,-0.5953282,-0.6293205)
                    },
                    TerrainSun = new GlobalLight()
                    {
                        Ambient = new Vec3D(0.1568628,0.1411765,0.1568628),
                        Color = new Vec3D(1.247059,0.9960784,0.9019608),
                        Direction = new Vec3D(0.4059417,0.6496425,-0.6427876)
                    }
                },
                new GlobalLightingConfiguration()
                {
                    TerrainAccent1 = new GlobalLight()
                    {
                        Ambient = new Vec3D(0,0,0),
                        Color = new Vec3D(0.3215686,0.5019608,0.7764706),
                        Direction = new Vec3D(-0.9395494,0.01639982,-0.3420202)
                    },
                    TerrainAccent2 = new GlobalLight()
                    {
                        Ambient = new Vec3D(0,0,0),
                        Color = new Vec3D(0.4862745,0.5019608,0.6666667),
                        Direction = new Vec3D(0.3036552,-0.7153665,-0.6293205)
                    },
                    TerrainSun = new GlobalLight()
                    {
                        Ambient = new Vec3D(0.1803922,0.1411765,0.08627451),
                        Color = new Vec3D(1.670588,1.105882,0.7450981),
                        Direction = new Vec3D(0.6730281,0.5450075,-0.5000001)
                    }
                },
                new GlobalLightingConfiguration()
                {
                    TerrainAccent1 = new GlobalLight()
                    {
                        Ambient = new Vec3D(0,0,0),
                        Color = new Vec3D(0.2509804,0.2509804,0.3764706),
                        Direction = new Vec3D(0.9781476,0.2079117,-4.371139E-08)
                    },
                    TerrainAccent2 = new GlobalLight()
                    {
                        Ambient = new Vec3D(0,0,0),
                        Color = new Vec3D(0.2509804,0.2509804,0.3764706),
                        Direction = new Vec3D(-0.05233583,-0.9986296,-4.371139E-08)
                    },
                    TerrainSun = new GlobalLight()
                    {
                        Ambient = new Vec3D(0,0,0),
                        Color = new Vec3D(0.9411765,0.9411765,1.247059),
                        Direction = new Vec3D(-0.3210197,0.3210196,-0.8910066)
                    }
                },
            };
            return globalLighting;
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