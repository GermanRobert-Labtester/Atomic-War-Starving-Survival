# Plans 126–129 — Ownership & Authority Decisions (Phase 0 Outcome)

**Status:** Approved decision note — binding for all Plan 126–129 implementation waves.
**Date:** 2026-09-11
**Scope:** Biological Fermentation (126), Tethered Recon Drone (127), Continuous Steel Casting (128), Atmospheric Lidar Hazard Mapping (129).

---

## 1. Baseline captured (Phase 0)

| Gate | Result |
|---|---|
| `dotnet build Ashfall.Core` | PASS, 0 warnings / 0 errors |
| `dotnet build Ashfall.csproj` | PASS, 0 warnings / 0 errors |
| `dotnet test Ashfall.Core.Tests` | **10847/10847 PASS** (33 s) |
| `--data-integrity-selftest` | PASS — 303 catalogs, 0 errors / 0 warnings, 12742 authored ids |
| `--content-utilization-selftest` | PASS — deep-chain gate 0 hard failures (1 pre-existing warn: hydroponic_crops loader wiring) |
| `--bridge-selftest` | PASS |
| `--scene-binding-selftest` | PASS — 25/25 |
| `run-gates.py --tier fast` | **1 of 47 gates FAILS pre-existing:** `core_systems_catalog_drift` — the generator stamps today's date (2026-09-11) into `CORE_SYSTEMS_CATALOG.md` while the committed file carries 2026-09-10. Date-only drift, not a code defect; not introduced by this tranche. Re-run after any working-day commit regenerates identically. |

Pre-existing working-tree state (not owned by this tranche): 157 changed files on lane
`lane/trapping-flagship-verification` (deleted `.agents/skills` batch, AGENTS/CLAUDE/rulebook edits,
concurrent flagship test tweaks). None of the Plan 126–129 target paths exist yet.

## 2. Audits performed (Plan §3 list)

- **Food authority:** `KitchenNutritionSystem` (meals, pantry/spoilage, preservation method by cellar/refrigeration) and
  `FoodPreservationSystem` (cohort tiers `preservation_*`, curing jobs, `preservative_item_id` bills, `food_preservation.json`).
  **Decision:** fermentation never mutates spoilage or cohorts. It delivers an item
  (`item_fermentation_preservation_concentrate`) that `food_preservation.json` curing recipes consume as their
  `preservative_item_id` — recipe-level bridge, identical to the existing "chemicals" preservative.
- **Chemical authority:** `ChlorAlkaliSynthesisEngine` owns mineral industrial acid (`item_industrial_acid_carboy`).
  **Decision:** fermentation does not extend chlor-alkali. Its acid output is a distinct fermentation-family item
  (`item_fermented_organic_acid_carboy`).
- **Foundry/metallurgy authority (for Plan 128, audited now):** `CupolaFoundryEngine` (Plan 90, furnace condition/defect rolls),
  `SilentFoundrySystem` (+ `SilentFoundrySystem.Metallurgy.cs` heavy batches, `MetallurgyHeavyCatalog`),
  `PowderMetallurgySystem`. Canonical items already exist: `item_metallurgy_steel_billet`,
  `item_metallurgy_heavy_i_beam`, `item_metallurgy_shoring_plate`.
  **Decision:** `ContinuousCastingEngine` (Wave 3) consumes only metallurgy-qualified steel billet items and properties;
  no new metal inventory. Outputs land in canonical structural item ids.
- **Observation/sensing landscape (for Plans 127/129, audited now):** No periscope/optical-range engine exists yet
  (Plan 83 style); current radio surface is `DirectionFindingCatalog`/`AcousticDirectionFindingCatalog`,
  `WastelandMapSystem` (nodes/routes/markers/intel), `ExpeditionSystem` (travel/encounter pipeline),
  `TacticalCombatSystem` (pure resolution engine).
  **Decision:** introduce the shared contact model in Wave 4/5 when the first sensor lands (lidar), not before —
  no speculative abstraction. Contact records are sensor-owned intelligence snapshots referencing
  world/expedition authorities; systems never mutate foreign state.
