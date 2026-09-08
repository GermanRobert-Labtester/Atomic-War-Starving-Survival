# Plan 116 — Deep Lore Locations Expansion (10 → 25): Forensic Baseline & Reconnaissance Report

**Catalog Authority:** `Assets/StreamingAssets/Data/deep_lore_locations.json`
**Build Mirror:** `builds/linux/Assets/StreamingAssets/Data/deep_lore_locations.json`
**Core Loaders:** `Assets/Ashfall.Core/Maritime/DeepLoreLocationCatalogLoader.cs`
**Loot Model:** `Assets/Ashfall.Core/Maritime/VariableLootNode.cs`, `Assets/Ashfall.Core/Maritime/ProceduralScavengeSystem.cs`
**Baseline Date:** 2026-09-08
**Verification Baseline:** 9,891 tests passing (0 failures), 0 data integrity errors across 298 catalogs, 25/25 scene bindings passing, 0 scene-lint errors, 0 build warnings.

---

## 1. Catalog Architecture & DTO Semantics

### 1.1 JSON Structure
```json
{
  "schema_version": 1,
  "locations": [
    {
      "id": "location_municipal_library",
      "displayName": "The Municipal Library",
      "radiationUSv": 12.0,
      "dangerLevel": 4,
      "travelHours": 1.8,
      "lootTable": [
        {
          "itemId": "book",
          "minQty": 0,
          "maxQty": 12,
          "spawnChance": 0.65,
          "degradationChance": 0.4,
          "degradedItemId": "paper_scrap"
        }
      ]
    }
  ]
}
```

### 1.2 DTOs (`DeepLoreLocationCatalogLoader.cs` & `VariableLootNode.cs`)
- **`DeepLoreLocationContainer`**:
  - `locations` (List<DeepLoreLocationEntry>): Root collection.
- **`DeepLoreLocationEntry`**:
  - `id` (string): Stable identifier with `location_*` prefix.
  - `displayName` (string): Evocative, survivor-facing site name.
  - `radiationUSv` (float): Ambient radiation in µSv/h (0.0 to ~30.0+ scale in existing data).
  - `dangerLevel` (int): Threat tier (1 to 8 in existing data, typically 3–8).
  - `travelHours` (float): One-way expedition travel time from shelter (1.8 to 5.2 hours).
  - `lootTable` (List<VariableLootNode>): Array of loot entries.
- **`VariableLootNode`**:
  - `ItemId` (string, maps from `itemId`): Resolves in canonical item catalogs (`items.json`, `black_flotilla_items.json`, `greenhouse_items.json`).
  - `MinQty` (int, maps from `minQty`): Minimum quantity if rolled.
  - `MaxQty` (int, maps from `maxQty`): Maximum quantity if rolled (`maxQty >= minQty`).
  - `SpawnChance` (float, maps from `spawnChance`): Independent probability (0.0 to 1.0) of node triggering.
  - `DegradationChance` (float, maps from `degradationChance`): Probability (0.0 to 1.0) that item degrades if spawned.
  - `DegradedItemId` (string, maps from `degradedItemId`): Target item id if degraded.
  - `Description` (string, maps from `description`, optional): Optional node narrative context.

---

## 2. Loot Generation & Degradation Semantics

### 2.1 Loot Roll Execution (`ProceduralScavengeSystem.cs`)
- For each entry in `lootTable`:
  1. `_rng.NextDouble() > node.SpawnChance` ⇒ Skip entry (independent roll per entry).
  2. `qty = RollQuantity(node.MinQty, node.MaxQty, _currentDay, visits)` (Poisson distribution skewed by campaign day and visit count).
  3. If `qty <= 0` ⇒ Skip entry.
  4. If `node.DegradationChance > 0f` and `_rng.NextDouble() < node.DegradationChance`:
     - Item is degraded: `qty = MathfCompat.Max(1, qty / 2)`.
     - Output item resolves to `node.DegradedItemId`.
  5. Contamination is flagged if `locationRads >= HighRadThreshold (15 µSv/h)` or `hasBioHazard`.

