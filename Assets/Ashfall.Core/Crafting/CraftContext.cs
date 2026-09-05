// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core.Crafting
{
    /// <summary>
    /// Typed context for completed or in-flight craft operations.
    /// Carries station, crafter survivor identity, profession/specialty, and recipe outcome
    /// so downstream systems (e.g. TradeSpecialtySystem) receive authoritative attribution.
    /// </summary>
    [Serializable]
    public sealed class CraftContext
    {
        public string StationId { get; set; } = string.Empty;
        public string CrafterSurvivorId { get; set; } = string.Empty;
        public string ProfessionId { get; set; } = string.Empty;
        public string RecipeId { get; set; } = string.Empty;
        public string ResultItemId { get; set; } = string.Empty;
        public int ResultAmount { get; set; } = 1;
        public int CompletedDay { get; set; } = 0;

        /// <summary>
        /// True if a real survivor was assigned to this craft operation.
        /// When false (unassigned/automated shelter craft), trade specialty progression is explicitly bypassed.
        /// </summary>
        public bool HasAssignedCrafter => !string.IsNullOrWhiteSpace(CrafterSurvivorId);
    }
}
