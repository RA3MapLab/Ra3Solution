using System.IO;
using MapCoreLib.Util;

namespace MapCoreLib.Core.Asset
{
    public class StandingWaterArea
    {
        public int id;

        public string name;

        public float UVScrollSpeed;

        public bool additiveBlending;

        public string bumpmapTexture;

        public string skyTexture;

        public Vec2D[] points;

        public int waterHeight;

        public string fxShader;

        public string depthColors;

        public StandingWaterArea()
        {
            
        }
        
        // public StandingWaterArea(int height, params Vec2D[] Points)
        // {
        //     id = 1;
        //     name = "";
        //     UVScrollSpeed = 0.0600000024f;
        //     additiveBlending = false;
        //     bumpmapTexture = "WaterRippleBump";
        //     skyTexture = "SkyEnv";
        //     points = Points;
        //     waterHeight = height;
        //     fxShader = "FXOceanRA3";
        //     depthColors = "LUTDepthTint.tga";
        // }
        
        public StandingWaterArea fromStream(BinaryReader binaryReader, MapDataContext context)
        {
            id = binaryReader.ReadInt32();
            name = binaryReader.readDefaultString();
            binaryReader.ReadInt16();
            UVScrollSpeed = binaryReader.ReadSingle();
            additiveBlending = binaryReader.ReadBoolean();
            bumpmapTexture = binaryReader.readDefaultString();
            skyTexture = binaryReader.readDefaultString();
            points = new Vec2D[binaryReader.ReadInt32()];
            for (int i = 0; i < points.Length; i++)
            {
                ref Vec2D reference = ref points[i];
                reference = new Vec2D(binaryReader);
            }
            waterHeight = binaryReader.ReadInt32();
            fxShader = binaryReader.readDefaultString();
            depthColors = binaryReader.readDefaultString();
            return this;
        }

        public void saveData(BinaryWriter binaryWriter, MapDataContext context)
        {
            binaryWriter.Write(id);
            binaryWriter.writeDefaultString(name);
            binaryWriter.Write((short)0);
            binaryWriter.Write(UVScrollSpeed);
            binaryWriter.Write(additiveBlending);
            binaryWriter.writeDefaultString(bumpmapTexture);
            binaryWriter.writeDefaultString(skyTexture);
            binaryWriter.Write(points.Length);
            Vec2D[] array = points;
            foreach (Vec2D point in array)
            {
                point.Save(binaryWriter);
            }
            binaryWriter.Write(waterHeight);
            binaryWriter.writeDefaultString(fxShader);
            binaryWriter.writeDefaultString(depthColors);
        }

        public static StandingWaterArea newInstance(MapDataContext context, int width, int height)
        {
            var standingWaterArea = new StandingWaterArea();
            standingWaterArea.id = 1;
            standingWaterArea.name = "";
            standingWaterArea.UVScrollSpeed = 0.0600000024f;
            standingWaterArea.additiveBlending = false;
            standingWaterArea.bumpmapTexture = "WaterRippleBump";
            standingWaterArea.skyTexture = "SkyEnv";
            standingWaterArea.points = new []
            {
                new Vec2D(-context.border * 10, -context.border * 10),
                new Vec2D(-context.border * 10, (height + context.border) * 10),
                new Vec2D((width + context.border) * 10, (height + context.border) * 10), new Vec2D((width + context.border) * 10, -context.border * 10)
            };
            standingWaterArea.waterHeight = 200;
            standingWaterArea.fxShader = "FXOceanRA3";
            standingWaterArea.depthColors = "LUTDepthTint.tga";

            return standingWaterArea;
        }
    }
}