### 2.2 Degradation Classification: **D3 (Coupled Optional Fields)**
- Analysis of existing data in `deep_lore_locations.json`:
  - When `degradationChance > 0`, `degradedItemId` is ALWAYS present and resolves to a valid item id.
  - When `degradedItemId` is present, `degradationChance` is ALWAYS > 0.
  - Runtime behavior in `ProceduralScavengeSystem.cs` line 79: `DegradedItemId = degraded && !string.IsNullOrEmpty(node.DegradedItemId) ? node.DegradedItemId : null!`.
  - **Rule:** If degradation is enabled on a loot entry, both `degradationChance` and `degradedItemId` must be authored together.

### 2.3 Canonical Degradation Pairs
- `book` → `paper_scrap`
- `cardboard_box` → `paper_scrap`
- `canned_food` → `spoiled_canned_food`
- `raw_meat` → `spoiled_meat`
- `blood_bag` → `spoiled_blood_bag`
- `clean_water` → `dirty_water`
- `item_ro_resin` → `scrap_mechanical`

---

## 3. Expedition Model Classification: **E0 (Direct Consumption & Asset Registration)**
- Deep lore locations are registered in `AssetRegistry.cs` and `AssetCoverageScanner.cs`:
  `("location", "deep_lore_locations.json", "id")`.
- `ContentUtilizationScanner.cs` maps `deep_lore_locations.json` directly to `MaritimeDiveSystem`, `MaritimePanel`, and `DeepLoreLocationCatalogLoader`.
- Location IDs use the `location_*` namespace, recognized globally by `CatalogIntegrityValidator.IdPrefixes`.

---

## 4. Existing 10 Locations Parity Table

| # | ID | Display Name | Family | Rads (µSv/h) | Danger | Travel (h) | Loot Rows | Primary Loot Identity |
|---|---|---|---|---:|---:|---:|---:|---|
| 1 | `location_municipal_library` | The Municipal Library | Urban | 12.0 | 4 | 1.8 | 8 | Books, Paper Scrap, Scrap Wood |
| 2 | `location_sunshine_daycare` | Sunshine Daycare | Urban | 8.0 | 3 | 1.8 | 8 | Civilian Medical, Blankets, Toys, CZ75 |
| 3 | `location_regional_blood_bank` | Regional Blood Bank | Urban | 18.0 | 6 | 2.7 | 7 | Antiseptics, Blood Bags, Medical Parts |
| 4 | `location_grand_cinema` | Grand Cinema | Urban | 14.0 | 5 | 2.7 | 7 | Concessions, Vacuum Tubes, Fuel, Textiles |
| 5 | `location_upland_logging_camp` | Upland Logging Camp | Wilderness | 20.0 | 7 | 4.5 | 8 | Timber, Nails, Hatchets, Ash Ghillie |
| 6 | `location_stadium_evacuation_center` | Stadium Evacuation Center | Urban | 18.0 | 6 | 3.6 | 7 | Refugee Belongings, Luggage, Textiles |
| 7 | `location_automated_abattoir` | Automated Abattoir | Industrial | 12.0 | 7 | 3.6 | 7 | Meat/Fat, Bone Saws, Ammonia, Bleach |
| 8 | `location_central_postal_hub` | Central Postal Hub | Urban | 14.0 | 4 | 2.7 | 8 | Packaging, Brass Fittings, Documents |
| 9 | `location_municipal_water_reservoir` | Municipal Water Reservoir | Industrial | 25.0 | 8 | 4.5 | 6 | Water Treatment Chemicals, Pipe Wrenches |
| 10 | `location_television_studio` | Television Studio | Urban | 15.0 | 4 | 1.8 | 7 | Broadcast Tubes, Sound Foam, Cassettes |

---

## 5. Expansion Roster: 15 New Destinations Across 6 Families

