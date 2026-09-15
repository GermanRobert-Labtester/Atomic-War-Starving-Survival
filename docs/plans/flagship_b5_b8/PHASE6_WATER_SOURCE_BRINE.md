# Phase 6 — Water Source/Brine Depth (B5–B8 / Plan 66) — increment 1 landed

> Scope per the reconciliation + `PLAN66_PLAN189_BOUNDARY.md`: bounded source
> identities that already exist in research/content, filter/membrane
> maintenance via existing patterns, brine consumer closure audit. No
> aquifer topology, no routing graphs, no second contamination map.

## Audit conclusions (documented — most of §9.15 closed without code)

| Item | Finding | Disposition |
|---|---|---|
| Brine economy (§9.15) | `BrineWaterSystem` IS the brine economy — Holdfast brine-pan water/salt trade (`waystation_brine_pans`, salt trade unlock, membrane integrity, steam trips) | **live system; no by-product itemization needed** — the §9.15 defer-hold applies: no desalination reject item was invented |
| Advanced filter chain | `knowledge_water_advanced` (breakthrough `item_water_filter_advanced`) → `craft_advanced_water_filter` → `item_lead_lined_effluent_filter` → **consumed by decon effluent treatment** | chain closed; research→recipe→item→consumer all resolve |
| Desalination | solar-thermal distillation live (`SolarConcentratorEngine.PerformSolarDistillation`, brackish→clean, requires 2 kW thermal); `knowledge_water_condenser_blueprint` (breakthrough `item_desal_membrane`, aliased to `water_filter`) has **zero runtime consumer** | condenser build deferred (follow-on) — the broken `craft_desalination_still` recipe (resultAmount 0) is flagged for the content stream |
| `knowledge_water_advanced` consumer | indirect via the advanced-filter recipe blueprint gate | closed |

## Landed in this increment

