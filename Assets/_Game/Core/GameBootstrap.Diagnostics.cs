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
        /// </summary>
        private static bool TryGetRegistryAliases(string propertyName, out string[] aliases)
        {
            switch (propertyName)
            {
                case "ExpeditionSystem":
                    aliases = new[] { "expeditions" }; return true;
                case "CorpseSystem":
                    aliases = new[] { "corpses" }; return true;
                case "BlackRainHazardSystem":
                    aliases = new[] { "black_rain" }; return true;
                case "HatchDilemmaPromptField":
                    aliases = new[] { "hatch_dilemma" }; return true;
                case "ParleyOfferPromptField":
                    aliases = new[] { "parley_offer" }; return true;
                case "LifeboatTransmissionSystem":
                    aliases = new[] { "lifeboat" }; return true;
                case "AmputationSystem":
                    aliases = new[] { "amputation_daily" }; return true;
                case "ScurvySystem":
                    aliases = new[] { "scurvy_daily" }; return true;
                case "Mutagenesis":
                    aliases = new[] { "mutagenesis_tick", "mutagenesis_daily" }; return true;
                case "DeadDropSystem":
                    aliases = new[] { "dead_drops" }; return true;
                case "DeserterSystem":
                    aliases = new[] { "deserter_daily", "deserter" }; return true;
                case "EcosystemSystem":
                    aliases = new[] { "ecosystem_daily", "ecosystem" }; return true;
                case "HatchVisibilitySystem":
                    aliases = new[] { "hatch_visibility_daily", "hatch_visibility" }; return true;
                case "WeaponMaintenanceSystem":
                    aliases = new[] { "weapon_maint" }; return true;
                case "AntibioticResistSystem":
                    aliases = new[] { "antibiotic_resist" }; return true;
                case "HaulingSystem":
                    aliases = new[] { "hauling" }; return true;
                case "TriageSystem":
                    aliases = new[] { "triage" }; return true;
                case "ScrapWeaponSystem":
                    aliases = new[] { "scrap_weapon" }; return true;
                case "ClothingSystem":
                    aliases = new[] { "clothing" }; return true;
                case "AestheticsSystem":
                    aliases = new[] { "aesthetics" }; return true;
                case "CultMoralSystem":
                    aliases = new[] { "cult_moral" }; return true;
                case "BloodTransfusion":
                    aliases = new[] { "blood_transfusion" }; return true;
                case "GriefKeepsakes":
                    aliases = new[] { "grief_keepsakes" }; return true;
                case "SkillAtrophy":
                    aliases = new[] { "skill_atrophy" }; return true;
                case "SkillProgression":
                    aliases = new[] { "skill_progression", "skill_progression_daily" }; return true;
                case "Addiction":
                    aliases = new[] { "addiction" }; return true;
                case "PhantomIntruders":
                    aliases = new[] { "phantom_intruders" }; return true;
                case "StructuralIntegrity":
                    aliases = new[] { "structural_integrity" }; return true;
                case "FactionRadioIntercepts":
                    aliases = new[] { "faction_radio_intercepts" }; return true;
                case "PowerNetwork":
                    aliases = new[] { "power_network" }; return true;
                case "WaterStorage":
                    aliases = new[] { "water_storage" }; return true;
                case "KnowledgeMap":
                    aliases = new[] { "knowledge_map" }; return true;
                case "GeneratedMap":
                    aliases = new[] { "generated_map" }; return true;
                case "EventRunner":
                    aliases = new[] { "event_runner" }; return true;
                case "SuspicionTracker":
                    aliases = new[] { "suspicion_tracker" }; return true;
                case "VictoryProject":
                    aliases = new[] { "victory_project" }; return true;
                case "TimeSystem":
                    aliases = new[] { "time" }; return true;
                case "SaveSystem":
                    aliases = new[] { "save" }; return true;
                case "WorkbenchSystem":
                    aliases = new[] { "workbench" }; return true;
                case "UtilityAI":
                    aliases = new[] { "utility_ai" }; return true;
                case "RadioSystem":
                    aliases = new[] { "radio" }; return true;
                case "GameState":
                    aliases = new[] { "game_state" }; return true;
                case "EndgameEngine":
                    aliases = new[] { "endgame" }; return true;
                case "ShelterLayout":
                    aliases = new[] { "shelter_layout" }; return true;
                case "SleepQualitySystem":
                    aliases = new[] { "sleep_quality" }; return true;
                default:
                    aliases = null; return false;
            }
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
