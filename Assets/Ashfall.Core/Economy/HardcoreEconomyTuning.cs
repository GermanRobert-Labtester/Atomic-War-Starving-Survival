namespace Ashfall.Core.Economy
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Scarcity-tier entry: applies a multiplier to a bucket of item ids
    /// for a given day range.
    /// </summary>
    public readonly struct ScarcityEntry
    {
        public ScarcityTier Tier { get; }
        public float Multiplier { get; }
        public string DayRangeLabel { get; }
        public IReadOnlyList<string> AffectedItemIds { get; }
        public string Rationale { get; }

        public ScarcityEntry(ScarcityTier tier, float multiplier, string dayRangeLabel,
            IEnumerable<string> affectedItemIds, string rationale)
        {
            if (multiplier <= 0f) throw new ArgumentOutOfRangeException(nameof(multiplier));
            Tier = tier;
            Multiplier = multiplier;
            DayRangeLabel = dayRangeLabel ?? string.Empty;
            AffectedItemIds = affectedItemIds is null ? Array.Empty<string>() : new List<string>(affectedItemIds);
            Rationale = rationale ?? string.Empty;
        }
    }

    /// <summary>
    /// Faction trade preference override.
    /// </summary>
    public readonly struct FactionTradePreference
    {
        public string FactionId { get; }
        public IReadOnlyList<string> BuysAtPremium { get; }
        public IReadOnlyList<string> Refuses { get; }
        public string TradeCurrency { get; }

        public FactionTradePreference(string factionId, IEnumerable<string> buysAtPremium,
            IEnumerable<string> refuses, string tradeCurrency)
        {
            FactionId = factionId ?? string.Empty;
            BuysAtPremium = buysAtPremium is null ? Array.Empty<string>() : new List<string>(buysAtPremium);
            Refuses = refuses is null ? Array.Empty<string>() : new List<string>(refuses);
            TradeCurrency = tradeCurrency ?? string.Empty;
        }
    }

    /// <summary>
    /// Transient price-shock rule: raises prices for a duration.
    /// </summary>
    public readonly struct PriceShockRule
    {
        public PriceShockKind Kind { get; }
        public float Multiplier { get; }
        public int DurationDays { get; }
        public IReadOnlyList<string> AffectedItemIds { get; }
        public string Trigger { get; }

        public PriceShockRule(PriceShockKind kind, float multiplier, int durationDays,
            IEnumerable<string> affectedItemIds, string trigger)
        {
            if (multiplier <= 0f) throw new ArgumentOutOfRangeException(nameof(multiplier));
            if (durationDays < 0) throw new ArgumentOutOfRangeException(nameof(durationDays));
            Kind = kind;
            Multiplier = multiplier;
            DurationDays = durationDays;
            AffectedItemIds = affectedItemIds is null ? Array.Empty<string>() : new List<string>(affectedItemIds);
            Trigger = trigger ?? string.Empty;
        }
    }

    /// <summary>
    /// Loaded tuning bundle: the validated, immutable output of the loader.
    /// </summary>
    public sealed class HardcoreEconomyTuningBundle
    {
        public IReadOnlyList<ScarcityEntry> ScarcityTiers { get; private set; }
        public IReadOnlyList<FactionTradePreference> FactionPreferences { get; private set; }
        public IReadOnlyList<PriceShockRule> PriceShockRules { get; private set; }

        public HardcoreEconomyTuningBundle()
        {
            // Default-constructed bundles must be empty, never null-backed:
            // IsActive and every lookup iterate these lists.
            ScarcityTiers = Array.Empty<ScarcityEntry>();
            FactionPreferences = Array.Empty<FactionTradePreference>();
            PriceShockRules = Array.Empty<PriceShockRule>();
        }

        public HardcoreEconomyTuningBundle(IReadOnlyList<ScarcityEntry> scarcityTiers,
            IReadOnlyList<FactionTradePreference> factionPreferences,
            IReadOnlyList<PriceShockRule> priceShockRules)
        {
            ScarcityTiers = scarcityTiers ?? Array.Empty<ScarcityEntry>();
            FactionPreferences = factionPreferences ?? Array.Empty<FactionTradePreference>();
            PriceShockRules = priceShockRules ?? Array.Empty<PriceShockRule>();
        }
    }

    /// <summary>
    /// Engine-agnostic Hardcore Economy Tuning overlay.
    ///
    /// This is the **One Source of Truth** for hardcore tuning data.
    /// It is **empty by default** — all lookups return 1.0f / no-op — until a
    /// host calls <see cref="Apply(HardcoreEconomyTuningBundle)"/> with a loaded
    /// tuning bundle. The host passes itself in so the overlay can query day/item
    /// context without owning game logic.
    /// </summary>
    public sealed class HardcoreEconomyTuning
    {
        private HardcoreEconomyTuningBundle _bundle = new();

        /// <summary>True when a tuning bundle has been loaded.</summary>
        public bool IsActive => _bundle.ScarcityTiers.Count > 0
            || _bundle.FactionPreferences.Count > 0
            || _bundle.PriceShockRules.Count > 0;

        /// <summary>
        /// Apply a loaded tuning bundle. Safe to call multiple times; last call wins.
        /// </summary>
        public void Apply(HardcoreEconomyTuningBundle bundle)
        {
            if (bundle is null) throw new ArgumentNullException(nameof(bundle));
            _bundle = bundle;
        }

        /// <summary>
        /// Returns the scarcity multiplier for the given day + item id, or 1.0f if
        /// no scarcity tier applies.
        /// </summary>
        public float GetScarcityMultiplier(int currentDay, string itemId)
        {
            if (!IsActive || string.IsNullOrEmpty(itemId)) return 1.0f;
            foreach (var entry in _bundle.ScarcityTiers)
            {
                if (MatchesDay(entry, currentDay) && MatchesItem(entry.AffectedItemIds, itemId))
                    return entry.Multiplier;
            }
            return 1.0f;
        }

        /// <summary>
        /// Tries to find a faction trade preference. Returns false when no tuning
        /// is loaded or the faction has no override.
        /// </summary>
        public bool TryGetFactionPreference(string factionId, out FactionTradePreference preference)
        {
            preference = default;
            if (!IsActive || string.IsNullOrEmpty(factionId)) return false;
            foreach (var f in _bundle.FactionPreferences)
            {
                if (string.Equals(f.FactionId, factionId, StringComparison.OrdinalIgnoreCase))
                {
                    preference = f;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Tries to find an active price shock for the given kind and day offset
        /// from the shock's start. Returns false when no tuning is loaded.
        /// </summary>
        public bool TryGetPriceShock(PriceShockKind kind, int dayOffsetFromShockStart, out PriceShockRule rule)
        {
            rule = default;
            if (!IsActive) return false;
            foreach (var s in _bundle.PriceShockRules)
            {
                if (s.Kind == kind && dayOffsetFromShockStart >= 0 && dayOffsetFromShockStart < s.DurationDays)
                {
                    rule = s;
                    return true;
                }
            }
            return false;
        }

        /// <summary>Day-range match: supports "Days X-Y" and "Days X+" patterns.</summary>
        private static bool MatchesDay(ScarcityEntry entry, int currentDay)
        {
            if (currentDay < 1) return false;
            var label = entry.DayRangeLabel ?? string.Empty;
            var parts = label.Split(new[] { ' ', '-', '+' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2 && int.TryParse(parts[^2], out var start))
            {
                if (label.Contains('+', StringComparison.Ordinal))
                    return currentDay >= start;
                if (parts.Length >= 3 && int.TryParse(parts[^1], out var end))
                    return currentDay >= start && currentDay <= end;
                return currentDay >= start;
            }
            return false;
        }

        /// <summary>Exact token match (no substring false positives).</summary>
        private static bool MatchesItem(IReadOnlyList<string> affectedIds, string itemId)
        {
            if (affectedIds.Count == 0) return true; // empty list = all items
            foreach (var token in affectedIds)
            {
                var trimmed = token.Trim();
                if (trimmed == "*" || string.Equals(trimmed, itemId, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }

    /// <summary>
    /// Load result for Hardcore Economy Tuning JSON.
    /// </summary>
    public sealed class HardcoreEconomyTuningLoadResult
    {
        public bool IsValid { get; private set; }
        public List<string> Errors { get; private set; } = new();
        public HardcoreEconomyTuningBundle? Bundle { get; private set; }

        public static HardcoreEconomyTuningLoadResult Success(HardcoreEconomyTuningBundle bundle)
            => new() { IsValid = true, Bundle = bundle };

        public static HardcoreEconomyTuningLoadResult Failure(IEnumerable<string> errors)
            => new() { IsValid = false, Errors = new List<string>(errors) };
    }
}
