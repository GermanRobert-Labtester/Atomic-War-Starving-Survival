# Crop Roster Forensic Report

**Date:** 2026-09-19
**Scope:** Eight crop-roster questions — oilseed consumers, 10-plant hydroponic balance, underground-flora yield chains, cryo cultivar identity, end-to-end playtest, `hydroponic_crops.json` utilization classification, breeding seams, and greenhouse/kitchen seasonal UX.
**Mode:** Evidence-first, read-only of production systems at inspection time. Subsequent authorized wiring of scanner maps and fungus consumers is recorded in `docs/plans/CROP_ROSTER_INTEGRATION_PLAN.md`.

---

# 1. Target

Operational targets (eight related questions, one agricultural surface):

1. Design the missing oil-pressing consumer chain so `crop_oilseed` can join the fully-integrated roster (lamp oil, rendered-fat confit, trade).
2. Stress-test the **10-plant hydroponic roster** for calorie-per-day yields vs growth ticks.
3. Wire `fungus_spores_medicinal` and `fungus_spores_common` into pharma and poison-crafting so all four underground flora strains have full yield chains.
4. Audit cryo cultivar lines whose recovery items collide.
5. Plan a vertical-slice playtest of the **12 greenhouse crops** (seed → plant → blight → harvest → preservation → kitchen → trade).
6. Review whether `hydroponic_crops.json` `UNRESOLVED` in the utilization baseline is stale, and regenerate the scanner artifact.
7. Explore plant-breeding where field strains and cryo cultivars combine into player-created cultivars — existing seams only.
8. Check `GreenhousePanel` and `KitchenNutritionPanel` for seasonal best-window communication of the 12-crop roster.

**Domains:** Greenhouse (`GreenhouseSystem`), hydroponics (`HydroponicBiomeSystem`), fungi (`FungiCultivationSystem`), cryo vault (`CryoVaultSystem`), agriculture overlay (`AgricultureSystem` / `crop_strains.json`), kitchen (`KitchenNutritionSystem`), crafting (`recipes.json`, `pharma_recipes.json`, `narcotics.json`), content utilization scanner.

**Synonyms searched:** oil press, chandlery, lamp oil, cooking_oil, rendered fat, confit, cultivar, cryo, hydroponic, greenhouse crop, fungus spores, pharma, poison, toxin, breed, mutation, seasonal window.

---

# 2. Executive Finding

There are **three live crop authorities**, not one roster:

| Authority | Catalog | Count | Runtime owner |
|---|---|---|---|
| Greenhouse field crops | `GreenhouseExpansionCatalog.CropCatalog` (hardcoded) + `items.json` / `greenhouse_items.json` | 12 live seeds + 2 B5–B8 winter extras (`frost_pea`, `glacier_greens`) | `GreenhouseSystem` |
| Hydroponic cryo/rack cultivars | `hydroponic_crops.json` | 10 | `HydroponicBiomeSystem` (wired in `Main.AdvancedShelterSystems.cs`) |
| Underground flora | `underground_flora.json` | 4 strains | `FungiCultivationSystem` |

`crop_oilseed` is **grown and harvested** but is typed `Material` with **no hunger, no press recipe, no lamp-oil output, and no economy-goods row**. Docs promise lamp oil / pressing / confit; live confit uses `crop_tuber` + `fuel` + salt, not oilseed. `ChandlerySystem` does not exist.

The 10 hydroponic cultivars are **live** (loader + system + tests + save). Their `UNRESOLVED` utilization class is **scanner-table stale**, not a missing consumer. `ContentUtilizationScanner` never lists `hydroponic_crops.json`. Regenerating artifacts without adding scanner maps would leave the class unchanged.

Four underground strains already yield. Two already have downstream chains (bioluminescence light, kitchen mash). Medicinal spores are authored on `NarcoticsSystem` (`chem_fungal_antibiotic`) but **not** on `PharmaLabSystem`. Common spores are inoculum fallback only — **no poison-craft consumer**. Narcotic brew outputs `item_chem_fungal_antibiotic` / `item_chem_spore_sedative` are **missing from `items.json`**.

Cryo lines recover **canonical seed IDs**. `traits[]` is catalog flavor: `CryoVaultSystem` never reads it. Several pairs are storage-stat variants of the same seed.

Breeding **already exists** as catalog-bound mutation (`AgricultureSystem` → authored `result_strain_id`) plus hydroponic rack traits (`TryStabilizeTrait`). Player-created cultivar IDs do **not** exist; adding them would be a new save-backed authority.

