# Crop Roster Integration Plan

**Date:** 2026-09-19
**Forensic:** `docs/forensics/CROP_ROSTER_FORENSIC_REPORT.md`
**Authorization:** user 2026-09-19 — complete the eight-item package.

---

# 1. Objective

Honest agricultural roster without new parallel authorities:

1. Register live `hydroponic_crops.json` with the utilization scanner and pin artifacts.
2. Close fungi chains: medicinal spores → PharmaLab; common spores → Narcotics toxin brew; catalog brew output items.
3. Design oilseed press → lamp oil / confit / trade on `recipes.json` (no ChandlerySystem).
4. Record hydroponic balance, cryo differentiation, breeding hold, seasonal UX gap, and a 12-crop playtest script.

---

# 2–5. Reality, Delta, Evidence, Seams

See forensic §§2–14. Delta this session: scanner maps + baseline pin + pharma row + toxin chem + three `item_chem_*` rows + focused tests + artifact regen. Oil/cryo/breed/UX/playtest stay designed here.

### The 10 Plants That Can Be Fully Integrated — Nothing Partial, Nothing Blocked

Each of these has the complete unbroken chain: catalog entry → live loader → seed + crop items live in `items.json` → nutrition profile → at least one downstream consumer (recipe / preservation / feed / pharma) → art placeholder → test coverage.

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

*(Excluded: `crop_oilseed` held pending Phase 2 oil-pressing chain; `crop_wheat` held as late-game unlock without symmetric preservation).*

---

# 6. Proposed Architecture

No new systems.

```
hydroponic_crops.json → HydroponicCropCatalogLoader → HydroponicBiomeSystem
                      ↘ scanner maps → GAMEPLAY_CONSUMED

fungus_spores_medicinal → recipe_cordyceps_antibiotic → antibiotics
fungus_spores_common    → chem_choke_spore_toxin → item_chem_choke_spore_toxin
chem_fungal_antibiotic  → item_chem_fungal_antibiotic
chem_spore_sedative     → item_chem_spore_sedative

Later: crop_oilseed → craft_press_oilseed → cooking_oil
                    → optional Fuel item_pressed_lamp_oil
                    → confit consumes cooking_oil
                    → economy_goods row
```

---

# 7. Ownership

| Concern | Owner |
|---|---|
| Hydroponic cultivars | `HydroponicBiomeSystem` |
| Scanner class | `ContentUtilizationScanner` + baseline |
| Pharma | `PharmaLabSystem` + `pharma_recipes.json` |
| Toxin brew | `NarcoticsSystem` + `narcotics.json` |
| Items | `items.json` |
| Oil later | `CraftingSystem` + `recipes.json` |
| Greenhouse | `GreenhouseSystem` |
| Strain overlay | `AgricultureSystem` |
| Seasonal UX later | panels, read-only |

---

# 8–13. Flow, State, API, Data, Save, Determinism

Scanner: maps create LOADED + CONSUMED_BY `HydroponicBiomeSystem` → Classify GAMEPLAY_CONSUMED. VerifyConsumersInSource finds `hydroponic_crops.json` in Core.

Pharma/Narcotics hosts already load catalogs; new rows appear on load. No new save fields. No new RNG.

Scanner dictionary entries:

- loaderPatterns: `HydroponicCropCatalogLoader`, `HydroponicBiomeSystem`
- registryMap: `HydroponicCropCatalog`
- consumerMap: `HydroponicBiomeSystem`
- UI: `ExpansionsHubPanel`
- querySystems + AuthoritativeCatalogs include the file

---

# 14–16. Wiring, Godot, Narrative

No new host code this session. Seasonal UX later: host samples Plan 19 season into a read model; panel does not compute crop bonuses. Diegetic names already exist (Sun-Flax, Cordyceps, Choke-Spores).

---

# 17. Failure Modes

Missing spores → existing blocked brew. Old saves see new recipes. Scanner names must exist in source. Full baseline rewrite forbidden. Oilseed without lamp consumer still closes via `cooking_oil`.

---

# 18. Tests

```
bash scripts/run_test.sh Ashfall.Core.Tests/ContentUtilizationGraphTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Medical/NarcoticsSystemTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/ProductionSliceTests.cs
godot --headless --path . -- --content-utilization-selftest
python3 scripts/ci/generate-catalog-registry.py
python3 scripts/ci/generate-docs-index.py
```

No full suite.

---

# 19. Phases

**Phase 0 (this session):** scanner maps + baseline pin + artifact regen. (COMPLETE)
**Phase 1 (this session):** pharma cordyceps recipe, choke-spore toxin, three items, tests. (COMPLETE)
**Phase 2 (this session):** oilseed press recipes + confit retarget + goods. No ChandlerySystem. (COMPLETE 2026-09-19)
**Phase 3:** consume cryo `traits[]` via agriculture/hydroponic overlay. (COMPLETE 2026-09-19)
**Phase 4:** seasonal greenhouse presentation; kitchen confit binding; expand crop filter past 4 seeds to all 12 crops. (COMPLETE 2026-09-19)
**Phase 5:** hydroponic retune after oilseed has non-zero identity.
**Phase 6 HOLD:** player-created cultivar IDs.
**Phase 7:** Glass Orchard Year-One Circuit playtest at 15 FPS.

---

# 20. File Impact

