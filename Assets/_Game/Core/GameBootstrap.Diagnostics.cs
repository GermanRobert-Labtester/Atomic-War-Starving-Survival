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
                // Convert PascalCase property name to snake_case for registry lookup.
                string snakeName = System.Text.RegularExpressions.Regex.Replace(
                    name, "([a-z])([A-Z])", "$1_$2").ToLowerInvariant();
                if (!Registry.IsSystemTicked(snakeName) && !Registry.IsSystemTicked(name))
                {
                    unticked.Add(name);
                }
            }
            return unticked;
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
        /// AUDIT-003: Pure helper — returns property names that are null among the
        /// foundation set. Safe for EditMode unit tests without a live bootstrap.
        /// </summary>
        public static IReadOnlyList<string> CollectMissingFoundationSystems(
            GameState gameState,
            TimeSystem timeSystem,
            WeatherSystem weatherSystem,
            TemperatureSystem temperatureSystem,
            PhotoperiodSystem photoperiodSystem,
            NeedsSystem needsSystem,
            RadiationSystem radiationSystem,
            Shelter.Shelter shelter)
        {
            var missing = new List<string>(FoundationSystemNames.Length);
            if (gameState == null) missing.Add(nameof(GameState));
            if (timeSystem == null) missing.Add(nameof(TimeSystem));
            if (weatherSystem == null) missing.Add(nameof(WeatherSystem));
            if (temperatureSystem == null) missing.Add(nameof(TemperatureSystem));
            if (photoperiodSystem == null) missing.Add(nameof(PhotoperiodSystem));
            if (needsSystem == null) missing.Add(nameof(NeedsSystem));
            if (radiationSystem == null) missing.Add(nameof(RadiationSystem));
            if (shelter == null) missing.Add(nameof(Shelter));
            return missing;
        }

        /// <summary>
        /// AUDIT-003: After InitializeSystems, assert every foundation system is
        /// non-null. Logs an error and throws <see cref="InvalidOperationException"/>
        /// so partial init cannot reach TickSystems and NullReference there.
        /// </summary>
        public void AssertFoundationSystems()
        {
            var missing = CollectMissingFoundationSystems(
                GameState,
                TimeSystem,
                WeatherSystem,
                TemperatureSystem,
                PhotoperiodSystem,
                NeedsSystem,
                RadiationSystem,
                Shelter);

            if (missing.Count == 0) return;

            string msg =
                $"[GameBootstrap] Foundation systems missing after InitializeSystems: " +
                string.Join(", ", missing);
            Debug.LogError(msg);
            throw new InvalidOperationException(msg);
        }

    }
}