- **Persistence conventions:** `SaveStore<T>` + `SchemaVersionedEnvelope<T>` static façades (`FungiSaveStore` pattern),
  section registered in `SaveSectionRegistry` (entry + `SectionFileNames` map), `SaveAll` aggregates only captured bytes.
- **Power convention:** engines take no direct power action; host projects `IsRoomPowered`/draw into preflights
  the same way `CupolaFoundryEngine` takes `availablePowerWatts`. Fermentation batches pause (stall/no progress)
  when the fermenter bay breaker is open — power is a host-supplied predicate, never engine-owned.
- **Validator/utilization conventions:** `CatalogIntegrityValidator.ReferenceKeys` (tier-2) + definition prefixes;
  `ContentUtilizationScanner` explicit claims map (`filename → consumer systems`) plus a real source reference
  (`res://Assets/StreamingAssets/Data/<file>` in `Main.Plans126_129.cs`).

## 3. Binding decisions per plan

| Concern | Owner | Fermentation (126) | Caster (128, wave 3) | Lidar (129, wave 4) | Drone (127, wave 5) |
|---|---|---|---|---|---|
| Feed/metal/world truth | existing authority | `FoodPreservationSystem` + `KitchenNutritionSystem` + canonical `Inventory` | `CupolaFoundryEngine` / `SilentFoundrySystem` metallurgy items | world hazard systems (`WastelandMapSystem`, weather/fallout) | `WastelandMapSystem` + `ExpeditionSystem` + `TacticalCombatSystem` |
| New engine owns | machine/process state, health, faults, outputs-as-items | `BioFermentationEngine` | `ContinuousCastingEngine` | `RamanLidarEngine` | `TetheredDroneEngine` |
| Direct foreign mutation | **forbidden** | none | none | none (detections are contacts, clouds stay world-owned) | none (no combat/map writes) |
| Specialist traits | code constants, granted via bound `TraitsOf` delegate; **not** added to `development_traits.json` generation pool this wave (avoids disturbing `GenerationalSystem` pins) | `trait_bioprocess_engineer`, `trait_fermentation_microbiologist` | `trait_continuous_caster_operator`, `trait_rolling_mill_master` | `trait_lidar_spectroscopist`, `trait_atmospheric_chemist` | `trait_drone_pilot`, `trait_fiber_optic_technician` |

## 4. Wave 1 decisions (shared)

1. **Common observation-contact abstraction:** deferred to Wave 4 (lidar). `AtmosphericHazardContact` will be the
   first consumer; periscope/drone re-use follows once proven (Plan 83 has no live engine yet, so there is nothing
   to unify against today).
2. **Catalog schemas** land with each wave's catalog — no placeholder schemas for later plans.
3. **Save versioning:** each engine uses `SchemaVersionedEnvelope<T>` with legacy default `Unbuilt`/empty on missing
   sections. No envelope for a system that has never been built.
4. **Determinism convention (all four engines):** fixed RNG draw order per tick, documented per engine
   (Cupola precedent). No wall-clock or frame-rate input. Completed batches/scans/contacts persist their outcomes —
   restore is strictly non-operative.

## 5. Plan 126 concrete decisions

- Reactor id namespace `bio_ferm_*` registered as a validator prefix (one line in the prefix list).
- New items: 9 (see catalog/item manifest in the closeout doc). All resolve through `items.json` (tier-2).
- Fermentation outputs: preservation concentrate (→ `food_preservation.json` curing recipe
  `recipe_cure_ferment_concentrate`), cleaning reagent (→ `workshop_recipes.json`
  `recipe_workshop_rust_treatment`, rusted scrap → `item_metallurgy_steel_billet` via the workshop authority),
  organic acid carboy (trade commodity). **Soil-cleanup bridge: NOT enabled** — no soil-wash mechanic exists in the
  greenhouse authority yet; adding one would be a second authority for soil mutation. Documented as follow-up
  (`item_fermented_organic_acid_carboy` is the future recipe input).
- Traits entered by bind-time snapshot: modifiers are computed **once at `StartBatch`** from `operator_trait_ids`,
  not re-read per tick (once-application pinned by test).
- `ServiceReactor` is the single maintenance action that clears contamination/fault and restores the filter;
  it never rescues a contaminated batch (batch loss is deliberate and documented).