| # | ID | Display Name | Family | Rads | Danger | Travel | Primary Identity | Integrations |
|---|---|---|---|---:|---:|---:|---|---|
| 11 | `location_apartment_block` | The Split Apartment Block | Urban | 10.0 | 3 | 1.8 | Domestic textiles, coats, personal memories | Vertical Slice 1 |
| 12 | `location_metro_station` | Central Metro Concourse | Urban | 16.0 | 4 | 2.2 | Transit tokens, maintenance tools, batteries | Subterranean Seam |
| 13 | `location_police_station` | District Police Station | Urban | 12.0 | 5 | 2.5 | Security lockers, 9mm ammo, evidence | Civil Authority |
| 14 | `location_chemical_plant` | North Chemical Works | Industrial | 22.0 | 6 | 3.8 | Scrubbers, chemicals, Prussian blue | Plan 112 Vector |
| 15 | `location_steelworks` | Riverside Steelworks | Industrial | 15.0 | 5 | 3.2 | Rebar, heavy culvert braces, scrap metal | Structural Salvage |
| 16 | `location_power_substation` | Eastern Power Substation | Industrial | 14.0 | 4 | 2.8 | Faraday mesh, electrical scrap, batteries | Plan 76 Target |
| 17 | `location_ammunition_depot` | Reserve Ammunition Depot | Military | 20.0 | 8 | 5.2 | High-caliber AP ammo, mine prods | Late Risk/Reward |
| 18 | `location_radar_site` | Hill Radar Annex | Military | 24.0 | 6 | 4.8 | Avionics, sensor modules, Faraday mesh | Plan 113 / Plan 76 |
| 19 | `location_weather_station` | Upper Ridge Weather Station | Scientific | 18.0 | 4 | 4.0 | Radiosondes, barographs, arctic parkas | Plan 113 / Plan 48 |
| 20 | `location_agricultural_research` | Agricultural Research Annex | Scientific | 12.0 | 3 | 3.0 | Tuber/grain seeds, grow media, fertilizers | Plan 76 Target |
| 21 | `location_metro_tunnel` | Service Tunnel Six | Subterranean | 14.0 | 6 | 2.6 | Rebreather scrubbers, radon electrets | Plan 112 Vector |
| 22 | `location_drainage_network` | South Drainage Network | Subterranean | 16.0 | 5 | 2.4 | Sump piping, sludge cake, bleach | Plan 112 Vector |
| 23 | `location_irradiated_forest` | The Grey Forest | Wilderness | 28.0 | 7 | 4.2 | High-rad cordwood, hot dust drums | Plan 112 Vector |
| 24 | `location_frozen_wetland` | Frozen Reed Marsh | Wilderness | 15.0 | 5 | 5.0 | Fur mittens, mukluk boots, ice picks | Plan 48 Blizzard |
| 25 | `location_burned_woodland` | Burned Pine Belt | Wilderness | 22.0 | 6 | 3.6 | Charred fuel wood, nails, ghillie salvage | Plan 48 Fallout |

---

## 6. Cross-Plan Integration Commitments
1. **Plan 112 (Disease & Vectors):**
   - 4 locations aligned with environmental contamination vectors:
     - `location_drainage_network` (waterborne / sump contamination)
     - `location_metro_tunnel` (fungal spore / respiratory stagnant air)
     - `location_chemical_plant` (toxic chemical inhalation / bio-industrial residue)
     - `location_irradiated_forest` (high-fallout bio-vector / wildlife contamination)
2. **Plan 48 (Weather Sensitivity):**
   - 3 locations aligned with weather exposure:
     - `location_frozen_wetland` (Blizzard / extreme subzero cold access sensitivity)
     - `location_burned_woodland` (Fallout storm wind-scour sensitivity)
     - `location_weather_station` (Exposed ridge high-wind / blizzard sensitivity)
3. **Plan 76 (Expedition Destinations):**
   - 3 primary technical expedition targets:
     - `location_power_substation` (Electrical restoration)
     - `location_agricultural_research` (Food security & seed recovery)
     - `location_radar_site` (Long-range telemetry & signal intelligence)
4. **Plan 113 (The Verdict Cases):**
   - 2 key forensic investigation sites:
     - `location_weather_station` (Upper Ridge meteorological record discrepancies)
     - `location_radar_site` (Early-warning radar intercept archives)
