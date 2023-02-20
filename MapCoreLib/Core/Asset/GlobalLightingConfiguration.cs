using System.IO;

namespace MapCoreLib.Core.Asset
{
    public class GlobalLightingConfiguration
    {
        public GlobalLight TerrainSun;
        public GlobalLight TerrainAccent1;
        public GlobalLight TerrainAccent2;

        public GlobalLightingConfiguration fromStream(BinaryReader binaryReader, MapDataContext context)
        {
            TerrainSun = new GlobalLight().fromStream(binaryReader, context);
            TerrainAccent1 = new GlobalLight().fromStream(binaryReader, context);
            TerrainAccent2 = new GlobalLight().fromStream(binaryReader, context);
            return this;
        }

        public void saveData(BinaryWriter binaryWriter, MapDataContext context)
        {
            TerrainSun.saveData(binaryWriter, context);
            TerrainAccent1.saveData(binaryWriter, context);
            TerrainAccent2.saveData(binaryWriter, context);
        }
    }
}