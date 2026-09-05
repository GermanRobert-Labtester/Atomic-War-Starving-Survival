using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>
    /// Plan VIII · Task 21 — typed treaty consequence contract.
    /// A ratified/broken/expired treaty is not just a ledger row: its world
    /// consequences are described by <see cref="TreatyActiveEffect"/> descriptors
    /// that consumers (economy, raid pressure, broadcast) pull from
    /// <see cref="RegionalTreatySystem"/> each read. Effects are derived from
    /// treaty status, never granted-and-persisted, so save/restore can never
    /// double-apply them.
    /// </summary>
    public enum TreatyEffectKind
    {
        /// <summary>Caravan buy-price relief for the pact partner (fraction, e.g. 0.10 = −10%).</summary>
        TradeDiscount,
        /// <summary>Supply-price relief (fraction) while ratified.</summary>
        SupplyPriceRelief,
        /// <summary>Raid-pressure relief while ratified (subtractive modifier on raid chance).</summary>
        RaidPressureRelief,
        /// <summary>Informational water quota (lpm) — displayed, no world consumer yet.</summary>
        WaterQuota,
        /// <summary>Informational power quota (kW) — displayed, no world consumer yet.</summary>
        PowerQuota
    }

    /// <summary>Why a treaty left its ratified state. Betrayal and compliance
    /// failure both apply breach consequences; expiry does not.</summary>
    public enum TreatyViolationCause
    {
        None,
        /// <summary>Player-initiated breach (<see cref="RegionalTreatySystem.BreakTreaty"/>).</summary>
        Betrayal,
        /// <summary>Compliance decay reached zero on the interval check.</summary>
        ComplianceFailure
    }

    /// <summary>One active world consequence of a ratified treaty (or one that
    /// just ended). <see cref="SourceId"/> is the stable add/remove identity:
    /// treaty:{treatyId}:effect:{kind}.</summary>
    [Serializable]
    public sealed class TreatyActiveEffect
    {
        public string TreatyId = string.Empty;
        public string FactionId = string.Empty;
        public TreatyEffectKind Kind;
        public string TargetId = string.Empty;
        public float Value;
        public string SourceId = string.Empty;

        public static string MakeSourceId(string treatyId, TreatyEffectKind kind) =>
            $"treaty:{treatyId}:effect:{kind.ToString().ToLowerInvariant()}";
    }

    /// <summary>Typed transition record emitted after the treaty state has
    /// mutated. Never emitted during RestoreState — consumers must be able to
    /// treat transitions as exactly-once per lifecycle change.</summary>
    [Serializable]
    public sealed class TreatyTransition
    {
        public string TreatyId = string.Empty;
        public string FactionId = string.Empty;
        public TreatyStatus From;
        public TreatyStatus To;
        public int Day;
        public TreatyViolationCause Cause = TreatyViolationCause.None;
        /// <summary>Effects that were active before the transition and ended with it.</summary>
        public List<TreatyActiveEffect> EndedEffects = new List<TreatyActiveEffect>();
        /// <summary>Effects that became active with the transition (ratification).</summary>
        public List<TreatyActiveEffect> StartedEffects = new List<TreatyActiveEffect>();
        public bool IsBreach => Cause != TreatyViolationCause.None;
    }

    /// <summary>Maps legacy data <see cref="TreatyEffect.effect_type"/> strings
    /// onto the typed contract. The data authority keeps its authored strings;
    /// this is the single sanctioned interpretation point.</summary>
    public static class TreatyEffectTable
    {
        public const float DefaultTradeDiscount = 0.10f;
        public const float DefaultSupplyPriceRelief = 0.10f;
        public const float DefaultRaidPressureRelief = 0.05f;
        /// <summary>Added raid chance per treaty in Violated state (breach consequence).</summary>
        public const float BreachRaidPressure = 0.15f;
        /// <summary>Symmetric clamp on the aggregate raid-pressure modifier.</summary>
        public const float RaidPressureModifierClamp = 0.5f;

        public static bool TryMapKind(string effectType, out TreatyEffectKind kind, out float fallbackValue)
        {
            switch (effectType)
            {
                case "economy_discount":
                case "trade_discount":
                    kind = TreatyEffectKind.TradeDiscount; fallbackValue = DefaultTradeDiscount; return true;
                case "supply_relief":
                case "supply_price_relief":
                    kind = TreatyEffectKind.SupplyPriceRelief; fallbackValue = DefaultSupplyPriceRelief; return true;
                case "raid_pressure_relief":
                case "security":
                    kind = TreatyEffectKind.RaidPressureRelief; fallbackValue = DefaultRaidPressureRelief; return true;
                case "water_quota":
                    kind = TreatyEffectKind.WaterQuota; fallbackValue = 0f; return true;
                case "power":
                    kind = TreatyEffectKind.PowerQuota; fallbackValue = 0f; return true;
                default:
                    kind = TreatyEffectKind.TradeDiscount; fallbackValue = 0f; return false;
            }
        }
    }

    /// <summary>Plan VIII · Task 21.6 — plain-language radio copy for typed treaty
    /// transitions. Cold, exhausted, restrained: facts only, no faction ids, no
    /// moralizing. The host injects the returned line through
    /// RadioScheduleCoordinator.InjectTreatyAlert; tests pin the composition.</summary>
    public static class TreatyBulletins
    {
        public static string Compose(TreatyTransition transition, TreatyDefinition? definition)
        {
            string title = definition?.display_name;
            if (string.IsNullOrEmpty(title)) title = transition.TreatyId;
            string consequences = SummarizeEffects(
                transition.IsBreach ? transition.EndedEffects : FirstNonEmpty(transition.StartedEffects, transition.EndedEffects),
                transition.IsBreach);
            return transition.Cause switch
            {
                TreatyViolationCause.Betrayal =>
                    $"{title}: broken. {consequences} The signatories will remember.",
                TreatyViolationCause.ComplianceFailure =>
                    $"{title}: obligations unmet too long. {consequences}",
                _ when transition.To == TreatyStatus.Expired =>
                    $"{title}: term served. {consequences}",
                _ when transition.To == TreatyStatus.Ratified =>
                    $"{title}: ratified. {consequences}",
                _ => $"{title}: status now {transition.To}. {consequences}"
            };
        }

        private static IReadOnlyList<TreatyActiveEffect> FirstNonEmpty(
            IReadOnlyList<TreatyActiveEffect> first, IReadOnlyList<TreatyActiveEffect> second) =>
            first.Count > 0 ? first : second;

        private static string SummarizeEffects(IReadOnlyList<TreatyActiveEffect> effects, bool ended)
        {
            if (effects.Count == 0) return string.Empty;
            var parts = new List<string>();
            for (int i = 0; i < effects.Count; i++)
            {
                var e = effects[i];
                string tail = ended ? " no longer holds" : " while the accord holds";
                switch (e.Kind)
                {
                    case TreatyEffectKind.TradeDiscount:
                        parts.Add($"caravan prices {(ended ? "back to full tariff" : $"ease {FormatPct(e.Value)}")}");
                        break;
                    case TreatyEffectKind.SupplyPriceRelief:
                        parts.Add($"supply prices {(ended ? "back to full rate" : $"ease {FormatPct(e.Value)}")}");
                        break;
                    case TreatyEffectKind.RaidPressureRelief:
                        parts.Add(ended ? "the roads are less patient now" : "raiders keep their distance");
                        break;
                    case TreatyEffectKind.WaterQuota:
                        parts.Add($"{e.Value:0} liters a day guaranteed" + (ended ? "" : tail));
                        break;
                    case TreatyEffectKind.PowerQuota:
                        parts.Add($"{e.Value:0} kilowatts allotted" + (ended ? "" : tail));
                        break;
                }
            }
            return parts.Count > 0 ? string.Join("; ", parts) + "." : string.Empty;
        }

        private static string FormatPct(float fraction) =>
            $"{MathF.Round(fraction * 100f):0}%";
    }
}
