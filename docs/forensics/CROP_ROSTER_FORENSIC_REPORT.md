# Crop Roster Forensic Report

**Date:** 2026-09-19
**Scope:** Eight crop-roster questions — oilseed consumers, 10-plant hydroponic balance, underground-flora yield chains, cryo cultivar identity, end-to-end playtest, `hydroponic_crops.json` utilization classification, breeding seams, and greenhouse/kitchen seasonal UX.
**Companion plan:** `docs/plans/CROP_ROSTER_INTEGRATION_PLAN.md`

---

# 1. Target

1. Design the missing oil-pressing chain so `crop_oilseed` joins lamp oil, confit, and trade.
2. Stress-test the **10-plant hydroponic roster** (yield × hunger vs growth ticks).
3. Wire `fungus_spores_medicinal` / `fungus_spores_common` into pharma and poison-crafting so all four underground strains have yield chains.
4. Audit cryo lines that recover identical seed items.
5. Plan a 12-crop greenhouse playtest: seed → plant → blight → harvest → preservation → kitchen → trade.
6. Review `hydroponic_crops.json` `UNRESOLVED` and regenerate scanner artifacts.
7. Explore field×cryo breeding into player-created cultivars — existing seams only.
8. Check `GreenhousePanel` / `KitchenNutritionPanel` for seasonal best-window UX.

---

# 2. Executive Finding

Three live produce authorities, not one roster:

| Authority | Data | Count | Owner |
|---|---|---|---|
| Greenhouse field crops | `GreenhouseExpansionCatalog.CropCatalog` | 12 + 2 winter extras | `GreenhouseSystem` |
| Hydroponic rack cultivars | `hydroponic_crops.json` | 10 | `HydroponicBiomeSystem` (wired in `Main.AdvancedShelterSystems.cs`) |
| Underground flora | `underground_flora.json` | 4 | `FungiCultivationSystem` |

`crop_oilseed` **grows** but is `Material` with **no hunger, no press recipe, no lamp-oil item, no economy-goods row**. Docs promise lamp oil / pressing / confit. Live `craft_rendered_fat_confit` uses tuber + **fuel** + salt. `ChandlerySystem` does not exist.

The 10 hydroponic cultivars are **live**. `UNRESOLVED` was **scanner-table stale**: `ContentUtilizationScanner` omitted the file. Regenerating artifacts without maps leaves UNRESOLVED. Maps + rebuild + selftest move it to `GAMEPLAY_CONSUMED` and clear the deep-chain warn.

Four fungi strains yield. Biolum → light (LIVE). Grey mycelium → kitchen mash (LIVE, Plan 204). Medicinal spores sit on `NarcoticsSystem` (`chem_fungal_antibiotic`) but not `PharmaLabSystem`. Common spores are inoculum fallback only. Narcotic brew IDs `item_chem_fungal_antibiotic` / `item_chem_spore_sedative` were missing from `items.json`.

Cryo `traits[]` is unused. Several pairs recover the same canonical seed; only canister stats differ.

Breeding already exists as catalog `hardy_strain` mutation and hydroponic `TryStabilizeTrait`. Player-created cultivar IDs do not exist.

Seasonal best-windows are **documentation only**. `GreenhouseSystem.TickDay` takes `growLightHours` + ash; neither panel shows a season.

---

# 3. Evidence Summary

