# Plan 116 — Deep Lore Locations Expansion (10 → 25): Implementation Closeout Report

**Theme:** Turn the exploration layer from a short list of scavenging nodes into a real world map with materially distinct expedition identities.
**Catalog Authority:** `Assets/StreamingAssets/Data/deep_lore_locations.json`
**Linux Build Mirror:** `builds/linux/Assets/StreamingAssets/Data/deep_lore_locations.json`
**Core Loaders:** `Assets/Ashfall.Core/Maritime/DeepLoreLocationCatalogLoader.cs`
**Loot Model:** `Assets/Ashfall.Core/Maritime/VariableLootNode.cs`, `Assets/Ashfall.Core/Maritime/ProceduralScavengeSystem.cs`
**Completion Date:** 2026-09-08
**Verification Status:** 9,905 / 9,905 tests passing (0 failed), 0 data integrity errors across 298 catalogs (12,109 authored IDs), 25/25 scene bindings passed, 0 scene-lint errors, 0 build warnings.

---

## 1. Executive Summary

Plan 116 successfully expanded `deep_lore_locations.json` from the verified baseline of 10 scavenging nodes to exactly **25 exploration destinations** across six major location families:
- **3 Urban:** `location_apartment_block`, `location_metro_station`, `location_police_station`
- **3 Industrial:** `location_chemical_plant`, `location_steelworks`, `location_power_substation`
- **2 Military:** `location_ammunition_depot`, `location_radar_site`
- **2 Scientific:** `location_weather_station`, `location_agricultural_research`
- **2 Subterranean:** `location_metro_tunnel`, `location_drainage_network`
- **3 Wilderness:** `location_irradiated_forest`, `location_frozen_wetland`, `location_burned_woodland`

Every location has a distinct risk/reward profile, evocative display name, calibrated radiation, threat danger level, travel hours, and 7 coherent loot entries with a unique primary loot identity.

Zero Core C# engine changes were required. The expansion is 100% data-first, preserving Invariant 1 (zero engine coupling in Core), Invariant 4 (seeded determinism), and Invariant 6 (JSON data authority).

---

## 2. Architecture, Semantics & Classifications

### 2.1 Schema & Loot Resolution Semantics
Each location is defined within the `locations` array of `deep_lore_locations.json` (schema_version 1):
```json
{
  "id": "location_*",
  "displayName": "...",
  "radiationUSv": 10.0,
  "dangerLevel": 3,
  "travelHours": 1.8,
  "lootTable": [
    {
      "itemId": "cloth",
      "minQty": 10,
      "maxQty": 35,
      "spawnChance": 0.9,
      "degradationChance": 0.0,
      "degradedItemId": ""
    }
  ]
}
```
- **Independent Entry Rolls:** In `ProceduralScavengeSystem.RollLootTable()`, every loot table entry is evaluated independently using `_rng.NextDouble() > node.SpawnChance`.
- **Quantity Skewing:** When spawned, quantities roll between `minQty` and `maxQty` via a Poisson distribution that skews downward as world days advance and site visits accumulate.
- **Contamination:** Locations with ambient radiation `radiationUSv >= 15.0 µSv/h` automatically mark rolled items as contaminated.

### 2.2 Degradation Model Classification: **D3 (Strict Coupling)**
- When `degradationChance > 0f`, `degradedItemId` is required, non-empty, and resolves to a valid degraded item definition.
- When `degradedItemId` is present, `degradationChance` must be `> 0f`.
- When degradation fires, quantity is halved (`MathfCompat.Max(1, qty / 2)`) and the returned item ID transforms to `degradedItemId`.
- Canonical degradation pairs used:
  - `book` → `paper_scrap`
  - `cardboard_box` → `paper_scrap`
  - `canned_food` → `spoiled_canned_food`
  - `raw_meat` → `spoiled_meat`
  - `blood_bag` → `spoiled_blood_bag`
  - `clean_water` → `dirty_water`

### 2.3 Expedition Model Classification: **E0 (Direct Exposure)**
- Locations in `deep_lore_locations.json` use the global `location_*` namespace recognized in `CatalogIntegrityValidator.IdPrefixes`.
- Asset discovery and coverage scanning automatically maps all 25 locations via `AssetRegistry.cs` and `AssetCoverageScanner.cs`.
- Content utilization tracks `deep_lore_locations.json` as `GAMEPLAY_CONSUMED` via `DeepLoreLocationCatalogLoader`, `MaritimeDiveSystem`, and `MaritimePanel`.