Seasonal best-windows exist as **documentation only**. `GreenhouseSystem.TickDay` takes `growLightHours` and ash rate; it does not read Plan 19 season. Neither panel displays a seasonal window.

**Confidence:** High on wiring/data/classification. Medium on intended daily calorie budget (no authored kcal/day table; `hungerRestore` is the live proxy).

---

# 3. Evidence Summary

| Claim | Class | Evidence |
|---|---|---|
| 12 greenhouse crops in Core catalog | LIVE_CORE | `GreenhouseExpansionCatalog.CropCatalog.All` |
| Hydroponic 10 cultivars loaded and simulated | LIVE_CORE + LIVE_GODOT | `HydroponicCropCatalogLoader`, `Main.AdvancedShelterSystems.cs:336`, `HydroponicBiomeTests` |
| `hydroponic_crops.json` baseline `UNRESOLVED` | STALE SCANNER MAP | `artifacts/content-utilization-baseline.json:111`; scanner has **zero** `hydroponic` string |
| Deep-chain warn LOADER_WITHOUT_SYSTEM | PARTIAL | `artifacts/content-utilization-deep-chain.json` hop `warn-greenhouse-crops` |
| Oilseed grown, not pressed | PARTIAL | `items.json` `crop_oilseed` type Material; no `crop_oilseed` in `recipes.json` |
| Confit recipe ≠ oilseed | LIVE_CORE (wrong inputs vs docs) | `recipes.json` `craft_rendered_fat_confit` uses `crop_tuber` + `fuel` + salt |
| No ChandlerySystem | PLANNED_ONLY | Batch 31 plan only; zero `class Chandlery` |
| `item_lamp_oil` revealed, not catalogued | DATA_ONLY / GAP | `RadioDistressSystem.cs:738`; no `item_lamp_oil` in `items.json` |
| Four fungi strains authored | DATA + LIVE_CORE | `underground_flora.json`; `FungiCultivationSystem` |
| Medicinal spores in narcotics, not pharma lab | PARTIAL | `narcotics.json` `chem_fungal_antibiotic`; `pharma_recipes.json` has no fungus IDs |
| Common spores = inoculum only | PARTIAL | `FungiCultivationSystem.CultivateSpores` fallback |
| Cryo traits unused at runtime | DATA_ONLY | `CryoCultivarDef.traits`; single `traits` hit in `CryoVaultSystem.cs` (the field) |
| Strain mutation is catalog-bound | LIVE_CORE | `AgricultureSystem.RollHardyVariant`; `crop_strains.json` `result_strain_id` |
| Hydroponic trait stabilize exists | LIVE_CORE | `HydroponicBiomeSystem.TryStabilizeTrait` |
| Seasonal matrix is docs-only | PLANNED_ONLY vs runtime | `docs/production/SEASONAL_CROP_MATRIX.md`; no season in `GreenhouseSystem` / panels |
| Kitchen panel hardcodes 6 recipes | LIVE_GODOT (presentation authority leak) | `KitchenNutritionPanel.cs:209-226` |

---

# 4. Architecture Placement

```
items.json / greenhouse_items.json          hydroponic_crops.json           underground_flora.json
        │                                           │                                │
        ▼                                           ▼                                ▼
GreenhouseExpansionCatalog.CropCatalog    HydroponicCropCatalog           FungiCultivationSystem
        │                                           │                                │
        ▼                                           ▼                                ▼
GreenhouseSystem  ← AgricultureSystem     HydroponicBiomeSystem           kitchen / narcotics / light
        │                                 (racks, traits, seed vault)
        ▼
GreenhousePanel / KitchenNutritionPanel

cryo_cultivars.json → CryoVaultSystem → recovery_item_id (canonical seed)
crop_strains.json   → AgricultureSystem overlay on greenhouse plots
```

**Constraint:** Do not merge greenhouse, hydroponic racks, fungi beds, and cryo vault into one system. Each already has a save section and host.

---

# 5. Current Implementation

## 5.1 Greenhouse 12-crop roster (plus 2 winter extras)

Canonical IDs and live growth numbers live in `GreenhouseExpansionCatalog`, matching `docs/production/GREENHOUSE_CROP_MATRIX.md` for the original 12.

