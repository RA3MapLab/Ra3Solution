using System;
using System.IO;
using MapCoreLib.Util;

namespace MapCoreLib.Core.Asset
{
    public class BuildListItem
    {
        public string BuildingName;
        public string Name;
        public Vec3D position;
        public float angle;
        public bool StructureAlreadyBuilt;
        public bool unknown1;
        public int rebuilds;
        public string script;
        public int startingHealth;
        public bool unknown2;
        public bool unknown3;
        public bool unknown4;
        public BuildListItem fromStream(BinaryReader binaryReader, MapDataContext context)
        {
            BuildingName = binaryReader.readDefaultString();
            Name = binaryReader.readDefaultString();
            position = binaryReader.readVec3D();
            angle = binaryReader.ReadSingle();
            StructureAlreadyBuilt = binaryReader.ReadBoolean();
            
            unknown1 = binaryReader.ReadBoolean();
            rebuilds = binaryReader.ReadInt32();

            script = binaryReader.readDefaultString();

            startingHealth = binaryReader.ReadInt32();

            unknown2 = binaryReader.ReadBoolean();
            unknown3 = binaryReader.ReadBoolean();
            unknown4 = binaryReader.ReadBoolean();
            return this;
        }

        public void saveData(BinaryWriter binaryWriter, MapDataContext context)
        {
            binaryWriter.writeDefaultString(BuildingName);
            binaryWriter.writeDefaultString(Name);
            binaryWriter.writeVec3D(position);
            binaryWriter.Write(angle);
            binaryWriter.Write(StructureAlreadyBuilt);
            
            binaryWriter.Write(unknown1);
            binaryWriter.Write(rebuilds);
            binaryWriter.writeDefaultString(script);
            binaryWriter.Write(startingHealth);
            binaryWriter.Write(unknown2);
            binaryWriter.Write(unknown3);
            binaryWriter.Write(unknown4);
        }

        public void inits(string encodedString)
        {
            // var random = new Random();
            // BuildingName = System.Guid.NewGuid().ToString().Substring(0,4);
            // Name = System.Guid.NewGuid().ToString().Substring(0,4);
            // position = new Vec3D(random.NextDouble(), random.NextDouble());
            
            BuildingName = encodedString;
            Name = "SovietAirfield_Collapse";
            position = new Vec3D(-100,-100,0);
            angle = 0;
            StructureAlreadyBuilt = true;
            unknown1 = true;
            
            rebuilds = 2;
            script = "wu";
            startingHealth = 100;
            
            unknown2 = true;
            unknown3 = true;
            unknown4 = true;
        }
    }
}