using System.Collections.Immutable;

namespace DanielleDotnet.Utils;

public static class LevelConsts
{
    
    
    public enum TalosLocation
    {
        LOBBY,
        HARDWARE_LABS,
        SHUTTLE_BAY,
        PSYCHOTRONICS,
        GUTS,
        ARBORETUM,
        BRIDGE,
        CREW_QUARTERS,
        DEEP_STORAGE,
        CARGO_BAY,
        LIFE_SUPPORT,
        POWER_PLANT,
        EXTERIOR
    }

    public const string NeuromodDivision = "research/simulationlabs";
    public const string Lobby = "research/lobby";
    public const string HardwareLabs = "research/prototype";
    public const string ShuttleBay = "research/shuttlebay";
    public const string Psychotronics = "research/psychotronics";
    public const string Guts = "research/zerog_utilitytunnels";
    public const string Arboretum = "executive/arboretum";
    public const string CrewQuarters = "executive/crewfacilities";
    public const string DeepStorage = "executive/corporateit";
    public const string Bridge = "executive/bridge";
    public const string CargoBay = "engineering/cargobay";
    public const string LifeSupport = "engineering/lifesupport";
    public const string PowerPlant = "engineering/powersource";
    public const string Exterior = "station/exterior";
    public const string Endgame = "endgame";

    public static readonly ImmutableDictionary<string, string>
        LEVEL_PATH_TO_NAME =
            ImmutableDictionary.CreateRange(
                new Dictionary<string, string>
                {
                    [NeuromodDivision] = "SimulationLabs",
                    [Lobby] = "Lobby",
                    [HardwareLabs] = "Prototype",
                    [ShuttleBay] = "ShuttleBay",
                    [Psychotronics] = "psychotronics",
                    [Guts] = "ZeroG_UtilityTunnels",
                    [Arboretum] = "Arboretum",
                    [CrewQuarters] = "CrewFacilities",
                    [DeepStorage] = "CorporateIT",
                    [Bridge] = "Bridge",
                    [CargoBay] = "CargoBay",
                    [LifeSupport] = "LifeSupport",
                    [PowerPlant] = "PowerSource",
                    [Exterior] = "Exterior",
                    [Endgame] = "EndGame"
                });

    public static readonly ImmutableDictionary<TalosLocation, string>
        SPAWN_LOCATION_TO_LEVEL =
            ImmutableDictionary.CreateRange(
                new Dictionary<TalosLocation, string>
                {
                    [TalosLocation.LOBBY] = Lobby,
                    [TalosLocation.HARDWARE_LABS] = HardwareLabs,
                    [TalosLocation.SHUTTLE_BAY] = ShuttleBay,
                    [TalosLocation.PSYCHOTRONICS] = Psychotronics,
                    [TalosLocation.GUTS] = Guts,
                    [TalosLocation.ARBORETUM] = Arboretum,
                    [TalosLocation.BRIDGE] = Bridge,
                    [TalosLocation.CREW_QUARTERS] = CrewQuarters,
                    [TalosLocation.DEEP_STORAGE] = DeepStorage,
                    [TalosLocation.CARGO_BAY] = CargoBay,
                    [TalosLocation.LIFE_SUPPORT] = LifeSupport,
                    [TalosLocation.POWER_PLANT] = PowerPlant,
                    [TalosLocation.EXTERIOR] = Exterior
                });

    // Used for iterating through levels to install/uninstall randomization.
    public static readonly ImmutableArray<string> LEVEL_DIRS =
    [
        Endgame,
        CargoBay,
        LifeSupport,
        PowerPlant,
        Arboretum,
        Bridge,
        DeepStorage,
        CrewQuarters,
        Lobby,
        HardwareLabs,
        Psychotronics,
        ShuttleBay,
        NeuromodDivision,
        Guts,
        Exterior
    ];

