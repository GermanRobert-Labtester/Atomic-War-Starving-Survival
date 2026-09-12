// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Economy;

namespace Ashfall.Core.Narrative
{
    /// <summary>
    /// PLAN 147 — the contraband broker caravan: the barter acquisition route
    /// for activated contraband records. Built FROM the reviewed activation
    /// map (single gate authority): each activation becomes one stock line at
    /// the broker — canonical item, activation-grant quantity, and the
    /// activation's day gate as the per-arrival stock gate, so the stash
    /// route and the barter route share the same real-campaign-state gating.
    ///
    /// Pricing is canonical by flag (<c>use_canonical_item_values</c>) with a
    /// 25% scarcity premium (12500 bp): contraband costs MORE than the item's
    /// trade value, so buy→sell round-trips strictly lose value — no
    /// arbitrage, no second pricing authority (the item table stays the
    /// value authority; the premium is a visible multiplier, not a fork).
    ///
    /// Deterministic: same activations ⇒ identical caravan definition.
    /// </summary>
    public static class ContrabandBrokerCaravan
    {
        public const string CaravanId = "caravan_contraband_broker";

        /// <summary>Scarcity premium over canonical trade value: 12500 bp = 1.25x.</summary>
        public const int ScarcityPremiumBp = 12500;

        public const int SchedulePeriodDays = 10;
        public const int StayDurationDays = 2;

        public static MerchantCaravanDef Build(
            BunkerContrabandCatalog catalog,
            IReadOnlyList<ContrabandStashActivation> activations)
        {
            var stock = new List<CaravanStockItem>();
            if (activations != null)
            {
                foreach (var activation in activations
                    .Where(a => a != null
                                && !string.IsNullOrWhiteSpace(a.entryId)
                                && catalog.GetById(a.entryId) != null
                                && !string.IsNullOrWhiteSpace(a.canonicalItemId)
                                && a.grantQuantity > 0)
                    .OrderBy(a => a.entryId, StringComparer.Ordinal))
                {
                    stock.Add(new CaravanStockItem
                    {
                        item_id = activation.canonicalItemId,
                        quantity = activation.grantQuantity,
                        price_multiplier_bp = ScarcityPremiumBp,
                        available_from_day = Math.Max(0, activation.minDay)
                    });
                }
            }

            return new MerchantCaravanDef
            {
                caravan_id = CaravanId,
                name = "The Quiet Counter",
                description = "A folding table, a lantern with the shielded side down, and a voice that never asks where things came from. Prices are what they are because you cannot go elsewhere.",
                faction_id = "faction_wasteland_outlaws",
                schedule_period_days = SchedulePeriodDays,
                stay_duration_days = StayDurationDays,
                barter_tolerance_bp = 10000,
                counterfeit_risk_bp = 0,
                use_canonical_item_values = true,
                stock = stock
            };
        }
    }
}