---

## 3. Existing 10 Locations Parity Audit

The baseline 10 locations remain 100% intact with zero rebalancing or regressions:

| ID | Display Name | Family | Rads (µSv/h) | Danger | Travel (h) | Loot Rows | Primary Loot Identity |
|---|---|---|---:|---:|---:|---:|---|
| `location_municipal_library` | The Municipal Library | Urban | 12.0 | 4 | 1.8 | 8 | Books, Paper Scrap, Scrap Wood |
| `location_sunshine_daycare` | Sunshine Daycare | Urban | 8.0 | 3 | 1.8 | 8 | Civilian Medical, Blankets, Toys, CZ75 |
| `location_regional_blood_bank` | Regional Blood Bank | Urban | 18.0 | 6 | 2.7 | 7 | Antiseptics, Blood Bags, Medical Parts |
| `location_grand_cinema` | Grand Cinema | Urban | 14.0 | 5 | 2.7 | 7 | Concessions, Vacuum Tubes, Fuel, Textiles |
| `location_upland_logging_camp` | Upland Logging Camp | Wilderness | 20.0 | 7 | 4.5 | 8 | Timber, Nails, Hatchets, Ash Ghillie |
| `location_stadium_evacuation_center` | Stadium Evacuation Center | Urban | 18.0 | 6 | 3.6 | 7 | Refugee Belongings, Luggage, Textiles |
| `location_automated_abattoir` | Automated Abattoir | Industrial | 12.0 | 7 | 3.6 | 7 | Meat/Fat, Bone Saws, Ammonia, Bleach |
| `location_central_postal_hub` | Central Postal Hub | Urban | 14.0 | 4 | 2.7 | 8 | Packaging, Brass Fittings, Documents |
| `location_municipal_water_reservoir` | Municipal Water Reservoir | Industrial | 25.0 | 8 | 4.5 | 6 | Water Treatment Chemicals, Pipe Wrenches |
| `location_television_studio` | Television Studio | Urban | 15.0 | 4 | 1.8 | 7 | Broadcast Tubes, Sound Foam, Cassettes |

---

## 4. Complete 25-Location Master Roster

