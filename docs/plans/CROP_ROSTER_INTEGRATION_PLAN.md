# Crop Roster Integration Plan

**Date:** 2026-09-19
**Forensic source:** `docs/forensics/CROP_ROSTER_FORENSIC_REPORT.md`
**Authorization:** user 2026-09-19 — complete the eight-item crop-roster package.

---

# 1. Objective

Make the agricultural roster honest end-to-end without new parallel authorities:

1. Register the already-live hydroponic catalog with the utilization scanner and refresh artifacts.
2. Close underground-flora yield chains: medicinal spores → PharmaLab; common spores → Narcotics toxin brew; catalog the missing brew output items.
3. Design (do not invent a ChandlerySystem) the oilseed press → lamp oil / confit / trade chain on `recipes.json`.
4. Record balance findings for the 10 hydroponic cultivars and a 12-crop playtest script.
5. Propose cryo trait differentiation that reuses greenhouse/agriculture seams.
6. Name breeding seams; hold player-created IDs.
7. State the seasonal UX gap; presentation-only follow-on.

---

# 2. Current Reality

Three live produce authorities (greenhouse 12+2, hydroponic 10, fungi 4). Oilseed grows. Hydroponic racks simulate. Fungi beds yield. Cryo recovers canonical seeds. Scanner still classifies `hydroponic_crops.json` as `UNRESOLVED` because its hardcoded maps omit the file. Kitchen and greenhouse panels do not show seasonal windows.

---

# 3. Required Delta

| Item | Delta |
|---|---|
| Scanner | Add maps → `GAMEPLAY_CONSUMED`; pin baseline key; regen registry/manifest |
| Fungus medicinal | One `pharma_recipes.json` row using `fungus_spores_medicinal` → `antibiotics` |
| Fungus common | One `narcotics.json` toxin chem using `fungus_spores_common` |
| Narcotics outputs | Add missing `item_chem_*` rows for fungal antibiotic, spore sedative, new toxin |
| Oilseed | Design only this package: recipes + goods + lamp-oil item on existing CraftingSystem |
| Balance / playtest / cryo / breed / UX | Documents in this plan; no Core rewrite |

---

# 4. Evidence

See forensic report §§3–16. Key pins:

- `HydroponicCropCatalogLoader.DefaultFileName = "hydroponic_crops.json"`
- `Main.AdvancedShelterSystems.cs` loads it into `HydroponicBiomeSystem`
- Scanner `loaderPatterns` / `consumerMap` have no hydroponic entry
- Selftest rewrites manifest always; baseline only if missing
- `NarcoticsSystem.BrewChem` emits `item_{chem_id}`
- `PharmaLabSystem.LoadCatalog` reads `pharma_recipes.json`

---

# 5. Existing Extension Seams

- `ContentUtilizationScanner` dictionary maps
- `pharma_recipes.json` / `PharmaRecipeCatalogLoader`
- `narcotics.json` / `NarcoticsSystem`
- `items.json` ItemCatalog
- `recipes.json` CraftingSystem (oil, later)
- `economy_goods.json` (oil trade, later)
- `AgricultureSystem` + `crop_strains.json` (cryo/breed later)
- `HydroponicBiomeSystem.TryStabilizeTrait` (breed later)
- Greenhouse/Kitchen panels as presentation only (season later)

---

# 6. Proposed Architecture

No new systems.

```
hydroponic_crops.json → HydroponicCropCatalogLoader → HydroponicBiomeSystem
                      ↘ ContentUtilizationScanner maps → GAMEPLAY_CONSUMED

fungus_spores_medicinal → PharmaLab recipe_cordyceps_antibiotic → antibiotics
fungus_spores_common    → Narcotics chem_choke_spore_toxin → item_chem_choke_spore_toxin
chem_fungal_antibiotic  → item_chem_fungal_antibiotic (catalog the brew output)
chem_spore_sedative     → item_chem_spore_sedative

Later: crop_oilseed → craft_press_oilseed → cooking_oil
                    → craft_pressed_lamp_oil → item_pressed_lamp_oil (Fuel)
                    → confit consumes cooking_oil
                    → economy_goods row
```

