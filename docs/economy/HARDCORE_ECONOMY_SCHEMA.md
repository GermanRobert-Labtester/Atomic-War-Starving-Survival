# Hardcore Economy Schema Specification

## 1. Document Structure

The canonical data file `Assets/StreamingAssets/Data/hardcore_economy_tuning.json` adheres to the root shape mapped by `HardcoreEconomyTuningDto.cs`:

```json
{
  "schema_version": 1,
  "version": 1,
  "scarcity_tiers": [ ... ],
  "faction_preferences": [ ... ],
  "price_shock_rules": [ ... ]
}
```

---

## 2. Field Specifications

### 2.1 `scarcity_tiers[]` (`ScarcityEntryDto`)
- `tier` (string, required): Enum name matching `ScarcityTier` (e.g. `Critical`, `High`, `Moderate`, `Stable`, `Reconstruction`, `LateScarcity`, `DeepWinter`, `Endgame`). Case-insensitive.
- `multiplier` (float, required): Multiplier applied to base trade value. Must be strictly `> 0.0`.
- `day_range_label` (string, required): Formatted day interval string (e.g. `Days 1-15`, `Days 41-100`, `Days 341+`, or `1-10`).
- `affected_item_ids` (string[], required): Non-empty array of item IDs, wildcard prefixes (e.g. `ammo_*`), or `*` catch-all.
- `rationale` (string, required): Non-empty narrative justification of the economic conditions.

### 2.2 `faction_preferences[]` (`FactionTradePreferenceDto`)
- `faction_id` (string, required): Unique canonical faction ID (e.g. `central_garrison_remnants`, `faction_the_scale`).
- `buys_at_premium` (string[], required): Non-empty array of item IDs or prefixes that the faction prioritizes and values highly.
- `refuses` (string[], required): Non-empty array of item IDs or prefixes that the faction will not buy or accept in trade.
- `trade_currency` (string, required): Grounded in-world prose describing the faction's recognized medium of exchange.

### 2.3 `price_shock_rules[]` (`PriceShockRuleDto`)
- `kind` (string, required): Enum name matching `PriceShockKind` (`PlumePassing`, `ConvoyAmbush`, `FactionConflict`, `SeasonalScarcity`, `DiseaseOutbreak`, `FuelShortage`). Case-insensitive.
- `multiplier` (float, required): Multiplier applied during the shock. Must be strictly `> 0.0`.
- `duration_days` (int, required): Duration of shock in game days. Must be `>= 0`.
- `affected_item_ids` (string[], required): Array of item IDs or `*` for universal commodity spikes.
- `trigger` (string, required): In-world event description explaining the cause of the market disruption.
