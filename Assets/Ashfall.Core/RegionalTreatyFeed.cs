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
    /// </summary>
    public static class RegionalTreatyFeed
    {
        public const float FlatRatificationCostScrap = 10f;

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
                    violation_penalty_affinity = -20f
                };
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
                result.Add(def);
            }
            return result;
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
