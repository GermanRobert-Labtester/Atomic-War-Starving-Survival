// SPDX-License-Identifier: MIT
// ============================================================================
// Core read model — Supply Forecast (P016)
// Owner      : pure projection of the ration + inventory owners' inputs.
// Consumers  : StatusPanel "3-DAY SUPPLY FORECAST" strip (host-computed rows).
// Contract   : no mutable state, no save section, no authority of its own. The
//              host reads the *existing* ration-policy consumption rates and
//              inventory counts and projects them forward here.
// ============================================================================
using System.Collections.Generic;

namespace Ashfall.Core.Campaign
{
    /// <summary>One projected day of the needs-vs-supply forecast.</summary>
    public readonly struct SupplyForecastDay
    {
        /// <summary>Simulation day this row projects (startDay + n).</summary>
        public readonly int Day;
        /// <summary>Food units remaining after that day's consumption (may go negative).</summary>
        public readonly int FoodAfter;
        /// <summary>Water units remaining after that day's consumption (may go negative).</summary>
        public readonly int WaterAfter;

        public SupplyForecastDay(int day, int foodAfter, int waterAfter)
        {
            Day = day;
            FoodAfter = foodAfter;
            WaterAfter = waterAfter;
        }

        public bool FoodShort => FoodAfter < 0;
        public bool WaterShort => WaterAfter < 0;
    }

    /// <summary>
    /// Deterministic, side-effect-free projection of stores against the
    /// per-day consumption the ration owner already applies. It does not model
    /// production or trade — it answers only "if nothing changes, when do the
    /// stores run out?".
    /// </summary>
    public static class SupplyForecast
    {
        public const int DefaultHorizonDays = 3;

        /// <summary>
        /// Project <paramref name="horizonDays"/> days of consumption. Negative
        /// remainders are preserved so the strip can name the shortfall rather
        /// than clamping it to a false zero.
        /// </summary>
        public static IReadOnlyList<SupplyForecastDay> Project(
            int startDay,
            int foodUnits,
            int waterUnits,
            int foodPerDay,
            int waterPerDay,
            int horizonDays = DefaultHorizonDays)
        {
            var rows = new List<SupplyForecastDay>();
            if (horizonDays <= 0) return rows;

            int food = foodUnits;
            int water = waterUnits;
            for (int i = 1; i <= horizonDays; i++)
            {
                food -= foodPerDay;
                water -= waterPerDay;
                rows.Add(new SupplyForecastDay(startDay + i, food, water));
            }
            return rows;
        }

        /// <summary>First projected day with a food or water shortfall, or 0 when none.</summary>
        public static int FirstShortfallDay(IReadOnlyList<SupplyForecastDay> rows)
        {
            if (rows == null) return 0;
            for (int i = 0; i < rows.Count; i++)
            {
                if (rows[i].FoodShort || rows[i].WaterShort)
                    return rows[i].Day;
            }
            return 0;
        }
    }
}