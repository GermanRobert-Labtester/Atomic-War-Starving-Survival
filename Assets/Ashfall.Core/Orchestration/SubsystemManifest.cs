// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Orchestration
{
    /// <summary>
    /// Lifecycle phase of an ASHFALL subsystem.
    /// </summary>
    public enum LifecyclePhase
    {
        /// <summary>Fundamental bootstrap systems initialized before simulation starts.</summary>
        Bootstrap,

        /// <summary>Core campaign simulation systems participating in daily ticks, state mutations, and persistence.</summary>
        CoreSimulation,

        /// <summary>Host presentation, UI coordination, and audio/diagnostic adapters.</summary>
        HostPresentation,

        /// <summary>Modular expansion subsystems (e.g. greenhouse, maritime, vehicle garage).</summary>
        Expansion
    }

    /// <summary>
    /// Declarative metadata for a single ASHFALL subsystem, unifying its setup,
    /// persistence (save section), campaign advance (day owner), and player surface (panel route).
    /// </summary>
    public record SubsystemDescriptor(
        string Id,
        string DisplayName,
        LifecyclePhase Phase,
        string? SaveSectionKey,
        string? DayOwnerId,
        string? PrimaryPanelRoute,
        bool HasDedicatedSetup,
        string Description
    );

    /// <summary>
    /// Declarative subsystem manifest for ASHFALL (Plan 28 / C2[9] Orchestration Spine).
    /// Acts as the single architectural declaration linking subsystem identity,
    /// save persistence, day owner execution, and UI surface routing.
    /// </summary>
    public static class SubsystemManifest
    {
        private static readonly Dictionary<string, SubsystemDescriptor> ById =
            new(StringComparer.Ordinal);

        public static readonly IReadOnlyList<SubsystemDescriptor> All = new List<SubsystemDescriptor>
        {
            new(
                "journal",
                "Campaign Journal",
                LifecyclePhase.Bootstrap,
                "journal",
                null,
                "journal",
                true,
                "Player journal, logs, knowledge entries, and briefing archives."
            ),
            new(
                "needs",
                "Survivor Needs",
                LifecyclePhase.CoreSimulation,
                "survivors",
                "needs",
                "survivors",
                true,
                "Survivor physiological needs: hunger, thirst, warmth, rest, and health."
            ),
            new(
                "inventory",
                "Shelter Inventory",
                LifecyclePhase.CoreSimulation,
                "inventory",
                null,
                "inventory",
                true,
                "Central item storage, weight/volume constraints, and item consumption."
            ),
            new(
                "weather",
                "Wasteland Weather",
                LifecyclePhase.CoreSimulation,
                "world",
                "weather",
                "weather",
                true,
                "Atmospheric conditions, storms, forecast generation, and station calibration."
            ),
            new(
                "radiation",
                "Radiation & Shielding",
                LifecyclePhase.CoreSimulation,
                "dose_ledger",
                "radiation",
                "radiation_detail",
                true,
                "Environmental radiation, shelter attenuation, acute sickness, and dose records."
            ),
            new(
                "radio",
                "Radio Communications",
                LifecyclePhase.CoreSimulation,
                "radio",
                "radio",
                "radio",
                true,
                "Tuner frequencies, distress signals, broadcast reception, and propagation."
            ),
            new(
                "expeditions",
                "Wasteland Expeditions",
                LifecyclePhase.CoreSimulation,
                "expedition",
                "expeditions",
                "expeditions",
                true,
                "Sortie planning, survivor travel, hazard encounters, and scavenged loot."
            ),
            new(
                "duty_roster",
                "Duty Roster",
                LifecyclePhase.CoreSimulation,
                "duty_roster",
                null,
                "quests",
                true,
                "Survivor work assignments, shift hours, fitness-for-duty, and overwork."
            ),
            new(
                "crafting",
                "Shelter Workshop Crafting",
                LifecyclePhase.CoreSimulation,
                "crafting",
                null,
                "crafting",
                true,
                "Recipe catalog execution, equipment fabrication, and gear repair."
            ),
            new(
                "research",
                "Research & Technology",
                LifecyclePhase.CoreSimulation,
                "research",
                null,
                "research",
                true,
                "Technological advancement tree, project progression, and discipline unlocks."
            ),
            new(
                "medical",
                "Medical Ward & Afflictions",
                LifecyclePhase.CoreSimulation,
                "medical",
                "medical",
                "medical",
                true,
                "Patient triage, disease progression, medical treatments, and bed recovery."
            ),
            new(
                "factions",
                "Faction Standing",
                LifecyclePhase.CoreSimulation,
                "regional_treaty",
                "factions",
                "factions",
                true,
                "Faction relations, trust thresholds, political diplomacy, and territorial influence."
            ),
            new(
                "economy",
                "Regional Economy & Barter",
                LifecyclePhase.CoreSimulation,
                "economy",
                "economy",
                "trade",
                true,
                "Trade goods, regional price atlas, embargoes, and merchant caravans."
            ),
            new(
                "greenhouse",
                "Hydroponic Greenhouse",
                LifecyclePhase.Expansion,
                "greenhouse",
                "greenhouse",
                "greenhouse",
                true,
                "Food cultivation, crop cycles, water/nutrient consumption, and harvest."
            ),
            new(
                "shelter_defense",
                "Sky Defense Battery",
                LifecyclePhase.Expansion,
                "sky_defense_battery",
                "shelter_defense",
                "sky_defense_battery",
                true,
                "Anti-air artillery, ordnance inventory, interception chance, and airspace security."
            ),
            new(
                "vehicle_garage",
                "Vehicle Garage",
                LifecyclePhase.Expansion,
                "vehicle_garage",
                null,
                "vehicle_garage",
                true,
                "Expedition vehicles, slot upgrades, wear maintenance, and recovery."
            ),
            new(
                "black_market",
                "Black Market",
                LifecyclePhase.Expansion,
                "black_market",
                null,
                "black_market",
                true,
                "Underground contraband dealer, loan financing, and immediate inventory settlement."
            ),
            new(
                "memorial",
                "Iron Cenotaph Memorial",
                LifecyclePhase.Expansion,
                "memorial",
                null,
                "iron_cenotaph_memorial",
                true,
                "Casualty records, eulogies, mourning vigils, and psychological grief resolution."
            )
        };

        static SubsystemManifest()
        {
            foreach (var descriptor in All)
            {
                ById[descriptor.Id] = descriptor;
            }
        }

        /// <summary>
        /// Attempts to get the descriptor for a subsystem ID.
        /// </summary>
        public static bool TryGet(string id, out SubsystemDescriptor? descriptor)
        {
            descriptor = null;
            if (string.IsNullOrEmpty(id)) return false;
            return ById.TryGetValue(id, out descriptor);
        }

        /// <summary>
        /// Checks whether a subsystem ID is registered.
        /// </summary>
        public static bool Contains(string id) => !string.IsNullOrEmpty(id) && ById.ContainsKey(id);

        /// <summary>
        /// Gets the descriptor for a subsystem ID, throwing if not found.
        /// </summary>
        public static SubsystemDescriptor Get(string id)
        {
            if (TryGet(id, out var descriptor) && descriptor != null)
                return descriptor;
            throw new KeyNotFoundException($"Subsystem '{id}' is not registered in SubsystemManifest.");
        }

        /// <summary>
        /// Returns all subsystems in a specific lifecycle phase.
        /// </summary>
        public static IReadOnlyList<SubsystemDescriptor> ForPhase(LifecyclePhase phase)
        {
            return All.Where(s => s.Phase == phase).ToList();
        }
    }
}