---

# 7. Ownership Matrix

| Concern | Owner |
|---|---|
| Hydroponic cultivars | `HydroponicBiomeSystem` + `hydroponic_crops.json` |
| Scanner classification | `ContentUtilizationScanner` + baseline artifact |
| Pharma compounding | `PharmaLabSystem` + `pharma_recipes.json` |
| Toxin brew | `NarcoticsSystem` + `narcotics.json` |
| Item definitions | `items.json` / `ItemCatalog` |
| Oil press (later) | `CraftingSystem` + `recipes.json` |
| Greenhouse growth | `GreenhouseSystem` |
| Strain overlay | `AgricultureSystem` |
| UI season (later) | Godot panels, read-only |

---

# 8. Data Flow

**Scanner:** Scan → loader edge → consumer CONSUMED_BY `HydroponicBiomeSystem` → Classify GAMEPLAY_CONSUMED → VerifyConsumersInSource finds `hydroponic_crops.json` in Core → gate vs baseline.

**Pharma:** Player starts PharmaLab job → consume medicinal spores + solvent + water → `antibiotics`.

**Toxin:** Player `BrewChem("chem_choke_spore_toxin")` → consume common spores + solvent → `item_chem_choke_spore_toxin` → `AdministerChem` applies toxicity (existing narcotics path).

---

# 9. State Model

No new persistent fields for scanner, pharma row, or narcotics chem. Old saves: recipes appear when catalogs load.

---

# 10. API / Contracts

No new public types. Additive catalog rows. Scanner dictionary entries:

```
loaderPatterns["hydroponic_crops.json"] = HydroponicCropCatalogLoader
registryMap["hydroponic_crops.json"] = HydroponicCropCatalog
consumerMap["hydroponic_crops.json"] = HydroponicBiomeSystem
querySystems += hydroponic_crops.json
uiConsumers["hydroponic_crops.json"] = ExpansionsHubPanel
AuthoritativeCatalogs += hydroponic_crops.json
```

---

# 11. Data Changes

**This package:**

- `pharma_recipes.json`: `recipe_cordyceps_antibiotic`
- `narcotics.json`: `chem_choke_spore_toxin` (category Toxin; abstract; no synthesis procedure)
- `items.json`: `item_chem_fungal_antibiotic`, `item_chem_spore_sedative`, `item_chem_choke_spore_toxin`

**Later oil package:**

- `recipes.json`: `craft_press_oilseed` (crop_oilseed×2 → cooking_oil×1); optional `craft_pressed_lamp_oil`
- Retarget `craft_rendered_fat_confit` fuel → cooking_oil
- `economy_goods.json`: `crop_oilseed`, `cooking_oil` if missing
- Do **not** add ChandlerySystem
- Decide `item_lamp_oil` (radio) vs new `item_pressed_lamp_oil` vs Crossing oil — signature if creating the radio ID

---

# 12. Save/Load

None for this package. Oil recipes later: none. Breeding player IDs: blocked until save owner exists.

---

# 13. Determinism

No new RNG. Narcotics administer already uses injected `ISeededRng`.

---

# 14. System / Event Wiring

PharmaLab and Narcotics hosts already load their catalogs. New rows appear automatically.

---

# 15. Godot Integration

None this package. Seasonal UX later: GreenhousePanel detail pane + kitchen must **read** Core/host season, not compute crop bonuses.

---

# 16. Narrative / Content Integration

Diegetic names already exist (Sun-Flax, Cordyceps spores, Choke-Spores). New items use the same voice. No real-country or real-war references.

---

# 17. Failure Modes

