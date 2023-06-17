using System;
using System.Diagnostics.Eventing.Reader;

namespace MapCoreLib.Core
{
    public static class Ra3MapConst
    {
        
        public const uint UnCompressorFlag = 1884121923u;
        public const uint CompressorFlag = 5390661u;

        #region 最外层的asset
        
        public const string ASSET_ObjectsList = "ObjectsList";
        public const string ASSET_AssetList = "AssetList";
        public const string ASSET_GlobalVersion = "GlobalVersion";
        public const string ASSET_HeightMapData = "HeightMapData";
        public const string ASSET_BlendTileData = "BlendTileData";
        public const string ASSET_WorldInfo = "WorldInfo";
        public const string ASSET_MPPositionList = "MPPositionList";
        public const string ASSET_MPPositionInfo = "MPPositionInfo";
        public const string ASSET_SidesList = "SidesList";
        public const string ASSET_LibraryMapLists = "LibraryMapLists";
        public const string ASSET_LibraryMaps = "LibraryMaps";
        public const string ASSET_Teams = "Teams";
        public const string ASSET_PlayerScriptsList = "PlayerScriptsList";
        public const string ASSET_BuildLists = "BuildLists";
        public const string ASSET_TriggerAreas = "TriggerAreas";
        public const string ASSET_GlobalWaterSettings = "GlobalWaterSettings";
        public const string ASSET_FogSettings = "FogSettings";
        public const string ASSET_MissionHotSpots = "MissionHotSpots";
        public const string ASSET_StandingWaterAreas = "StandingWaterAreas";
        public const string ASSET_RiverAreas = "RiverAreas";
        public const string ASSET_StandingWaveAreas = "StandingWaveAreas";
        public const string ASSET_GlobalLighting = "GlobalLighting";
        public const string ASSET_PostEffectsChunk = "PostEffectsChunk";
        public const string ASSET_EnvironmentData = "EnvironmentData";
        public const string ASSET_NamedCameras = "NamedCameras";
        public const string ASSET_CameraAnimationList = "CameraAnimationList";
        public const string ASSET_WaypointsList = "WaypointsList";
        
        #endregion
        
        public const string ASSET_MapObject = "Object";
        public const string ASSET_ScriptList = "ScriptList";
        public const string ASSET_Script = "Script";
        public const string ASSET_ScriptGroup = "ScriptGroup";
        public const string ASSET_OrCondition = "OrCondition";
        public const string ASSET_ScriptAction = "ScriptAction";
        public const string ASSET_ScriptActionFalse = "ScriptActionFalse";


        public const string ELEMENT_SCRIPT_LIST = "ScriptList";
        public const string ATTR_SCRIPT_LIST_PLAYER = "player";
        public const string ELEMENT_SCRIPT_GROUP = "ScriptGroup";
        public const string ATTR_NAME = "name";
        public const string ATTR_ACTIVE = "active";
        public const string ATTR_SUB_ROUTINE = "subRoutine";
        public const string ELEMENT_SCRIPT = "Script";
        public const string ATTR_DEACTIVATE_UPON_SUCCESS = "DeactivateUponSuccess";
        public const string ATTR_LOOP_ACTIONS = "LoopActions";
        public const string ATTR_EVALUATION_INTERVAL = "EvaluationInterval";
        public const string ATTR_ActiveIn_Easy = "ActiveInEasy";
        public const string ATTR_ActiveIn_Medium = "ActiveInMedium";
        public const string ATTR_ActiveIn_Hard = "ActiveInHard";
        public const string ELEMENT_OR_CONDITION = "OrCondition";
        public const string ELEM_NAME_MapScript = "MapScript";
    }
}