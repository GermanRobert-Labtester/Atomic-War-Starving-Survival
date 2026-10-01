// SPDX-License-Identifier: MIT
// ============================================================================
// Host surface : Supply forecast (P016)
// Owner        : Main (host). Pure projection lives in Core: SupplyForecast.
// Purpose      : Feeds the StatusPanel "3-DAY SUPPLY FORECAST" strip from the
//                *existing* ration owner's policy and the inventory store
//                counts. It writes nothing and owns no state; it mirrors the
//                rates StartingLevelRationsDayOwner already applies each day.
// ============================================================================
using System.Collections.Generic;
using Godot;
using Ashfall.Core.Campaign;
using Ashfall.Core.StartingLevel;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        /// <summary>
        /// P016 — read-only projection of the next three days of consumption
        /// against current stores. Rates come from the ration policy and the
        /// cohort child-food rule the day owner uses; counts come from the
        /// inventory authority. Never fabricates a rate or a store count.
        /// </summary>
        private IReadOnlyList<SupplyForecastDay> BuildSupplyForecast()
        {
            SetupStartingLevel();
            SetupInventory();
            SetupDoseLedger();

            var policy = _startingLevel?.System?.State?.rationPolicy ?? RationPolicy.Standard;
            int baseFood = policy == RationPolicy.Half ? 2 : 3;
            int childFood = _doseLedger?.Cohort?.CalculateChildFoodUnits(policy) ?? 0;
            int waterPerDay = policy == RationPolicy.Irradiated ? 0
                : (policy == RationPolicy.Half ? 2 : 3);

            int food = _inventory?.Inventory?.CountById("canned_food") ?? 0;
            int water = _inventory?.Inventory?.CountById("clean_water") ?? 0;

            return SupplyForecast.Project(_simDay, food, water, baseFood + childFood, waterPerDay);
        }
    }
}