| Case | Behavior |
|---|---|
| Missing medicinal spores | PharmaLab job blocked (existing insufficient-input) |
| Missing common spores | BrewChem false, failure reason names reagent |
| Old save | New recipes available; no migration |
| Scanner maps name a type not in source | VerifyConsumersInSource would orphan — use real type names |
| Full baseline rewrite | Forbidden unless delta reviewed |
| Oilseed press without lamp consumer | Trade + cooking_oil still close the Material dead-end |

---

# 18. Test Strategy

- `ContentUtilizationGraphTests.CiGate_CommittedBaseline_MatchesCurrentScanWithoutRegressions` after baseline pin
- New/extended: Pharma catalog contains `recipe_cordyceps_antibiotic`; Narcotics loads `chem_choke_spore_toxin` and brew consumes `fungus_spores_common`
- `NarcoticsSystemTests` existing brew still green
- `ProductionSliceTests` pharma load still green
- Focused: `bash scripts/run_test.sh Ashfall.Core.Tests/ContentUtilizationGraphTests.cs`
- Focused: `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/NarcoticsSystemTests.cs`
- Focused: `bash scripts/run_test.sh Ashfall.Core.Tests/ProductionSliceTests.cs`
- Data integrity if Godot available
- `--content-utilization-selftest` to refresh manifest + deep-chain (warn-greenhouse-crops should clear)

Do not run the full suite.

---

# 19. Dependency-Ordered Phases

## Phase 0 — Scanner maps + artifact pin (this session)

Why: UNRESOLVED is stale; user asked to regenerate.
Files: `ContentUtilizationScanner.cs`, `artifacts/content-utilization-baseline.json`, generated utilization/registry after scan.
Gate: hydroponic class GAMEPLAY_CONSUMED; CiGate test green; deep-chain hydroponic warn gone.
Must not: rewrite unrelated baseline keys if scan only improved hydroponic.

## Phase 1 — Fungus yield chains (this session)

Why: user asked to wire medicinal and common.
Files: `pharma_recipes.json`, `narcotics.json`, `items.json`, focused tests.
Gate: catalog load + brew/pharma id assertions.
Must not: ChemWarfare recipes, chemical_syntheses munitions, new poison system.

## Phase 2 — Oilseed press (follow-on)

`recipes.json` + goods + optional Fuel item. Fix confit inputs. No ChandlerySystem.

## Phase 3 — Cryo trait consumption (follow-on)

Either unique recovery items **or** map `traits[]` onto `AgricultureSystem` strain modifiers / hydroponic trait pool. Signature required if greenhouse crop stats must become data-driven.

## Phase 4 — Seasonal UX (follow-on)

GreenhousePanel: best-window + current season from host. Expand crop filter beyond 4 seeds. Kitchen: stop claiming scurvy on every recipe; bind `recipes.json` or Core catalog instead of hardcoded six.

## Phase 5 — Balance retune (follow-on)

After oilseed has a calorie or fuel identity, retune hydroponic `base_yield_quantity` / ticks against a declared hunger-per-survivor-day. Do not retune in the dark.

## Phase 6 — Breeding (HOLD)

Catalog mutation + stabilize-trait are enough until a signed player-cultivar save owner exists.

## Phase 7 — Playtest (follow-on)

Execute the scenario in § playtest below on a 15 FPS Godot session when a human/foreman schedules it.

---

# 20. File Impact Map

| File | Action | Reason | Risk |
|---|---|---|---|
| `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs` | MODIFY | Register hydroponic catalog | Low |
| `artifacts/content-utilization-baseline.json` | MODIFY | Pin hydroponic GAMEPLAY_CONSUMED | Med (pin only that key) |
| `artifacts/content-utilization.json` | REGEN | Scanner manifest | Low |
| `artifacts/content-utilization.md` | REGEN | Human report | Low |
| `artifacts/content-utilization-deep-chain.json` | REGEN | Clear hydroponic warn | Low |
| `docs/data/CATALOG_REGISTRY.md` | REGEN | Loader/class columns | Low |
| `docs/INDEX.md` | REGEN | New forensic/plan pages | Low |
| `Assets/StreamingAssets/Data/pharma_recipes.json` | MODIFY | Medicinal spore recipe | Low |
| `Assets/StreamingAssets/Data/narcotics.json` | MODIFY | Common-spore toxin | Low |
| `Assets/StreamingAssets/Data/items.json` | MODIFY | Chem output items | Low |
| `Ashfall.Core.Tests/Medical/NarcoticsSystemTests.cs` | MODIFY | Brew toxin case | Low |
| `Ashfall.Core.Tests/ProductionSliceTests.cs` | MODIFY | Pharma id pin | Low |
| `docs/forensics/CROP_ROSTER_FORENSIC_REPORT.md` | CREATE | Forensic | None |
| `docs/plans/CROP_ROSTER_INTEGRATION_PLAN.md` | CREATE | This plan | None |