| # | ID | Display Name | Family | Rads | Danger | Travel | Loot Rows | Primary Loot Identity | Cross-Plan Links |
|---|---|---|---|---:|---:|---:|---:|---|---|
| 1 | `location_municipal_library` | The Municipal Library | Urban | 12.0 | 4 | 1.8 | 8 | Paper & Books | — |
| 2 | `location_sunshine_daycare` | Sunshine Daycare | Urban | 8.0 | 3 | 1.8 | 8 | Childcare Comfort | — |
| 3 | `location_regional_blood_bank` | Regional Blood Bank | Urban | 18.0 | 6 | 2.7 | 7 | Blood & Antiseptics | — |
| 4 | `location_grand_cinema` | Grand Cinema | Urban | 14.0 | 5 | 2.7 | 7 | Theatre Supplies & Tubes | — |
| 5 | `location_upland_logging_camp` | Upland Logging Camp | Wilderness | 20.0 | 7 | 4.5 | 8 | Logging Timber & Hatchets | — |
| 6 | `location_stadium_evacuation_center` | Stadium Evacuation Center | Urban | 18.0 | 6 | 3.6 | 7 | Refugee Luggage & Cash | — |
| 7 | `location_automated_abattoir` | Automated Abattoir | Industrial | 12.0 | 7 | 3.6 | 7 | Processed Meat & Saws | — |
| 8 | `location_central_postal_hub` | Central Postal Hub | Urban | 14.0 | 4 | 2.7 | 8 | Postal Packaging & Letters | — |
| 9 | `location_municipal_water_reservoir` | Municipal Water Reservoir | Industrial | 25.0 | 8 | 4.5 | 6 | Water Purification Reagents | — |
| 10 | `location_television_studio` | Television Studio | Urban | 15.0 | 4 | 1.8 | 7 | Broadcast Audio & Tubes | — |
| 11 | `location_apartment_block` | The Split Apartment Block | Urban | 10.0 | 3 | 1.8 | 7 | Domestic Textiles & Coats | Vertical Slice 1 |
| 12 | `location_metro_station` | Central Metro Concourse | Urban | 16.0 | 4 | 2.2 | 7 | Transit Tokens & Maintenance | Subterranean Seam |
| 13 | `location_police_station` | District Police Station | Urban | 12.0 | 5 | 2.5 | 7 | Controlled Munitions & Evidence | Security Cache |
| 14 | `location_chemical_plant` | North Chemical Works | Industrial | 22.0 | 6 | 3.8 | 7 | Industrial Reagents & Scrubbers | Plan 112 Vector |
| 15 | `location_steelworks` | Riverside Steelworks | Industrial | 15.0 | 5 | 3.2 | 7 | Rebar & Structural Braces | Heavy Metal Salvage |
| 16 | `location_power_substation` | Eastern Power Substation | Industrial | 14.0 | 4 | 2.8 | 7 | High-Capacity Storage & Mesh | Plan 76 Target |
| 17 | `location_ammunition_depot` | Reserve Ammunition Depot | Military | 20.0 | 8 | 5.2 | 7 | Heavy Munitions & Mine Prods | High-Risk Apex |
| 18 | `location_radar_site` | Hill Radar Annex | Military | 24.0 | 6 | 4.8 | 7 | Radar Telemetry & Avionics | Plan 113 / Plan 76 |
| 19 | `location_weather_station` | Upper Ridge Weather Station | Scientific | 18.0 | 4 | 4.0 | 7 | Radiosondes & Sounding Logs | Plan 113 / Plan 48 |
| 20 | `location_agricultural_research` | Agricultural Research Annex | Scientific | 12.0 | 3 | 3.0 | 7 | Seed Envelopes & Grow Media | Plan 76 Target |
| 21 | `location_metro_tunnel` | Service Tunnel Six | Subterranean | 14.0 | 6 | 2.6 | 7 | Rebreather Filters & Electrets | Plan 112 Vector |
| 22 | `location_drainage_network` | South Drainage Network | Subterranean | 16.0 | 5 | 2.4 | 7 | Sump Pipe Fittings & Sludge | Plan 112 Vector |
| 23 | `location_irradiated_forest` | The Grey Forest | Wilderness | 28.0 | 7 | 4.2 | 7 | High-Rad Timber & Hot Dust | Plan 112 Vector |
| 24 | `location_frozen_wetland` | Frozen Reed Marsh | Wilderness | 15.0 | 5 | 5.0 | 7 | Subzero Mittens & Mukluks | Plan 48 Blizzard |
| 25 | `location_burned_woodland` | Burned Pine Belt | Wilderness | 22.0 | 6 | 3.6 | 7 | Charred Cordwood & Ash Ghillie | Plan 48 Fallout |

---

## 5. Loot Tables & Normalized Expected Value QA

Each of the 15 new destinations features 7 authored loot rows. Expected yield per expedition visit:
$$E[\text{Units}] = \sum_i \left(\text{spawnChance}_i \times \frac{\text{minQty}_i + \text{maxQty}_i}{2}\right)$$
The probability of receiving zero items on an expedition:
$$P(\text{No Loot}) = \prod_i (1 - \text{spawnChance}_i)$$

### 5.1 Urban Family
- **`location_apartment_block`**:
  - `cloth` (10–35, p=0.90, E=20.25)
  - `wool_blanket` (1–4, p=0.75, E=1.88)
  - `family_photograph` (1–8, p=0.70, E=3.15)
  - `canned_food` (1–6, p=0.50, deg_p=0.50 → `spoiled_canned_food`, E=1.75)
  - `clean_water` (1–4, p=0.45, deg_p=0.40 → `dirty_water`, E=1.13)
  - `item_heavy_wool_coat` (0–1, p=0.15, E=0.08)
  - `book` (1–6, p=0.60, deg_p=0.40 → `paper_scrap`, E=2.10)
  - *Total Expected Units:* ~30.3 units | *P(No Loot):* 0.04%

- **`location_metro_station`**:
  - `battery` (2–10, p=0.80, E=4.80)
  - `paper_scrap` (15–60, p=0.90, E=33.75)
  - `mechanical_parts` (2–12, p=0.75, E=5.25)
  - `duct_tape` (1–6, p=0.70, E=2.45)
  - `scrap_metal` (5–25, p=0.85, E=12.75)
  - `gas_mask` (0–1, p=0.20, E=0.10)
  - `cardboard_box` (2–10, p=0.65, deg_p=0.50 → `paper_scrap`, E=3.90)
  - *Total Expected Units:* ~63.0 units | *P(No Loot):* 0.01%