| Seed | Yield | Hours | Water/d | Light h | Base yield | Hunger (item) |
|---|---|---|---|---|---|---|
| `item_seed_mushroom` | `crop_mushroom` | 96 | 8 | 4 | 2 | 6 |
| `item_seed_tuber` | `crop_tuber` | 144 | 12 | 6 | 3 | 12 |
| `item_seed_grain` | `crop_grain` | 192 | 16 | 8 | 4 | 18 |
| `item_seed_wheat` | `crop_wheat` | 240 | 20 | 10 | 6 | 30 (unlock) |
| `item_seed_hardy_tuber` | `crop_hardy_tuber` | 120 | 10 | 5 | 3 | 14 |
| `item_seed_ash_grain` | `crop_ash_grain` | 168 | 14 | 6 | 4 | 20 |
| `item_seed_biolum_mushroom` | `crop_biolum_mushroom` | 84 | 6 | 2 | 2 | 5 |
| `item_seed_nutrient_algae` | `crop_nutrient_algae` | 72 | 18 | 8 | 5 | 15 |
| `item_seed_medicinal_herb` | `crop_medicinal_herb` | 160 | 12 | 7 | 3 | 2 (Medical type) |
| `item_seed_leafy_green` | `crop_leafy_green` | 60 | 10 | 6 | 3 | 8 |
| `item_seed_oilseed` | `crop_oilseed` | 210 | 15 | 9 | 4 | **none (Material)** |
| `item_seed_cold_legume` | `crop_cold_legume` | 150 | 11 | 6 | 4 | 16 |
| `item_seed_frost_pea` | `crop_frost_pea` | 168 | 8 | 2 | 4 | (B5–B8 extra) |
| `item_seed_glacier_greens` | `crop_glacier_greens` | 120 | 6 | 3 | 3 | (B5–B8 extra) |

`seed_packets` aliases the tuber profile (F18).

## 5.2 Hydroponic 10-cultivar roster

Indoor LED racks. Cycle length = `germination_ticks + growth_ticks` campaign days. Yield item is a **greenhouse crop ID**, not a unique hydroponic produce ID.

| Cultivar ID | Days | Qty | Yield item | Item hunger | Calorie proxy (qty×hunger) |
|---|---|---|---|---|---|
| `crop_winter_rye_x1` | 8 | 4 | `crop_ash_grain` | 20 | 80 |
| `crop_rad_scrubbing_kelp` | 5 | 5 | `crop_nutrient_algae` | 15 | 75 |
| `crop_calcium_mushroom` | 7 | 3 | `crop_biolum_mushroom` | 5 | 15 |
| `crop_oilseed_brassica` | 11 | 4 | `crop_oilseed` | 0 | **0** |
| `crop_medicinal_poppy_v2` | 10 | 3 | `crop_medicinal_herb` | 2 | 6 |
| `crop_dwarf_soy_protein` | 8 | 4 | `crop_cold_legume` | 16 | 64 |
| `crop_hardy_ash_tuber` | 9 | 5 | `crop_hardy_tuber` | 14 | 70 |
| `crop_bioluminescence_lichen` | 6 | 3 | `crop_biolum_mushroom` | 5 | 15 |
| `crop_iron_spinach_hybrid` | 5 | 4 | `crop_leafy_green` | 8 | 32 |
| `crop_cold_tolerant_maize` | 11 | 5 | `crop_ash_grain` | 20 | 100 |

Two pairs dump identical food: rye+maize → `crop_ash_grain`; calcium mushroom+lichen → `crop_biolum_mushroom`.

## 5.3 Oilseed consumer chain (missing)

**Exists:**

- Seed + harvest items (`item_seed_oilseed`, `crop_oilseed`).
- Greenhouse and hydroponic (`crop_oilseed_brassica`) production.
- Cryo recovery `cryo_seed_press_oilseed_line` → `item_seed_oilseed`.
- Agriculture overlay `strain_oilseed_press` (nutrition fats 0.9) on greenhouse oilseed.
- Item prose claims a screw press and lamp fuel.

**Does not exist:**

- Recipe consuming `crop_oilseed`.
- `ChandlerySystem` / oil-press station.
- Canonical `item_lamp_oil` in `items.json` (Crossing has `item_lamp_oil_crossing`; radio reveals `item_lamp_oil`).
- `crop_oilseed` on `economy_goods.json`.
- Confit that consumes pressed oil. Live `craft_rendered_fat_confit` = tuber + **fuel** + salt → `item_fat_confit`. Docs say tuber + rendered fat + salt.
- `craft_rendered_fat` renders **raw_meat** → `cooking_oil` (Food, hunger 10). Animal fat, not seed oil.