---

# 21. Risks

See forensic §17. Additional: `items.json` is a shared catalog — additive rows only, no reformatting.

---

# 22. Out of Scope

- ChandlerySystem
- Player-created cultivar IDs
- JSON-ifying `GreenhouseExpansionCatalog` crop stats
- Merging hydroponic racks into GreenhouseSystem
- ChemWarfare poison munitions
- Full-suite test run
- Seasonal Core growth table (docs already exist; runtime coupling is a later signature)
- Editing `WORKTREE_OWNERSHIP.md` / `INTEGRATION_PLANS.md` (foreman ledger)

---

# 23. Rollback

- Scanner maps: revert dictionary rows + baseline key.
- Data: delete the three additive recipe/chem/item IDs.
- Artifacts: restore previous generated files.

---

# 24. Definition of Done (this session)

- [x] Forensic report written
- [ ] Hydroponic scanner maps live
- [ ] Baseline hydroponic key GAMEPLAY_CONSUMED
- [ ] Medicinal spores in PharmaLab catalog
- [ ] Common spores brew a toxin chem
- [ ] Chem output items exist
- [ ] Focused tests green
- [ ] Oil/cryo/breed/UX/playtest designed in this document, not silently implemented

---

# 25. Implementation Handoff

## MUST PRESERVE

Greenhouse, hydroponic, fungi, cryo as separate owners. Existing narcotics brew of hyper-stim. Existing pharma recipes. Unrelated baseline classifications.

## MUST ADD

Scanner maps. Pharma cordyceps recipe. Narcotics choke-spore toxin. Three `item_chem_*` rows. Tests. Artifact pin.

## MUST NOT DO

New systems. Munitions recipes. Mass baseline rewrite. Kitchen panel hardcoded oil recipes. Player cultivar IDs.

## VERIFY WITH