    // Hard-coded list of tags that must never be randomized to avoid softlocks.
    public static readonly ImmutableArray<string> DO_NOT_TOUCH_NPC_TAGS =
    [
        // Changing the weavers in Psychotronics will softlock the game at live extraction.
        "research/psychotronics;ArkNpcSpawner_Weaver6",
        "research/psychotronics;ArkNpcSpawner_Weaver2",
        "research/psychotronics;ArkNpcSpawner_Weaver4",
        "executive/arboretum;ArkNpcSpawner_Telepath1",
        "executive/arboretum;Spawner_GreenhouseTelepath",

        // Changing the telepaths in Crew Quarters will softlock the cook's sidequest.
        "executive/crewfacilities;ArkNpcSpawner16",
        "executive/crewfacilities;ArkNpcSpawner_Telepath2",
        "station/exterior;ArkNpcSpawner_Telepath4",
        "engineering/lifesupport;ArkNpcSpawner_Telepath1",
        "MilitaryOperatorCopycat",
        @"Operators\Named",
        "Kaspar",
        "engineering/cargobay;ArkNpcSpawner_Weaver3",
        "engineering/cargobay;ArkNpcSpawner_Weaver",
        "engineering/cargobay;ArkNpcSpawner_Weaver5",

        // The lift technopath and phantom.
        "research/lobby;ArkNpcSpawner_Technopath1",
        "research/lobby;ArkNpcSpawner_BasePhantom5",

        // Life support technopath.
        "FakeTechnopath",
        "engineering/lifesupport;ArkNpcSpawner_Technopath1"
    ];

    // Hard-coded list of tags that must never be spawned to avoid gameplay issues.
    public static readonly ImmutableArray<string> DO_NOT_OUTPUT_NPC_TAGS =
    [
        "Tentacle_Large_Guard",
        "Tentacle_Medium_Guard",
        "Tentacle_Small_Guard",
        "FakeTechnopath",
        "Cystoid_IgnorePlayer",
        "EthericDoppelganger",
        "MilitaryOperatorFriendly"
    ];

    // Sets global quest states to unlock important areas.
    public static readonly ImmutableDictionary<string, string>
        UNLOCK_QUESTS_GAME_TOKENS =
            ImmutableDictionary.CreateRange(
                new Dictionary<string, string>
                {
                    ["CrewFacility.VoiceSampleProcessingPercent"] = "100"
                });

    // Opens all exterior airlocks.
    public static readonly ImmutableDictionary<string, string>
        UNLOCK_EXTERIOR_GAME_TOKENS =
            ImmutableDictionary.CreateRange(
                new Dictionary<string, string>
                {
                    ["AirlockStates.AirlockSealedInternally_ShuttleBay"] = "false",
                    ["AirlockStates.AirlockSealedInternally_HardwareLabs"] = "false",
                    ["AirlockStates.AirlockSealedInternally_PowerSource"] = "false",
                    ["AirlockStates.AirlockSealedInternally_Arboretum"] = "false",
                    ["AirlockStates.AirlockSealedInternally_Psychotronics"] = "false"
                });

    public static readonly ImmutableDictionary<string, string>
        PSYCHOTRONICS_SKIP_CALIBRATION_TOKENS =
            ImmutableDictionary.CreateRange(
                new Dictionary<string, string>
                {
                    ["WorldState.PsychoscopeCalibrationComplete"] = "true"
                });

    public static readonly ImmutableDictionary<string, string>
        ENABLE_NIGHTMARE_TOKENS =
            ImmutableDictionary.CreateRange(
                new Dictionary<string, string>
                {
                    ["Arboretum.NightmareSpawnedInArboretum"] = "true"
                });

    public const string SELF_DESTRUCT_TIMER_TOKEN_NAME =
        "WorldState.TalosSelfDestructTime";

    public static readonly ImmutableDictionary<string, string>
        START_SELF_DESTRUCT_TOKENS =
            ImmutableDictionary.CreateRange(
                new Dictionary<string, string>
                {
                    ["CharacterFates.DahlBrainwashed"] = "true",
                    ["AlexAndDahlOutcomes.DahlArrivedInShuttle"] = "true"
                });

    public static readonly ImmutableDictionary<string, string>
        SKIP_JOVAN_TOKENS =
            ImmutableDictionary.CreateRange(
                new Dictionary<string, string>
                {
                    ["SimLab.Decontamination_SawMimicScene"] = "true"
                });

    public static readonly ImmutableDictionary<string, string>
        ALLOW_GAME_SAVE_TOKENS =
            ImmutableDictionary.CreateRange(
                new Dictionary<string, string>
                {
                    ["WorldState.AllowSaveGame"] = "true"
                });

    public static readonly ImmutableDictionary<string, string>
        START_OUTSIDE_APARTMENT_TOKENS =
            ImmutableDictionary.CreateRange(
                new Dictionary<string, string>
                {
                    ["LevelTransition.LevelTransitionDestination"] = "From_Lobby"
                });
}