## 5.4 Underground flora (four strains)

| Strain | Category | Yield | Downstream at inspection |
|---|---|---|---|
| `strain_phosphor_bracket` | Bioluminescent | `fungus_spores_bioluminescent` | Inoculum + `GetBioluminescentLightOutput` (LIVE) |
| `strain_grey_mycelium` | Edible | `harvested_mushrooms_subterranean` | Kitchen mash (LIVE, Plan 204) |
| `strain_cordyceps_mutant` | Medicinal | `fungus_spores_medicinal` | `narcotics.json` `chem_fungal_antibiotic` (PARTIAL — not PharmaLab) |
| `strain_black_rot_mold` | Toxic | `fungus_spores_common` | Inoculum fallback only |

`chem_spore_sedative` consumes **harvested mushrooms**, not medicinal spores.

No dedicated poison-crafting system. Closest live brew authority is `NarcoticsSystem.BrewChem`. `ChemWarfareSystem` is CBRN defense, not a craft bench. `toxic_chemical_catalog.json` is hazard profiles.

## 5.5 Cryo cultivars

18 lines in `cryo_cultivars.json`. Recovery is always an existing seed/ampoule. `traits[]` is unused by `CryoVaultSystem`.

Colliding recovery pairs:

| Recovery item | Lines | Trait labels (flavor only) |
|---|---|---|
| `item_seed_wheat` | Radiant-Wept vs Verity Common | radiation_tolerant+heirloom vs heirloom |
| `item_seed_cold_legume` | Cold Legate vs Deep-Furrow | protein_rich+frost_hardy vs protein_rich |
| `item_seed_hardy_tuber` | Hardtack vs Glacier Bed | frost_hardy+heirloom vs frost_hardy |
| `item_seed_ash_grain` | Ash Grain vs Veil Terrace | radiation_tolerant vs rapid_growth |
| `item_seed_mushroom` | Pale-Heart vs Cellar-Rot | low_light vs low_light+rapid_growth |
| `item_seed_medicinal_herb` | Comfrey vs Bittercress | pharmaceutical_yield vs pharmaceutical_yield+heirloom |
| `item_hermetic_sample_ampoule` | Plate-Remedy vs Iodine Yeast | pharmaceutical_yield vs pharmaceutical_yield+rapid_growth |

Gameplay differences that **do** fire: `recovery_amount`, decay rates, `storage_tier`, `recovery_difficulty`, `recovery_days`, `radiation_sensitivity`. Identity after recovery is the same seed.

## 5.6 Breeding seams (already present)

A. **AgricultureSystem** (Plan 162): greenhouse plot carries `strain_id`. On first mature tick, radiation/toxicity vs `mutation_threshold` rolls authored `mutation_outcomes`. `hardy_strain` writes `mutation_strain_id` from catalog `result_strain_id` (e.g. `strain_tuber_heirloom` → `strain_tuber_radline`). No new IDs.

B. **HydroponicBiomeSystem**: radiation + `mutationAffinity` adds a trait from a 5-string pool. `TryStabilizeTrait` spends chemicals+water and copies unlocked traits onto **every new planting**. Not a new cultivar record.

C. **CropStrainCatalog** contract: a strain never invents a new seed or yield.

D. Cryo `traits[]` is not consumed.

Player-created cultivars combining field × cryo would be **genuinely new** (new IDs, save, validator, UI).

---

# 6. Runtime Wiring

| System | Created by | Tick | Save |
|---|---|---|---|
| `GreenhouseSystem` | Greenhouse host | `TickDay(day, growLightHours, ash)` | `greenhouse` |
| `HydroponicBiomeSystem` | `Main.AdvancedShelterSystems` | rack `TickDay` | `hydroponic_biomes` |
| `FungiCultivationSystem` | Plans 190–193 / 204 host | `TickDay` + thermal | `fungi_cultivation` |
| `CryoVaultSystem` | B69 host | viability decay | cryo vault section |
| `AgricultureSystem` | Plan 162 host | composes greenhouse + mutation | agriculture overlay |
| `KitchenNutritionSystem` | Kitchen host | prep jobs / spoilage | kitchen state |
| `PharmaLabSystem` | Crafting host | `pharma_recipes.json` | pharma lab |
| `NarcoticsSystem` | Plans 182–185 | `narcotics.json` brew | narcotics profiles |

