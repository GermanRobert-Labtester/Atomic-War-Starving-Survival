# ASHFALL — snake_case Migration Tracker (Plans 47+)

**Policy:** spelling-only migration. ID values, schema versions and value
semantics are never modified. Canonical snake_case is the wire standard;
legacy camelCase remains accepted through `CatalogKeyNormalizer`
(`Assets/Ashfall.Core/IO/CatalogKeyNormalizer.cs`) until the explicit alias
removal cleanup wave. Conflicting dual spellings fail loudly — never an
arbitrary winner.

**Companion survey:** `artifacts/snake-case-survey.json` (regenerate with
`python3 scripts/tools/snake_case_survey.py`).
**Conversion tool:** `scripts/tools/convert_snake_case_keys.py` +
`scripts/tools/snake_case_aliases.json`.

## Status vocabulary

| Status | Meaning |
|---|---|
| `UNSURVEYED` | present in survey artifact, no mapping decided |
| `CANDIDATE` | ranked for a future wave |
| `ALIASED` | loader dual-read in place, JSON not yet converted |
| `MIGRATED` | JSON converted to snake_case, aliases retained |
| `VERIFIED` | MIGRATED + dual-read tests + integrity + utilization green |
| `LEGACY_ALIAS_PENDING_REMOVAL` | cleanup-wave scope (deferred by policy) |

## Wave 1 (Plan 47) — COMPLETE

| Path | Old keys | Canonical keys | Loader | Consumers | Alias support | JSON migrated | Tests | Integrity | Utilization | Checksum | Cleanup wave |
|---|---|---|---|---|---|---|---|---|---|---|---|
| chemical_syntheses.json | displayName, requiredApparatusTier, inputItems, outputItems, processingTicks, heatBand, volatilityRating, scrubberDemand, equipmentWear, corrosionRating, skillRequirement | display_name, required_apparatus_tier, input_items, output_items, processing_ticks, heat_band, volatility_rating, scrubber_demand, equipment_wear, corrosion_rating, skill_requirement | ChemicalSynthesisCatalogLoader | ChemicalSynthesisSystem | CatalogKeyNormalizer (retained) | yes | SnakeCaseWave1Tests | 0 errors | unchanged | DTO checksum equal legacy/canonical | Wave 5+ |
| mineral_acid_synthesis_catalog.json | (same family as chemical_syntheses) | (same) | ChemicalSynthesisCatalogLoader | ChemicalSynthesisSystem | CatalogKeyNormalizer (retained) | yes | SnakeCaseWave1Tests | 0 errors | unchanged | DTO checksum equal legacy/canonical | Wave 5+ |
| hydroponic_crops.json | displayName, germinationTicks, growthTicks, waterLitresPerDay, nutrientUnitsPerDay, ledPowerWatts, preferredSpectrum, baseYieldItemId, baseYieldQuantity, coldTolerance, contaminationTolerance, mutationAffinity | display_name, germination_ticks, growth_ticks, water_litres_per_day, nutrient_units_per_day, led_power_watts, preferred_spectrum, base_yield_item_id, base_yield_quantity, cold_tolerance, contamination_tolerance, mutation_affinity | HydroponicCropCatalogLoader | HydroponicsShelterSystem | CatalogKeyNormalizer (retained) | yes | SnakeCaseWave1Tests | 0 errors | unchanged | n/a (definitions not saved) | Wave 5+ |
| nuclear_core_profiles.json | displayName, powerClass, baseElectricalOutput, thermalClass, radiationClass, coolingDemand, shieldingRequirement, wearRate, decayClass, emergencyShutdownItemId | display_name, power_class, base_electrical_output, thermal_class, radiation_class, cooling_demand, shielding_requirement, wear_rate, decay_class, emergency_shutdown_item_id | NuclearCoreCatalogLoader | NuclearCore lifecycle systems | CatalogKeyNormalizer (retained) | yes | SnakeCaseWave1Tests | 0 errors | unchanged | n/a (definitions not saved) | Wave 5+ |
| duty_roster_seasons.json | windowMinDays, windowMaxDays, encounterWeight, steamTripChanceBoost | window_min_days, window_max_days, encounter_weight, steam_trip_chance_boost | DutyRosterCatalogLoader (seasons path) | DutyRosterSystem | CatalogKeyNormalizer (retained) | yes | SnakeCaseWave1Tests | 0 errors | unchanged | n/a (definitions not saved) | Wave 5+ |
| armored_crawler_modules.json | displayName, slotType, powerDraw, crewBerths, armorModifier, fuelModifier, cargoModifier, workshopCapability, lifeSupportModifier | display_name, slot_type, power_draw, crew_berths, armor_modifier, fuel_modifier, cargo_modifier, workshop_capability, life_support_modifier | ArmoredCrawlerModuleCatalogLoader | ArmoredCrawlerExpedition | CatalogKeyNormalizer (retained) | yes | SnakeCaseWave1Tests | 0 errors | unchanged | n/a (definitions not saved) | Wave 5+ |

Verification at wave close: 10787/10787 core tests PASS,
`--data-integrity-selftest` 0 errors across 299 catalogs,
semantic-identity proof (HEAD-vs-working JSON compared under alias rename)
for all six files.

## Old-spelling sweep disposition (Phase 13F)

Live reads of the six wave-1 key families are confined to:
- the migrated loaders themselves (alias tables + `CatalogKeyNormalizer` calls);
- `Ashfall.Core.Tests/Tooling/JsonNamingMixPinTests.cs` — pin list updated;
  the two pinned chemical catalogs were removed under the test's own
  migration clause;
- `scripts/ci/generate-architecture-map.py` / `src/Host/ContentUtilizationRuntimeCollector.cs`
  — file-name references only, no wire-key reads.

Remaining camelCase wire keys elsewhere in the tree belong to catalogs that
are **not** in wave 1; they are tracked below and remain explained scope.

## Wave 2 candidates (risk-ranked from artifacts/snake-case-survey.json)

1. `trade_screen_scenarios.json` (8.8) — 2 keys, but `$schema` + PascalCase
   scenario keys; verify loader style first.
2. `decontamination_protocol_catalog.json` (16.8) — 1 key
   (`interlock_threshold_mSv_per_h` already snake — remaining violations are
   unit-suffixed keys; confirm exact mapping before aliasing).
3. `quests_bureaucratic_morality.json` (17.6) — 2 keys, 0 consumer files
   matched; confirm it is loaded before migrating.
4. `agriculture_items.json` (55.2) — 3 keys, loads through the shared
   ItemCatalogLoader family; migrate together with the item-family wave.
5. `duty_roster_seasons.json`-style small catalogs already done; next family:
   `audio_logs_expansion_05.json` (126.0, 1 key `bodyText`).
6. Larger families (`radio.json`, `echoes.json`, `events.json`,
   `expeditions.json`, `survivors.json`) require their own dedicated waves —
   note that `echoes.json` and `events.json` already contain genuine dual-key
   drift (`minDay` AND `MinDay`): migrating those catalogs must first decide
   whether the dual spellings conflict per-object (the Core normalizer will
   fail loudly if they do).