| Claim | Class | Evidence |
|---|---|---|
| 12 greenhouse crops | LIVE_CORE | `GreenhouseExpansionCatalog.CropCatalog.All` |
| Hydroponic 10 live | LIVE_CORE + LIVE_GODOT | `HydroponicCropCatalogLoader`; `Main.AdvancedShelterSystems.cs` |
| Baseline UNRESOLVED without maps | STALE SCANNER | `artifacts/content-utilization-baseline.json`; scanner had zero `hydroponic` string |
| Deep-chain warn | PARTIAL | `warn-greenhouse-crops` LOADER_WITHOUT_SYSTEM until maps |
| Oilseed not pressed | PARTIAL | no `crop_oilseed` in `recipes.json` |
| Confit ≠ oil | LIVE_CORE | `craft_rendered_fat_confit` uses `fuel` |
| No ChandlerySystem | PLANNED_ONLY | Batch 31 prose only |
| `item_lamp_oil` missing | GAP | `RadioDistressSystem.cs:738`; not in `items.json` |
| Medicinal in narcotics not PharmaLab | PARTIAL | `narcotics.json` vs `pharma_recipes.json` |
| Common spores inoculum only | PARTIAL | `FungiCultivationSystem.CultivateSpores` |
| Cryo traits unread | DATA_ONLY | single `traits` hit is the field |
| Strain mutation catalog-bound | LIVE_CORE | `AgricultureSystem.RollHardyVariant` |
| Seasonal matrix docs-only | PLANNED_ONLY | `SEASONAL_CROP_MATRIX.md`; no season in `GreenhouseSystem` |
| Kitchen hardcodes 6 recipes | LIVE_GODOT | `KitchenNutritionPanel.cs` |

---

# 4. Architecture Placement

```
items.json / greenhouse_items.json     hydroponic_crops.json      underground_flora.json
        │                                      │                           │
        ▼                                      ▼                           ▼
GreenhouseExpansionCatalog           HydroponicCropCatalog       FungiCultivationSystem
        │                                      │                           │
        ▼                                      ▼                           ▼
GreenhouseSystem ← AgricultureSystem HydroponicBiomeSystem       kitchen / narcotics / light
        │
        ▼
GreenhousePanel / KitchenNutritionPanel

cryo_cultivars.json → CryoVaultSystem → recovery_item_id (canonical seed)
crop_strains.json   → AgricultureSystem overlay
```

Do not merge these four owners.

---

# 5. Current Implementation

## 5.1 Greenhouse 12 (+ 2 winter)

Hours/water/light/yield match `GREENHOUSE_CROP_MATRIX.md`. Hunger from items:

| Yield | Hours | Hunger |
|---|---|---|
| `crop_mushroom` | 96 | 6 |
| `crop_tuber` | 144 | 12 |
| `crop_grain` | 192 | 18 |
| `crop_wheat` | 240 | 30 (unlock) |
| `crop_hardy_tuber` | 120 | 14 |
| `crop_ash_grain` | 168 | 20 |
| `crop_biolum_mushroom` | 84 | 5 |
| `crop_nutrient_algae` | 72 | 15 |
| `crop_medicinal_herb` | 160 | 2 (Medical) |
| `crop_leafy_green` | 60 | 8 |
| `crop_oilseed` | 210 | **none (Material)** |
| `crop_cold_legume` | 150 | 16 |

Plus `item_seed_frost_pea` / `item_seed_glacier_greens` (B5–B8). `seed_packets` aliases tuber.

### 5.1.1 The 10 Plants Fully Integrated (Zero Partial / Zero Blocked)

Across the 12 cataloged field crops, exactly 10 possess the complete unbroken chain:
`catalog entry → live loader → seed + crop items live in items.json → nutrition profile → at least one downstream consumer (recipe / preservation / feed / pharma) → art placeholder → test coverage`.