`GreenhouseHostSession.TickDay` defaults `growLightHours = 6f` — not a Plan 19 season table.

---

# 7. Data Flow

**Greenhouse:** seed item → `CropCatalog.Get` → plot stages → harvest yield item → inventory.

**Hydroponic:** cultivar id in seed vault **or** inventory cost of cultivar id → rack → `base_yield_item_id` into inventory.

**Fungi:** spore item (or common fallback) → plot → yield item.

**Kitchen panel:** hardcoded `RecipeOption[]` passed as `inputRequirements` into `KitchenNutritionSystem.StartPrepJob`. Core kitchen does not load `recipes.json` for those six meals. Preservation recipes in `recipes.json` are the CraftingSystem path.

**Pharma lab vs narcotics:** two brew authorities. PharmaLab uses `pharma_recipes.json`. Narcotics uses `narcotics.json` `recipe_inputs` and emits `item_{chem_id}`.

---

# 8. State Ownership

No new owners proposed for oil/fungus/scanner work. Breeding player cultivars would need a new owner — **do not add** without a signed package.

Cryo traits are catalog fields, not runtime state.

Hydroponic `unlockedStabilizedTraits` is already persisted.

---

# 9. Save/Load

Existing sections: `greenhouse`, `hydroponic_biomes`, `fungi_cultivation`, cryo vault, agriculture overlay, kitchen, pharma, narcotics.

Oil-pressing as `recipes.json` rows needs **no new save section**.

Fungus pharma/narcotics rows need **no new save section**.

Scanner baseline is an artifact, not a save.

Player-created cultivars would require a new save section — out of scope.

---

# 10. Determinism

Greenhouse blight: `SeededRng(_seed * 397 + blightRollCount)` with persisted count.

Hydroponic mutation: `_rng.NextDouble()` on the injected `ISeededRng`.

Agriculture mutation: campaign mutation stream.

Fungi Plan 204: seeded substrate prep; bloom threshold deterministic.

No `System.Random` found on these paths.

---

# 11. UI / Player Feedback

**GreenhousePanel:** beds, stage, water, soil mSv, growth %, dry warning, seed name, blight/vault/supply rail, plant/water/treat/harvest (Plan 22). Sidebar filters: all / fallow / damaged / ready / apiary. Crop-type filter still only mushroom/tuber/grain/wheat (comment at line 39). **No season, no best-window, no 12-crop roster legend.**

**KitchenNutritionPanel:** six hardcoded recipes (fungal stew, subterranean mash, canned mash, greenhouse salad, jerky broth, ash-flour bread). Covers 2 of 12 greenhouse yields (`crop_biolum_mushroom`, `crop_leafy_green`) plus fungi harvest. **No season. No oilseed. No preservation recipes.** "Scurvy Prevention: High Vitamin C Equivalent" is shown for every selected recipe (untrue for jerky/canned mash).

**ExpansionsHubPanel:** hydroponic status strip, not per-cultivar seasons.

**FungiCultivationBedPanel:** biolum light output.

---

# 12. Tests & Verification

| Target | What it proves |
|---|---|
| `GreenhouseCropExpansionTests` | 8 Plan-22 seed/yield pairs plantable |
| `HydroponicBiomeTests` | catalog load, plant, mutate, persist |
| `FungiCultivationPlan204Tests` | harvest edible + kitchen-routable |
| `FungiCultivationSystemTests` | biolum light, common-spore inoculum |
| `CryoVaultB69Tests` | recovery item IDs include oilseed |
| `PreservationAndCulinaryTests` | confit recipe id/output/station (not oilseed) |
| `NarcoticsSystemTests` | brew of hyper-stim, not fungal chems |
| `ContentUtilizationGraphTests.CiGate_CommittedBaseline_*` | scan vs baseline has no regressions/new orphans |
| `ContentDeepChainGateTests` | synthetic graph; hydroponic edge is **DutyRosterCatalog** (fixture, not production) |
| `--content-utilization-selftest` | writes `content-utilization.{json,md}` + deep-chain; baseline rewrite only if file missing |
| `--greenhouse-selftest` | greenhouse host loop |

---

# 13. Duplicates / Legacy / Forks

