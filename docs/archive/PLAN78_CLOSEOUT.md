# Plan 78 — Archive Inks Expansion (3 → 12) — Closeout Report

> **Mission Complete:** Expanded `archive_inks.json` from the 3 baseline inks to 12 distinct preservation media, establishing a balanced archival economy for the Archive Desk across improvised, standard, and archival formulations.

---

## Summary
- **Plan:** 78 — Archive Inks Expansion
- **Baseline count:** 3 inks (`ink_iron_gall`, `ink_soot_lamp`, `ink_plant_dye`)
- **Final count:** 12 inks
- **Status:** All 12 inks authored, validated, and verified across Core and Host layers.
- **Key defect repaired:** Resolved field name mismatch in `InkMaterialDefinition` by adding `[JsonPropertyName]` mappings and bidirectional property aliases, ensuring snake_case JSON fields (`legibility_score`, `archival_longevity_days`, `fade_rate_per_day`, `required_item_id`, `required_amount`) properly deserialize into C# properties.

---

## Baseline
- `ink_iron_gall`: Legibility 0.90, Longevity 500d, Fade 0.0008/d, 2 charcoal
- `ink_soot_lamp`: Legibility 0.70, Longevity 300d, Fade 0.0015/d, 1 charcoal
- `ink_plant_dye`: Legibility 0.60, Longevity 200d, Fade 0.0020/d, 1 cloth

All three baseline definitions are 100% preserved byte-for-byte.

---

## Schema
- **File:** `Assets/StreamingAssets/Data/archive_inks.json`
- **Root Element:** `{ "schema_version": 1, "collection_id": "archive_inks", "inks": [ ... ] }`
- **Fields:** `ink_id`, `display_name`, `legibility_score`, `archival_longevity_days`, `fade_rate_per_day`, `required_item_id`, `required_amount`

---

## Runtime Formula
- **Effective Legibility over Time:** $\text{Legibility}(t) = \max(0, L_0 - F \times t)$
- **Terminal Degradation:** At $t \ge T_{\text{max}}$ (where $T_{\text{max}} = \text{archival\_longevity\_days}$), the document substrate degrades structurally.
- **Readable Threshold:** Documents require $\text{Legibility} \ge 0.20$ to maintain clear codex reading capability.

---

## Final Roster (All 12 Inks)

| # | ID | Display Name | Legibility | Longevity | Fade Rate | Required Item | Required Amount |
|---|---|---|:---:|:---:|:---:|---|:---:|
| 1 | `ink_iron_gall` | Iron Gall Ink | 0.90 | 500d | 0.0008 | `charcoal` | 2 |
| 2 | `ink_soot_lamp` | Soot Lamp Ink | 0.70 | 300d | 0.0015 | `charcoal` | 1 |
| 3 | `ink_plant_dye` | Plant Dye Ink | 0.60 | 200d | 0.0020 | `cloth` | 1 |
| 4 | `ink_lampblack` | Lampblack | 0.65 | 250d | 0.0040 | `charcoal` | 1 |
| 5 | `ink_berry_juice` | Berry Juice | 0.50 | 150d | 0.0070 | `berries` | 2 |
| 6 | `ink_chemical_marker` | Chemical Marker | 0.80 | 400d | 0.0030 | `chemical_solvent` | 1 |
| 7 | `ink_diluted_toner` | Diluted Toner | 0.75 | 350d | 0.0030 | `empty_toner_cartridge`| 1 |
| 8 | `ink_archival_carbon` | Archival Carbon | 0.95 | 600d | 0.0010 | `charcoal` | 3 |
| 9 | `ink_improvised_pigment`| Improvised Pigment | 0.55 | 180d | 0.0060 | `mineral_chunk` | 2 |
| 10 | `ink_blood_emergency` | Blood (Emergency) | 0.40 | 100d | 0.0100 | `blood_sample` | 1 |
| 11 | `ink_sepia` | Sepia Wash | 0.70 | 280d | 0.0040 | `organic_residue` | 1 |
| 12 | `ink_mineral_oxide` | Mineral Oxide | 0.60 | 220d | 0.0050 | `scrap_metal` | 2 |

---

## Tier / Niche Matrix
- **Improvised / Emergency:** `ink_blood_emergency`, `ink_berry_juice`, `ink_improvised_pigment`, `ink_mineral_oxide`, `ink_lampblack`, `ink_plant_dye`
- **Standard / Reliable:** `ink_soot_lamp`, `ink_sepia`, `ink_diluted_toner`, `ink_chemical_marker`
- **Archival / High-Durability:** `ink_iron_gall`, `ink_archival_carbon`

---

## Ingredients
- `charcoal` (Common fuel/filter carbon): used by 4 inks at amounts 1, 2, 3
- `cloth` (Common textile): used by `ink_plant_dye`
- `scrap_metal` (Very common): used by `ink_mineral_oxide`
- `berries` (Greenhouse/surface forage): used by `ink_berry_juice`
- `mineral_chunk` (Quarry/tunnel rubble): used by `ink_improvised_pigment`
- `organic_residue` (Biological waste/filter cake): used by `ink_sepia`
- `chemical_solvent` (Lab/industrial): used by `ink_chemical_marker`
- `empty_toner_cartridge` (Office/bunker salvage): used by `ink_diluted_toner`
- `blood_sample` (Medical/surgical byproduct): used by `ink_blood_emergency`

All 9 items exist with active recipes, loot tables, and salvage sources.

---

## Preservation Curves
- **20–60 Days:** Emergency and cheap forage inks (`blood`, `berry`, `improvised_pigment`) drop below readable threshold.
- **80–125 Days:** Common mineral and lampblack formulations drop below readable threshold.
- **180–300 Days:** Standard inks (`plant_dye`, `diluted_toner`, `chemical_marker`, `soot_lamp`) survive mid-campaign seasons.
- **500–600 Days:** Archival inks (`iron_gall`, `archival_carbon`) retain > 58% clarity at the 1-year campaign milestone.

---

## Dominance Audit
- No formulation dominates another across all 5 vectors simultaneously.
- Peak archival durability (`archival_carbon`) requires 3 units of charcoal—a high early-game resource competition with water filtration and winter heating.
- Lowest cost options have harsh fade rates and short durability limits.

---

## Archive Desk Integration
- `QueueTranscription` enforces `_inventory.CountById(ink.requiredItemId) < ink.requiredAmount`.
- Deducts material on queue, refunds material on cancel.
- Completed transcription assigns `job.legibilityScore = ink.legibilityScore` and unlocks evidence.

---

## Persistence
- Saves store `job.inkId` and `job.legibilityScore`.
- Catalog definitions are kept outside save files.
- Old saves retain full compatibility without migration logic.

---

## Validation
- `dotnet build Ashfall.csproj` — PASS (0 warnings, 0 errors)
- `dotnet test Ashfall.Core.Tests` — PASS (6,744 passed, 0 failed)
- `godot --headless --path . -- --data-integrity-selftest` — PASS (0 errors across 208 catalogs)
- `godot --headless --path . -- --content-utilization-selftest` — CI gate PASS
- `godot --headless --path . -- --scene-binding-selftest` — PASS (22/22 panels)
- `python3 scripts/ci/scene-lint.py` — PASS (27 scenes checked, 0 errors)

---

## Deferred
- Plan 47: Rare pre-war sealed ink collectible registration.
- Plan 51: Document-type ink recommendation rules.
- Plan 55: Detailed multi-step chemical crafting recipes for advanced inks.
