// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core.Expeditions
{
    /// <summary>
    /// Plans 146–149 MED: pure travel-stretch projection shared by estimate
    /// preview and live Start/Dispatch. Never mutates a registry catalog entry.
    /// </summary>
    public static class ExpeditionTravelStretch
    {
        /// <summary>
        /// Returns <paramref name="baseDistanceTicks"/> stretched by
        /// <paramref name="travelMultiplier"/> (ceil, min 1). Neutral or
        /// non-positive multipliers leave the base unchanged.
        /// </summary>
        public static int ProjectDistanceTicks(int baseDistanceTicks, float travelMultiplier)
        {
            int baseTicks = Math.Max(1, baseDistanceTicks);
            if (travelMultiplier <= 0f || Math.Abs(travelMultiplier - 1f) <= 0.001f)
                return baseTicks;
            return Math.Max(1, (int)Math.Ceiling(baseTicks * travelMultiplier));
        }

        /// <summary>
        /// Shallow-clone <paramref name="def"/> with stretched
        /// <see cref="ExpeditionDefinition.distanceTicks"/> when the multiplier
        /// changes travel length; otherwise returns the same instance.
        /// </summary>
        public static ExpeditionDefinition ProjectDefinition(ExpeditionDefinition def, float travelMultiplier)
        {
            if (def == null) throw new ArgumentNullException(nameof(def));
            int stretched = ProjectDistanceTicks(def.distanceTicks, travelMultiplier);
            if (stretched == def.distanceTicks)
                return def;

            return new ExpeditionDefinition
            {
                id = def.id,
                displayName = def.displayName,
                distanceTicks = stretched,
                dangerLevel = def.dangerLevel,
                encounterChancePerTick = def.encounterChancePerTick,
                baseStaminaDrainPerHour = def.baseStaminaDrainPerHour,
                lootCategories = def.lootCategories != null
                    ? new System.Collections.Generic.List<string>(def.lootCategories)
                    : new System.Collections.Generic.List<string>(),
                scavenging_table_id = def.scavenging_table_id,
                requiresDiscovery = def.requiresDiscovery
            };
        }
    }
}