- **`location_police_station`**:
  - `ammo_9x19` (2–16, p=0.40, E=3.60)
  - `pistol_cz75_9x19` (0–1, p=0.12, E=0.06)
  - `sealed_government_document` (1–3, p=0.35, E=0.70)
  - `alcohol_wipes_box_10_of_10` (1–6, p=0.70, E=2.45)
  - `item_suitcase_locked` (0–2, p=0.30, E=0.30)
  - `bandage` (2–8, p=0.80, E=4.00)
  - `scrap_metal` (5–20, p=0.75, E=9.38)
  - *Total Expected Units:* ~20.5 units | *P(No Loot):* 0.44%

### 5.2 Industrial Family
- **`location_chemical_plant`**:
  - `chemicals` (8–40, p=0.90, E=21.60)
  - `industrial_bleach` (2–10, p=0.75, E=4.50)
  - `gas_mask` (0–2, p=0.35, E=0.35)
  - `rubber_hose` (3–15, p=0.80, E=7.20)
  - `item_co2_scrubber_cartridge` (1–3, p=0.30, E=0.60)
  - `ammonia_tank` (0–2, p=0.25, E=0.25)
  - `item_prussian_blue_chelating_pellets` (0–2, p=0.20, E=0.20)
  - *Total Expected Units:* ~34.7 units | *P(No Loot):* 0.16%

- **`location_steelworks`**:
  - `scrap_metal` (25–90, p=0.95, E=54.63)
  - `item_galvanized_rebar` (2–10, p=0.70, E=4.20)
  - `box_of_nails_10` (3–15, p=0.80, E=7.20)
  - `mechanical_parts` (5–25, p=0.85, E=12.75)
  - `fuel_1l` (1–8, p=0.60, E=2.70)
  - `pipe_wrench` (0–2, p=0.40, E=0.40)
  - `item_high_tensile_steel_culvert_brace` (0–2, p=0.25, E=0.25)
  - *Total Expected Units:* ~82.1 units | *P(No Loot):* 0.01%

- **`location_power_substation`**:
  - `electronic_scrap` (10–45, p=0.90, E=24.75)
  - `battery` (6–25, p=0.85, E=13.18)
  - `brass_fittings` (2–10, p=0.75, E=4.50)
  - `item_faraday_mesh` (1–4, p=0.40, E=1.00)
  - `vacuum_tube` (2–10, p=0.65, E=3.90)
  - `duct_tape` (2–8, p=0.70, E=3.50)
  - `scrap_metal` (10–35, p=0.80, E=18.00)
  - *Total Expected Units:* ~68.8 units | *P(No Loot):* 0.004%

### 5.3 Military Family
- **`location_ammunition_depot`**:
  - `ammo_762x54r_jhp_ap` (2–14, p=0.45, E=3.60)
  - `ammo_9x19` (5–25, p=0.50, E=7.50)
  - `item_mine_prod` (0–2, p=0.35, E=0.35)
  - `scrap_metal` (15–50, p=0.85, E=27.63)
  - `item_ash_ghillie` (0–1, p=0.10, E=0.05)
  - `sealed_government_document` (0–2, p=0.25, E=0.25)
  - `military_grade_hatchet` (0–1, p=0.20, E=0.10)
  - *Total Expected Units:* ~39.5 units | *P(No Loot):* 0.94%

- **`location_radar_site`**:
  - `electronic_scrap` (15–60, p=0.90, E=33.75)
  - `vacuum_tube` (4–18, p=0.75, E=8.25)
  - `battery` (4–20, p=0.80, E=9.60)
  - `item_faraday_mesh` (1–5, p=0.50, E=1.50)
  - `sealed_government_document` (1–3, p=0.40, E=0.80)
  - `cassette_tape` (1–8, p=0.60, E=2.70)
  - `item_detector_sensor_module` (0–2, p=0.25, E=0.25)
  - *Total Expected Units:* ~56.9 units | *P(No Loot):* 0.04%

### 5.4 Scientific Family
- **`location_weather_station`**:
  - `item_radiosonde` (1–3, p=0.60, E=1.20)
  - `battery` (4–16, p=0.80, E=8.00)
  - `electronic_scrap` (8–30, p=0.85, E=16.15)
  - `paper_scrap` (10–50, p=0.90, E=27.00)
  - `item_thermal_parka` (0–1, p=0.18, E=0.09)
  - `antiseptic_1l_of_1l` (1–4, p=0.60, E=1.50)
  - `vacuum_tube` (1–6, p=0.50, E=1.75)
  - *Total Expected Units:* ~55.7 units | *P(No Loot):* 0.03%

