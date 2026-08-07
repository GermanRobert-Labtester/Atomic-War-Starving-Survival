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
            { "BicycleSystem",           new[] { "bicycle" } },
            { "WaterEconomySystem",      new[] { "water_economy" } },
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
            { "ShelterModuleAcidTrap",   new[] { "shelter_module_acid_trap" } },
            { "ShelterModuleAutodoc",    new[] { "shelter_module_autodoc" } },
            { "ShelterModuleCctv",       new[] { "shelter_module_cctv" } },
            { "ShelterModuleClassroom",  new[] { "shelter_module_classroom" } },
            { "ShelterModuleConfessional", new[] { "shelter_module_confessional" } },
            { "ShelterModuleConveyor",   new[] { "shelter_module_conveyor" } },
            { "ShelterModuleDaylightSensor", new[] { "shelter_module_daylight_sensor" } },
            { "ShelterModuleDroneStation", new[] { "shelter_module_drone_station" } },
            { "ShelterModuleHoloEmitter", new[] { "shelter_module_holo_emitter" } },
            { "ShelterModuleInsectFarm", new[] { "shelter_module_insect_farm" } },
            { "ShelterModuleLathe",      new[] { "shelter_module_lathe" } },
            { "ShelterModuleMortar",     new[] { "shelter_module_mortar" } },
            { "ShelterModulePanicButton", new[] { "shelter_module_panic_button" } },
            { "ShelterModulePitfall",    new[] { "shelter_module_pitfall" } },
            { "ShelterModuleReloader",   new[] { "shelter_module_reloader" } },
            { "ShelterModuleSorter",     new[] { "shelter_module_sorter" } },
            { "ShelterModuleThermostat", new[] { "shelter_module_thermostat" } },
            { "ShelterModuleWasteChute", new[] { "shelter_module_waste_chute" } },
            { "ShelterModuleAutopsy", new[] { "shelter_module_autopsy" } },
            { "ShelterModuleBatteryBank", new[] { "shelter_module_battery_bank" } },
            { "ShelterModuleBioLatrine", new[] { "shelter_module_bio_latrine" } },
            { "ShelterModuleChoreBoard", new[] { "shelter_module_chore_board" } },
            { "ShelterModuleDeadManSwitch", new[] { "shelter_module_dead_man_switch" } },
            { "ShelterModuleDeconShower", new[] { "shelter_module_decon_shower" } },
            { "ShelterModuleDialysis", new[] { "shelter_module_dialysis" } },
            { "ShelterModuleDistressBeacon", new[] { "shelter_module_distress_beacon" } },
            { "ShelterModuleDronePad", new[] { "shelter_module_drone_pad" } },
            { "ShelterModuleGarage", new[] { "shelter_module_garage" } },
            { "ShelterModuleGunRack", new[] { "shelter_module_gun_rack" } },
            { "ShelterModuleHammock", new[] { "shelter_module_hammock" } },
            { "ShelterModuleHandCrank", new[] { "shelter_module_hand_crank" } },
            { "ShelterModuleHotShower", new[] { "shelter_module_hot_shower" } },
            { "ShelterModuleIncinerator", new[] { "shelter_module_incinerator" } },
            { "ShelterModuleMagmaTap", new[] { "shelter_module_magma_tap" } },
            { "ShelterModuleMotionSensor", new[] { "shelter_module_motion_sensor" } },
            { "ShelterModulePanicRoom", new[] { "shelter_module_panic_room" } },
            { "ShelterModulePrintingPress", new[] { "shelter_module_printing_press" } },
            { "ShelterModulePunchingBag", new[] { "shelter_module_punching_bag" } },
            { "ShelterModuleRainBarrel", new[] { "shelter_module_rain_barrel" } },
            { "ShelterModuleRecordPlayer", new[] { "shelter_module_record_player" } },
            { "ShelterModuleSprinklers", new[] { "shelter_module_sprinklers" } },
            { "ShelterModuleThumper", new[] { "shelter_module_thumper" } },
            { "ShelterModuleTreadmillGen", new[] { "shelter_module_treadmill_gen" } },
            { "ShelterModuleTurret", new[] { "shelter_module_turret" } },
            { "ShelterModuleVaultDoor", new[] { "shelter_module_vault_door" } },
            { "ShelterModuleWoodStove", new[] { "shelter_module_wood_stove" } },
            { "EventBrawl",              new[] { "event_brawl" } },
            { "EventComingOfAge",        new[] { "event_coming_of_age" } },
            { "EventCultBlessing",       new[] { "event_cult_blessing" } },
            { "EventCultInitiation",     new[] { "event_cult_initiation" } },
            { "EventCultOfAi",           new[] { "event_cult_of_ai" } },
            { "EventEmpCascade",         new[] { "event_emp_cascade" } },
            { "EventFeralRescue",        new[] { "event_feral_rescue" } },
            { "EventFoundDiary",         new[] { "event_found_diary" } },
            { "EventGriefCascade",       new[] { "event_grief_cascade" } },
            { "EventHungerStrike",       new[] { "event_hunger_strike" } },
            { "EventNodeCollapse",       new[] { "event_node_collapse" } },
            { "EventRansomNote",         new[] { "event_ransom_note" } },
            { "EventSchism",             new[] { "event_schism" } },
            { "EventSecretSociety",      new[] { "event_secret_society" } },
            { "EventSiblingFeud",        new[] { "event_sibling_feud" } },
            { "EventSpontaneousMurder",  new[] { "event_spontaneous_murder" } },
            { "EventTeenRebellion",      new[] { "event_teen_rebellion" } },
            { "EventWitchHunt",          new[] { "event_witch_hunt" } },
            { "EventEuthanasiaPact", new[] { "event_euthanasia_pact" } },
            { "EventFactionMerger", new[] { "event_faction_merger" } },
            { "EventMudslide", new[] { "event_mudslide" } },
            { "EventNumbersStation", new[] { "event_numbers_station" } },
            { "EventProjectSabotage", new[] { "event_project_sabotage" } },
            { "EventSinkhole", new[] { "event_sinkhole" } },
            { "EventTriangulation", new[] { "event_triangulation" } },
            { "EventVaultCollision", new[] { "event_vault_collision" } },
            { "EventWarlordSuccession", new[] { "event_warlord_succession" } },
            // CoreFamilies bulk diagnostics (auto)
            { "FalloutStormHazard", new[] { "fallout_storm_hazard_system" } },
            { "ActionCrawlspace", new[] { "action_crawlspace" } },
            { "ActionPlay", new[] { "action_play" } },
            { "ActionSlaughterPet", new[] { "action_slaughter_pet" } },
            { "ActionTeachChild", new[] { "action_teach_child" } },
            { "ActionTellStories", new[] { "action_tell_stories" } },
            { "ItemAshGoat", new[] { "item_ash_goat" } },
            { "ItemBoots", new[] { "item_boots" } },
            { "ItemLiveTrap", new[] { "item_live_trap" } },
            { "ItemMutantChicken", new[] { "item_mutant_chicken" } },
            { "ItemToys", new[] { "item_toys" } },
            { "TraitAshTongue", new[] { "trait_ash_tongue" } },
            { "TraitKleptomaniac", new[] { "trait_kleptomaniac" } },
            { "TraitMascot", new[] { "trait_mascot" } },
            { "TraitStuntedEmpathy", new[] { "trait_stunted_empathy" } },
            { "TraitSuperstitious", new[] { "trait_superstitious" } },
            { "AfflictionBunkerFever", new[] { "affliction_bunker_fever" } },
            { "AfflictionZoonoticFlu", new[] { "affliction_zoonotic_flu" } },
            { "ModuleRationLock", new[] { "module_ration_lock" } },
            { "NodeOrphanage", new[] { "node_orphanage" } },
            { "PetGuardDog", new[] { "pet_guard_dog" } },
            { "ActionAdministerPlacebo", new[] { "action_administer_placebo" } },
            { "ActionBarricadeDoor", new[] { "action_barricade_door" } },
            { "ActionBoilBatteries", new[] { "action_boil_batteries" } },
            { "ActionBroadcastPropaganda", new[] { "action_broadcast_propaganda" } },
            { "ActionBurnCharcoal", new[] { "action_burn_charcoal" } },
            { "ActionBuryTimeCapsule", new[] { "action_bury_time_capsule" } },
            { "ActionCallCaravan", new[] { "action_call_caravan" } },
            { "ActionCoverTracks", new[] { "action_cover_tracks" } },
            { "ActionCrackMainframe", new[] { "action_crack_mainframe" } },
            { "ActionDecrypt", new[] { "action_decrypt" } },
            { "ActionDemandTribute", new[] { "action_demand_tribute" } },
            { "ActionEstablishRoute", new[] { "action_establish_route" } },
            { "ActionExile", new[] { "action_exile" } },
            { "ActionFish", new[] { "action_fish" } },
            { "ActionHarvestOrgans", new[] { "harvest_organs" } },
            { "ActionInfectSelf", new[] { "infect_self" } },
            { "ActionIsotopeTrace", new[] { "action_isotope_trace" } },
            { "ActionMercy", new[] { "action_mercy" } },
            { "ActionMixCement", new[] { "action_mix_cement" } },
            { "ActionMixChems", new[] { "mix_chems" } },
            { "ActionOverwatch", new[] { "action_overwatch" } },
            { "ActionPhysicalTherapy", new[] { "action_physical_therapy" } },
            { "ActionPirateRadio", new[] { "action_pirate_radio" } },
            { "ActionPlaceBait", new[] { "action_place_bait" } },
            { "ActionPullTooth", new[] { "action_pull_tooth" } },
            { "ActionRigCorpse", new[] { "action_rig_corpse" } },
            { "ActionRoutePower", new[] { "action_route_power" } },
            { "ActionSabotage", new[] { "action_sabotage" } },
            { "ActionScorchedEarth", new[] { "action_scorched_earth" } },
            { "ActionSealRoom", new[] { "action_seal_room" } },
            { "ActionSelfSurgery", new[] { "self_surgery" } },
            { "ActionSilentTakedown", new[] { "action_silent_takedown" } },
            { "ActionSiphonGas", new[] { "action_siphon_gas" } },
            { "ActionStabilizeDNA", new[] { "action_stabilize_dna" } },
            { "ActionStargazing", new[] { "action_stargazing" } },
            { "ActionWorshipIdol", new[] { "action_worship_idol" } },
            { "AfflictionAdrenalineCrash", new[] { "affliction_adrenaline_crash" } },
            { "AfflictionAmnesia", new[] { "affliction_amnesia" } },
            { "AfflictionBrainwashed", new[] { "affliction_brainwashed" } },
            { "AfflictionBrittleBones", new[] { "affliction_brittle_bones" } },
            { "AfflictionCaveMadness", new[] { "affliction_cave_madness" } },
            { "AfflictionFeralRegression", new[] { "affliction_feral_regression" } },
            { "AfflictionImaginaryFriend", new[] { "affliction_imaginary_friend" } },
            { "AfflictionNerveDamage", new[] { "affliction_nerve_damage" } },
            { "AfflictionOldAge", new[] { "affliction_old_age" } },
            { "AfflictionPhantomLimb", new[] { "affliction_phantom_limb" } },
            { "AfflictionRadHallucinations", new[] { "affliction_rad_hallucinations" } },
            { "AfflictionRadiationBlindness", new[] { "affliction_radiation_blindness" } },
            { "AfflictionScurvyDegeneration", new[] { "affliction_scurvy_degeneration" } },
            { "AfflictionSporeLung", new[] { "affliction_spore_lung" } },
            { "AfflictionSterile", new[] { "affliction_sterile" } },
            { "AfflictionSurvivorsGuilt", new[] { "affliction_survivors_guilt" } },
            { "AfflictionTBI", new[] { "affliction_tbi" } },
            { "AfflictionThyroidCancer", new[] { "affliction_thyroid_cancer" } },
            { "AfflictionTrenchFoot", new[] { "affliction_trench_foot" } },
            { "AudioEventDeafening", new[] { "audio_event_deafening" } },
            { "AudioEventHeartbeat", new[] { "audio_event_heartbeat" } },
            { "CombatBleedOut", new[] { "combat_bleed_out" } },
            { "CombatFlanking", new[] { "combat_flanking" } },
            { "CombatSuppression", new[] { "combat_suppression" } },
            { "CombatStanceLastStand", new[] { "combat_stance_last_stand" } },
            { "CrisisFeralFlora", new[] { "crisis_feral_flora" } },
            { "CrisisStructuralFailure", new[] { "crisis_structural_failure" } },
            { "DurabilitySuppressor", new[] { "durability_suppressor" } },
            { "EndgameUltimatum", new[] { "endgame_ultimatum" } },
            { "HazardCookOff", new[] { "hazard_cook_off" } },
            { "HazardExplosiveCrafting", new[] { "hazard_explosive_crafting" } },
            { "HazardFriendlyFire", new[] { "hazard_friendly_fire" } },
            { "HazardMethane", new[] { "hazard_methane" } },
            { "HazardMimicCrate", new[] { "hazard_mimic_crate" } },
            { "HazardSurgicalBotch", new[] { "hazard_surgical_botch" } },
            { "HazardWeaponBurst", new[] { "hazard_weapon_burst" } },
            { "HiddenStatUnseen", new[] { "hidden_stat_unseen" } },
            { "ItemAICoreData", new[] { "item_ai_core_data" } },
            { "ItemAmmoTypes", new[] { "item_ammo_types" } },
            { "ItemAmmonia", new[] { "item_ammonia" } },
            { "ItemAmphetamines", new[] { "item_amphetamines" } },
            { "ItemAshGhillie", new[] { "item_ash_ghillie" } },
            { "ItemAutoDoc", new[] { "item_auto_doc" } },
            { "ItemBioPlastic", new[] { "item_bio_plastic" } },
            { "ItemBloodBag", new[] { "item_blood_bag" } },
            { "ItemBoneSaw", new[] { "bone_saw" } },
            { "ItemC4", new[] { "item_c4" } },
            { "ItemCaltrops", new[] { "item_caltrops" } },
            { "ItemCarrierBird", new[] { "item_carrier_bird" } },
            { "ItemChildsDrawing", new[] { "item_childs_drawing" } },
            { "ItemCigarettes", new[] { "item_cigarettes" } },
            { "ItemClimbingGear", new[] { "item_climbing_gear" } },
            { "ItemDecoy", new[] { "item_decoy" } },
            { "ItemDogTags", new[] { "item_dog_tags" } },
            { "ItemEMPGrenade", new[] { "item_emp_grenade" } },
            { "ItemEncryptedDrive", new[] { "item_encrypted_drive" } },
            { "ItemEpiPen", new[] { "item_epipen" } },
            { "ItemExosuit", new[] { "item_exosuit" } },
            { "ItemFaradayPack", new[] { "item_faraday_pack" } },
            { "ItemForeignBook", new[] { "item_foreign_book" } },
            { "ItemGeigerCalibrator", new[] { "item_geiger_calibrator" } },
            { "ItemGlowingMushroom", new[] { "item_glowing_mushroom" } },
            { "ItemGoldBars", new[] { "item_gold_bars" } },
            { "ItemGuitar", new[] { "item_guitar" } },
            { "ItemHeirloom", new[] { "item_heirloom" } },
            { "ItemIBeam", new[] { "item_i_beam" } },
            { "ItemImpureIodine", new[] { "item_impure_iodine" } },
            { "ItemJuggernautArmor", new[] { "item_juggernaut_armor" } },
            { "ItemKevlarVest", new[] { "item_kevlar_vest" } },
            { "ItemKeycards", new[] { "item_keycards" } },
            { "ItemLandmine", new[] { "item_landmine" } },
            { "ItemLeadApron", new[] { "item_lead_apron" } },
            { "ItemLiquidStitches", new[] { "item_liquid_stitches" } },
            { "ItemMaggots", new[] { "maggots" } },
            { "ItemMilGasMask", new[] { "item_mil_gas_mask" } },
            { "ItemMutantGland", new[] { "item_mutant_gland" } },
            { "ItemNanites", new[] { "item_nanites" } },
            { "ItemNightVision", new[] { "item_night_vision" } },
            { "ItemPackMule", new[] { "item_pack_mule" } },
            { "ItemPasswordNote", new[] { "item_password_note" } },
            { "ItemPhotoAlbum", new[] { "item_photo_album" } },
            { "ItemPotassiumIodide", new[] { "item_potassium_iodide" } },
            { "ItemPresidentialSeal", new[] { "item_presidential_seal" } },
            { "ItemPrussianBlue", new[] { "item_prussian_blue" } },
            { "ItemRTGBattery", new[] { "item_rtg_battery" } },
            { "ItemSeedLedger", new[] { "item_seed_ledger" } },
            { "ItemShockCollar", new[] { "item_shock_collar" } },
            { "ItemSnowshoes", new[] { "item_snowshoes" } },
            { "ItemSurgicalTubing", new[] { "item_surgical_tubing" } },
            { "ItemTearGas", new[] { "item_tear_gas" } },
            { "ItemTeddyBear", new[] { "item_teddy_bear" } },
            { "ItemTrashHazmat", new[] { "item_trash_hazmat" } },
            { "ItemUndeliveredMail", new[] { "item_undelivered_mail" } },
            { "ItemVacuumTubes", new[] { "item_vacuum_tubes" } },
            { "ItemVinylCollection", new[] { "item_vinyl_collection" } },
            { "ItemVitamins", new[] { "item_vitamins" } },
            { "ItemWalkieTalkie", new[] { "item_walkie_talkie" } },
            { "ItemWastelandSoap", new[] { "item_wasteland_soap" } },
            { "ItemWaterTabs", new[] { "item_water_tabs" } },
            { "ItemWeldingGoggles", new[] { "item_welding_goggles" } },
            { "ItemWristDosimeter", new[] { "item_wrist_dosimeter" } },
            { "LocationArcade", new[] { "location_arcade" } },
            { "LocationSlaveMarket", new[] { "location_slave_market" } },
            { "LocationStrandedYacht", new[] { "location_stranded_yacht" } },
            { "MapAquifer", new[] { "map_aquifer" } },
            { "AshDriftSystem", new[] { "ash_drift_system" } },
            { "BurnWardSystem", new[] { "burn_ward_system" } },
            { "CognitiveDecaySystem", new[] { "cognitive_decay_system" } },
            { "LightningStrikesSystem", new[] { "lightning_strikes_system" } },
            { "LocationStateRuinSystem", new[] { "location_state_ruin_system" } },
            { "MobileCampSystem", new[] { "mobile_camp_system" } },
            { "MoralDilemmaSystem", new[] { "moral_dilemma_system" } },
            { "NeedleSterilizationSystem", new[] { "needle_sterilization_system" } },
            { "NightScavengeSystem", new[] { "night_scavenge_system" } },
            { "ProstheticCraftingSystem", new[] { "prosthetic_crafting_system" } },
            { "SeismicVentsSystem", new[] { "seismic_vents_system" } },
            { "SevereFrostbiteSystem", new[] { "severe_frostbite_system" } },
            { "TetanusAfflictionSystem", new[] { "tetanus_affliction_system" } },
            { "TimeSystemSys", new[] { "time_system" } },
            { "ToothDecaySystem", new[] { "tooth_decay_system" } },
            { "VehicleStrandingSystem", new[] { "vehicle_stranding_system" } },
            { "VehicleSystem", new[] { "vehicle_system" } },
            { "VisionLossSystem", new[] { "vision_loss_system" } },
            { "VisitorRNGSystem", new[] { "visitor_rngsystem" } },
            { "NPCAddictsPassive", new[] { "npc_addicts_passive" } },
            { "NPCAggroScavengers", new[] { "npc_aggro_scavengers" } },
            { "NPCAggroTrader", new[] { "npc_aggro_trader" } },
            { "NPCBandits", new[] { "npc_bandits" } },
            { "NPCBlackOps", new[] { "npc_black_ops" } },
            { "NPCBroker", new[] { "npc_broker" } },
            { "NPCCannibals", new[] { "npc_cannibals" } },
            { "NPCChemScientists", new[] { "npc_chem_scientists" } },
            { "NPCCityResidents", new[] { "npc_city_residents" } },
            { "NPCCollaborators", new[] { "npc_collaborators" } },
            { "NPCConscripts", new[] { "npc_conscripts" } },
            { "NPCDesperateFamily", new[] { "npc_desperate_family" } },
            { "NPCDrunksAggro", new[] { "npc_drunks_aggro" } },
            { "NPCHomeless", new[] { "npc_homeless" } },
            { "NPCLonePsychopath", new[] { "npc_lone_psychopath" } },
            { "NPCLooters", new[] { "npc_looters" } },
            { "NPCMercenaries", new[] { "npc_mercenaries" } },
            { "NPCMilitaryPatrol", new[] { "npc_military_patrol" } },
            { "NPCPassiveScavengers", new[] { "npc_passive_scavengers" } },
            { "NPCPassiveTrader", new[] { "npc_passive_trader" } },
            { "NPCPsychopathPair", new[] { "npc_psychopath_pair" } },
            { "NPCRebelMilitia", new[] { "npc_rebel_militia" } },
            { "NPCRebelModerates", new[] { "npc_rebel_moderates" } },
            { "NPCRebelSnipers", new[] { "npc_rebel_snipers" } },
            { "NPCRebelZealots", new[] { "npc_rebel_zealots" } },
            { "NPCSlavers", new[] { "npc_slavers" } },
            { "NPCSpecOps", new[] { "npc_spec_ops" } },
            { "NPCSurvivalists", new[] { "npc_survivalists" } },
            { "NPCTaxCollector", new[] { "npc_tax_collector" } },
            { "NPCTerrorists", new[] { "npc_terrorists" } },
            { "NPCTheNegotiator", new[] { "npc_the_negotiator" } },
            { "NPCTheOld", new[] { "npc_the_old" } },
            { "NPCTheParents", new[] { "npc_the_parents" } },
            { "NPCTravelingCouple", new[] { "npc_traveling_couple" } },
            { "NodeAutomatedArmory", new[] { "node_automated_armory" } },
            { "NodeGhostShip", new[] { "node_ghost_ship" } },
            { "NodeMutantHive", new[] { "node_mutant_hive" } },
            { "NodePlayerBank", new[] { "node_player_bank" } },
            { "NodeSector7G", new[] { "node_sector_7g" } },
            { "NodeSporeHive", new[] { "node_spore_hive" } },
            { "PetFeralCat", new[] { "pet_feral_cat" } },
            { "ProjectBioReactor", new[] { "project_bio_reactor" } },
            { "ProjectDeepWell", new[] { "project_deep_well" } },
            { "ProjectElevator", new[] { "project_elevator" } },
            { "ProjectMinecart", new[] { "project_minecart" } },
            { "ProjectRadioArray", new[] { "project_radio_array" } },
            { "ProjectSurfaceDome", new[] { "project_surface_dome" } },
            { "ShelterEventCaravanAmbush", new[] { "shelter_event_caravan_ambush" } },
            { "ShelterEventFalseCure", new[] { "event_false_cure" } },
            { "ShelterEventRansom", new[] { "shelter_event_ransom" } },
            { "ShelterEventRefugees", new[] { "shelter_event_refugees" } },
            { "ShelterEventTheMirror", new[] { "shelter_event_the_mirror" } },
            { "ShelterEventTribute", new[] { "shelter_event_tribute" } },
            { "SkirmishBandit_vs_Terror", new[] { "skirmish_bandit_vs_terror" } },
            { "SkirmishMil_vs_Rebel", new[] { "skirmish_mil_vs_rebel" } },
            { "SkirmishMil_vs_Terror", new[] { "skirmish_mil_vs_terror" } },
            { "SkirmishRebel_vs_Bandit", new[] { "skirmish_rebel_vs_bandit" } },
            { "SkirmishRebel_vs_Terror", new[] { "skirmish_rebel_vs_terror" } },
            { "TraderPlagueConvoy", new[] { "trader_plague_convoy" } },
            { "TraitAnthropophobia", new[] { "trait_anthropophobia" } },
            { "TraitClairvoyant", new[] { "trait_clairvoyant" } },
            { "TraitGenerationalTrauma", new[] { "trait_generational_trauma" } },
            { "TraitInheritedGenetics", new[] { "trait_inherited_genetics" } },
            { "TraitMatriarch", new[] { "trait_matriarch" } },
            { "TraitPTSD", new[] { "trait_ptsd" } },
            { "UIEventBlurredVision", new[] { "ui_event_blurred_vision" } },
            { "UIEventCorruptionScare", new[] { "ui_event_corruption_scare" } },
            { "UIEventFalseInventory", new[] { "ui_event_false_inventory" } },
            { "UIEventGhostRadio", new[] { "ui_event_ghost_radio" } },
            { "UIEventHacking", new[] { "ui_event_hacking" } },
            { "UIEventLowPower", new[] { "ui_event_low_power" } },
            { "UIEventMapRot", new[] { "ui_event_map_rot" } },
            { "UIEventPhantomBlip", new[] { "ui_event_phantom_blip" } },
            { "VehicleArmoredTruck", new[] { "vehicle_armored_truck" } },
            { "VehicleMotorcycle", new[] { "vehicle_motorcycle" } },
            { "VehicleRowboat", new[] { "vehicle_rowboat" } },
            { "VisitorAbandonedState", new[] { "visitor_abandoned" } },
            { "VisitorChurchHostile", new[] { "visitor_church_hostile" } },
            { "VisitorChurchSanctuary", new[] { "visitor_church_sanctuary" } },
            { "VisitorExplodedState", new[] { "visitor_exploded_state" } },
            { "VisitorFleeingHorde", new[] { "visitor_fleeing_horde" } },
            { "VisitorHospitalPatients", new[] { "visitor_hospital_patients" } },
            { "VisitorHospitalStaff", new[] { "visitor_hospital_staff" } },
            { "VisitorMilTrainingYard", new[] { "visitor_mil_training_yard" } },
            { "VisitorQuestFaction", new[] { "visitor_quest_faction" } },
            { "VisitorRebelTrainingYard", new[] { "visitor_rebel_training_yard" } },
            { "WeaponChainsaw", new[] { "weapon_chainsaw" } },
            { "WeaponFlamethrower", new[] { "weapon_flamethrower" } },
            { "WeaponHMG", new[] { "weapon_hmg" } },
            { "WeaponRPG", new[] { "weapon_rpg" } },
            { "WorldEventDeforestation", new[] { "world_event_deforestation" } },
            { "WorldEventFinalWinter", new[] { "world_event_final_winter" } },
            { "WorldEventFissure", new[] { "world_event_fissure" } },
            { "WorldEventGreatFamine", new[] { "world_event_great_famine" } },
            { "WorldEventMegafauna", new[] { "world_event_megafauna" } },

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