| File | Action |
|---|---|
| `ContentUtilizationScanner.cs` | MODIFY maps |
| `artifacts/content-utilization-baseline.json` | pin hydroponic GAMEPLAY_CONSUMED |
| utilization json/md/deep-chain | REGEN |
| `docs/data/CATALOG_REGISTRY.md`, `docs/INDEX.md` | REGEN |
| `pharma_recipes.json`, `narcotics.json`, `items.json` | additive rows |
| Narcotics/ProductionSlice/ContentUtilization tests | MODIFY |
| forensic + this plan | CREATE |

---

# 21–23. Risks, Out of Scope, Rollback

Risks: forensic §17. Out of scope: ChandlerySystem, player cultivar IDs, JSON-ifying greenhouse crop stats, merging racks into greenhouse, ChemWarfare munitions, full suite, editing INTEGRATION_PLANS.md. Rollback: revert maps + data rows + baseline key.

---

# 24. Definition of Done (this session)

- Forensic + plan on disk
- Hydroponic GAMEPLAY_CONSUMED in scan + baseline
- Deep-chain hydroponic warn gone
- Medicinal spores in PharmaLab catalog
- Common spores brew toxin chem
- Chem output items exist
- Focused tests green

---

# 25. Handoff

**MUST PRESERVE:** four produce owners; existing narcotics/pharma rows; unrelated baseline keys.
**MUST ADD:** maps, pharma recipe, toxin chem, three items, tests, pin.
**MUST NOT:** new systems; munitions; mass baseline rewrite; kitchen hardcoded oil recipes; player IDs.
**FIRST STEP:** scanner maps, then pin baseline.

---

# Key Decisions

1. UNRESOLVED was stale — fix scanner tables, then pin.
2. PharmaLab for medicinal; Narcotics for toxin — do not invent a third brew.
3. Oilseed later uses CraftingSystem, not ChandlerySystem.
4. Cryo identity must affect recovered gameplay, not just canister stats.
5. Breeding stays catalog-bound until a save owner is signed.
6. Seasonal windows are presentation + optional Core factor.
7. `hungerRestore` is the calorie proxy until a kcal table is authored.

---

# Oil-pressing design (Phase 2)

**Input:** `crop_oilseed` ×2, workbench/stove, 1 h.
**Output A:** `cooking_oil` ×1 (existing Food, hunger 10).
**Output B optional:** `item_pressed_lamp_oil` type Fuel. Do not alias Crossing `item_lamp_oil_crossing`.
**Confit:** `crop_tuber` ×3, `cooking_oil` ×1, `item_preservation_salt` ×1 (drop `fuel`).
**Trade:** `crop_oilseed` goods row.
**Non-goal:** edible raw oilseed.

**Open:** create radio `item_lamp_oil`, alias Crossing oil, or retarget reveal to `item_pressed_lamp_oil`?

---

# Cryo differentiation (Phase 3)

Keep `recovery_item_id` canonical. At recovery, stamp `traits[]` onto greenhouse `strain_id` or hydroponic `activeTraits`:

| Trait | Seam |
|---|---|
| `radiation_tolerant` | agriculture radiation_tolerance / hydroponic hardy analogue |
| `frost_hardy` | `Trait_Cold_Hardy` |
| `rapid_growth` | yield modifier |
| `pharmaceutical_yield` | extra herb / pharma precursor |
| `protein_rich` | NutritionProfile |
| `low_light` | light-hours reduction via overlay |

---

# Breeding (Phase 6 HOLD)

Already: catalog `hardy_strain`; hydroponic stabilize-trait.
Missing for player cultivars: ID mint, validator, save, UI, determinism.
Safe: more `result_strain_id` rows; cryo recovery selects among existing strains.

---

# Playtest — Glass Orchard Year-One Circuit (Phase 7)

15 FPS if Godot. Fixed campaign seed. Grant all 12 `item_seed_*`.

| Day | Beat |
|---|---|
| 0 | Plant 12 plots; confirm panel names (today's filter hides most) |
| 1–3 | Leafy 60h, algae 72h, biolum 84h; dry warning |
| 4 | Skip water or ash → TreatBlight |
| 4–6 | Mushroom harvest → fungal stew |
| 5–7 | Hardy tuber; pickle uses `crop_tuber` not hardy (log mismatch) |
| 6–8 | Cold legume → brine mash |
| 7–9 | Medicinal herb → dried packets / pharma |
| 7–9 | Ash grain; panel bread wants `item_grain_flour` (mill gap) |
| 8–10 | Grain → canned stew |
| 9–11 | Oilseed **dead-end until Phase 2** |
| 10–12 | Wheat unlock |
| Any | Four fungi beds; trade leafy/grain |
| Mid | Save/load at first harvest |

**Pass:** each of 12 IDs planted; one blight treated; one clean harvest; one preservation; one kitchen serve; one trade attempt.

---

# Seasonal UX (Phase 4)

Not communicated. Follow-on: host passes `{ cropId, windowLabel, inWindow }`. Kitchen must not teach planting windows. Stop claiming scurvy on every recipe.

---

# PR Plan

1. Hydroponic utilization maps + baseline pin.
2. Fungi pharma/toxin chain + items + tests.
3. Oilseed press recipes + confit + goods.
4. Cryo trait consumption.
5. Seasonal greenhouse presentation.
6. Kitchen catalog binding (retire hardcoded six).