- Greenhouse vs hydroponic vs fungi vs cryo: **four** produce-food surfaces. Intentional split (Plan 162 recon: racks out of greenhouse scope).
- `cooking_oil` vs missing `item_lamp_oil` vs `item_lamp_oil_crossing`.
- `PharmaLabSystem` vs `NarcoticsSystem` vs `ChemWarfareSystem`.
- Kitchen panel recipes vs `recipes.json` preservation recipes vs `KitchenNutritionSystem` generic prep jobs.
- Docs `food_fat_confit` vs live `item_fat_confit`.
- Scanner maps vs live `HydroponicCropCatalogLoader`.

---

# 14. Existing Extension Seams

| Need | Seam |
|---|---|
| Oil press | `recipes.json` + `RecipeCatalog` / `CraftingSystem`. Reuse `cooking_oil` and/or add Fuel `item_pressed_lamp_oil`. Do not create ChandlerySystem. |
| Confit truth | Change `craft_rendered_fat_confit` inputs to consume pressed oil (`cooking_oil`) instead of `fuel`. |
| Trade | `economy_goods.json` row for `crop_oilseed` / pressed oil. |
| Medicinal spores → pharma lab | Additive `pharma_recipes.json` row; output existing `antibiotics`. |
| Common spores → poison brew | Additive `narcotics.json` toxin chem (only live brew path). Not ChemWarfare, not chemical_syntheses munitions. |
| Scanner class | `ContentUtilizationScanner` loaderPatterns + registryMap + consumerMap (+ querySystems/UI). |
| Cryo identity | Consume `traits[]` in greenhouse/agriculture **or** unique recovery item IDs. Do not invent a second crop catalog. |
| Breeding | Extend `crop_strains.json` outcomes and/or hydroponic trait pool. Do not add player-authored IDs without a save owner. |
| Seasonal UX | Presentation of host-sampled season + catalog window table. Core must not grow a parallel season table in the panel. |

---

# 15. Functional Equivalents

| Request | Verdict |
|---|---|
| Oil-pressing chain | **Genuinely missing** (C). Animal `cooking_oil` is a partial equivalent. |
| 10-plant balance | **Exists**; numbers are unbalanced once oilseed hunger=0 is counted. |
| Fungus full chains | **Partial equivalent** (C): 2/4 complete; medicinal narcotics-only; common inoculum-only. |
| Cryo identity | **Partial**: storage stats differ; recovered seed does not. |
| Playtest scenario | **Not authored**. Systems exist to support one. |
| UNRESOLVED stale? | **Yes (scanner maps).** Gameplay consumer is live. |
| Player-created cultivars | **Genuinely new** (E). Catalog mutation + stabilize-trait are partial equivalents. |
| Seasonal UX | **Missing** (docs-only). |

---

# 16. Confirmed Gaps

1. **G-OIL-1:** No recipe consumes `crop_oilseed`.
2. **G-OIL-2:** `crop_oilseed` is Material, hunger 0 — hydroponic brassica calorie proxy is 0.
3. **G-OIL-3:** Confit recipe does not use oilseed or `cooking_oil`.
4. **G-OIL-4:** `item_lamp_oil` is referenced by radio, absent from `items.json`.
5. **G-OIL-5:** `crop_oilseed` absent from `economy_goods.json`.
6. **G-FUNGI-1:** `pharma_recipes.json` has no fungus inputs.
7. **G-FUNGI-2:** `fungus_spores_common` has no poison/toxin brew.
8. **G-FUNGI-3:** `item_chem_fungal_antibiotic` and `item_chem_spore_sedative` missing from `items.json` (narcotics brew IDs).
9. **G-FUNGI-4:** `chem_spore_sedative` uses `item_clean_water`, which is not an item ID (live `clean_water`).
10. **G-CRYO-1:** `traits[]` never read.
11. **G-CRYO-2:** Seven recovery-item collisions.
12. **G-SCAN-1:** Scanner tables omit `hydroponic_crops.json` → UNRESOLVED despite live loader/system.
13. **G-UI-1:** GreenhousePanel has no seasonal window and filters only 4 of 12 crops.
14. **G-UI-2:** KitchenNutritionPanel hardcodes recipes; no season; false scurvy line.
15. **G-BAL-1:** Hydroponic calorie proxy spans 0–100 per cycle; two cultivar pairs share yield IDs.
16. **G-BREED-1:** No player-created cultivar ID space.

---

# 17. Risks