- **`location_agricultural_research`**:
  - `item_seed_tuber` (2–8, p=0.65, E=3.25)
  - `item_seed_grain` (3–12, p=0.60, E=4.50)
  - `item_seed_wheat` (0–2, p=0.15, E=0.15)
  - `item_grow_medium` (1–4, p=0.50, E=1.25)
  - `item_ammonium_nitrate_sack` (1–3, p=0.40, E=0.80)
  - `clean_water` (2–10, p=0.70, deg_p=0.30 → `dirty_water`, E=4.20)
  - `paper_scrap` (5–25, p=0.80, E=12.00)
  - *Total Expected Units:* ~26.2 units | *P(No Loot):* 0.07%

### 5.5 Subterranean Family
- **`location_metro_tunnel`**:
  - `item_rebreather_scrubber` (0–2, p=0.30, E=0.30)
  - `canned_food` (1–8, p=0.55, deg_p=0.40 → `spoiled_canned_food`, E=2.48)
  - `item_radon_detector_electret` (0–2, p=0.25, E=0.25)
  - `mechanical_parts` (4–18, p=0.80, E=8.80)
  - `rubber_hose` (2–10, p=0.70, E=4.20)
  - `scrap_metal` (10–40, p=0.85, E=21.25)
  - `battery` (2–12, p=0.75, E=5.25)
  - *Total Expected Units:* ~42.5 units | *P(No Loot):* 0.03%

- **`location_drainage_network`**:
  - `pipe_wrench` (0–2, p=0.45, E=0.45)
  - `brass_fittings` (2–12, p=0.75, E=5.25)
  - `rubber_hose` (4–16, p=0.85, E=8.50)
  - `scrap_metal` (12–50, p=0.90, E=27.90)
  - `clean_water` (1–6, p=0.40, deg_p=0.60 → `dirty_water`, E=1.40)
  - `industrial_bleach` (1–5, p=0.65, E=1.95)
  - `item_sludge_cake` (1–6, p=0.70, E=2.45)
  - *Total Expected Units:* ~47.9 units | *P(No Loot):* 0.02%

### 5.6 Wilderness Family
- **`location_irradiated_forest`**:
  - `wood_block` (15–65, p=0.95, E=38.00)
  - `scrap_wood` (20–80, p=1.00, E=50.00)
  - `sawdust_block` (5–30, p=0.80, E=14.00)
  - `item_hot_dust_drum` (0–2, p=0.25, E=0.25)
  - `item_prussian_blue_chelating_pellets` (0–2, p=0.20, E=0.20)
  - `item_frostbite_salve` (1–4, p=0.50, E=1.25)
  - `raw_meat` (2–15, p=0.40, deg_p=0.70 → `spoiled_meat`, E=3.40)
  - *Total Expected Units:* ~107.1 units | *P(No Loot):* 0.00% (scrap_wood has p=1.0)

- **`location_frozen_wetland`**:
  - `item_fur_mittens` (0–2, p=0.35, E=0.35)
  - `item_insulated_boots` (0–1, p=0.20, E=0.10)
  - `item_ice_pick` (0–2, p=0.45, E=0.45)
  - `cloth` (5–25, p=0.80, E=12.00)
  - `wool_blanket` (1–4, p=0.65, E=1.63)
  - `clean_water` (2–12, p=0.60, deg_p=0.30 → `dirty_water`, E=4.20)
  - `item_frostbite_salve` (1–3, p=0.55, E=1.10)
  - *Total Expected Units:* ~19.8 units | *P(No Loot):* 0.17%

- **`location_burned_woodland`**:
  - `scrap_wood` (25–90, p=1.00, E=57.50)
  - `sawdust_block` (8–40, p=0.85, E=20.40)
  - `fuel_1l` (1–6, p=0.50, E=1.75)
  - `scrap_metal` (10–35, p=0.80, E=18.00)
  - `box_of_nails_10` (1–8, p=0.65, E=2.93)
  - `military_grade_hatchet` (0–1, p=0.20, E=0.10)
  - `item_ash_ghillie` (0–1, p=0.12, E=0.06)
  - *Total Expected Units:* ~100.7 units | *P(No Loot):* 0.00% (scrap_wood has p=1.0)

---

## 6. Cross-System Integrations

