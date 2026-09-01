# Plan 46 — Location-Specific Scavenging Tables: Closeout Report

## 1. Executive Summary
Plan 46 introduces authoritative, location-specific, weighted scavenging loot ecologies to ASHFALL. Generic loot category rolls have been upgraded to deterministic weighted tables representing 20 distinct wasteland location types.

---

## 2. Deliverables Summary
1. **Authoritative JSON Catalog (`Assets/StreamingAssets/Data/scavenging_tables.json`):**
   - 20 location-type loot tables (`table_loot_hospital`, `table_loot_rail_yard`, `table_loot_school`, `table_loot_military_depot`, `table_loot_apartment_block`, `table_loot_fire_station`, `table_loot_metro_station`, `table_loot_police_station`, `table_loot_industrial_district`, `table_loot_shopping_center`, `table_loot_power_substation`, `table_loot_chemical_plant`, `table_loot_warehouse`, `table_loot_farm`, `table_loot_forestry_compound`, `table_loot_hunting_cabin`, `table_loot_monastery`, `table_loot_clinic`, `table_loot_observatory`, `table_loot_greenhouse`).
   - All entry `item_id`s resolve against validated active items in `items.json`.
   - Depletion models (`finite` vs `renewable`) and hazard probabilities (`radiation`, `disease`, `chemical`) specified.
2. **Core Catalog Domain Loader (`Assets/Ashfall.Core/Expeditions/ScavengingTableCatalog.cs`):**
   - `ScavengingTableCatalogContainer`, `ScavengingTableDef`, `ScavengingLootEntryDef`, `ScavengingRollResult`.
   - Seeded deterministic loot rolling via `ISeededRng`.
3. **Expedition Integration (`Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` & `ExpeditionCatalogLoader.cs`):**
   - Added `scavenging_table_id` to `ExpeditionDefinition` and `ExpeditionJsonDto`.
   - `PerformLootRoll` and `AddLoot` support multi-quantity item yields and table resolution with fallback.
4. **Data Integrity Namespace Registration:**
   - Added `"table_loot_"` and `"scavenge_"` to `CatalogIntegrityValidator.cs` and `CatalogIntegrityRules.cs`.
   - Added `"scavenging_table_id"` to `ReferenceKeys`.
5. **Unit Test Suite (`Ashfall.Core.Tests/Expeditions/ScavengingTableCatalogTests.cs`):**
   - 7 comprehensive facts verifying catalog loading, weight totals, deterministic reproducibility, 10,000-roll distribution accuracy within 3.5% tolerance, hazard triggering rates, and `ExpeditionSystem` end-to-end integration.

---

## 3. Verification & Compliance
- `dotnet test Ashfall.Core.Tests`: 100% PASS
- `godot --headless --path . -- --data-integrity-selftest`: 0 errors
- `godot --headless --path . -- --content-utilization-selftest`: 0 errors
- `godot --headless --path . -- --scene-binding-selftest`: 22/22 pass
- `python3 scripts/ci/scene-lint.py`: 0 errors
