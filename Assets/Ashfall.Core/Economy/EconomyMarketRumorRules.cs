// SPDX-License-Identifier: MIT
using System;
using System.Globalization;

namespace Ashfall.Core.Economy
{
    /// <summary>
    /// Plan 212 deferred follow-up — trade rumors derived from REAL market
    /// state. Pure projection: a market shock event becomes one restrained
    /// radio-rumor line. No RNG, no invented prices — the text is a
    /// deterministic function of the shock the canonical market itself
    /// applied. Tone: restrained, human, diegetic; no real-world references.
    /// </summary>
    public static class EconomyMarketRumorRules
    {
        /// <summary>Line for a shock the market actually applied (start event).</summary>
        public static string ShockStartedLine(MarketShockState shock)
        {
            if (shock == null) return string.Empty;
            string category = shock.categoryId ?? string.Empty;
            return shock.isShortage
                ? string.Format(CultureInfo.InvariantCulture,
                    "Market word: {0} is running thin. Prices climb while it lasts — the counters say so, not the rumor mill.",
                    category)
                : string.Format(CultureInfo.InvariantCulture,
                    "Market word: {0} is flooding the counters. Sellers take what they can get while it holds.",
                    category);
        }

        /// <summary>Line for a shock the market actually expired (expiry event).</summary>
        public static string ShockExpiredLine(MarketShockState shock)
        {
            if (shock == null) return string.Empty;
            string category = shock.categoryId ?? string.Empty;
            return string.Format(CultureInfo.InvariantCulture,
                "Market word: the {0} squeeze has eased. Prices walk back toward the honest counter.",
                category);
        }
    }
}