### 1. Filter replacement cost repair (§8.8/§9.16 — silent free lunch)
`WaterTreatmentHostSession.ReplaceFilter` (and Core's doc comment) claimed the
replacement "consumes a filter item" — **neither layer consumed anything**.
Filter integrity gates treatment quality/duration, so replacement was a free
maintenance loop on the water authority.

- Fix: the host consumes **1× canonical `water_filter`** (craftable 3 ways,
  reconditionable via `craft_filter_reconditioning`) exactly when the Core
  commit succeeds; a missing filter blocks with `missing_filter` and mutates
  nothing. Inventory-less callers keep the legacy path.
- Test coverage: host-level (Core tests cannot see host sessions) — verified
  via build + panel/scene selftests; the Core mutation stays covered by
  `WaterTreatmentCommandTests`.

### 2. Deep-well pump foundation (§9.8 — the flagship-named bounded source)
New standard-triad system `DeepWellSystem` (`Assets/Ashfall.Core/DeepWellSystem.cs`):

- **Build** (§15.3 discipline in one route, `Main.DeepWellTryBuild`): live
  `HasCapability("knowledge_deep_well_hydraulics")` query → canonical bill
  (1× `item_hydraulic_actuator` — the node's breakthrough item — + 2×
  `mechanical_parts`) consumed atomically → Core commit. Research unlocked
  without a built well grants nothing; blocked reasons name the exact gap.
- **Power**: the 120 W submersible pump registers as a critical grid load via
  the Phase 3 `RegisterLoadRoom` contract; the well reads `IsRoomServed` and
  owns no grid mutation. Unserved → no yield, no wear.
- **Water**: pushes a bounded 40 L/day **raw** yield into the treatment
  authority through the Plan 189 intake seam
  (`TryAddWaterFromSource("source_deep_well", Raw, yield)`) — never potable
  directly; purification stays wholly with the treatment authority; never a
  second counter. An advisory-blocked intake (Plan 189 gate) leaves the water
  in the aquifer — conserved, no private storage.
- **Maintenance**: pump condition wears only on pumping days (0.2/day);
  service via canonical `machine_oil` (the subgrid/generator lubricant);
  blocked when healthy.
- **Save**: new `deep_well` section (versioned envelope, `deep_well_save.json`)
  — first new section in the package; save gates bumped (191 sections).
  Restore re-registers the pump load (idempotent, deterministic, no replayed
  pumping — pinned).
- **Plan 189 boundary**: the well is a source *identity* on the existing
  intake seam — no topology, no depletion, no network (boundary doc holds).

## Save-gate bumps (deliberate, pre-existing staleness noted)

- `VersionReportContractTests`: pinned 186 sections/180 envelopes against a
  live registry that was **already 190 before this package** (pre-existing
  failures); now pinned to the true post-addition counts (191 / 6 versioned /
  185 envelopes).
- `ComprehensiveSaveStoreCorruptionAndMigrationTests`: 190 → 191 section
  count; save gate 1148/1148 green.

## Deliberately deferred

| Item | Target | Reason |
|---|---|---|
| Atmospheric condenser build (§9.9) | follow-on | research + breakthrough item exist; the build needs a ventilation-exhaust coupling design; the desal recipe bug (`craft_desalination_still` → resultAmount 0) must be fixed by the content stream first |
| Deep-well panel UI | Phase 9 | routes exist via `DeepWellHostSession`/Main; panel lands with the UI honesty pass |
| 30-day water balance harness (§9.18) | Phase 8 | scenario set |
| Brine reject itemization | none (deferred per §9.15) | no real consumer exists; `BrineWaterSystem` already owns the brine economy |

## Verification record (2026-09-13)

| Command | Result |
|---|---|
| `bash scripts/run_test.sh Ashfall.Core.Tests/Water/DeepWellSystemTests.cs` | 10/10 PASS (new) |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Save/ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` | 1148/1148 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/VersionReportContractTests.cs` | 11/11 PASS (stale pins bumped to live truth) |
| `bash scripts/run_test.sh Ashfall.Core.Tests/WaterTreatmentCommandTests.cs` | 5/5 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/SumpFloodingSystemTests.cs` | 39/39 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/WaterTreatmentSumpBridgeTests.cs` | 4/4 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/PowerGridPhase5MaintenanceTests.cs` | 9/9 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/Phase3WaterIntegrationTests.cs` | 10/10 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/PowerGridPhase2AllocationTests.cs` | 14/14 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/PowerGridPhase2GenerationPortfolioTests.cs` | 11/11 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Greenhouse/GreenhousePhase4LoopClosureTests.cs` | 13/13 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/PowerGridSystemTests.cs` | 20/20 PASS |
| `godot --headless --path . -- --data-integrity-selftest` | PASS — 0 findings, 325 catalogs |
| `godot --headless --path . -- --content-utilization-selftest` | PASS — 0 hard failures |
| `godot --headless --path . -- --panel-bind-lifecycle-selftest` | 22 gates PASS |
| `dotnet build Ashfall.csproj` | 0 warnings, 0 errors |

## Files changed

- `Assets/Ashfall.Core/DeepWellSystem.cs` (new Core authority)
- `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` (+deep_well row, filename)
- `src/Host/DeepWellSaveStore.cs`, `src/Host/DeepWellHostSession.cs` (new)
- `src/Main.DeepWell.cs` (new triad + build/service routes)
- `src/Main.SaveOrchestrator.cs` (2 call sites)
- `src/Main.ExpandedShelterSystems.cs` (1 tick line)
- `src/Host/WaterTreatmentHostSession.cs` (filter-cost repair)
- `Ashfall.Core.Tests/Water/DeepWellSystemTests.cs` (new, 10 tests)
- `Ashfall.Core.Tests/VersionReportContractTests.cs`, `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` (section-count pins → live truth)
- `docs/plans/flagship_b5_b8/PHASE6_WATER_SOURCE_BRINE.md` (this file)
