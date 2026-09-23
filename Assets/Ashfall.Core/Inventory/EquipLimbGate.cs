// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Medical;

namespace Ashfall.Core.Inventory
{
    /// <summary>
    /// Pure functional limb and grip gating for equipable items.
    /// Implements UNBLOCK-01 F14-A / F14-B / XP-06 equipment gating.
    /// Engine-free, headless-testable.
    /// </summary>
    public static class EquipLimbGate
    {
        /// <summary>
        /// Computes effective hands and whether the survivor is restricted to simple-grip only.
        /// Intact limbs provide 1 full-grip hand.
        /// Prosthetics provide their configured hands and grip class (if condition > 0).
        /// </summary>
        public static (int hands, bool simpleOnly) EffectiveHands(
            IEnumerable<LimbState>? limbs,
            Func<string, ItemDefinition?>? catalog = null,
            Func<string, float>? conditionProvider = null)
        {
            if (limbs == null) return (2, false); // Default intact parity

            int total = 0;
            bool hasFullGrip = false;
            bool hasAnyHand = false;

            foreach (var state in limbs)
            {
                if (state.limb != LimbId.LeftArm && state.limb != LimbId.RightArm)
                    continue;

                if (state.condition == LimbCondition.Intact ||
                    state.condition == LimbCondition.Wounded ||
                    state.condition == LimbCondition.Infected ||
                    state.condition == LimbCondition.Gangrenous)
                {
                    total += 1;
                    hasAnyHand = true;
                    hasFullGrip = true;
                }
                else if (state.condition == LimbCondition.Prosthetic ||
                         state.condition == LimbCondition.Bionic)
                {
                    if (string.IsNullOrEmpty(state.prostheticId) || catalog == null)
                    {
                        // Fallback if no catalog: default prosthetic gives 1 simple hand
                        total += 1;
                        hasAnyHand = true;
                        continue;
                    }

                    var item = catalog(state.prostheticId);
                    if (item == null || item.providesLimb == null)
                        continue;

                    if (conditionProvider != null && conditionProvider(state.prostheticId) <= 0f)
                        continue; // Failed prosthetic behaves as amputated

                    int providedHands = item.providesLimb.hands;
                    total += providedHands;
                    if (providedHands > 0)
                    {
                        hasAnyHand = true;
                        if (string.Equals(item.providesLimb.gripClass, "full", StringComparison.OrdinalIgnoreCase))
                        {
                            hasFullGrip = true;
                        }
                    }
                }
                // Amputated contributes 0
            }

            bool simpleOnly = hasAnyHand && !hasFullGrip;
            return (total, simpleOnly);
        }

        /// <summary>
        /// Determines whether an item can be equipped given the survivor's limb state.
        /// Absent limbRequirements returns true (legacy compatibility).
        /// </summary>
        public static bool CanEquip(
            ItemDefinition? item,
            IEnumerable<LimbState>? limbs,
            Func<string, ItemDefinition?>? catalog = null,
            Func<string, float>? conditionProvider = null)
        {
            if (item == null) return false;
            var req = item.limbRequirements;
            if (req == null) return true; // Legacy items have no requirements

            var (hands, simpleOnly) = EffectiveHands(limbs, catalog, conditionProvider);
            if (req.hands > hands) return false;
            if (string.Equals(req.gripClass, "full", StringComparison.OrdinalIgnoreCase) && simpleOnly)
                return false;

            return true;
        }
    }
}
