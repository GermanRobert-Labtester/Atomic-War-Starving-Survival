using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Crafting;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Flashpoint;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Simulation;
using AtomicWar._Game.UI;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        public IReadOnlyList<string> GetUntickedSystemNames()
        {
            if (Registry == null) return Array.Empty<string>();
            // Collect all known public system property names that are not null.
            var constructed = new List<string>();
            var type = typeof(GameBootstrap);
            foreach (var prop in type.GetProperties(
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                if (IsAtomicWarSystemProperty(prop))
                {
                    var value = prop.GetValue(this);
                    if (value != null)
                        constructed.Add(prop.Name);
                }
            }
            // Filter out those that are registered in any tick category.
            var unticked = new List<string>();
            foreach (var name in constructed)
            {
                if (!IsPropertyRegisteredInRegistry(name))
                    unticked.Add(name);
            }
            return unticked;
        }

        /// <summary>
        /// Map a GameBootstrap property name to registry keys and test membership.
        /// Handles PascalCase→snake_case, stripping trailing "_system", and known
        /// irregular aliases (e.g. ExpeditionSystem → expeditions).
        /// </summary>
        private bool IsPropertyRegisteredInRegistry(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || Registry == null) return false;
            if (Registry.IsSystemTicked(propertyName)) return true;

            string snake = System.Text.RegularExpressions.Regex.Replace(
                propertyName, "([a-z])([A-Z])", "$1_$2").ToLowerInvariant();
            if (Registry.IsSystemTicked(snake)) return true;

            if (snake.EndsWith("_system", StringComparison.Ordinal))
            {
                string stripped = snake.Substring(0, snake.Length - "_system".Length);
                if (Registry.IsSystemTicked(stripped)) return true;
            }

            // Irregular property → registry name aliases.
            if (TryGetRegistryAliases(propertyName, out var aliases))
            {
                for (int i = 0; i < aliases.Length; i++)
                {
                    if (Registry.IsSystemTicked(aliases[i])) return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Known mismatches between GameBootstrap property names and registry keys
        /// that cannot be derived by snake_case + strip "_system".
        /// Replaced 46-return switch with a static dictionary (audit smell fix).
        /// </summary>
        private static readonly Dictionary<string, string[]> RegistryAliases =
            new Dictionary<string, string[]>
        {
            // Core systems
            { "ExpeditionSystem",        new[] { "expeditions" } },
            { "CorpseSystem",            new[] { "corpses" } },
            { "BlackRainHazardSystem",   new[] { "black_rain" } },
            { "HatchDilemmaPromptField",  new[] { "hatch_dilemma" } },
            { "ParleyOfferPromptField",   new[] { "parley_offer" } },
            { "LifeboatTransmissionSystem", new[] { "lifeboat" } },
            { "EventRunner",             new[] { "event_runner" } },
            { "SuspicionTracker",        new[] { "suspicion_tracker" } },
            { "VictoryProject",          new[] { "victory_project" } },
            { "TimeSystem",              new[] { "time" } },
            { "SaveSystem",              new[] { "save" } },
            { "GameState",               new[] { "game_state" } },
            { "EndgameEngine",           new[] { "endgame" } },
            { "ShelterLayout",           new[] { "shelter_layout" } },
            { "SleepQualitySystem",      new[] { "sleep_quality" } },
            { "UtilityAI",               new[] { "utility_ai" } },
            { "RadioSystem",             new[] { "radio" } },
            // Medical systems
            { "AmputationSystem",        new[] { "amputation_daily" } },
            { "ScurvySystem",            new[] { "scurvy_daily" } },
            { "Mutagenesis",             new[] { "mutagenesis_tick", "mutagenesis_daily" } },
            { "BloodTransfusion",        new[] { "blood_transfusion" } },
            { "Addiction",               new[] { "addiction" } },
            { "BloodToxicity",           new[] { "blood_toxicity" } },
            { "GraftRejection",          new[] { "graft_rejection_daily", "graft_rejection" } },
            { "PheromoneMasking",        new[] { "pheromone_masking" } },
            { "ChemTolerance",           new[] { "tolerance" } },
            { "LastWill",                new[] { "last_will" } },
            { "LegacyStart",             new[] { "legacy_start" } },
            { "BloodTypes",              new[] { "blood_types" } },
            { "EpilogueStats",           new[] { "epilogue_stats" } },
            { "RiverNodeSystem",         new[] { "river_nodes" } },
            // Tactical / world systems
            { "DeadDropSystem",          new[] { "dead_drops" } },
            { "DeserterSystem",          new[] { "deserter_daily", "deserter" } },
            { "EcosystemSystem",         new[] { "ecosystem_daily", "ecosystem" } },
            { "HatchVisibilitySystem",    new[] { "hatch_visibility_daily", "hatch_visibility" } },
            { "CultMoralSystem",         new[] { "cult_moral" } },
            // Simulation systems
            { "WeaponMaintenanceSystem",  new[] { "weapon_maint" } },
            { "AntibioticResistSystem",   new[] { "antibiotic_resist" } },
            { "HaulingSystem",           new[] { "hauling" } },
            { "TriageSystem",            new[] { "triage" } },
            { "ScrapWeaponSystem",       new[] { "scrap_weapon" } },
            { "ClothingSystem",          new[] { "clothing" } },
            { "AestheticsSystem",        new[] { "aesthetics" } },
            // Perk / misc systems
            { "SkillAtrophy",            new[] { "skill_atrophy" } },
            { "SkillProgression",        new[] { "skill_progression", "skill_progression_daily" } },
            { "GriefKeepsakes",          new[] { "grief_keepsakes" } },
            { "PhantomIntruders",        new[] { "phantom_intruders" } },
            { "BunkerSocial",            new[] { "bunker_social" } },
            { "Gossip",                  new[] { "gossip", "gossip_daily" } },
            { "AdaptiveWarlords",        new[] { "adaptive_warlords" } },
            { "BilgePumps",              new[] { "bilge_pumps", "bilge_pumps_daily" } },
            { "CarrionBirds",            new[] { "carrion_birds", "carrion_birds_daily" } },
            { "LogicGates",              new[] { "logic_gates" } },
            { "ModLoader",               new[] { "mod_loader" } },
            { "TwitchAPI",               new[] { "twitch_api" } },
            { "DiseaseExpansion",        new[] { "disease_expansion" } },
            { "Scapegoat",               new[] { "dynamic_scapegoat" } },
            { "IronMan",                 new[] { "mode_iron_man" } },
            { "AndroidNpcs",             new[] { "npc_android" } },
            { "Sheriff",                 new[] { "role_sheriff" } },
            { "ScenarioGen",             new[] { "ui_scenario_gen" } },
            { "SpeedrunTimer",           new[] { "ui_speedrun_timer" } },
            { "TrueEnding",              new[] { "victory_true_ending" } },
            { "VictoryAirlift",          new[] { "victory_airlift" } },
            { "VictoryAscendancy",       new[] { "victory_ascendancy" } },
            { "VictoryBuriedAlive",      new[] { "victory_buried_alive" } },
            { "VictoryCannibalKing",     new[] { "victory_cannibal_king" } },
            { "VictoryDefection",        new[] { "victory_defection" } },
            { "VictoryIcebreaker",       new[] { "victory_icebreaker" } },
            { "VictoryLoneSurvivor",     new[] { "victory_lone_survivor" } },
            { "VictoryMAD",              new[] { "victory_mad" } },
            { "VictoryMigration",        new[] { "victory_migration" } },
            { "VictoryTheBroadcast",     new[] { "victory_the_broadcast" } },
            { "VictoryTheCure",          new[] { "victory_the_cure" } },
            { "VictoryTheMartian",       new[] { "victory_the_martian" } },
            { "VictoryUndergroundCity",  new[] { "victory_underground_city" } },
            { "VictoryUnifier",          new[] { "victory_unifier" } },
            { "MapHazardAcidGeyser",     new[] { "map_hazard_acid_geyser" } },
            { "MapHazardAshlanche",      new[] { "map_hazard_ashlanche" } },
            { "MapHazardBiometricDoor",  new[] { "map_hazard_biometric_door" } },
            { "MapHazardCraterWall",     new[] { "map_hazard_crater_wall" } },
            { "MapHazardCrevice",        new[] { "map_hazard_crevice" } },
            { "MapHazardFlammableGas",   new[] { "map_hazard_flammable_gas" } },
            { "MapHazardGasPockets",     new[] { "map_hazard_gas_pockets" } },
            { "MapHazardMagneticAnomaly",new[] { "map_hazard_magnetic_anomaly" } },
            { "MapHazardSinkholeCollapse", new[] { "map_hazard_sinkhole_collapse" } },
            { "MapHazardVenusTrap",      new[] { "map_hazard_venus_trap" } },
            { "MapAnomalyAshDunes",      new[] { "map_anomaly_ash_dunes" } },
            { "MapAnomalyBoilingLake",   new[] { "map_anomaly_boiling_lake" } },
            { "MapAnomalyCherenkov",     new[] { "map_anomaly_cherenkov" } },
            { "MapAnomalyDogDen",        new[] { "map_anomaly_dog_den" } },
            { "MapAnomalyDontLook",      new[] { "map_anomaly_dont_look" } },
            { "MapAnomalyDryCoral",      new[] { "map_anomaly_dry_coral" } },
            { "MapAnomalyFloodedSubway", new[] { "map_anomaly_flooded_subway" } },
            { "MapAnomalyGlassCrater",   new[] { "map_anomaly_glass_crater" } },
            { "MapAnomalyMassGrave",     new[] { "map_anomaly_mass_grave" } },
            { "MapAnomalyMirage",        new[] { "map_anomaly_mirage" } },
            { "MapAnomalyPetrifiedForest", new[] { "map_anomaly_petrified_forest" } },
            { "MapAnomalyQuietZone",     new[] { "map_anomaly_quiet_zone" } },
            { "MapAnomalyRustedTank",    new[] { "map_anomaly_rusted_tank" } },
            { "MapAnomalyServerFarm",    new[] { "map_anomaly_server_farm" } },
            { "MapAnomalySinkhole",      new[] { "map_anomaly_sinkhole" } },
            { "MapAnomalyTangledDrop",   new[] { "map_anomaly_tangled_drop" } },
            { "MapAnomalyTireFire",      new[] { "map_anomaly_tire_fire" } },
            { "MapAnomalyUxoNuke",       new[] { "map_anomaly_uxo_nuke" } },
            { "BiomeAshSwamp",           new[] { "biome_ash_swamp" } },
            { "BiomeGlassDesert",        new[] { "biome_glass_desert" } },
            { "BiomeHighwayTunnel",      new[] { "biome_highway_tunnel" } },
            { "BiomeSaltFlats",          new[] { "biome_salt_flats" } },
            { "BiomeSkyscraperTops",     new[] { "biome_skyscraper_tops" } },
            { "BiomeSuburbs",            new[] { "biome_suburbs" } },
            { "WeatherAcidSnow",         new[] { "weather_acid_snow" } },
            { "WeatherBioFog",           new[] { "weather_bio_fog" } },
            { "WeatherBlackSnow",        new[] { "weather_black_snow" } },
            { "WeatherBloodRain",        new[] { "weather_blood_rain" } },
            { "WeatherDeadWind",         new[] { "weather_dead_wind" } },
            { "WeatherDeepFreeze",       new[] { "weather_deep_freeze" } },
            { "WeatherDustDevil",        new[] { "weather_dust_devil" } },
            { "WeatherEmpStorm",         new[] { "weather_emp_storm" } },
            { "WeatherFalseSpring",      new[] { "weather_false_spring" } },
            { "WeatherGlassStorm",       new[] { "weather_glass_storm" } },
            { "WeatherOzoneHole",        new[] { "weather_ozone_hole" } },
            { "WeatherRadHail",          new[] { "weather_rad_hail" } },
            { "WeatherSilentSpring",     new[] { "weather_silent_spring" } },
            { "WeatherSolarFlare",       new[] { "weather_solar_flare" } },
            { "WeatherStaticCharge",     new[] { "weather_static_charge" } },
            { "EncounterAmalgamation",   new[] { "encounter_amalgamation" } },
            { "EncounterBurrowers",      new[] { "encounter_burrowers" } },
            { "EncounterFloodedMaze",    new[] { "encounter_flooded_maze" } },
            { "EncounterGlowingDead",    new[] { "encounter_glowing_dead" } },
            { "EncounterGlowingStag",    new[] { "encounter_glowing_stag" } },
            { "EncounterHitAndRun",      new[] { "encounter_hit_and_run" } },
            { "EncounterLeeches",        new[] { "encounter_leeches" } },
            { "EncounterMirelurker",     new[] { "encounter_mirelurker" } },
            { "EncounterPressurePlate",  new[] { "encounter_pressure_plate" } },
            { "EncounterRiverPirates",   new[] { "encounter_river_pirates" } },
            { "EncounterRoadblock",      new[] { "encounter_roadblock" } },
            { "EncounterRobotDog",       new[] { "encounter_robot_dog" } },
            { "EncounterSleepingCamp",   new[] { "encounter_sleeping_camp" } },
            { "EncounterTripwireMaze",   new[] { "encounter_tripwire_maze" } },
            { "EncounterWarlordTank",    new[] { "encounter_warlord_tank" } },
            { "SiegeArtillery",          new[] { "siege_artillery" } },
            { "SiegeBiowarfare",         new[] { "siege_biowarfare" } },
            { "SiegeBlockade",           new[] { "siege_blockade" } },
            { "SiegeHostageShield",      new[] { "siege_hostage_shield" } },
            { "SiegeNightRaid",          new[] { "siege_night_raid" } },
            { "SiegeSappers",            new[] { "siege_sappers" } },
            { "SiegeSmokeOut",           new[] { "siege_smoke_out" } },
            { "SiegeVehicleRam",         new[] { "siege_vehicle_ram" } },
            { "PersonalQuests",          new[] { "personal_quests", "personal_quests_daily", "personal_quests_morale" } },
            { "SocialPerks",             new[] { "social_perks", "social_perks_daily" } },
            { "CombatPerks",             new[] { "combat_perks" } },
            { "SurvivalPerks",           new[] { "survival_perks" } },
            { "ShelterPerks",            new[] { "shelter_perks" } },
            { "MedicalPerks",            new[] { "medical_perks" } },
            { "ExpeditionPerks",         new[] { "expedition_perks" } },
            { "CookingSystem",           new[] { "cooking" } },
            { "PetSystem",               new[] { "pets" } },
            { "FuelDecaySystem",         new[] { "fuel_decay", "fuel_decay_daily" } },
            { "ScapegoatSystem",         new[] { "scapegoat" } },
            { "ChildSystem",             new[] { "child" } },
            // Infrastructure
            { "StructuralIntegrity",      new[] { "structural_integrity" } },
            { "FactionRadioIntercepts",   new[] { "faction_radio_intercepts" } },
            { "PowerNetwork",            new[] { "power_network" } },
            { "WaterStorage",            new[] { "water_storage" } },
            { "KnowledgeMap",            new[] { "knowledge_map" } },
            { "GeneratedMap",            new[] { "generated_map" } },
            { "WorkbenchSystem",         new[] { "workbench" } },
            { "DiaryCatalog",            new[] { "diary_catalog" } },
        };

        private static bool TryGetRegistryAliases(string propertyName, out string[] aliases)
        {
            return RegistryAliases.TryGetValue(propertyName, out aliases);
        }

        /// <summary>
        /// Diagnostic: after InitializeSystems, returns names of all systems
        /// that were constructed but never registered in any tick category.
        /// Non-empty list indicates a C-1 class bug.
        /// </summary>
        private static bool IsAtomicWarSystemProperty(System.Reflection.PropertyInfo prop)
        {
            if (prop == null || prop.Name == "Registry") return false;
            if (!prop.PropertyType.IsClass || prop.PropertyType.IsGenericType) return false;
            string ns = prop.PropertyType.Namespace;
            return ns != null && ns.StartsWith("AtomicWar");
        }

        /// <summary>
        /// AUDIT-003: Names of foundation systems that TickSystems invokes without
        /// null-conditional guards. Used by <see cref="AssertFoundationSystems"/>
        /// and EditMode tests.
        /// </summary>
        public static readonly string[] FoundationSystemNames =
        {
            nameof(GameState),
            nameof(TimeSystem),
            nameof(WeatherSystem),
            nameof(TemperatureSystem),
            nameof(PhotoperiodSystem),
            nameof(NeedsSystem),
            nameof(RadiationSystem),
            nameof(Shelter),
        };

        /// <summary>
        /// Snapshot of foundation systems for AUDIT-003 null checks.
        /// Prefer this over a long positional parameter list.
        /// </summary>
        public sealed class FoundationSystemsSnapshot
        {
            public GameState GameState;
            public TimeSystem TimeSystem;
            public WeatherSystem WeatherSystem;
            public TemperatureSystem TemperatureSystem;
            public PhotoperiodSystem PhotoperiodSystem;
            public NeedsSystem NeedsSystem;
            public RadiationSystem RadiationSystem;
            public Shelter.Shelter Shelter;
        }

        /// <summary>
        /// AUDIT-003: Pure helper — returns property names that are null among the
        /// foundation set. Safe for EditMode unit tests without a live bootstrap.
        /// </summary>
        public static IReadOnlyList<string> CollectMissingFoundationSystems(
            FoundationSystemsSnapshot systems)
        {
            var missing = new List<string>(FoundationSystemNames.Length);
            if (systems == null)
            {
                missing.AddRange(FoundationSystemNames);
                return missing;
            }
            if (systems.GameState == null) missing.Add(nameof(GameState));
            if (systems.TimeSystem == null) missing.Add(nameof(TimeSystem));
            if (systems.WeatherSystem == null) missing.Add(nameof(WeatherSystem));
            if (systems.TemperatureSystem == null) missing.Add(nameof(TemperatureSystem));
            if (systems.PhotoperiodSystem == null) missing.Add(nameof(PhotoperiodSystem));
            if (systems.NeedsSystem == null) missing.Add(nameof(NeedsSystem));
            if (systems.RadiationSystem == null) missing.Add(nameof(RadiationSystem));
            if (systems.Shelter == null) missing.Add(nameof(Shelter));
            return missing;
        }

        /// <summary>
        /// AUDIT-003: After InitializeSystems, assert every foundation system is
        /// non-null. Logs an error and throws <see cref="InvalidOperationException"/>
        /// so partial init cannot reach TickSystems and NullReference there.
        /// </summary>
        public void AssertFoundationSystems()
        {
            var missing = CollectMissingFoundationSystems(new FoundationSystemsSnapshot
            {
                GameState = GameState,
                TimeSystem = TimeSystem,
                WeatherSystem = WeatherSystem,
                TemperatureSystem = TemperatureSystem,
                PhotoperiodSystem = PhotoperiodSystem,
                NeedsSystem = NeedsSystem,
                RadiationSystem = RadiationSystem,
                Shelter = Shelter
            });

            if (missing.Count == 0) return;

            string msg =
                $"[GameBootstrap] Foundation systems missing after InitializeSystems: " +
                string.Join(", ", missing);
            Debug.LogError(msg);
            throw new InvalidOperationException(msg);
        }

    }
}
