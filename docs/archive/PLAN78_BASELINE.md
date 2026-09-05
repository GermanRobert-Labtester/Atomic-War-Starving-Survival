# Plan 78 — Archive Inks Baseline Reconnaissance

> **Status:** Grounded baseline inspection completed 2026-09-03.
> **Authority:** `Assets/StreamingAssets/Data/archive_inks.json`, `Assets/Ashfall.Core/ArchiveInkCatalogLoader.cs`, `Assets/Ashfall.Core/ArchiveDeskSystem.cs`, `src/Host/ArchiveDeskHostSession.cs`.

---

## 1. Executive Summary

`Assets/StreamingAssets/Data/archive_inks.json` contains 12 definitions preserving the 3 original baseline inks (`ink_iron_gall`, `ink_soot_lamp`, `ink_plant_dye`) along with 9 distinct formulations covering improvised, standard, and archival tiers.

```json
{
  "schema_version": 1,
  "collection_id": "archive_inks",
  "inks": [ ... ]
}
```

---

## 2. Codebase Forensics & Field Consumers

1. **`InkMaterialDefinition` (`Assets/Ashfall.Core/ArchiveDeskSystem.cs`):**
   - `ink_id`: unique snake_case string with `ink_` prefix.
   - `display_name`: human-readable presentation string.
   - `legibility_score` / `legibilityScore`: transcription legibility multiplier `[0.3, 1.0]`.
   - `archival_longevity_days` / `archivalLongevityDays`: days of preservation durability before total degradation `[50, 1000]`.
   - `fade_rate_per_day` / `fadeRatePerDay`: linear degradation rate per campaign day `[0.0005, 0.02]`.
   - `required_item_id` / `requiredItemId`: foreign key resolving into `items.json`.
   - `required_amount` / `requiredAmount`: positive integer ingredient requirement `[1, 5]`.

2. **Deserialization Bug Repaired:**
   - Forensic inspection revealed that `InkMaterialDefinition` in `ArchiveDeskSystem.cs` previously lacked `[JsonPropertyName]` mappings for snake_case JSON keys (`legibility_score`, `archival_longevity_days`, `fade_rate_per_day`, `required_item_id`, `required_amount`).
   - Added `[JsonPropertyName]` mappings and bidirectional accessor aliases in `Assets/Ashfall.Core/ArchiveDeskSystem.cs`, ensuring JSON values populate correctly into the Core system and Host session.

3. **Original 3 Inks Preserved:**
   - `ink_iron_gall`: Legibility 0.90, Longevity 500d, Fade 0.0008/d, 2 charcoal
   - `ink_soot_lamp`: Legibility 0.70, Longevity 300d, Fade 0.0015/d, 1 charcoal
   - `ink_plant_dye`: Legibility 0.60, Longevity 200d, Fade 0.0020/d, 1 cloth
