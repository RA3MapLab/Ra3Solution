using System.Collections.Generic;
using System.IO;

namespace MapCoreLib.Core.Asset
{
    public class WorldInfo: MajorAsset
    {
        
        public static Dictionary<string, string> textures = new Dictionary<string, string>
        {
            {"RA3Grid1", "RA3Grid1;RA3Grid1_NRM;"},
            {"Mud_Havana01", "Mud_Havana01;Mud_Havana01_NRM;"},
            {"Mud_Havana02", "Mud_Havana02;Mud_Havana02_NRM;"},
            {"Dirt_Yucatan02", "TDirt_Yucatan02;TDirt_Yucatan02_nrm;"},
            {"RA3_Elevation0", "RA3_Elevation0;RA3_Elevation0_NRM;"},
            {"Dirt_Yucatan04", "TDirt_Yucatan04;TDirt_Yucatan04_nrm;"},
            {"Grass_Yucatan02", "TGrass_Yucatan02;TGrass_Yucatan02_nrm;"},
            {"Dirt_Yucatan03", "TDirt_Yucatan03;TDirt_Yucatan03_nrm;"},
            {"Grass_Yucatan04", "TGrass_Yucatan04;TGrass_Yucatan04_nrm;"},
            {"Dirt_Yucatan06", "TDirt_Yucatan06;TDirt_Yucatan06_nrm;"},
            {"Grass_Yucatan08", "TGrass_Yucatan08;TGrass_Yucatan08_nrm;"},
            {"Grass_Yucatan09", "TGrass_Yucatan09;TGrass_Yucatan09_nrm;"},
            {"Gravel_Yucatan01", "TGravel_Yucatan01;TGravel_Yucatan01_nrm;"},
            {"Reef_Yucatan01", "TReef_Yucatan01;TReef_Yucatan01_nrm;"},
            {"Grass_Yucatan07", "TGrass_Yucatan07;TGrass_Yucatan07_nrm;"},
            {"Dirt_Yucatan05", "TDirt_Yucatan05;TDirt_Yucatan05_nrm;"},
            {"Grass_Yucatan06", "TGrass_Yucatan06;TGrass_Yucatan06_nrm;"},
            {"Dirt_Yucatan01", "TDirt_Yucatan01;TDirt_Yucatan01_nrm;"},
            {"Transition_Yucatan05", "TTransition_Yucatan05;TTransition_Yucatan05_nrm;"},
            {"Rock_Yucatan01", "TRock_Yucatan01;TRock_Yucatan01_nrm;"},
            {"Grass_Yucatan05", "TGrass_Yucatan05;TGrass_Yucatan05_nrm;"},
            {"Transition_Yucatan18", "TTransition_Yucatan18;TTransition_Yucatan18_nrm;"},
            {"Transition_Yucatan20", "TTransition_Yucatan20;TTransition_Yucatan20_nrm;"},
            {"Grass_CapeCod17", "TGrass_CapeCod17;TGrass_CapeCod17_nrm;"},
            {"Grass_Yucatan01", "TGrass_Yucatan01;TGrass_Yucatan01_nrm;"},
            {"Transition_Yucatan21", "TTransition_Yucatan21;TTransition_Yucatan21_nrm;"},
            {"Transition_Yucatan19", "TTransition_Yucatan19;TTransition_Yucatan19_nrm;"},
            {"Grass_Yucatan03", "TGrass_Yucatan03;TGrass_Yucatan03_nrm;"},
            {"Transition_Yucatan04", "TTransition_Yucatan04;TTransition_Yucatan04_nrm;"},
            {"Transition_Yucatan03", "TTransition_Yucatan03;TTransition_Yucatan03_nrm;"},
            {"Transition_Yucatan13", "TTransition_Yucatan13;TTransition_Yucatan13_nrm;"},
            {"Transition_Yucatan01", "TTransition_Yucatan01;TTransition_Yucatan01_nrm;"},
            {"Grass_CapeCod14", "TGrass_CapeCod14;TGrass_CapeCod14_nrm;"},
            {"Transition_Yucatan02", "TTransition_Yucatan02;TTransition_Yucatan02_nrm;"},
            {"Pavement_Yucatan01", "TPavement_Yucatan01;TPavement_Yucatan01_nrm;"},
            {"Pavement_Geneva05", "TPave_Geneva05;TPave_Geneva05_NRM;"},
            {"Transition_Yucatan14", "TTransition_Yucatan14;TTransition_Yucatan14_nrm;"},
            {"Transition_Yucatan16", "TTransition_Yucatan16;TTransition_Yucatan16_nrm;"},
            {"Transition_Yucatan17", "TTransition_Yucatan17;TTransition_Yucatan17_nrm;"},
            {"Transition_Yucatan10", "TTransition_Yucatan10;TTransition_Yucatan10_nrm;"},
            {"Pavement_Geneva04", "TPave_Geneva04;TPave_Geneva04_NRM;"},
            {"Transition_Yucatan15", "TTransition_Yucatan15;TTransition_Yucatan15_nrm;"},
            {"RA3_ShallowSeaFloor", "RA3_ShallowSeaFloor;RA3_ShallowSeaFloor_NRM;"},
            {"Sand_Cannes06", "TSand_Cannes06;TSand_Cannes06_NRM;"},
            {"Sand_Cannes08", "TSand_Cannes08;TSand_Cannes08_NRM;"},
            {"Sand_Cannes03", "TSand_Cannes03;TSand_Cannes03_NRM;"},
            {"Sand_Cannes02", "TSand_Cannes02;TSand_Cannes02_NRM;"},
            {"Grass_Heidelberg09", "Grass_Heidelberg09;Grass_Heidelberg09_nrm;"},
            {"Grass_Heidelberg01", "Grass_Heidelberg01;Grass_Heidelberg01_nrm;"},
            {"Sand_Cannes09", "TSand_Cannes09;TSand_Cannes09_NRM;"},
            {"Pavement_Gibraltar1", "Pavement_Gibraltar01;Pavement_Gibraltar01_NRM;"},
            {"Grass_MtRush01", "Grass_MtRush01;Grass_MtRush01_NRM;"},
            {"Grass_Gibraltar3", "Grass_Gibraltar03;Grass_Gibraltar03_NRM;"},
            {"Grass_Heidelberg11", "Grass_Heidelberg11;Grass_Heidelberg11_nrm;"},
            {"Sand_Cannes01", "TSand_Cannes01;TSand_Cannes01_NRM;"},
            {"Grass_Heidelberg10", "Grass_Heidelberg10;Grass_Heidelberg10_nrm;"},
            {"Grass_Gibraltar1", "Grass_Gibraltar01;Grass_Gibraltar01_NRM;"},
            {"Pavement_Gibraltar3", "Pavement_Gibraltar03;Pavement_Gibraltar03_NRM;"},
            {"Grass_Heidelberg02", "Grass_Heidelberg02;Grass_Heidelberg02_nrm;"},
            {"Sand_Cannes07", "TSand_Cannes07;TSand_Cannes07_NRM;"},
            {"Gravel_Heidelberg01", "Gravel_Heidelberg01;Gravel_Heidelberg01_NRM;"},
            {"Grass_Heidelberg06", "Grass_Heidelberg06;Grass_Heidelberg06_nrm;"},
            {"Pavement_Gibraltar2", "Pavement_Gibraltar02;Pavement_Gibraltar02_NRM;"},
            {"Transition_Heidelberg05", "Transition_Heidelberg05;Transition_Heidelberg05_nrm;"},
            {"Sand_Cannes04", "TSand_Cannes04;TSand_Cannes04_NRM;"},
            {"Cliff_Gibraltar1", "Cliff_Gibralter1;Cliff_Gibralter1_NRM;"},
            {"Cliff_Gibraltar2", "Cliff_Gibralter2;Cliff_Gibralter2_NRM;"},
            {"Transition_Heidelberg08", "Transition_Heidelberg08;Transition_Heidelberg08_nrm;"},
            {"Transition_Heidelberg07", "Transition_Heidelberg07;Transition_Heidelberg07_nrm;"},
            {"Transition_Heidelberg06", "Transition_Heidelberg06;Transition_Heidelberg06_nrm;"},
            {"Grass_Heidelberg08", "Grass_Heidelberg08;Grass_Heidelberg08_nrm;"},
            {"Pavement_SaintPetersburg07", "Pavement_SaintPetersburg07;Pavement_SaintPetersburg07_NRM;"},
            {"Pavement_SaintPetersburg02", "Pavement_SaintPetersburg02;Pavement_SaintPetersburg02_NRM;"},
            {"Pavement_Gibraltar5", "Pavement_GibraltarBoardwalk;Pavement_GibraltarBoardwalk_NRM;"},
            {"Pave_Cannes03", "TPave_Cannes03;TPave_Cannes03_NRM;"},
            {"Pavement_Heidel07", "TPave_Heidel07;TPave_Heidel07_nrm;"},
            {"Pavement_Heidelberg10", "Pavement_Heidelberg10;Pavement_Heidelberg10_nrm;"},
            {"Pavement_Gibraltar6", "Pavement_Gibraltar04;Pavement_Gibraltar04_NRM;"},
            {"Pavement_Heidel02", "TPave_Heidel02;TPave_Heidel02_nrm;"},
            {"Pavement_Heidelberg12", "Pavement_Heidelberg12;Pavement_Heidelberg12_nrm;"},
            {"Transition_Heidelberg01", "Transition_Heidelberg01;Transition_Heidelberg01_nrm;"},
            {"Grass_Heidelberg07", "Grass_Heidelberg07;Grass_Heidelberg07_nrm;"},
            {"Transition_Heidelberg02", "Transition_Heidelberg02;Transition_Heidelberg02_nrm;"},
            {"Pavement_Heidel01", "TPave_Heidel01;TPave_Heidel01_nrm;"},
            {"Reef_Havana02", "Reef_Havana02;Reef_Havana02_NRM;"},
            {"Pavement_Heidelberg11", "Pavement_Heidelberg11;Pavement_Heidelberg11_nrm;"},
            {"Reef_Yucatan02", "TReef_Yucatan02;TReef_Yucatan02_nrm;"},
            {"Pavement_Heidel04", "TPave_Heidel04;TPave_Heidel04_nrm;"},
            {"Grass_CapeCod12", "TGrass_CapeCod12;TGrass_CapeCod12_nrm;"},
            {"Pavement_Heidel09", "TPave_Heidel09;TPave_Heidel09_nrm;"},
            {"Grass_Heidelberg04", "Grass_Heidelberg04;Grass_Heidelberg04_nrm;"},
            {"Grass_Heidelberg03", "Grass_Heidelberg03;Grass_Heidelberg03_nrm;"},
            {"Dirt_Heidelberg01", "Dirt_Heidelberg01;Dirt_Heidelberg01_NRM;"},
            {"Grass_Heidelberg05", "Grass_Heidelberg05;Grass_Heidelberg05_nrm;"},
            {"Cliff_Easter01", "TCliff_Easter01;TCliff_Easter01_NRM;"},
            {"Dirt_Easter08", "TDirt_Easter08;TDirt_Easter08_NRM;"},
            {"Dirt_Easter06", "TDirt_Easter06;TDirt_Easter06_NRM;"},
            {"Dirt_Easter05", "TDirt_Easter05;TDirt_Easter05_NRM;"},
            {"Dirt_Easter09", "TDirt_Easter09;TDirt_Easter09_NRM;"},
            {"Dirt_Easter01", "TDirt_Easter01;TDirt_Easter01_NRM;"},
            {"Dirt_Easter07", "TDirt_Easter07;TDirt_Easter07_NRM;"},
            {"Dirt_Easter03", "TDirt_Easter03;TDirt_Easter03_NRM;"},
            {"Grass_CapeCod05", "TGrass_CapeCod05;TGrass_CapeCod05_nrm;"},
            {"Cliff_Easter02", "TCliff_Easter02;TCliff_Easter02_NRM;"},
            {"Grass_Easter01", "TGrass_Easter01;TGrass_Easter01_NRM;"},
            {"Grass_CapeCod19", "TGrass_CapeCod19;TGrass_CapeCod19_nrm;"},
            {"Sand_Hawaii03", "TSand_Hawaii03;TSand_Hawaii03_NRM;"},
            {"Grass_CapeCod11", "TGrass_CapeCod11;TGrass_CapeCod11_nrm;"},
            {"Dirt_Easter02", "TDirt_Easter02;TDirt_Easter02_NRM;"},
            {"Cliff_Easter03", "TCliff_Easter03;TCliff_Easter03_NRM;"},
            {"Grass_MtRush07", "Grass_MtRush07;Grass_MtRush07_NRM;"},
            {"Dirt_CapeCod02", "TDirt_CapeCod02;TDirt_CapeCod02_nrm;"},
            {"Grass_CapeCod01", "TGrass_CapeCod01;TGrass_CapeCod01_nrm;"},
            {"Grass_CapeCod06", "TGrass_CapeCod06;TGrass_CapeCod06_nrm;"},
            {"Grass_CapeCod09", "TGrass_CapeCod09;TGrass_CapeCod09_nrm;"},
            {"Dirt_CapeCod04", "TDirt_CapeCod04;TDirt_CapeCod04_nrm;"},
            {"Grass_CapeCod03", "TGrass_CapeCod03;TGrass_CapeCod03_nrm;"},
            {"Grass_CapeCod07", "TGrass_CapeCod07;TGrass_CapeCod07_nrm;"},
            {"Grass_CapeCod04", "TGrass_CapeCod04;TGrass_CapeCod04_nrm;"},
            {"Grass_CapeCod02", "TGrass_CapeCod02;TGrass_CapeCod02_nrm;"},
            {"Grass_CapeCod08", "TGrass_CapeCod08;TGrass_CapeCod08_nrm;"},
            {"Asphalt05", "TMisc_Asphalt05;TMisc_Asphalt05_nrm;"},
            {"Pavement_Mykonos04", "TPave_Mykonos04;TPave_Mykonos04_NRM;"},
            {"Pavement_CapeCod01", "TPave_CapeCod01;TPave_CapeCod01_nrm;"},
            {"Dirt_CapeCod01", "TDirt_CapeCod01;TDirt_CapeCod01_nrm;"},
            {"Grass_CapeCod18", "TGrass_CapeCod18;TGrass_CapeCod18_nrm;"},
            {"Grass_MtRush05", "Grass_MtRush05;Grass_MtRush05_NRM;"},
            {"Grass_Hawaii07", "Grass_Hawaii07;Grass_Hawaii07_NRM;"},
            {"Grass_Hawaii04", "Grass_Hawaii04;Grass_Hawaii04_NRM;"},
            {"Grass_MtRush03", "Grass_MtRush03;Grass_MtRush03_NRM;"},
            {"Grass_MtRush04", "Grass_MtRush04;Grass_MtRush04_NRM;"},
            {"Grass_MtRush02", "Grass_MtRush02;Grass_MtRush02_NRM;"},
            {"Transition_Yucatan08", "TTransition_Yucatan08;TTransition_Yucatan08_nrm;"},
            {"Transition_Yucatan06", "TTransition_Yucatan06;TTransition_Yucatan06_nrm;"},
            {"Transition_Yucatan07", "TTransition_Yucatan07;TTransition_Yucatan07_nrm;"},
            {"Rock_Yucatan05", "TRock_Yucatan05;TRock_Yucatan05_nrm;"},
            {"Transition_Heidelberg03", "Transition_Heidelberg03;Transition_Heidelberg03_nrm;"},
            {"Transition_Heidelberg04", "Transition_Heidelberg04;Transition_Heidelberg04_nrm;"},
            {"Sand_SantaMonica01", "TSand_SantaMonica01;TSand_SantaMonica01_NRM;"},
            {"Pave_Cannes01", "TPave_Cannes01;TPave_Cannes01_NRM;"},
            {"Pavement_Amsterdam03", "TPave_Amsterdam03;TPave_Amsterdam03_NRM;"},
            {"Grass_Hawaii01", "Grass_Hawaii01;Grass_Hawaii01_NRM;"},
            {"Rock_Yucatan03", "TRock_Yucatan03;TRock_Yucatan03_nrm;"},
            {"Pavement_Havana03", "Pavement_Havana03;Pavement_Havana03_NRM;"},
            {"Grass_Hawaii02", "Grass_Hawaii02;Grass_Hawaii02_NRM;"},
            {"Pave_SantaMonica01", "TPave_SantaMonica01;TPave_SantaMonica01_NRM;"},
            {"Pave_Cannes02", "TPave_Cannes02;TPave_Cannes02_NRM;"},
            {"Pavement_Geneva01", "TPave_Geneva01;TPave_Geneva01_NRM;"},
            {"Pave_SantaMonica04", "TPave_SantaMonica04;TPave_SantaMonica04_NRM;"},
            {"Pavement_SaintPetersburg01", "Pavement_SaintPetersburg01;Pavement_SaintPetersburg01_NRM;"},
            {"Grass_Hawaii03", "Grass_Hawaii03;Grass_Hawaii03_NRM;"},
            {"Grass_Hawaii08", "Grass_Hawaii08;Grass_Hawaii08_NRM;"},
            {"Cliff_Easter04", "TCliff_Easter04;TCliff_Easter04_NRM;"},
            {"Mud_Fortress01", "Mud_Fortress01;Mud_Fortress01_NRM;"},
            {"Transition_Yucatan11", "TTransition_Yucatan11;TTransition_Yucatan11_nrm;"},
            {"Transition_Yucatan12", "TTransition_Yucatan12;TTransition_Yucatan12_nrm;"},
            {"Transition_Yucatan09", "TTransition_Yucatan09;TTransition_Yucatan09_nrm;"},
            {"Dirt_Iceland04", "TDirt_Iceland04;TDirt_Iceland04_NRM;"},
            {"Dirt_Iceland03", "TDirt_Iceland03;TDirt_Iceland03_NRM;"},
            {"Dirt_Gypsy01", "TDirt_Gypsy01;TDirt_Gypsy01_NRM;"},
            {"Rock_Yucatan02", "TRock_Yucatan02;TRock_Yucatan02_nrm;"},
            {"Cliff_MtRush02", "Cliff_MtRush02;Cliff_MtRush02_NRM;"},
            {"Pavement_Odessa01", "Pavement_Odessa01;Pavement_Odessa01_NRM;"},
            {"Pavement_Heidel08", "TPave_Heidel08;TPave_Heidel08_nrm;"},
            {"Cliff_CapeCod01", "TCliff_CapeCod01;TCliff_CapeCod01_nrm;"},
            {"BB_Pavement02", "BB_Pavement02;BB_Pavement02_NRM;"},
            {"Grass_Geneva04", "TGrass_Geneva04;TGrass_Geneva04_NRM;"},
            {"Grass_CapeCod20", "TGrass_CapeCod20;TGrass_CapeCod20_nrm;"},
            {"Grass_Geneva05", "TGrass_Geneva05;TGrass_Geneva05_NRM;"},
            {"Grass_Gibraltar2", "Grass_Gibraltar02;Grass_Gibraltar02_NRM;"},
            {"Grass_CapeCod16", "TGrass_CapeCod16;TGrass_CapeCod16_nrm;"},
            {"Sand_Cannes05", "TSand_Cannes05;TSand_Cannes05_NRM;"},
            {"Asphalt02", "TMisc_Asphalt02;TMisc_Asphalt02_nrm;"},
            {"Asphalt03", "TMisc_Asphalt03;TMisc_Asphalt03_nrm;"},
            {"Pavement_SaintPetersburg05", "Pavement_SaintPetersburg05;Pavement_SaintPetersburg05_NRM;"},
            {"Grass_NewYork01", "TGrass_NewYork01;TGrass_NewYork01_NRM;"},
            {"SteelDeck01", "TMisc_SteelDeck01;TMisc_SteelDeck01_nrm;"},
            {"Cliff_CapeCod05", "TCliff_CapeCod05;TCliff_CapeCod05_nrm;"},
            {"Cliff_CapeCod03", "TCliff_CapeCod03;TCliff_CapeCod03_nrm;"},
            {"Dirt_Mykonos01", "TDirt_Mykonos01;TDirt_Mykonos01_NRM;"},
            {"Asphalt01", "TMisc_Asphalt01;TMisc_Asphalt01_nrm;"},
            {"SteelDeck02", "TMisc_SteelDeck02;TMisc_SteelDeck02_nrm;"},
            {"Asphalt04", "TMisc_Asphalt04;TMisc_Asphalt04_nrm;"},
            {"SteelDeck03", "TMisc_SteelDeck03;TMisc_SteelDeck03_nrm;"},
            {"SteelDeck04", "TMisc_SteelDeck04;TMisc_SteelDeck04_nrm;"},
            {"Sand_Hawaii01", "TSand_Hawaii01;TSand_Hawaii01_NRM;"},
            {"Pavement_TokyoHarbor04", "Pavement_TokyoHarbor04;Pavement_TokyoHarbor04_NRM;"},
            {"Dirt_CapeCod07", "TDirt_CapeCod07;TDirt_CapeCod07_nrm;"},
            {"Pavement_CapeCod02", "TPave_CapeCod02;TPave_CapeCod02_nrm;"},
            {"Dirt_CapeCod08", "TDirt_CapeCod08;TDirt_CapeCod08_nrm;"},
            {"Grass_CapeCod15", "TGrass_CapeCod15;TGrass_CapeCod15_nrm;"},
            {"Pavement_TokyoHarbor09", "Pavement_TokyoHarbor09;Pavement_TokyoHarbor09_NRM;"},
            {"Grass_CapeCod21", "TGrass_CapeCod21;TGrass_CapeCod21_nrm;"},
            {"Dirt_Gypsy03", "TDirt_Gypsy03;TDirt_Gypsy03_NRM;"},
            {"Sand_SantaMonica02", "TSand_SantaMonica02;TSand_SantaMonica02_NRM;"},
            {"Pave_Golf02", "TPave_Golf02;TPave_Golf02_NRM;"},
            {"Pavement_TokyoHarbor03", "Pavement_TokyoHarbor03;Pavement_TokyoHarbor03_NRM;"},
            {"Pave_Golf03", "TPave_Golf03;TPave_Golf03_NRM;"},
            {"Pavement_TokyoHarbor01", "Pavement_TokyoHarbor01;Pavement_TokyoHarbor01_NRM;"},
        };
        