### 6.1 Plan 112 (Disease & Vectors)
Four locations provide distinct biological and chemical hazard profiles:
1. **`location_drainage_network`**: Sump runoff & black water vector (`item_sludge_cake`, high water degradation).
2. **`location_metro_tunnel`**: Deep stagnation & underground mold spores (`item_rebreather_scrubber`, `item_radon_detector_electret`).
3. **`location_chemical_plant`**: Aerosolized corrosive vapors & toxic effluents (`chemicals`, `ammonia_tank`, `industrial_bleach`).
4. **`location_irradiated_forest`**: Extreme fallout biotope & radioactive carcass scavenging (`locationRads: 28.0 µSv/h`, `raw_meat` decaying to `spoiled_meat`).

### 6.2 Plan 48 (Weather Route & Site Gates)
Three destinations represent distinct weather-sensitive risk profiles:
1. **`location_frozen_wetland`**: Exposed open marshland vulnerable to blinding Blizzards and subzero frostbite.
2. **`location_burned_woodland`**: Wind-scoured charcoal flats exposed to Fallout Storm particulate dispersal.
3. **`location_weather_station`**: High-altitude ridge installation subject to whiteout blizzards and gale winds.

### 6.3 Plan 76 (Expedition Destinations)
Three primary strategic goals for midgame infrastructure recovery:
1. **`location_power_substation`**: Source for high-capacity batteries and Faraday shielding to restore shelter power systems.
2. **`location_agricultural_research`**: Essential seed bank (`item_seed_tuber`, `item_seed_grain`, `item_seed_wheat`) and grow media for the Glass Orchard greenhouse.
3. **`location_radar_site`**: Long-range telemetry intercept equipment and high-frequency vacuum tubes for radio signal intelligence.

### 6.4 Plan 113 (The Verdict Cases)
Two high-value forensic investigation sites:
1. **`location_weather_station`**: Atmospheric anomaly sounding records and radiosonde archives.
2. **`location_radar_site`**: Military signal logs, flight paths, and encrypted telemetry records.

---

## 7. Verification Evidence & Test Matrix

| Verification Step | Command | Result | Details |
|---|---|---|---|
| **Data Integrity Selftest** | `godot --headless --path . -- --data-integrity-selftest` | **PASS (0 errors)** | 12,109 authored IDs across 298 catalogs (+15 new location IDs clean). |
| **Content Utilization** | `godot --headless --path . -- --content-utilization-selftest` | **PASS (CI gate)** | `deep_lore_locations.json` recognized as `GAMEPLAY_CONSUMED`. |
| **Scene Binding Selftest** | `godot --headless --path . -- --scene-binding-selftest` | **PASS (25/25)** | All UI panel scene bindings passed. |
| **Scene Lint** | `python3 scripts/ci/scene-lint.py` | **PASS (0 errors)** | 30 production scenes checked; 0 errors, 0 warnings. |
| **Asset Registry** | `python3 scripts/ci/generate-asset-registry.py` | **PASS** | Manifest and report updated with 375 total locations. |
| **Host Compilation** | `dotnet build Ashfall.csproj` | **PASS (0 errors)** | Godot C# host assembly compiled clean with 0 warnings. |
| **Dedicated Expansion Tests** | `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter FullyQualifiedName~DeepLoreLocationExpansionTests` | **PASS (14/14)** | Catalog loading, parity, D3 degradation coupling, uniqueness, simulation, and determinism verified. |
| **Full Core Test Suite** | `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | **PASS (9,905/9,905)** | 0 failures, 0 skipped, 9,905 passed across entire test suite. |

---

## 8. Definition of Done Compliance Checklist

- [x] `deep_lore_locations.json` contains exactly 25 valid locations.
- [x] Original 10 IDs remain compatible, unchanged, and preserved.
- [x] All 15 new destinations are reachable, unique, and prefixed `location_*`.
- [x] Every item ID and degraded item ID resolves against canonical catalogs.
- [x] All loot tables are coherent and contain 4–8 entries (exactly 7 entries per new site).
- [x] Degradation coupling matches runtime D3 contract (both fields present together).
- [x] Primary loot uniqueness verified across all 15 new destinations.
- [x] Risk/reward and rare-stock economies reviewed and balanced.
- [x] 4 disease, 3 weather, 3 expedition, and 2 Verdict integrations aligned with real authorities.
- [x] Save/load compatibility and deterministic loot behavior verified.
- [x] Zero engine coupling in Core and zero new Core code introduced.
- [x] Full test matrix and CI gates completely green.
