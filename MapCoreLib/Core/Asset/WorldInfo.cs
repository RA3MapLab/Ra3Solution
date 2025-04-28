using System;
using System.Collections.Generic;
using System.IO;
using MapCoreLib.Util;

namespace MapCoreLib.Core.Asset
{
    public class WorldInfo: MajorAsset
    {
        
        public static Dictionary<string, string> textures = new Dictionary<string, string>
        {
                // Romania
                {"Pavement_Romania01", "Pavement_Romania01;Pavement_Romania01_NRM;"},
                {"Dirt_Romania01", "Dirt_Romania01;Dirt_Romania01_NRM;"},
                {"Dock_Romania01", "Dock_Romania01;Dock_Romania01_NRM;"},
                // Hot_Springs
                {"Dirt_HotSprings01", "TDirt_HotSprings01;TDirt_HotSprings01_NRM;"},
                {"Dirt_HotSprings02", "TDirt_HotSprings02;TDirt_HotSprings02_NRM;"},
                {"Dirt_HotSprings03", "TDirt_HotSprings03;TDirt_HotSprings03_NRM;"},
                {"Dirt_HotSprings04", "TDirt_HotSprings04;TDirt_HotSprings04_NRM;"},
                {"Dirt_HotSprings05", "TDirt_HotSprings05;TDirt_HotSprings05_NRM;"},
                {"Dirt_HotSprings06", "TDirt_HotSprings06;TDirt_HotSprings06_NRM;"},
                {"Dirt_HotSprings07", "TDirt_HotSprings07;TDirt_HotSprings07_NRM;"},
                {"Dirt_HotSprings08", "TDirt_HotSprings08;TDirt_HotSprings08_NRM;"},
                {"Grass_HotSprings01", "TGrass_HotSprings01;TGrass_HotSprings01_NRM;"},
                {"Grass_HotSprings02", "TGrass_HotSprings02;TGrass_HotSprings02_NRM;"},
                {"Grass_HotSprings03", "TGrass_HotSprings03;TGrass_HotSprings03_NRM;"},
                {"Grass_HotSprings04", "TGrass_HotSprings04;TGrass_HotSprings04_NRM;"},
                {"Grass_HotSprings05", "TGrass_HotSprings05;TGrass_HotSprings05_NRM;"},
                {"Grass_HotSprings06", "TGrass_HotSprings06;TGrass_HotSprings06_NRM;"},
                {"Grass_HotSprings07", "TGrass_HotSprings07;TGrass_HotSprings07_NRM;"},
                {"Grass_HotSprings08", "TGrass_HotSprings08;TGrass_HotSprings08_NRM;"},
                {"Transition_HotSprings01", "Transition_HotSprings01;Transition_HotSprings01_NRM;"},
                {"Transition_HotSprings02", "Transition_HotSprings02;Transition_HotSprings02_NRM;"},
                {"Transition_HotSprings03", "Transition_HotSprings03;Transition_HotSprings03_NRM;"},
                {"Transition_HotSprings04", "Transition_HotSprings04;Transition_HotSprings04_NRM;"},
                // Island_Fortress
                {"SteelDeck01", "TMisc_SteelDeck01;TMisc_SteelDeck01_nrm;"},
                {"SteelDeck02", "TMisc_SteelDeck02;TMisc_SteelDeck02_nrm;"},
                {"SteelDeck03", "TMisc_SteelDeck03;TMisc_SteelDeck03_nrm;"},
                {"SteelDeck04", "TMisc_SteelDeck04;TMisc_SteelDeck04_nrm;"},
                {"SteelDeck05", "TMisc_SteelDeck05;TMisc_SteelDeck05_nrm;"},
                {"SteelDeck06", "TMisc_SteelDeck06;TMisc_SteelDeck06_nrm;"},
                {"Asphalt01", "TMisc_Asphalt01;TMisc_Asphalt01_nrm;"},
                {"Asphalt02", "TMisc_Asphalt02;TMisc_Asphalt02_nrm;"},
                {"Asphalt03", "TMisc_Asphalt03;TMisc_Asphalt03_nrm;"},
                {"Asphalt04", "TMisc_Asphalt04;TMisc_Asphalt04_nrm;"},
                {"Asphalt05", "TMisc_Asphalt05;TMisc_Asphalt05_nrm;"},
                {"FortressBlackEdge", "TMisc_BlackEdge;TMisc_BlackEdge_nrm;"},
                {"Mud_Fortress01", "Mud_Fortress01;Mud_Fortress01_NRM;"},
                {"Mud_Fortress02", "Mud_Fortress02;Mud_Fortress02_NRM;"},
                // Yucatan
                {"Grass_Yucatan01", "TGrass_Yucatan01;TGrass_Yucatan01_nrm;"},
                {"Grass_Yucatan02", "TGrass_Yucatan02;TGrass_Yucatan02_nrm;"},
                {"Grass_Yucatan03", "TGrass_Yucatan03;TGrass_Yucatan03_nrm;"},
                {"Grass_Yucatan04", "TGrass_Yucatan04;TGrass_Yucatan04_nrm;"},
                {"Grass_Yucatan05", "TGrass_Yucatan05;TGrass_Yucatan05_nrm;"},
                {"Grass_Yucatan06", "TGrass_Yucatan06;TGrass_Yucatan06_nrm;"},
                {"Grass_Yucatan07", "TGrass_Yucatan07;TGrass_Yucatan07_nrm;"},
                {"Grass_Yucatan08", "TGrass_Yucatan08;TGrass_Yucatan08_nrm;"},
                {"Grass_Yucatan09", "TGrass_Yucatan09;TGrass_Yucatan09_nrm;"},
                {"Rock_Yucatan01", "TRock_Yucatan01;TRock_Yucatan01_nrm;"},
                {"Rock_Yucatan02", "TRock_Yucatan02;TRock_Yucatan02_nrm;"},
                {"Rock_Yucatan03", "TRock_Yucatan03;TRock_Yucatan03_nrm;"},
                {"Rock_Yucatan04", "TRock_Yucatan04;TRock_Yucatan04_nrm;"},
                {"Rock_Yucatan05", "TRock_Yucatan05;TRock_Yucatan05_nrm;"},
                {"Gravel_Yucatan01", "TGravel_Yucatan01;TGravel_Yucatan01_nrm;"},
                {"Dirt_Yucatan01", "TDirt_Yucatan01;TDirt_Yucatan01_nrm;"},
                {"Dirt_Yucatan02", "TDirt_Yucatan02;TDirt_Yucatan02_nrm;"},
                {"Dirt_Yucatan03", "TDirt_Yucatan03;TDirt_Yucatan03_nrm;"},
                {"Dirt_Yucatan04", "TDirt_Yucatan04;TDirt_Yucatan04_nrm;"},
                {"Dirt_Yucatan05", "TDirt_Yucatan05;TDirt_Yucatan05_nrm;"},
                {"Dirt_Yucatan06", "TDirt_Yucatan06;TDirt_Yucatan06_nrm;"},
                {"Reef_Yucatan01", "TReef_Yucatan01;TReef_Yucatan01_nrm;"},
                {"Reef_Yucatan02", "TReef_Yucatan02;TReef_Yucatan02_nrm;"},
                {"Pavement_Yucatan01", "TPavement_Yucatan01;TPavement_Yucatan01_nrm;"},
                {"Transition_Yucatan01", "TTransition_Yucatan01;TTransition_Yucatan01_nrm;"},
                {"Transition_Yucatan02", "TTransition_Yucatan02;TTransition_Yucatan02_nrm;"},
                {"Transition_Yucatan03", "TTransition_Yucatan03;TTransition_Yucatan03_nrm;"},
                {"Transition_Yucatan04", "TTransition_Yucatan04;TTransition_Yucatan04_nrm;"},
                {"Transition_Yucatan05", "TTransition_Yucatan05;TTransition_Yucatan05_nrm;"},
                {"Transition_Yucatan06", "TTransition_Yucatan06;TTransition_Yucatan06_nrm;"},
                {"Transition_Yucatan07", "TTransition_Yucatan07;TTransition_Yucatan07_nrm;"},
                {"Transition_Yucatan08", "TTransition_Yucatan08;TTransition_Yucatan08_nrm;"},
                {"Transition_Yucatan09", "TTransition_Yucatan09;TTransition_Yucatan09_nrm;"},
                {"Transition_Yucatan10", "TTransition_Yucatan10;TTransition_Yucatan10_nrm;"},
                {"Transition_Yucatan11", "TTransition_Yucatan11;TTransition_Yucatan11_nrm;"},
                {"Transition_Yucatan12", "TTransition_Yucatan12;TTransition_Yucatan12_nrm;"},
                {"Transition_Yucatan13", "TTransition_Yucatan13;TTransition_Yucatan13_nrm;"},
                {"Transition_Yucatan14", "TTransition_Yucatan14;TTransition_Yucatan14_nrm;"},
                {"Transition_Yucatan15", "TTransition_Yucatan15;TTransition_Yucatan15_nrm;"},
                {"Transition_Yucatan16", "TTransition_Yucatan16;TTransition_Yucatan16_nrm;"},
                {"Transition_Yucatan17", "TTransition_Yucatan17;TTransition_Yucatan17_nrm;"},
                {"Transition_Yucatan18", "TTransition_Yucatan18;TTransition_Yucatan18_nrm;"},
                {"Transition_Yucatan19", "TTransition_Yucatan19;TTransition_Yucatan19_nrm;"},
                {"Transition_Yucatan20", "TTransition_Yucatan20;TTransition_Yucatan20_nrm;"},
                {"Transition_Yucatan21", "TTransition_Yucatan21;TTransition_Yucatan21_nrm;"},
                // Golf_Course
                {"Pave_Golf01", "TPave_Golf01;TPave_Golf01_NRM;"},
                {"Pave_Golf02", "TPave_Golf02;TPave_Golf02_NRM;"},
                {"Pave_Golf03", "TPave_Golf03;TPave_Golf03_NRM;"},
                {"Pave_Golf04", "TPave_Golf04;TPave_Golf04_NRM;"},
                {"Pave_Golf05", "TPave_Golf05;TPave_Golf05_NRM;"},
                {"Pave_Golf06", "TPave_Golf06;TPave_Golf06_NRM;"},
                {"Pave_Golf07", "TPave_Golf07;TPave_Golf07_NRM;"},
                {"Grass_Golf01", "TGrass_Golf01;TGrass_Golf01_NRM;"},
                {"Grass_Golf02", "TGrass_Golf02;TGrass_Golf02_NRM;"},
                {"Grass_Golf03", "TGrass_Golf03;TGrass_Golf03_NRM;"},
                {"Grass_Golf04", "TGrass_Golf04;TGrass_Golf04_NRM;"},
                {"Grass_Golf05", "TGrass_Golf05;TGrass_Golf05_NRM;"},
                {"Grass_Golf06", "TGrass_Golf06;TGrass_Golf06_NRM;"},
                {"Grass_Golf07", "TGrass_Golf07;TGrass_Golf07_NRM;"},
                {"Grass_Golf08", "TGrass_Golf08;TGrass_Golf08_NRM;"},
                // Solvang
                {"Snow_Solvang01", "TSnow_Solvang01;TSnow_Solvang01_nrm;"},
                {"Snow_Solvang02", "TSnow_Solvang02;TSnow_Solvang02_nrm;"},
                {"Snow_Solvang03", "TSnow_Solvang03;TSnow_Solvang03_nrm;"},
                {"Snow_Solvang04", "TSnow_Solvang04;TSnow_Solvang04_nrm;"},
                {"Snow_Solvang05", "TSnow_Solvang05;TSnow_Solvang05_nrm;"},
                {"Snow_Solvang06", "TSnow_Solvang06;TSnow_Solvang06_nrm;"},
                {"Snow_Solvang07", "TSnow_Solvang07;TSnow_Solvang07_nrm;"},
                {"Snow_Solvang08", "TSnow_Solvang08;TSnow_Solvang08_NRM;"},
                {"Snow_Solvang09", "TSnow_Solvang09;TSnow_Solvang09_NRM;"},
                {"Cliff_Solvang01", "TClif_Solvang01;TClif_Solvang01_NRM;"},
                {"Cliff_Solvang02", "TClif_Solvang02;TClif_Solvang02_NRM;"},
                {"Cliff_Solvang03", "TClif_Solvang03;TClif_Solvang03_NRM;"},
                {"Cliff_Solvang04", "TClif_Solvang04;TClif_Solvang04_NRM;"},
                {"Rock_Solvang01", "TRock_Solvang01;Temp_NRM;"},
                {"Rock_Solvang02", "TRock_Solvang02;Temp_NRM;"},
                {"Snow_Solvang10", "TSnow_Solvang10;Temp_NRM;"},
                {"Pavement_Solvang01", "TPave_Solvang01;TPave_Solvang01_nrm;"},
                // Heidelberg
                {"Grass_Heidelberg01", "Grass_Heidelberg01;Grass_Heidelberg01_nrm;"},
                {"Grass_Heidelberg02", "Grass_Heidelberg02;Grass_Heidelberg02_nrm;"},
                {"Grass_Heidelberg03", "Grass_Heidelberg03;Grass_Heidelberg03_nrm;"},
                {"Grass_Heidelberg04", "Grass_Heidelberg04;Grass_Heidelberg04_nrm;"},
                {"Grass_Heidelberg05", "Grass_Heidelberg05;Grass_Heidelberg05_nrm;"},
                {"Grass_Heidelberg06", "Grass_Heidelberg06;Grass_Heidelberg06_nrm;"},
                {"Grass_Heidelberg07", "Grass_Heidelberg07;Grass_Heidelberg07_nrm;"},
                {"Grass_Heidelberg08", "Grass_Heidelberg08;Grass_Heidelberg08_nrm;"},
                {"Grass_Heidelberg09", "Grass_Heidelberg09;Grass_Heidelberg09_nrm;"},
                {"Grass_Heidelberg10", "Grass_Heidelberg10;Grass_Heidelberg10_nrm;"},
                {"Grass_Heidelberg11", "Grass_Heidelberg11;Grass_Heidelberg11_nrm;"},
                {"Transition_Heidelberg01", "Transition_Heidelberg01;Transition_Heidelberg01_nrm;"},
                {"Transition_Heidelberg02", "Transition_Heidelberg02;Transition_Heidelberg02_nrm;"},
                {"Transition_Heidelberg03", "Transition_Heidelberg03;Transition_Heidelberg03_nrm;"},
                {"Transition_Heidelberg04", "Transition_Heidelberg04;Transition_Heidelberg04_nrm;"},
                {"Transition_Heidelberg05", "Transition_Heidelberg05;Transition_Heidelberg05_nrm;"},
                {"Transition_Heidelberg06", "Transition_Heidelberg06;Transition_Heidelberg06_nrm;"},
                {"Transition_Heidelberg07", "Transition_Heidelberg07;Transition_Heidelberg07_nrm;"},
                {"Transition_Heidelberg08", "Transition_Heidelberg08;Transition_Heidelberg08_nrm;"},
                {"Pavement_Heidel01", "TPave_Heidel01;TPave_Heidel01_nrm;"},
                {"Pavement_Heidel02", "TPave_Heidel02;TPave_Heidel02_nrm;"},
                {"Pavement_Heidel03", "TPave_Heidel03;TPave_Heidel03_nrm;"},
                {"Pavement_Heidel04", "TPave_Heidel04;TPave_Heidel04_nrm;"},
                {"Pavement_Heidel05", "TPave_Heidel05;TPave_Heidel05_nrm;"},
                {"Pavement_Heidel06", "TPave_Heidel06;TPave_Heidel06_nrm;"},
                {"Pavement_Heidel07", "TPave_Heidel07;TPave_Heidel07_nrm;"},
                {"Pavement_Heidel08", "TPave_Heidel08;TPave_Heidel08_nrm;"},
                {"Pavement_Heidel09", "TPave_Heidel09;TPave_Heidel09_nrm;"},
                {"Pavement_Heidelberg10", "Pavement_Heidelberg10;Pavement_Heidelberg10_nrm;"},
                {"Pavement_Heidelberg11", "Pavement_Heidelberg11;Pavement_Heidelberg11_nrm;"},
                {"Pavement_Heidelberg12", "Pavement_Heidelberg12;Pavement_Heidelberg12_nrm;"},
                {"Gravel_Heidelberg01", "Gravel_Heidelberg01;Gravel_Heidelberg01_NRM;"},
                {"Dirt_Heidelberg01", "Dirt_Heidelberg01;Dirt_Heidelberg01_NRM;"},
                // Geneva
                {"Grass_Geneva01", "TGrass_Geneva01;TGrass_Geneva01_NRM;"},
                {"Grass_Geneva02", "TGrass_Geneva02;TGrass_Geneva02_NRM;"},
                {"Grass_Geneva03", "TGrass_Geneva03;TGrass_Geneva03_NRM;"},
                {"Grass_Geneva04", "TGrass_Geneva04;TGrass_Geneva04_NRM;"},
                {"Grass_Geneva05", "TGrass_Geneva05;TGrass_Geneva05_NRM;"},
                {"Grass_GenevaClockA", "TGrass_GenevaClockA;TGrass_GenevaClockA_NRM;"},
                {"Grass_GenevaClockB", "TGrass_GenevaClockB;TGrass_GenevaClockB_NRM;"},
                {"Grass_GenevaClockC", "TGrass_GenevaClockC;TGrass_GenevaClockC_NRM;"},
                {"Grass_GenevaClockD", "TGrass_GenevaClockD;TGrass_GenevaClockD_NRM;"},
                {"Pavement_Geneva01", "TPave_Geneva01;TPave_Geneva01_NRM;"},
                {"Pavement_Geneva02", "TPave_Geneva02;TPave_Geneva02_NRM;"},
                {"Pavement_Geneva03", "TPave_Geneva03;TPave_Geneva03_NRM;"},
                {"Pavement_Geneva04", "TPave_Geneva04;TPave_Geneva04_NRM;"},
                {"Pavement_Geneva05", "TPave_Geneva05;TPave_Geneva05_NRM;"},
                {"Pavement_Geneva06", "TPave_Geneva06;TPave_Geneva06_NRM;"},
                // Cannes
                {"Sand_Cannes01", "TSand_Cannes01;TSand_Cannes01_NRM;"},
                {"Sand_Cannes02", "TSand_Cannes02;TSand_Cannes02_NRM;"},
                {"Sand_Cannes03", "TSand_Cannes03;TSand_Cannes03_NRM;"},
                {"Sand_Cannes04", "TSand_Cannes04;TSand_Cannes04_NRM;"},
                {"Sand_Cannes05", "TSand_Cannes05;TSand_Cannes05_NRM;"},
                {"Sand_Cannes06", "TSand_Cannes06;TSand_Cannes06_NRM;"},
                {"Sand_Cannes07", "TSand_Cannes07;TSand_Cannes07_NRM;"},
                {"Sand_Cannes08", "TSand_Cannes08;TSand_Cannes08_NRM;"},
                {"Sand_Cannes09", "TSand_Cannes09;TSand_Cannes09_NRM;"},
                {"Sand_Cannes10", "TSand_Cannes10;TSand_Cannes10_NRM;"},
                {"Pave_Cannes01", "TPave_Cannes01;TPave_Cannes01_NRM;"},
                {"Pave_Cannes02", "TPave_Cannes02;TPave_Cannes02_NRM;"},
                {"Pave_Cannes03", "TPave_Cannes03;TPave_Cannes03_NRM;"},
                // Havana
                {"Mud_Havana01", "Mud_Havana01;Mud_Havana01_NRM;"},
                {"Mud_Havana02", "Mud_Havana02;Mud_Havana02_NRM;"},
                {"Pavement_Havana01", "Pavement_Havana01;Pavement_Havana01_NRM;"},
                {"Pavement_Havana02", "Pavement_Havana02;Pavement_Havana02_NRM;"},
                {"Pavement_Havana03", "Pavement_Havana03;Pavement_Havana03_NRM;"},
                {"Pavement_Havana04", "Pavement_Havana04;Pavement_Havana04_NRM;"},
                {"Pavement_Havana05", "Pavement_Havana05;Pavement_Havana05_NRM;"},
                {"Reef_Havana01", "Reef_Havana01;Reef_Havana01_NRM;"},
                {"Reef_Havana02", "Reef_Havana02;Reef_Havana02_NRM;"},
                // Mykonos
                {"Pavement_Mykonos01", "TPave_Mykonos01;TPave_Mykonos01_NRM;"},
                {"Pavement_Mykonos02", "TPave_Mykonos02;TPave_Mykonos02_NRM;"},
                {"Pavement_Mykonos03", "TPave_Mykonos03;TPave_Mykonos03_NRM;"},
                {"Pavement_Mykonos04", "TPave_Mykonos04;TPave_Mykonos04_NRM;"},
                {"Pavement_Mykonos05", "TPave_Mykonos05;TPave_Mykonos05_NRM;"},
                {"Dirt_Mykonos01", "TDirt_Mykonos01;TDirt_Mykonos01_NRM;"},
                {"Dirt_Mykonos02", "TDirt_Mykonos02;TDirt_Mykonos02_NRM;"},
                {"Dirt_Mykonos03", "TDirt_Mykonos03;TDirt_Mykonos03_NRM;"},
                {"Dirt_Mykonos04", "TDirt_Mykonos04;TDirt_Mykonos04_NRM;"},
                {"Dirt_Mykonos05", "TDirt_Mykonos05;TDirt_Mykonos05_NRM;"},
                {"Dirt_Mykonos06", "TDirt_Mykonos06;TDirt_Mykonos06_NRM;"},
                {"Dirt_Mykonos07", "TDirt_Mykonos07;TDirt_Mykonos07_NRM;"},
                {"Grass_Mykonos01", "TGrass_Mykonos01;TGrass_Mykonos01_NRM;"},
                {"Grass_Mykonos02", "TGrass_Mykonos02;TGrass_Mykonos02_NRM;"},
                // Kremlin
                {"Pavement_Kremlin01", "Pavement_Kremlin01;Pavement_Kremlin01_NRM;"},
                {"Pavement_Kremlin02", "Pavement_Kremlin02;Pavement_Kremlin02_NRM;"},
                {"Pavement_Kremlin03", "Pavement_Kremlin03;Pavement_Kremlin03_NRM;"},
                {"Pavement_Kremlin04", "Pavement_Kremlin04;Pavement_Kremlin04_NRM;"},
                {"Pavement_Kremlin05", "Pavement_Kremlin05;Pavement_Kremlin05_NRM;"},
                {"Pavement_Kremlin06", "Pavement_Kremlin06;Pavement_Kremlin06_NRM;"},
                {"Transition_Kremlin01", "Transition_Kremlin01;Transition_Kremlin01_NRM;"},
                {"Transition_Kremlin02", "Transition_Kremlin02;Transition_Kremlin02_NRM;"},
                {"Transition_Kremlin03", "Transition_Kremlin03;Transition_Kremlin03_NRM;"},
                {"Transition_Kremlin04", "Transition_Kremlin04;Transition_Kremlin04_NRM;"},
                {"Transition_Kremlin05", "Transition_Kremlin05;Transition_Kremlin05_NRM;"},
                {"Transition_Kremlin06", "Transition_Kremlin06;Transition_Kremlin06_NRM;"},
                {"Transition_Kremlin07", "Transition_Kremlin07;Transition_Kremlin07_NRM;"},
                {"Transition_Kremlin08", "Transition_Kremlin08;Transition_Kremlin08_NRM;"},
                // Odessa
                {"Pavement_Odessa01", "Pavement_Odessa01;Pavement_Odessa01_NRM;"},
                {"Rock_Odessa01", "Rock_Odessa01;Rock_Odessa01_NRM;"},
                // Santa_Monica
                {"Pave_SantaMonica01", "TPave_SantaMonica01;TPave_SantaMonica01_NRM;"},
                {"Pave_SantaMonica02", "TPave_SantaMonica02;TPave_SantaMonica02_NRM;"},
                {"Pave_SantaMonica03", "TPave_SantaMonica03;TPave_SantaMonica03_NRM;"},
                {"Pave_SantaMonica04", "TPave_SantaMonica04;TPave_SantaMonica04_NRM;"},
                {"Pave_SantaMonica05", "TPave_SantaMonica05;TPave_SantaMonica05_NRM;"},
                {"Sand_SantaMonica01", "TSand_SantaMonica01;TSand_SantaMonica01_NRM;"},
                {"Sand_SantaMonica02", "TSand_SantaMonica02;TSand_SantaMonica02_NRM;"},
                {"Grass_SantaMonica01", "Grass_SantaMonica01;Grass_SantaMonica01_NRM;"},
                // Saint_Petersburg
                {"Pavement_SaintPetersburg01", "Pavement_SaintPetersburg01;Pavement_SaintPetersburg01_NRM;"},
                {"Pavement_SaintPetersburg02", "Pavement_SaintPetersburg02;Pavement_SaintPetersburg02_NRM;"},
                {"Pavement_SaintPetersburg03", "Pavement_SaintPetersburg03;Pavement_SaintPetersburg03_NRM;"},
                {"Pavement_SaintPetersburg04", "Pavement_SaintPetersburg04;Pavement_SaintPetersburg04_NRM;"},
                {"Pavement_SaintPetersburg05", "Pavement_SaintPetersburg05;Pavement_SaintPetersburg05_NRM;"},
                {"Pavement_SaintPetersburg06", "Pavement_SaintPetersburg06;Pavement_SaintPetersburg06_NRM;"},
                {"Pavement_SaintPetersburg07", "Pavement_SaintPetersburg07;Pavement_SaintPetersburg07_NRM;"},
                {"Pavement_SaintPetersburg08", "Pavement_SaintPetersburg08;Pavement_SaintPetersburg08_NRM;"},
                {"Grass_SaintPetersburg01", "Grass_SaintPetersburg01;Grass_SaintPetersburg01_NRM;"},
                // Easter_Island
                {"Dirt_Easter01", "TDirt_Easter01;TDirt_Easter01_NRM;"},
                {"Dirt_Easter02", "TDirt_Easter02;TDirt_Easter02_NRM;"},
                {"Dirt_Easter03", "TDirt_Easter03;TDirt_Easter03_NRM;"},
                {"Dirt_Easter04", "TDirt_Easter04;TDirt_Easter04_NRM;"},
                {"Dirt_Easter05", "TDirt_Easter05;TDirt_Easter05_NRM;"},
                {"Dirt_Easter06", "TDirt_Easter06;TDirt_Easter06_NRM;"},
                {"Dirt_Easter07", "TDirt_Easter07;TDirt_Easter07_NRM;"},
                {"Dirt_Easter08", "TDirt_Easter08;TDirt_Easter08_NRM;"},
                {"Dirt_Easter09", "TDirt_Easter09;TDirt_Easter09_NRM;"},
                {"Dirt_Easter10", "TDirt_Easter10;TDirt_Easter10_NRM;"},
                {"Dirt_Easter11", "TDirt_Easter11;TDirt_Easter11_NRM;"},
                {"Dirt_Easter12", "TDirt_Easter12;TDirt_Easter12_NRM;"},
                {"Grass_Easter01", "TGrass_Easter01;TGrass_Easter01_NRM;"},
                {"Grass_Easter02", "TGrass_Easter02;TGrass_Easter02_NRM;"},
                {"Grass_Easter03", "TGrass_Easter03;TGrass_Easter03_NRM;"},
                {"Grass_Easter04", "TGrass_Easter04;TGrass_Easter04_NRM;"},
                {"Grass_Easter05", "TGrass_Easter05;TGrass_Easter05_NRM;"},
                {"Grass_Easter06", "TGrass_Easter06;TGrass_Easter06_NRM;"},
                {"Grass_Easter07", "TGrass_Easter07;TGrass_Easter07_NRM;"},
                {"Cliff_Easter01", "TCliff_Easter01;TCliff_Easter01_NRM;"},
                {"Cliff_Easter02", "TCliff_Easter02;TCliff_Easter02_NRM;"},
                {"Cliff_Easter03", "TCliff_Easter03;TCliff_Easter03_NRM;"},
                {"Cliff_Easter04", "TCliff_Easter04;TCliff_Easter04_NRM;"},
                // Cape_Cod
                {"Grass_CapeCod01", "TGrass_CapeCod01;TGrass_CapeCod01_nrm;"},
                {"Grass_CapeCod02", "TGrass_CapeCod02;TGrass_CapeCod02_nrm;"},
                {"Grass_CapeCod03", "TGrass_CapeCod03;TGrass_CapeCod03_nrm;"},
                {"Grass_CapeCod04", "TGrass_CapeCod04;TGrass_CapeCod04_nrm;"},
                {"Grass_CapeCod05", "TGrass_CapeCod05;TGrass_CapeCod05_nrm;"},
                {"Grass_CapeCod06", "TGrass_CapeCod06;TGrass_CapeCod06_nrm;"},
                {"Grass_CapeCod07", "TGrass_CapeCod07;TGrass_CapeCod07_nrm;"},
                {"Grass_CapeCod08", "TGrass_CapeCod08;TGrass_CapeCod08_nrm;"},
                {"Grass_CapeCod09", "TGrass_CapeCod09;TGrass_CapeCod09_nrm;"},
                {"Grass_CapeCod10", "TGrass_CapeCod10;TGrass_CapeCod10_nrm;"},
                {"Grass_CapeCod11", "TGrass_CapeCod11;TGrass_CapeCod11_nrm;"},
                {"Grass_CapeCod12", "TGrass_CapeCod12;TGrass_CapeCod12_nrm;"},
                {"Grass_CapeCod13", "TGrass_CapeCod13;TGrass_CapeCod13_nrm;"},
                {"Grass_CapeCod14", "TGrass_CapeCod14;TGrass_CapeCod14_nrm;"},
                {"Grass_CapeCod15", "TGrass_CapeCod15;TGrass_CapeCod15_nrm;"},
                {"Grass_CapeCod16", "TGrass_CapeCod16;TGrass_CapeCod16_nrm;"},
                {"Grass_CapeCod17", "TGrass_CapeCod17;TGrass_CapeCod17_nrm;"},
                {"Grass_CapeCod18", "TGrass_CapeCod18;TGrass_CapeCod18_nrm;"},
                {"Grass_CapeCod19", "TGrass_CapeCod19;TGrass_CapeCod19_nrm;"},
                {"Grass_CapeCod20", "TGrass_CapeCod20;TGrass_CapeCod20_nrm;"},
                {"Grass_CapeCod21", "TGrass_CapeCod21;TGrass_CapeCod21_nrm;"},
                {"Grass_CapeCod22", "TGrass_CapeCod22;TGrass_CapeCod22_nrm;"},
                {"Grass_CapeCod23", "TGrass_CapeCod23;TGrass_CapeCod23_nrm;"},
                {"Grass_CapeCod24", "TGrass_CapeCod24;TGrass_CapeCod24_nrm;"},
                {"Pavement_CapeCod01", "TPave_CapeCod01;TPave_CapeCod01_nrm;"},
                {"Pavement_CapeCod02", "TPave_CapeCod02;TPave_CapeCod02_nrm;"},
                {"Dirt_CapeCod01", "TDirt_CapeCod01;TDirt_CapeCod01_nrm;"},
                {"Dirt_CapeCod02", "TDirt_CapeCod02;TDirt_CapeCod02_nrm;"},
                {"Dirt_CapeCod03", "TDirt_CapeCod03;TDirt_CapeCod03_nrm;"},
                {"Dirt_CapeCod04", "TDirt_CapeCod04;TDirt_CapeCod04_nrm;"},
                {"Dirt_CapeCod05", "TDirt_CapeCod05;TDirt_CapeCod05_nrm;"},
                {"Dirt_CapeCod06", "TDirt_CapeCod06;TDirt_CapeCod06_nrm;"},
                {"Dirt_CapeCod07", "TDirt_CapeCod07;TDirt_CapeCod07_nrm;"},
                {"Dirt_CapeCod08", "TDirt_CapeCod08;TDirt_CapeCod08_nrm;"},
                {"Cliff_CapeCod01", "TCliff_CapeCod01;TCliff_CapeCod01_nrm;"},
                {"Cliff_CapeCod02", "TCliff_CapeCod02;TCliff_CapeCod02_nrm;"},
                {"Cliff_CapeCod03", "TCliff_CapeCod03;TCliff_CapeCod03_nrm;"},
                {"Cliff_CapeCod04", "TCliff_CapeCod04;TCliff_CapeCod04_nrm;"},
                {"Cliff_CapeCod05", "TCliff_CapeCod05;TCliff_CapeCod05_nrm;"},
                // New_York
                {"Pavement_NewYork01", "TPave_NewYork01;TPave_NewYork01_NRM;"},
                {"Pavement_NewYork02", "TPave_NewYork02;TPave_NewYork02_NRM;"},
                {"Pavement_NewYork03", "TPave_NewYork03;TPave_NewYork03_NRM;"},
                {"Grass_NewYork01", "TGrass_NewYork01;TGrass_NewYork01_NRM;"},
                // Mount_Rushmore
                {"Pavement_MtRush01", "TPave_MtRush01;TPave_MtRush01_NRM;"},
                {"Grass_MtRush01", "Grass_MtRush01;Grass_MtRush01_NRM;"},
                {"Grass_MtRush02", "Grass_MtRush02;Grass_MtRush02_NRM;"},
                {"Grass_MtRush03", "Grass_MtRush03;Grass_MtRush03_NRM;"},
                {"Grass_MtRush04", "Grass_MtRush04;Grass_MtRush04_NRM;"},
                {"Grass_MtRush05", "Grass_MtRush05;Grass_MtRush05_NRM;"},
                {"Grass_MtRush06", "Grass_MtRush06;Grass_MtRush06_NRM;"},
                {"Grass_MtRush07", "Grass_MtRush07;Grass_MtRush07_NRM;"},
                {"Grass_MtRush08", "Grass_MtRush08;Grass_MtRush08_NRM;"},
                {"Grass_MtRush09", "Grass_MtRush09;Grass_MtRush09_NRM;"},
                {"Grass_MtRush10", "Grass_MtRush10;Grass_MtRush10_NRM;"},
                {"Grass_MtRush11", "Grass_MtRush11;Grass_MtRush11_NRM;"},
                {"Grass_MtRush12", "Grass_MtRush12;Grass_MtRush12_NRM;"},
                {"Grass_MtRush13", "Grass_MtRush13;Grass_MtRush13_NRM;"},
                {"Cliff_MtRush01", "Cliff_MtRush01;Cliff_MtRush01_NRM;"},
                {"Cliff_MtRush02", "Cliff_MtRush02;Cliff_MtRush02_NRM;"},
                // Amsterdam
                {"Pavement_Amsterdam01", "TPave_Amsterdam01;TPave_Amsterdam01_NRM;"},
                {"Pavement_Amsterdam02", "TPave_Amsterdam02;TPave_Amsterdam02_NRM;"},
                {"Pavement_Amsterdam03", "TPave_Amsterdam03;TPave_Amsterdam03_NRM;"},
                {"Pavement_Amsterdam04", "TPave_Amsterdam04;TPave_Amsterdam04_NRM;"},
                {"Pavement_Amsterdam05", "TPave_Amsterdam05;TPave_Amsterdam05_NRM;"},
                // Iceland
                {"Cliff_Iceland01", "TClif_Iceland01;TClif_Iceland01_NRM;"},
                {"Cliff_Iceland02", "TClif_Iceland02;TClif_Iceland02_NRM;"},
                {"Cliff_Iceland03", "TClif_Iceland03;TClif_Iceland03_NRM;"},
                {"Cliff_Iceland04", "TClif_Iceland04;TClif_Iceland04_NRM;"},
                {"Cliff_Iceland05", "TClif_Iceland05;TClif_Iceland05_NRM;"},
                {"Cliff_Iceland06", "TClif_Iceland06;TClif_Iceland06_NRM;"},
                {"Cliff_Iceland07", "TClif_Iceland07;TClif_Iceland07_NRM;"},
                {"Snow_Iceland02", "TSnow_Iceland02;TSnow_Iceland02_NRM;"},
                {"Snow_Iceland03", "TSnow_Iceland03;TSnow_Iceland03_NRM;"},
                {"Snow_Iceland04", "TSnow_Iceland04;TSnow_Iceland04_NRM;"},
                {"Snow_Iceland05", "TSnow_Iceland05;TSnow_Iceland05_NRM;"},
                {"Rock_Iceland01", "TRock_Iceland01;TRock_Iceland01_NRM;"},
                {"Rock_Iceland02", "TRock_Iceland02;TRock_Iceland02_NRM;"},
                {"Rock_Iceland03", "TRock_Iceland03;TRock_Iceland03_NRM;"},
                {"Rock_Iceland04", "TRock_Iceland04;TRock_Iceland04_NRM;"},
                {"Dirt_Iceland02", "TDirt_Iceland02;TDirt_Iceland02_NRM;"},
                {"Dirt_Iceland03", "TDirt_Iceland03;TDirt_Iceland03_NRM;"},
                {"Dirt_Iceland04", "TDirt_Iceland04;TDirt_Iceland04_NRM;"},
                {"Dirt_Iceland05", "TDirt_Iceland05;TDirt_Iceland05_NRM;"},
                {"Dirt_Iceland06", "TDirt_Iceland06;TDirt_Iceland06_NRM;"},
                {"Transition_Iceland01", "Transition_Iceland01;Transition_Iceland01_NRM;"},
                {"Transition_Iceland02", "Transition_Iceland02;Transition_Iceland02_NRM;"},
                {"Transition_Iceland03", "Transition_Iceland03;Transition_Iceland03_NRM;"},
                {"Transition_Iceland04", "Transition_Iceland04;Transition_Iceland04_NRM;"},
                // Tokyo_Harbor
                {"Pavement_TokyoHarbor01", "Pavement_TokyoHarbor01;Pavement_TokyoHarbor01_NRM;"},
                {"Pavement_TokyoHarbor02", "Pavement_TokyoHarbor02;Pavement_TokyoHarbor02_NRM;"},
                {"Pavement_TokyoHarbor03", "Pavement_TokyoHarbor03;Pavement_TokyoHarbor03_NRM;"},
                {"Pavement_TokyoHarbor04", "Pavement_TokyoHarbor04;Pavement_TokyoHarbor04_NRM;"},
                {"Pavement_TokyoHarbor05", "TPave_TokyoHarbor05;TPave_TokyoHarbor05_NRM;"},
                {"Pavement_TokyoHarbor06", "TPave_TokyoHarbor06;TPave_TokyoHarbor06_NRM;"},
                {"Pavement_TokyoHarbor07", "TPave_TokyoHarbor07;TPave_TokyoHarbor07_NRM;"},
                {"Pavement_TokyoHarbor08", "TPave_TokyoHarbor08;TPave_TokyoHarbor08_NRM;"},
                {"Pavement_TokyoHarbor09", "Pavement_TokyoHarbor09;Pavement_TokyoHarbor09_NRM;"},
                {"Pavement_TokyoHarbor10", "TPave_TokyoHarbor10;TPave_TokyoHarbor10_NRM;"},
                // RA3
                {"RA3_DeepOcean", "RA3_DeepOcean;RA3_DeepOcean_NRM;"},
                {"RA3_ShallowSeaFloor", "RA3_ShallowSeaFloor;RA3_ShallowSeaFloor_NRM;"},
                {"RA3_Elevation0", "RA3_Elevation0;RA3_Elevation0_NRM;"},
                {"RA3_Elevation1", "RA3_Elevation1;RA3_Elevation1_NRM;"},
                {"RA3_Elevation2", "RA3_Elevation2;RA3_Elevation2_NRM;"},
                {"RA3Grid1", "RA3Grid1;RA3Grid1_NRM;"},
                // Gibraltar
                {"Cliff_Gibraltar1", "Cliff_Gibralter1;Cliff_Gibralter1_NRM;"},
                {"Cliff_Gibraltar2", "Cliff_Gibralter2;Cliff_Gibralter2_NRM;"},
                {"Pavement_Gibraltar1", "Pavement_Gibraltar01;Pavement_Gibraltar01_NRM;"},
                {"Pavement_Gibraltar2", "Pavement_Gibraltar02;Pavement_Gibraltar02_NRM;"},
                {"Pavement_Gibraltar3", "Pavement_Gibraltar03;Pavement_Gibraltar03_NRM;"},
                {"Pavement_Gibraltar5", "Pavement_GibraltarBoardwalk;Pavement_GibraltarBoardwalk_NRM;"},
                {"Pavement_Gibraltar6", "Pavement_Gibraltar04;Pavement_Gibraltar04_NRM;"},
                {"Grass_Gibraltar1", "Grass_Gibraltar01;Grass_Gibraltar01_NRM;"},
                {"Grass_Gibraltar2", "Grass_Gibraltar02;Grass_Gibraltar02_NRM;"},
                {"Grass_Gibraltar3", "Grass_Gibraltar03;Grass_Gibraltar03_NRM;"},
                // Gypsy_Village
                {"Grass_Gypsy01", "TGrass_Gypsy01;TGrass_Gypsy01_NRM;"},
                {"Grass_Gypsy02", "TGrass_Gypsy02;TGrass_Gypsy02_NRM;"},
                {"Grass_Gypsy03", "TGrass_Gypsy03;TGrass_Gypsy03_NRM;"},
                {"Grass_Gypsy04", "TGrass_Gypsy04;TGrass_Gypsy04_NRM;"},
                {"Dirt_Gypsy01", "TDirt_Gypsy01;TDirt_Gypsy01_NRM;"},
                {"Dirt_Gypsy02", "TDirt_Gypsy02;TDirt_Gypsy02_NRM;"},
                {"Dirt_Gypsy03", "TDirt_Gypsy03;TDirt_Gypsy03_NRM;"},
                // Hawaii
                {"Grass_Hawaii01", "Grass_Hawaii01;Grass_Hawaii01_NRM;"},
                {"Grass_Hawaii02", "Grass_Hawaii02;Grass_Hawaii02_NRM;"},
                {"Grass_Hawaii03", "Grass_Hawaii03;Grass_Hawaii03_NRM;"},
                {"Grass_Hawaii04", "Grass_Hawaii04;Grass_Hawaii04_NRM;"},
                {"Grass_Hawaii05", "Grass_Hawaii05;Grass_Hawaii05_NRM;"},
                {"Grass_Hawaii06", "Grass_Hawaii06;Grass_Hawaii06_NRM;"},
                {"Grass_Hawaii07", "Grass_Hawaii07;Grass_Hawaii07_NRM;"},
                {"Grass_Hawaii08", "Grass_Hawaii08;Grass_Hawaii08_NRM;"},
                {"Grass_Hawaii09", "Grass_Hawaii09;Grass_Hawaii09_NRM;"},
                {"Sand_Hawaii01", "TSand_Hawaii01;TSand_Hawaii01_NRM;"},
                {"Sand_Hawaii02", "TSand_Hawaii02;TSand_Hawaii02_NRM;"},
                {"Sand_Hawaii03", "TSand_Hawaii03;TSand_Hawaii03_NRM;"},
                {"Sand_Hawaii04", "TSand_Hawaii04;TSand_Hawaii04_NRM;"},
                // Vladivostok
                {"Pavement_Vlad01", "Pavement_Vlad01;Pavement_Vlad01_NRM;"},
                {"Pavement_Vlad02", "Pavement_Vlad02;Pavement_Vlad02_NRM;"},
                {"Pavement_Vlad03", "Pavement_Vlad03;Pavement_Vlad03_NRM;"},
                {"Pavement_Vlad04", "Pavement_Vlad04;Pavement_Vlad04_NRM;"},
                {"Pavement_Vlad05", "Pavement_Vlad05;Pavement_Vlad05_NRM;"},
                {"Pavement_Vlad06", "Pavement_Vlad06;Pavement_Vlad06_NRM;"},
                {"Pavement_Vlad07", "Pavement_Vlad07;Pavement_Vlad07_NRM;"},
                {"Pavement_Vlad08", "Pavement_Vlad08;Pavement_Vlad08_NRM;"},
                {"Pavement_Vlad09", "Pavement_Vlad09;Pavement_Vlad09_NRM;"},
                {"Pavement_Vlad10", "Pavement_Vlad10;Pavement_Vlad10_NRM;"},
                {"Pavement_Vlad11", "Pavement_Vlad11;Pavement_Vlad11_NRM;"},
                {"Pavement_Vlad12", "Pavement_Vlad12;Pavement_Vlad12_NRM;"},
                // Brighton_Beach
                {"BB_Gravel01", "BB_Gravel01;BB_Gravel01_NRM;"},
                {"BB_Gravel02", "BB_Gravel02;BB_Gravel02_NRM;"},
                {"BB_Dirt01", "BB_Dirt01;BB_Dirt01_NRM;"},
                {"BB_Dirt02", "BB_Dirt02;BB_Dirt02_NRM;"},
                {"BB_Pavement01", "BB_Pavement01;BB_Pavement01_NRM;"},
                {"BB_Pavement02", "BB_Pavement02;BB_Pavement02_NRM;"}

        };

        public static void RegisterTexture(string name, string textureFileName, string bumpTextureFileName)
        {
            if (textureFileName.EndsWith(".tga"))
            {
                textureFileName = textureFileName.Replace(".tga", "");
            }

            if (bumpTextureFileName.EndsWith(".tga"))
            {
                bumpTextureFileName = bumpTextureFileName.Replace(".tga", "");
            }

            string texture = textureFileName + ";" + bumpTextureFileName + ";";
            textures.put(name, texture);
        }
        
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
            worldInfo.properties.addProperty("mapDescription", $"by {Environment.UserName}", context);
            worldInfo.properties.addProperty("compression", CompressionType.RefPack, context);
            worldInfo.properties.addProperty("cameraGroundMinHeight", 0f, context);
            worldInfo.properties.addProperty("cameraGroundMaxHeight", 2500f, context);
            worldInfo.properties.addProperty("cameraMinHeight", 40f, context);
            worldInfo.properties.addProperty("cameraMaxHeight", 550f, context);
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