`bash scripts/run_test.sh Ashfall.Core.Tests/ContentUtilizationGraphTests.cs`
`bash scripts/run_test.sh Ashfall.Core.Tests/Medical/NarcoticsSystemTests.cs`
`bash scripts/run_test.sh Ashfall.Core.Tests/ProductionSliceTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

Register `hydroponic_crops.json` in `ContentUtilizationScanner` maps, then pin the baseline key.

---

# Key Decisions

1. **UNRESOLVED is stale.** Live consumer is `HydroponicBiomeSystem`. Fix the scanner tables, then pin artifacts.
2. **PharmaLab for medicinal spores; Narcotics for toxin.** Two existing brew authorities; do not invent a third.
3. **Oilseed later uses CraftingSystem recipes**, not ChandlerySystem.
4. **Cryo identity** must eventually affect recovered gameplay (strain overlay or unique items), not just canister stats.
5. **Breeding stays catalog-bound** until a save owner for player IDs is signed.
6. **Seasonal windows are presentation + optional Core season factor**, not a panel-owned growth table.
7. **HungerRestore is the calorie proxy** until a nutrition kcal table is authored.

---

# Oil-pressing design (Phase 2 contract)

**Goal:** `crop_oilseed` leaves Material dead-end and feeds lamp / confit / trade like the other integrated crops.

**Inputs:** `crop_oilseed` ×2, workbench or stove, 1.0 h.
**Output A:** `cooking_oil` ×1 (already Food, hunger 10, trade 4). Closes kitchen fat and confit.
**Output B (optional second recipe):** `item_pressed_lamp_oil` type Fuel, stack 10, trade ~8, consumed by existing fuel sinks **or** a future lamp consumer. Do not silently alias Crossing `item_lamp_oil_crossing`.
**Confit fix:** `craft_rendered_fat_confit` ingredients become `crop_tuber` ×3, `cooking_oil` ×1, `item_preservation_salt` ×1 (match docs; drop `fuel` as fake fat).
**Trade:** add `crop_oilseed` (base ~10) and ensure `cooking_oil` is a goods row.
**Non-goal:** edible raw oilseed (keep industrial identity).

---

# Hydroponic balance stress-test (findings)

Proxy: calorie-per-cycle = `base_yield_quantity × hungerRestore(yield item)`; days = germ+growth ticks. Indoor racks ignore outdoor season.

| Cultivar | Days | Proxy | Proxy/day |
|---|---|---|---|
| Glacial Dent Maize | 11 | 100 | 9.1 |
| Winter Rye X-1 | 8 | 80 | 10.0 |
| Radiotrophic Kelp | 5 | 75 | 15.0 |
| Deep-Root Ash Tuber | 9 | 70 | 7.8 |
| Dwarf Nitrogen Soy | 8 | 64 | 8.0 |
| Serrated Iron Greens | 5 | 32 | 6.4 |
| Calcified Agaric | 7 | 15 | 2.1 |
| Luminous Lichen | 6 | 15 | 2.5 |
| Alkaloid Latex V2 | 10 | 6 | 0.6 |
| Oilseed Brassica VII | 11 | **0** | **0** |

**Verdict:** Kelp is the calorie king (short cycle, edible algae). Oilseed is a zero-calorie industrial crop until Phase 2. Poppy is medical, not food — acceptable if kitchen/pharma consume `crop_medicinal_herb`. Duplicate yield IDs (rye/maize; agaric/lichen) erase cultivar identity after harvest — same class of bug as cryo recovery collisions.

**Intended calorie-per-day:** not authored. If a survivor burns ~20 hunger/day (typical `hungerRestore` meal scale), one maize rack covers ~0.45 survivor-days per campaign day; kelp covers ~0.75. Four default racks of kelp ≈ 3 survivor-days/day before blight/power/water. Staple-only play is feasible; mixed industrial/medical racks starve the kitchen. Retune only after oilseed has a non-zero identity (fuel value or press calories).

---

# Cryo differentiation proposal (Phase 3)

Do **not** invent parallel seed items for every line (ID explosion). Consume `traits[]` at recovery:

| Trait string | Seam |
|---|---|
| `radiation_tolerant` | Agriculture `radiation_tolerance` bump / hydroponic `Trait_Cold_Hardy` analogue |
| `frost_hardy` | Hydroponic `Trait_Cold_Hardy` or greenhouse water/light demand |
| `rapid_growth` | Yield modifier or reduced growth hours via strain overlay |
| `heirloom` | Morale on first harvest / wheat-style unlock flavor only if wired |
| `pharmaceutical_yield` | Extra `crop_medicinal_herb` or pharma precursor on harvest |
| `protein_rich` | NutritionProfile fats/protein on `crop_strains.json` |
| `low_light` | LightHoursPerDay reduction through Agriculture overlay |

Keep recovery_item_id canonical. Persist recovered trait onto the greenhouse plot's `strain_id` or hydroponic `activeTraits` at recovery time — CryoVault remains storage-only.

Until that lands, the only honest identity is canister difficulty/decay, which is invisible after recovery.

---

# Breeding exploration (Phase 6 HOLD)

**Already exists:** catalog `hardy_strain` mutations; hydroponic trait stabilize (global, not per-line).

**Missing for player-created cultivars:** ID minting, uniqueness, validator, save section, UI naming, determinism of combination, conflict with CropStrainCatalog "never invent a seed".

**Safe extension:** author more `result_strain_id` rows; allow cryo recovery to select among existing strains; do not mint `cultivar_player_*` IDs.

---

# Vertical-slice playtest scenario (Phase 7)

**Name:** Glass Orchard Year-One Circuit
**FPS:** 15 if Godot runtime.
**Roster:** 12 greenhouse crops (not hydroponic 10).
**Seed:** fixed campaign seed; record it.

| Day | Beat | Crop / item | Observe |
|---|---|---|---|
| 0 | Acquisition | Grant one of each `item_seed_*` (12) + blight treatment + grow medium | Vault counts |
| 0 | Plant | Fill 12 plots | GreenhousePanel lists all 12 seed names (today filters hide most) |
| 1–3 | Leafy / algae / biolum early | `crop_leafy_green` 60h, algae 72h, biolum 84h | Water drain; dry warning |
| 4 | Blight | Skip water on one tuber plot **or** apply ash | TreatBlight consumes `item_blight_treatment` |
| 4–6 | Mushroom harvest | `crop_mushroom` 96h | Kitchen fungal stew |
| 5–7 | Hardy tuber | 120h | Preservation pickle (`craft_pickled_tubers` uses `crop_tuber` not hardy — note mismatch) |
| 6–8 | Cold legume | 150h | `craft_brined_legume_mash` |
| 7–9 | Medicinal herb | 160h | `craft_dried_herb_packets`; pharma if wired |
| 7–9 | Ash grain | 168h | Ash-flour bread (panel uses `item_grain_flour` — mill gap) |
| 8–10 | Grain | 192h | Canned grain stew uses `crop_ash_grain` |
| 9–11 | Oilseed | 210h | **Expect dead-end until Phase 2** |
| 10–12 | Wheat | 240h + unlock flag | High morale bread |
| Any | Fungi beds | Four strains | Biolum light; mash; antibiotic; toxin after Phase 1 |
| Any | Trade | Sell leafy/grain; oilseed unsellable as good until Phase 2 | Market |
| Mid | Save/load | Capture at first harvest | Plot stages, blight count, pantry |

**Pass:** each of 12 greenhouse IDs is planted, at least one blight treated, at least one clean harvest, one preservation craft, one kitchen serve, one trade attempt. Oilseed trade/press may fail until Phase 2 — log as known.

---

# Seasonal UX check (Phase 4 contract)

**Finding:** not communicated.

- `SEASONAL_CROP_MATRIX.md` is documentation.
- `GreenhouseSystem.TickDay` does not take a season enum.
- `GreenhousePanel` has no season string.
- `KitchenNutritionPanel` has no season string and a false scurvy line.

**Follow-on:** host samples Plan 19 season once per day, passes light hours (already) **and** a read model `{ cropId, windowLabel, inWindow }` into the greenhouse detail pane. Kitchen stays recipe-truthful; it should not teach planting windows.

---

# PR Plan

1. **PR: Hydroponic utilization maps** — scanner + baseline pin + registry. Depends: none.
2. **PR: Fungi pharma/toxin chain** — pharma row, narcotics chem, three items, tests. Depends: none (parallel with 1).
3. **PR: Oilseed press recipes** — recipes.json, confit retarget, goods. Depends: none, but after 1–2 to keep diffs small.
4. **PR: Cryo trait consumption** — depends on agriculture/hydroponic trait API review.
5. **PR: Seasonal greenhouse presentation** — depends on host season read model.
6. **PR: Kitchen catalog binding** — retire hardcoded six recipes. Depends: kitchen Core catalog decision.

---

# Open Questions

1. Radio `item_lamp_oil`: create the missing item, alias to Crossing oil, or retarget the radio reveal to `item_pressed_lamp_oil`?
2. Declare an official survivor hunger-per-day so hydroponic retune is not guesswork?
3. Consume cryo `traits[]` via Agriculture strain overlay vs unique recovery items?