| # | Plant | ID | Why it's fully unblocked |
|---|---|---|---|
| 1 | Frost Tuber (Black Rutabaga) | `crop_hardy_tuber` | Live in GreenhouseExpansionCatalog; nutrition + preservation + Plan 196 food-type seam tests; it is regression scenario #6; fed by hydroponic Deep-Root Ash Tuber, 2 cryo lines, 1 field strain |
| 2 | Ash-Barley (Soot Rye) | `crop_ash_grain` | Richest chain in the repo: crafting recipes, GrainProcessingSystem + grain_processing.json, companion feed, preservation, 2 hydroponic crops, 2 cryo lines |
| 3 | Phosphor Cap Fungi | `crop_biolum_mushroom` | Consumed by KitchenNutritionPanel, preservation (drying), zero-light seasonal role, 2 hydroponic sources, Lantern-Cap cryo line, links into the Plan 204 fungi chain |
| 4 | Chlorella Slurry | `crop_nutrient_algae` | Aquaponics-integrated (aquaponics_system_catalog.json), preservation, nutrition, Radiotrophic Silt Kelp hydroponic source, Algae Strain V cryo line |
| 5 | Yarrow & Fever-Bark | `crop_medicinal_herb` | Crafting recipes and pharmaceutical tablet manufacturing, micro-location greenhouse integration tests, 2 cryo lines (Comfrey, Bittercress) |
| 6 | Winter Cress & Scurvy-Grass | `crop_leafy_green` | Recipes, companion-animal feed, KitchenNutritionPanel, green-manure compost recipe in crop_strains.json, 2 field strains, final-wish references |
| 7 | Iron Pea & Dun Vetch | `crop_cold_legume` | Recipes, brined legume mash preservation, nitrogen-fixation rotation mechanic (Seasonal Crop Matrix §2), 2 field strains, 2 cryo lines |
| 8 | Spore Mushroom | `crop_mushroom` | Baseline-live (planted/harvested per parity matrix), dried-mushroom preservation, bridges into the fully-verified Plan 204 subterranean cultivation system |
| 9 | Greenhouse Tuber | `crop_tuber` | Baseline-live, pickled-tubers preservation (regression scenario #11), agriculture persistence tests |
| 10 | Mutated Grain | `crop_grain` | Baseline-live, seasonal variance regression scenario #7, feeds the grain processing/milling chain |

*Note: `crop_oilseed` is partial due to the missing pressing/lamp oil chain (§5.3); `crop_wheat` is an unlock without symmetric preservation.*

## 5.2 Hydroponic 10

Days = germ + growth ticks. Yield item is a greenhouse crop ID.

| Cultivar | Days | Qty | Yield | Hunger | Proxy (qty×hunger) | /day |
|---|---|---|---|---|---|---|
| `crop_cold_tolerant_maize` | 11 | 5 | ash grain | 20 | 100 | 9.1 |
| `crop_winter_rye_x1` | 8 | 4 | ash grain | 20 | 80 | 10.0 |
| `crop_rad_scrubbing_kelp` | 5 | 5 | algae | 15 | 75 | 15.0 |
| `crop_hardy_ash_tuber` | 9 | 5 | hardy tuber | 14 | 70 | 7.8 |
| `crop_dwarf_soy_protein` | 8 | 4 | legume | 16 | 64 | 8.0 |
| `crop_iron_spinach_hybrid` | 5 | 4 | leafy | 8 | 32 | 6.4 |
| `crop_calcium_mushroom` | 7 | 3 | biolum | 5 | 15 | 2.1 |
| `crop_bioluminescence_lichen` | 6 | 3 | biolum | 5 | 15 | 2.5 |
| `crop_medicinal_poppy_v2` | 10 | 3 | herb | 2 | 6 | 0.6 |
| `crop_oilseed_brassica` | 11 | 4 | oilseed | 0 | **0** | **0** |

Kelp is the calorie king. Oilseed is a zero-calorie industrial crop until pressed. Rye/maize and agaric/lichen share yield IDs (same identity bug as cryo recovery).

No authored kcal/day. If a survivor burns ~20 hunger/day, four default kelp racks ≈ 3 survivor-days/day before blight/power/water.

## 5.3 Oilseed gap

Exists: seed, harvest, greenhouse, hydroponic brassica, cryo `cryo_seed_press_oilseed_line`, strain `strain_oilseed_press` (fats 0.9), item prose about a screw press.

Missing: recipe consuming `crop_oilseed`; ChandlerySystem; `item_lamp_oil` in `items.json`; economy-goods row; confit using oil. `craft_rendered_fat` renders **raw_meat** → `cooking_oil` (Food, hunger 10).

## 5.4 Four underground strains

| Strain | Yield | Downstream |
|---|---|---|
| `strain_phosphor_bracket` | `fungus_spores_bioluminescent` | inoculum + `GetBioluminescentLightOutput` |
| `strain_grey_mycelium` | `harvested_mushrooms_subterranean` | kitchen mash |
| `strain_cordyceps_mutant` | `fungus_spores_medicinal` | narcotics antibiotic; not PharmaLab |
| `strain_black_rot_mold` | `fungus_spores_common` | inoculum fallback only |

No poison-crafting system. Closest brew: `NarcoticsSystem.BrewChem`. `ChemWarfareSystem` is CBRN defense. `toxic_chemical_catalog.json` is hazard profiles.

## 5.5 Cryo collisions

Recovery pairs sharing seed: wheat, cold_legume, hardy_tuber, ash_grain, mushroom, medicinal_herb, hermetic ampoule. Traits are flavor. Live differences: recovery_amount, decay, storage_tier, difficulty, days, radiation_sensitivity.

## 5.6 Breeding seams

- `AgricultureSystem`: first-mature mutation → authored `result_strain_id`.
- `HydroponicBiomeSystem`: trait pool + `TryStabilizeTrait` (global, all new plantings).
- CropStrainCatalog: strain never invents a seed/yield.
- Cryo traits not consumed.

Player-created IDs = new save owner. **HOLD.**

---

# 6. Runtime Wiring

| System | Tick | Save |
|---|---|---|
| `GreenhouseSystem` | `TickDay(day, growLightHours, ash)` | `greenhouse` |
| `HydroponicBiomeSystem` | rack tick | `hydroponic_biomes` |
| `FungiCultivationSystem` | day + thermal | `fungi_cultivation` |
| `CryoVaultSystem` | viability decay | cryo section |
| `AgricultureSystem` | composes greenhouse | overlay |
| `KitchenNutritionSystem` | prep/spoil | kitchen |
| `PharmaLabSystem` | `pharma_recipes.json` | pharma |
| `NarcoticsSystem` | `narcotics.json` | narcotics |

`GreenhouseHostSession.TickDay` defaults `growLightHours = 6f`.

---

# 7. Data Flow

Greenhouse: seed → CropCatalog → stages → yield item.
Hydroponic: cultivar id in vault or inventory → rack → `base_yield_item_id`.
Fungi: spore (or common fallback) → yield.
Kitchen panel: hardcoded `RecipeOption[]` into `StartPrepJob` — does not load `recipes.json`.
PharmaLab vs Narcotics: two brew authorities.

---

# 8. State Ownership

No new owners for scanner/fungus/oil-as-recipes. Player cultivars would need a new owner — do not add.

---

# 9. Save/Load

Existing sections suffice for scanner maps, pharma/narcotics rows, and later `recipes.json` oil. Player cultivar IDs would need a new section.

---

# 10. Determinism

Greenhouse blight: persisted `blightRollCount` + `SeededRng`. Hydroponic mutation: injected `ISeededRng`. Agriculture mutation: campaign stream. No `System.Random` on these paths.

---

# 11. UI / Player Feedback

**GreenhousePanel:** stage, water, soil, growth, dry, blight, vault, plant/water/treat/harvest. Crop filter still mushroom/tuber/grain/wheat only. **No season.**

**KitchenNutritionPanel:** six hardcoded recipes covering `crop_biolum_mushroom`, `crop_leafy_green`, fungi harvest, rations, jerky, `item_grain_flour`. **No season. No oilseed.** Every recipe shows “High Vitamin C Equivalent”.

---

# 12. Tests

`GreenhouseCropExpansionTests`, `HydroponicBiomeTests`, `FungiCultivationPlan204Tests`, `CryoVaultB69Tests`, `PreservationAndCulinaryTests`, `NarcoticsSystemTests`, `ContentUtilizationGraphTests.CiGate_CommittedBaseline_*`, `--content-utilization-selftest` (rewrites manifest always; baseline only if missing).

---

# 13. Duplicates / Forks

Greenhouse vs hydroponic vs fungi vs cryo (intentional). `cooking_oil` vs missing `item_lamp_oil` vs `item_lamp_oil_crossing`. PharmaLab vs Narcotics vs ChemWarfare. Kitchen panel vs `recipes.json`. Docs `food_fat_confit` vs live `item_fat_confit`.

---

# 14. Existing Extension Seams

| Need | Seam |
|---|---|
| Oil press | `recipes.json` / CraftingSystem. Reuse `cooking_oil`. Do not create ChandlerySystem. |
| Confit truth | consume `cooking_oil` instead of `fuel` |
| Trade | `economy_goods.json` |
| Medicinal → pharma | additive `pharma_recipes.json` |
| Common → poison | additive `narcotics.json` toxin chem |
| Scanner | loaderPatterns + registryMap + consumerMap |
| Cryo identity | consume `traits[]` via agriculture/hydroponic overlay |
| Breeding | more `result_strain_id` rows; no player IDs |
| Seasonal UX | host read model into panel; panel must not own growth rules |

---

# 15. Functional Equivalents

Oil press: genuinely missing (animal `cooking_oil` is partial). Hydroponic balance: exists, unbalanced. Fungus chains: 2/4 complete. Cryo identity: partial (canister stats). Playtest: not authored. UNRESOLVED: stale maps. Player cultivars: genuinely new. Seasonal UX: missing.

---

# 16. Confirmed Gaps

G-OIL-1..5: no press recipe; Material hunger 0; confit ignores oil; `item_lamp_oil` missing; no goods row.
G-FUNGI-1..4: no pharma row; no toxin brew; missing `item_chem_*` outputs; `chem_spore_sedative` uses `item_clean_water` (not a live item).
G-CRYO-1..2: traits unread; seven recovery collisions.
G-SCAN-1: scanner omitted hydroponic file.
G-UI-1..2: no season; kitchen hardcoded + false scurvy.
G-BAL-1: proxy 0–100/cycle; duplicate yield IDs.
G-BREED-1: no player ID space.

---

# 17. Risks

| ID | Sev | Risk |
|---|---|---|
| R1 | HIGH | Full baseline rewrite hides drift — pin hydroponic key only |
| R2 | HIGH | New Chandlery/poison system forks authority |
| R3 | MED | Player cultivar IDs without save owner |
| R4 | MED | Panel-owned kitchen recipes |
| R5 | MED | Making oilseed edible collapses industrial identity |
| R6 | LOW | Unused cryo trait labels |

---

# 18. Constraints for Planning

One authority per concern. Core engine-free. Do not merge greenhouse/hydroponic/fungi/cryo. Scanner maps must name types present in Core/src. Selftest does not rewrite a present baseline. No munitions/poison-synthesis detail. Active XP/Wave11 claims do not cover these files.

---

# 19. Evidence Index

`GreenhouseExpansionCatalog.cs`, `GreenhouseSystem.cs`, `HydroponicCropCatalog.cs`, `HydroponicBiomeSystem.cs`, `Main.AdvancedShelterSystems.cs`, `FungiCultivationSystem.cs`, `AgricultureSystem.cs`, `CropStrainCatalog.cs`, `CryoVaultSystem.cs`, `PharmaLabSystem.cs`, `NarcoticsSystem.cs`, `ContentUtilizationScanner.cs`, `ContentDeepChainGate.cs`, `GreenhousePanel.cs`, `KitchenNutritionPanel.cs`, `GreenhouseHostSession.cs`, `ContentUtilizationSelfTest.cs`, catalogs under `Assets/StreamingAssets/Data/`, `artifacts/content-utilization-baseline.json`, production/greenhouse/Plan 204 docs.

---

# 20. Confidence & Unknowns

**High:** wiring; oilseed has no press; scanner omission; panel seasonal absence; cryo traits unread.
**Medium:** intended hunger/day; radio `item_lamp_oil` vs Crossing oil.
**Unknown:** whether some host path already maps Plan 19 season into `growLightHours` above GreenhouseHostSession.
**Not claimed:** full-suite run; live 12-crop playthrough.
