using MapSchemaGen.Model.Script;

namespace MapCoreLib.Core.Asset
{
    public enum ScriptArgumentType: int
    {
        [ArgType("xs:int")]
        Integer = 0,
        [ArgType("xs:float")]
        RealNumber = 1,
        [ArgType("xs:string")]
        ScriptName = 2,
        [ArgType("xs:string")]
        TeamName = 3,
        [ArgType("xs:string")]
        CounterName = 4,
        [ArgType("xs:string")]
        FlagName = 5,
        [ArgType("xs:int")]
        Comparison = 6,
        [ArgType("xs:string")]
        WaypointName = 7,
        [ArgType("xs:boolean")]
        Boolean = 8,
        [ArgType("xs:string")]
        TriggerAreaName = 9,
        [ArgType("xs:string")]
        Text = 10,
        [ArgType("xs:string")]
        PlayerName = 11,
        SoundName = 12,
        SubroutineName = 13,
        UnitName = 14,
        ObjectName = 15,
        PositionCoordinate = 16,
        Angle = 17,
        TeamState = 18,
        Relation = 19,
        AiMood = 20,
        SpeechName = 21,
        MusicName = 22,
        MovieName = 23,
        WaypointPathName = 24,
        LocalizedStringName = 25,
        BridgeName = 26,
        UnitOrStructureKind = 27,
        AttackPrioritySetName = 28,
        RadarEventType = 29,
        SpecialPowerName = 30,
        ScienceName = 31,
        UpgradeName = 32,
        UnitAbilityName = 33,
        BoundaryName = 34,
        Buildability = 35,
        SurfaceType = 36,
        CameraShakeIntensity = 37,
        CommandButtonName = 38,
        FontName = 39,
        ObjectStatus = 40,
        TeamAbilityName = 41,
        SkirmishApproachPath = 42,
        Color = 43,
        EmoticonName = 44,
        ObjectPanelFlag = 45,
        FactionName = 46,
        ObjectTypeListName = 47,
        MapRevealName = 48,
        ScienceAvailabilityName = 49,
        EvacuateContainerSide = 50,
        Percentage = 51,

        // Added in BFME
        Percentage2 = 52, // Dupe of 51, perhaps because BFME and ZH development overlapped?
        UnitReference = 54,
        TeamReference = 55,
        NearOrFar = 56,
        MathOperator = 57,
        ModelCondition = 58,
        AudioName = 59,
        ReverbRoomType = 60,
        ObjectType = 61,
        Hero = 62,
        Emotion = 63,
        
        
        CameraAnimation = 64,
        ObjectStance = 68,
        NewBoolean = 69,
        Personality = 70,
        Availability = 81,
        MouseHud = 84,
        ScriptEvent = 86,
        
        EVA = 78,
        CampaignFlag = 80,
        ProductionQueueTab = 85,
        MissionObjectiveStatus = 87,
        MissionHotSpot = 90,
        MissionObjective = 91,
        ObjectOutline = 92,
        SkimishPriorityScheme = 93,
        PlayerTech = 94,
        ObjectDisableType = 95,
        PathMusic = 96,
        MissionObjectiveCategory = 97,
        PathMusicDynamicSystem = 98,
        CommandMenuButton = 99,
        FixHUD = 100,
        UiState = 101,
        Platform = 102,
        AiBeaconType = 103,
        
    }
}