using System;
using System.Collections.Generic;
using Ashfall.Core.Narrative;

namespace Ashfall.Core
{
    /// <summary>
    /// Plan 25 · 25G.7 — bridges the canonical narrative treaty corpora
    /// (narrative/regional_treaty_protocols.json, read-model
    /// <see cref="RegionalTreatyEntry"/>) into mechanical
    /// <see cref="TreatyDefinition"/>s so the host's
    /// RegionalTreatySystem finally ships a catalog. Balance mapping is
    /// deliberately uniform: ratification costs a flat 10 scrap, effects
    /// carry the authored water (lpm) and power (kw) quotas verbatim, and the
    /// historical ratified_day is preserved as the mechanical ratification day.
    /// Deterministic: entry order preserved; no RNG.
    /// <para>Plan VIII · Task 21 — each definition also carries its full
    /// signatory list, an optional <c>term_days</c> term, and tag-derived world
    /// effects: tags naming security/peace accords map to
    /// <see cref="TreatyEffectKind.RaidPressureRelief"/>, tags naming trade/market
    /// charters map to <see cref="TreatyEffectKind.TradeDiscount"/> (values are the
    /// <see cref="TreatyEffectTable"/> defaults; authored data stays prose, the
    /// mapping stays the single interpretation point).</para>
    /// </summary>
    public static class RegionalTreatyFeed
    {
        public const float FlatRatificationCostScrap = 10f;

        private static readonly string[] SecurityTags = { "security", "peace", "sky_defense" };
        private static readonly string[] TradeTags = { "trade", "economy", "market", "barter" };

        public static List<TreatyDefinition> Map(
            IReadOnlyList<RegionalTreatyEntry> entries)
        {
            var result = new List<TreatyDefinition>();
            if (entries == null) return result;
            for (int i = 0; i < entries.Count; i++)
            {
                var e = entries[i];
                if (e == null || string.IsNullOrEmpty(e.treaty_id)) continue;
                var def = new TreatyDefinition
                {
                    treaty_id = e.treaty_id,
                    display_name = e.treaty_title ?? e.treaty_id,
                    faction_id = (e.signatory_factions != null && e.signatory_factions.Length > 0)
                        ? e.signatory_factions[0]
                        : string.Empty,
                    description = ComposeDescription(e),
                    ratification_cost_scrap = FlatRatificationCostScrap,
                    ratification_cost_day = e.ratified_day,
                    compliance_check_interval_days = 30f,
                    violation_penalty_affinity = -20f,
                    term_days = e.term_days
                };
                if (e.signatory_factions != null)
                    def.signatory_factions.AddRange(e.signatory_factions);
                if (e.water_allocation_lpm > 0)
                    def.effects.Add(new TreatyEffect
                    {
                        effect_type = "water_quota",
                        target_id = string.IsNullOrEmpty(e.demarcated_territory) ? e.treaty_id : e.demarcated_territory,
                        value = e.water_allocation_lpm
                    });
                if (e.power_quota_kw > 0)
                    def.effects.Add(new TreatyEffect
                    {
                        effect_type = "power",
                        target_id = string.IsNullOrEmpty(e.demarcated_territory) ? e.treaty_id : e.demarcated_territory,
                        value = e.power_quota_kw
                    });
                if (HasAnyTag(e, SecurityTags))
                    def.effects.Add(new TreatyEffect
                    {
                        effect_type = "raid_pressure_relief",
                        target_id = e.treaty_id,
                        value = TreatyEffectTable.DefaultRaidPressureRelief
                    });
                if (HasAnyTag(e, TradeTags))
                    def.effects.Add(new TreatyEffect
                    {
                        effect_type = "economy_discount",
                        target_id = e.treaty_id,
                        value = TreatyEffectTable.DefaultTradeDiscount
                    });
                result.Add(def);
            }
            return result;
        }

        private static bool HasAnyTag(RegionalTreatyEntry e, string[] wanted)
        {
            if (e.tags == null) return false;
            for (int i = 0; i < e.tags.Length; i++)
                for (int j = 0; j < wanted.Length; j++)
                    if (string.Equals(e.tags[i], wanted[j], StringComparison.OrdinalIgnoreCase))
                        return true;
            return false;
        }

        private static string ComposeDescription(RegionalTreatyEntry e)
        {
            var text = (e.treaty_articles ?? string.Empty);
            if (text.Length > 400) text = text.Substring(0, 400) + "…";
            if (!string.IsNullOrEmpty(e.penalties))
                text += "\nPenalties: " + e.penalties;
            return text;
        }
    }
}
