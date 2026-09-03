using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Economy
{
    /// <summary>
    /// Plan 56 phase 3 — makes <c>regionalSupply</c> provenance functionally
    /// meaningful. The annotation on each economy good declares where it is
    /// produced (foundry / traplines / greenhouse / settlement / flotilla /
    /// coastal / general); this router turns that provenance into market
    /// behavior:
    ///
    ///   - caravan cargo selection: specialty stock drawn from the origin
    ///     region's production pool instead of hand-coded tables;
    ///   - settlement shortage logic: locally produced goods ride out a
    ///     shortage (demand scale 0.5), general goods track it (1.0), pure
    ///     imports escalate (1.5) — and a waystation resupply during shortage
    ///     keeps local/general stock while import-only stock lapses.
    ///
    /// Pure static router: no state, no engine types. Every stochastic choice
    /// flows through the caller's ISeededRng; without one the selection is a
    /// stable ordinal rotation (deterministic by construction).
    /// </summary>
    public static class RegionalSupplyRouter
    {
        /// <summary>
        /// Origin-region → production-tag normalization. Handles both
        /// vocabularies in the wild: the legacy regional-specialty regions
        /// (flotilla / foundry / greenhouse / traplines / settlement) and the
        /// caravan route origin regions (deep_coast / industrial_belt /
        /// ash_flats / settlement). "general" is always included — general
        /// supply goods trade everywhere.
        /// </summary>
        public static IReadOnlyList<string> TagsForOrigin(string originRegion)
        {
            var tags = new List<string> { "general" };
            switch (originRegion ?? string.Empty)
            {
                case "flotilla":
                case "deep_coast":
                    tags.Add("flotilla");
                    tags.Add("coastal");
                    break;
                case "foundry":
                case "industrial_belt":
                    tags.Add("foundry");
                    break;
                case "greenhouse":
                case "ash_flats":
                    tags.Add("greenhouse");
                    break;
                case "traplines":
                    tags.Add("traplines");
                    break;
                default:
                    // Settlements and unknown regions trade the settlement pool.
                    tags.Add("settlement");
                    break;
            }
            return tags;
        }

        /// <summary>True when the origin region produces (or generally trades) the good.</summary>
        public static bool ProducesGood(GoodsCatalog catalog, string originRegion, string goodId)
        {
            if (catalog == null || string.IsNullOrEmpty(goodId)) return false;
            var good = catalog.Find(goodId);
            if (good == null) return false;
            var tags = TagsForOrigin(originRegion);
            for (int i = 0; i < tags.Count; i++)
                if (tags[i] == good.regionalSupply)
                    return true;
            return false;
        }

        /// <summary>
        /// Specialty cargo for a caravan originating in the region: annotated
        /// goods from the production pool, excluding the universal base
        /// staples (clean_water / canned_food / antibiotics — every caravan
        /// carries those). Quantity derives from the good's trade stack size;
        /// priceRations derives from basePrice against the ration anchor
        /// (clean water — the universal measure, per the good's own barter
        /// note). Pool smaller than the cap is returned as-is: specialty
        /// scarcity is data, not padding.
        /// </summary>
        public static List<CaravanCargoEntry> SpecialtyCargoForOrigin(
            GoodsCatalog catalog, string originRegion, int maxLots = 4, ISeededRng? rng = null)
        {
            var result = new List<CaravanCargoEntry>();
            if (catalog == null) return result;

            float rationAnchor = RationAnchor(catalog);
            var tags = TagsForOrigin(originRegion);
            var pool = new List<GoodDefinition>();
            foreach (var good in catalog.All())
            {
                if (IsBaseStaple(good.id)) continue;
                if (!string.IsNullOrEmpty(good.regionalSupply) && tags.Contains(good.regionalSupply))
                    pool.Add(good);
            }
            if (pool.Count == 0) return result;

            int cap = maxLots < 1 ? 1 : maxLots;
            // Deterministic order: ordinal id; seeded shuffle when a stream is supplied.
            if (rng != null)
            {
                for (int i = pool.Count - 1; i > 0; i--)
                {
                    int j = rng.Next(0, i + 1);
                    (pool[i], pool[j]) = (pool[j], pool[i]);
                }
            }

            for (int i = 0; i < pool.Count && result.Count < cap; i++)
            {
                var good = pool[i];
                int quantity = good.stackSize / 2 < 1 ? 1 : System.Math.Min(good.stackSize / 2, 8);
                int priceRations = System.Math.Max(1,
                    (int)System.Math.Round(good.basePrice / rationAnchor, System.MidpointRounding.AwayFromZero));
                result.Add(new CaravanCargoEntry
                {
                    GoodId = good.id,
                    Quantity = quantity,
                    PriceRations = priceRations,
                });
            }
            return result;
        }

        /// <summary>
        /// Settlement-shortage demand scale for a good relative to a region's
        /// production: locally produced goods are buffered (0.5), general
        /// supply tracks the market (1.0), pure imports escalate (1.5). Apply
        /// as a multiplier on any scarcity demand delta targeting the region.
        /// </summary>
        public static float ShortageDemandScale(string regionalSupply, string originRegion)
        {
            var tags = TagsForOrigin(originRegion);
            if (string.IsNullOrEmpty(regionalSupply)) return 1f;
            if (regionalSupply == "general") return 1f;
            for (int i = 0; i < tags.Count; i++)
                if (tags[i] == regionalSupply)
                    return 0.5f;
            return 1.5f;
        }

        /// <summary>
        /// Waystation resupply filter during a market shortage: locally
        /// produced and general goods keep their stock (they can be
        /// resupplied from the region), pure imports lapse until the market
        /// recovers. Item-space ids that are not economy goods at all are
        /// always kept — they are outside the market model.
        /// </summary>
        public static List<string> FilterStockForShortage(
            IReadOnlyList<string> stockItemIds, GoodsCatalog catalog, string originRegion)
        {
            var kept = new List<string>();
            if (stockItemIds == null) return kept;
            var tags = TagsForOrigin(originRegion);

            for (int i = 0; i < stockItemIds.Count; i++)
            {
                var id = stockItemIds[i];
                var good = catalog != null ? catalog.Find(id) : null;
                if (good == null)
                {
                    kept.Add(id); // item-space stock — outside the market model
                    continue;
                }
                if (string.IsNullOrEmpty(good.regionalSupply) || good.regionalSupply == "general")
                {
                    kept.Add(id); // general supply rides through
                    continue;
                }
                bool local = false;
                for (int t = 0; t < tags.Count; t++)
                    if (tags[t] == good.regionalSupply) { local = true; break; }
                if (local) kept.Add(id);
                // pure imports lapse during shortage — omitted
            }
            return kept;
        }

        /// <summary>
        /// The ration anchor: clean water's base price. "Every faction prices
        /// its offers against the litre" — one ration equals one water.
        /// Falls back to 8 when the anchor good is missing.
        /// </summary>
        public static float RationAnchor(GoodsCatalog catalog)
        {
            var water = catalog != null ? catalog.Find("clean_water") : null;
            return water != null && water.basePrice > 0f ? water.basePrice : 8f;
        }

        private static bool IsBaseStaple(string goodId)
        {
            return goodId == "clean_water" || goodId == "canned_food" || goodId == "antibiotics";
        }
    }

    /// <summary>One provenance-derived caravan cargo lot (Plan 56 phase 3).</summary>
    public sealed class CaravanCargoEntry
    {
        public string GoodId = string.Empty;
        public int Quantity;
        public int PriceRations;
    }
}