| ID | Severity | Risk |
|---|---|---|
| R1 | HIGH | Blind full baseline rewrite of `content-utilization-baseline.json` can hide unrelated classification drift. Pin hydroponic only after maps are added. |
| R2 | HIGH | New ChandlerySystem or poison-crafting system would fork authority. |
| R3 | MEDIUM | Player-created cultivar IDs without a save owner corrupt/fork agriculture state. |
| R4 | MEDIUM | Panel-owned kitchen recipes drift from `recipes.json`. |
| R5 | MEDIUM | Making oilseed edible (Food) without a press collapses its industrial identity. |
| R6 | LOW | Cryo trait labels vs unused runtime confuse content authors. |
| R7 | LOW | Deep-chain test fixture edges hydroponic to DutyRosterCatalog — do not treat as production wiring. |

---

# 18. Constraints for Planning

1. One authority per concern: oil via `recipes.json`/`CraftingSystem`; medicinal via existing pharma **or** narcotics (prefer PharmaLab for "pharma"); toxin via Narcotics brew, not a new poison system.
2. Core stays engine-free.
3. Do not merge greenhouse / hydroponic / fungi / cryo.
4. JSON is authoritative for recipes and hydroponic cultivars; greenhouse crop **stats** are still hardcoded in `GreenhouseExpansionCatalog` (do not silently JSON-ify in this package).
5. Scanner maps must name types that appear in Core/src (VerifyConsumersInSource substring check).
6. `--content-utilization-selftest` does **not** rewrite a present baseline. Update the hydroponic key (or delete baseline only as a last resort).
7. Kitchen panel must not become a new recipe authority when adding oil/fungus meals.
8. No real-world munitions/poison-synthesis detail.
9. Active claims (XP difficulty, Wave 11 part 2) do not cover these files.

---

# 19. Evidence Index

- `Assets/Ashfall.Core/Greenhouse/GreenhouseExpansionCatalog.cs`
- `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs`
- `Assets/Ashfall.Core/Shelter/HydroponicCropCatalog.cs`
- `Assets/Ashfall.Core/Shelter/HydroponicBiomeSystem.cs`
- `src/Main.AdvancedShelterSystems.cs`
- `Assets/Ashfall.Core/Farming/FungiCultivationSystem.cs`
- `Assets/Ashfall.Core/Farming/AgricultureSystem.cs`
- `Assets/Ashfall.Core/Farming/CropStrainCatalog.cs`
- `Assets/Ashfall.Core/Shelter/CryoVaultSystem.cs`
- `Assets/Ashfall.Core/PharmaLabSystem.cs`
- `Assets/Ashfall.Core/Medical/NarcoticsSystem.cs`
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`
- `Assets/Ashfall.Core/Content/ContentDeepChainGate.cs`
- `src/UI/GreenhousePanel.cs`
- `src/UI/KitchenNutritionPanel.cs`
- `src/Host/GreenhouseHostSession.cs`
- `src/Host/ContentUtilizationSelfTest.cs`
- `Assets/StreamingAssets/Data/{hydroponic_crops,cryo_cultivars,underground_flora,crop_strains,recipes,pharma_recipes,narcotics,items,greenhouse_items}.json`
- `artifacts/content-utilization-baseline.json`
- `artifacts/content-utilization-deep-chain.json`
- `docs/production/{GREENHOUSE_CROP_MATRIX,SEASONAL_CROP_MATRIX,PRESERVATION_RECIPE_MATRIX,PRODUCTION_CONTENT_UTILIZATION}.md`
- `docs/shelter/PLAN_204_MUSHROOM_CULTIVATION_CLOSEOUT.md`
- `docs/greenhouse/PLAN91_CLOSEOUT.md`

---

# 20. Confidence & Unknowns

**High:** wiring of greenhouse/hydroponic/fungi/cryo; oilseed has no press recipe; scanner omission; panel seasonal absence; cryo traits unread; two chem brew authorities.

**Medium:** intended survivor kcal/day (hungerRestore used as proxy; NeedsSystem daily burn not re-derived in this pass). Whether radio `item_lamp_oil` should be created vs aliased to Crossing lamp oil.

**Unknown:** whether Plan 19 seasonal provider is already sampled into `growLightHours` by some host path not named GreenhouseHostSession (AgricultureSystem uses `LightingAvailabilityPermille` from an env struct — seasonal coupling may exist one layer up). Not required to explain the panel gap.

**Not claimed:** full-suite test run; Godot playthrough of all 12 crops.
