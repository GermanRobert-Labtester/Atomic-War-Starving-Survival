# Grain Milling Item & Recipe Crosswalk

## 1. Directive & Authority Boundary

In accordance with **Plan 157 §10 (Workstream E)**, Plan 55 remains the sole authority over recipe definitions, inputs, processing times, and outputs. Plan 157 must not introduce hidden recipe coefficients, dynamic extraction buffs, or price modifiers based on narrative milling records.

This crosswalk audits all canonical items and recipes associated with the grain processing cycle and maps narrative measurements to their appropriate presentation tier.

---

## 2. Canonical Item & Recipe Inventory

| Entity Type | Canonical ID | Display Name | Live Gameplay Function | Plan 157 Interaction |
|---|---|---|---|---|
| **Seed** | `item_seed_ash_grain` | Ash-Grain Seed | Greenhouse planting stock | Thematic link to hard winter cereal varieties. |
| **Harvest Crop** | `crop_ash_grain` | Ash-Barley Grain | Unprocessed raw cereal sheaf | Thematic subject of tempering, moisture, and milling logs. |
| **Processed Milled** | `item_grain_flour` | Milled Ash-Barley Flour | Cooking ingredient (Hunger: +18) | Described in bolting and extraction yield records. |
| **Finished Food** | `item_flatbread` | Ash-Grain Flatbread | Baked staple food (Hunger: +22, Morale: +1) | Thematic culmination of clean sifting and milling. |
| **Emergency Rations** | `item_canned_grain_stew`| Sealed Ash-Grain Potage | Long-life winter ration | Preserved whole grain potage. |
| **Maintenance Item** | `item_silo_pest_treatment` | Silo Pest Treatment | Reduces silo pest pressure in `GrainProcessingSystem` | Directly contextualized by diatomaceous earth and CO2 records. |
| **Milling Recipe** | `recipe_ash_grain_flour` | Mill Ash-Barley Flour | 2 `crop_ash_grain` → 3 `item_grain_flour` (8.0h) | Unmodified baseline; extraction numbers remain descriptive. |

---

## 3. Narrative Measurement Crosswalk & Classification

| Authored Measurement | Range in Catalog | Live Mechanic Equivalent? | Presentation Tier | Boundary Rule |
|---|---|---|---|---|
| **`flour_extraction_yield_pct`** | 55.0% – 86.0% | `recipe_ash_grain_flour` output ratio (Fixed: 2 in → 3 out = 150% batch yield) | **Descriptive Only** (Codex & Inspection) | **Never modify recipe output**. The 55%–86% values describe historical separation efficiency of patent endosperm vs. bran in specific historical reels, not the quantity of flour added to inventory. |
| **`mesh_aperture_microns`** | 105.0 – 380.0 µm | None (Abstract sifter) | **Descriptive Only** (Codex & Technical Details) | Displayed as authentic historical instrumentation detail in inspection views. |
| **`cracks_per_inch_count`** | 8.0 – 18.0 cracks/in | None (Abstract millstone) | **Descriptive Only** (Codex & Maintenance Logs) | Illustrates stonemason craftsmanship; no effect on machine wear rate. |
| **`runner_rotational_rpm`** | 105.0 – 130.0 RPM | None (Abstract drive) | **Descriptive Only** (Codex & Mechanical Logs) | Does not alter shelter electric load or generator wattage. |
| **`grain_moisture_content_pct`**| 11.5% – 19.5% | `GrainSiloState.moisture_pct` (Baseline: 12.0%) | **Contextual Cross-Reference** | Historical silo readings provide lore context for why 12% is considered safe and >17% causes crusting/mold, but do NOT override live shelter silo moisture. |
| **`grain_temperature_celsius`** | 12.0°C – 48.0°C | None (No silo thermal sim) | **Descriptive Only** (Biological Hazard Logs) | 48°C hotspot serves as narrative illustration of fungal respiration. |
| **`tempering_water_addition_pct`**| 0.0% – 6.5% | None (No water required in `recipe_ash_grain_flour`) | **Descriptive Only** (Conditioning Assays) | Explains bran toughening without consuming shelter potable water. |
| **`conditioning_dwell_hours`** | 0.0 – 24.0 hours | None (No pre-soak step) | **Descriptive Only** (Conditioning Assays) | Explains hydration dwell times without imposing pre-crafting cooldowns. |

---

## 4. Item Inspection Integration

When inspecting `item_grain_flour` or `crop_ash_grain`, the UI may surface relevant discovered grain milling excerpts as archival provenance notes:
- Discovered **Millstone Dressing** entries describe the stone cut and grind quality of the flour.
- Discovered **Bolting Silk** entries explain the sifting mesh used to separate bran from patent endosperm.
- Discovered **Silo Weevil** entries explain the storage provenance and pest inspection history.
- Discovered **Tempering** entries document the kernel hydration and conditioning method.

All displayed excerpts carry the standard archival banner:
`[ARCHIVAL KNOWLEDGE — HISTORICAL OBSERVATION RECORDED AT SAMPLE TIME]`