        public AssetPropertyCollection properties = new AssetPropertyCollection();
        protected override void parseData(BinaryReader binaryReader, MapDataContext context)
        {
            properties = new AssetPropertyCollection().fromStream(binaryReader, context);
        }

        protected override void saveData(BinaryWriter binaryWriter, MapDataContext context)
        {
            properties.saveData(binaryWriter, context);
        }

        public override string getAssetName()
        {
            return Ra3MapConst.ASSET_WorldInfo;
        }

        public override short getVersion()
        {
            return 1;
        }

        public static WorldInfo newInstance(MapDataContext context, NewMapConfig config)
        {
            var worldInfo = new WorldInfo();
            worldInfo.name = Ra3MapConst.ASSET_WorldInfo;
            worldInfo.id = context.MapStruct.RegisterString(worldInfo.name);
            worldInfo.version = worldInfo.getVersion();
            worldInfo.properties.addProperty("musicZone", "MusicPalette:MusicPalette_NotSet", context);
            worldInfo.properties.addProperty("weather", WeatherType.Normal, context);
            worldInfo.properties.addProperty("terrainTextureStrings", $"{textures[config.defaultTexture]}", context);  //TODO 这个需要额外管理起来，避免重复
            worldInfo.properties.addProperty("mapName", "", context);
            worldInfo.properties.addProperty("mapDescription", "map created with ra3 map core lib by wu", context);
            worldInfo.properties.addProperty("compression", CompressionType.RefPack, context);
            worldInfo.properties.addProperty("cameraGroundMinHeight", 0f, context);
            worldInfo.properties.addProperty("cameraGroundMaxHeight", 2500f, context);
            worldInfo.properties.addProperty("cameraMinHeight", 40f, context);
            worldInfo.properties.addProperty("cameraMaxHeight", 800f, context);
            worldInfo.properties.addProperty("isScenarioMultiplayer", false, context);
            return worldInfo;
        }
        
        public enum WeatherType
        {
            Normal,
            Cloudy,
            Rainy,
            CloudyRainy,
            Sunny,
            Snowy,
            Invalid
        }
        
        public enum CompressionType
        {
            None,
            RefPack
        }
    }
}