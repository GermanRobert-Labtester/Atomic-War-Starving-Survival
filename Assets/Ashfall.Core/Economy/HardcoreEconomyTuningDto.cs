namespace Ashfall.Core.Economy
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    /// <summary>Mutable DTO for scarcity-tier deserialization.</summary>
    public sealed class ScarcityEntryDto
    {
        [JsonPropertyName("tier")]
        public string Tier { get; set; } = "Moderate";

        [JsonPropertyName("multiplier")]
        public float Multiplier { get; set; } = 1.0f;

        [JsonPropertyName("day_range_label")]
        public string DayRangeLabel { get; set; } = string.Empty;

        [JsonPropertyName("affected_item_ids")]
        public List<string> AffectedItemIds { get; set; } = new();

        [JsonPropertyName("rationale")]
        public string Rationale { get; set; } = string.Empty;
    }

    /// <summary>Mutable DTO for faction trade preference deserialization.</summary>
    public sealed class FactionTradePreferenceDto
    {
        [JsonPropertyName("faction_id")]
        public string FactionId { get; set; } = string.Empty;

        [JsonPropertyName("buys_at_premium")]
        public List<string> BuysAtPremium { get; set; } = new();

        [JsonPropertyName("refuses")]
        public List<string> Refuses { get; set; } = new();

        [JsonPropertyName("trade_currency")]
        public string TradeCurrency { get; set; } = string.Empty;
    }

    /// <summary>Mutable DTO for price-shock rule deserialization.</summary>
    public sealed class PriceShockRuleDto
    {
        [JsonPropertyName("kind")]
        public string Kind { get; set; } = "PlumePassing";

        [JsonPropertyName("multiplier")]
        public float Multiplier { get; set; } = 1.0f;

        [JsonPropertyName("duration_days")]
        public int DurationDays { get; set; } = 1;

        [JsonPropertyName("affected_item_ids")]
        public List<string> AffectedItemIds { get; set; } = new();

        [JsonPropertyName("trigger")]
        public string Trigger { get; set; } = string.Empty;
    }

    /// <summary>Root DTO for the tuning JSON document.</summary>
    public sealed class HardcoreEconomyTuningDto
    {
        [JsonPropertyName("version")]
        public int Version { get; set; } = 1;

        [JsonPropertyName("scarcity_tiers")]
        public List<ScarcityEntryDto> ScarcityTiers { get; set; } = new();

        [JsonPropertyName("faction_preferences")]
        public List<FactionTradePreferenceDto> FactionPreferences { get; set; } = new();

        [JsonPropertyName("price_shock_rules")]
        public List<PriceShockRuleDto> PriceShockRules { get; set; } = new();
    }
}
