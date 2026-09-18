// SPDX-License-Identifier: MIT

namespace Ashfall.Core.Campaign
{
    /// <summary>
    /// Plan 31 — semantic kind taxonomy for day events.
    /// Provides a total, test-enforced domain classification over the day event vocabulary.
    /// Every registered event kind maps to exactly one non-unknown <see cref="SemanticKind"/>.
    /// </summary>
    public enum SemanticKind
    {
        Unknown = 0,

        /// <summary>Internal steady-state simulation heartbeats; intentionally non-player-facing.</summary>
        Heartbeat = 1,

        /// <summary>Casualties and survivor mortality.</summary>
        Casualty = 2,

        /// <summary>Immediate hazards, structural failures, critical alerts, and emergency warnings.</summary>
        Hazard = 3,

        /// <summary>Survivor condition, nutrition, health, lifecycle, duty changes, and ward status.</summary>
        Survivor = 4,

        /// <summary>Shelter infrastructure, power grid, maintenance, sanitation, and physical facility state.</summary>
        Shelter = 5,

        /// <summary>Production, crafting, harvesting, resource deltas, trade, and economic shocks.</summary>
        Production = 6,

        /// <summary>Expeditions, caravan journeys, and subterranean rescue operations.</summary>
        Expedition = 7,

        /// <summary>Radio signals, broadcasts, intercepted communications, and distress frequencies.</summary>
        Communication = 8,

        /// <summary>Weather conditions, atmospheric changes, and forecast tracking.</summary>
        Weather = 9,

        /// <summary>Narrative arcs, memory echoes, personal quests, and mediated social developments.</summary>
        Narrative = 10
    }
}
