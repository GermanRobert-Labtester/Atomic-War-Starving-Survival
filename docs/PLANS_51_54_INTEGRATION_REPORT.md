# ASHFALL Plans 51–54 integration report

Date: 2026-09-10  
Status: implemented proof slice; validation green  
Commit status: no commits created in this worktree; the repository already contained unrelated user changes.

## Delivered

### Expedition / Plan 51

- Added `--expedition-playtest-selftest`, registry/help/manifest wiring, and a
  deterministic 30-day Core expedition campaign.
- Added daily and per-sortie JSON/Markdown artifacts:
  `artifacts/expedition-playtest-30d.json` and
  `artifacts/expedition-playtest-30d.md`.
- Exercised 10 sorties, foot fallback, all 8 authored vehicle profiles,
  estimate/actual ticks, fuel/loot conservation, no-engagement travel,
  breakdown transition, and midpoint save/resume.
- Corrected `ExpeditionSystem.Estimate` to use the execution path's discrete
  rounded travel step, including bicycle return speed. The existing V3 gate now
  characterizes short-route quantization as “no slower + fuel cost”; it does
  not require a strict win where the authoritative five-tick route ties.
- No `vehicles.json` tune was justified or applied.

Measured result: 30 snapshots, 10/10 completed sorties, 9 profiles, 0 estimate
mismatches, 0 negative-resource findings, 20 returned loot entries matching
completed loot entries, same-seed byte equality, different-seed divergence, and
mid-sortie save/load equality.

### Shelter maintenance / Plan 52

- Added typed, inventory-backed air-filter maintenance to
  `StartingLevelSystem`: preview, service, HEPA replacement, condition bands,
  research gate, atomic item consumption, and typed failure reasons.
- Bound the system in the production composition path to the shared inventory
  and shared research capability query; no shelter-local research boolean or
  free multiplier was introduced.
- Honored the authored `knowledge_air_filtration` “+50% lifespan” claim as a
  `2/3` degradation rate in the air owner.
- Added deterministic unit and 30-day maintenance tests, including rejected
  action conservation and researched/ordinary degradation characterization.

Measured result: first ordinary warning day 12, ignored failure day 21, and a
competent 30-day path using 4 real `scrap_mechanical` service parts from an
initial 8, ending at 55% integrity. Thermal/room physics remain owned by the
existing `ShelterThermalSystem`; no duplicate thermal model was introduced.

### Economy / Plan 53

- Added Core `PriceExplanation`, typed factor records, stable factor ordering,
  and `MarketTransactionSide` without moving player-facing strings into Core.
- `GetPrice` and `ExplainPrice` share the same authoritative quote path; the
  explanation is side-effect-free and its final price equals the quote.
- Exposed the typed explanation through `EconomyHostSession` and rendered the
  top two factor deltas in `EconomyDetailPanel` using player-facing copy.
- Preserved the existing stance, debt, caravan, and holdfast authorities and
  left volatile `trade_texts.json` untouched.

Measured result: new price explanation tests pass; the existing economy
selftest remains 11/11 and the full Core suite remains green. The plain
`MarketSystem` currently owns demand plus floor/ceiling clamps only, so stance
and debt are reported as owning-session concerns rather than fabricated market
factors.

### Medical / Plan 54

- Added typed shared-research capability checks to medical treatment preview.
- Made oxygen support a real 24-hour scheduled treatment with exclusive active
  state, reservation-before-completion, and consumption at completion.
- Added phase-2 tests for incomplete chelation research, atomic rejection,
  deterministic 30-day scheduled workload, and active procedure save/resume.
- The restored active procedure consumes exactly once and produces the same
  respiratory result as uninterrupted execution.

Measured result: 30/30 scheduled procedures completed, 0 active at day 30,
30 oxygen supplies consumed, 0 leaked reservations, deterministic final state,
and active scheduled-procedure round-trip green.

## Changed-file inventory

### Core

- `Assets/Ashfall.Core/Economy/MarketSystem.cs`
- `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs`
- `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs`
- `Assets/Ashfall.Core/Medical/MedicalTreatmentCatalog.cs`
- `Assets/Ashfall.Core/StartingLevel/StartingLevelSystem.cs`

### Host and composition

- `src/Host/HostCli.ExpeditionPlaytest.cs`
- `src/Host/HostCli.cs`
- `src/Host/HostCli.PanelTests.cs`
- `src/Host/EconomyHostSession.cs`
- `src/Host/StartingLevelHostSession.cs`
- `src/Main.Application.cs`
- `src/Main.GameFlow.cs`
- `src/Main.Inventory.cs`
- `src/Main.Medical.cs`
- `src/Main.World.cs`

### UI

- `src/UI/EconomyDetailPanel.cs`
- `src/UI/MedicalPanel.cs`

### Data and save surfaces

- No catalog or trade-text data was tuned.
- No new save store was introduced; active medical state uses the existing
  `MedicalPipelineSaveState` capture/restore shape.

### Tests

- `Ashfall.Core.Tests/Economy/PriceExplanationTests.cs`
- `Ashfall.Core.Tests/ExpeditionVehicleLogisticsTests.cs`
- `Ashfall.Core.Tests/Medical/MedicalPipelinePhase2Tests.cs`
- `Ashfall.Core.Tests/Medical/RespiratoryVerticalSliceTests.cs`
- `Ashfall.Core.Tests/Shelter/StartingLevelMaintenanceTests.cs`
- `Ashfall.Core.Tests/Shelter/StartingLevelMaintenance30DayTests.cs`

### Artifacts and docs

- `artifacts/expedition-playtest-30d.{json,md}`
- the eleven Plan 51–54 matrix/report documents listed below;
  `docs/architecture/ARCHITECTURE_TEST_MAP.md`, the CLI catalog, and
  `docs/INDEX.md` were regenerated by their canonical generators.

## Documentation and evidence

Added or updated the Plan 51–54 baseline/matrix/report documents:

- `docs/EXPEDITION_BALANCE_BASELINE.md`
- `docs/EXPEDITION_30_DAY_PLAYTEST_REPORT.md`
- `docs/EXPEDITION_VEHICLE_DOMINANCE_TABLE.md`
- `docs/SHELTER_MAINTENANCE_MATRIX.md`
- `docs/SHELTER_30_DAY_MAINTENANCE_REPORT.md`
- `docs/ECONOMY_PRICE_FACTOR_MATRIX.md`
- `docs/ECONOMY_FAIRNESS_AUDIT.md`
- `docs/MEDICAL_PIPELINE_JOURNEY.md`
- `docs/MEDICAL_DOSE_TREATMENT_MATRIX.md`
- `docs/MEDICAL_30_DAY_CAPACITY_REPORT.md`
- `docs/ACTION_RESULT_SURFACING_MATRIX.md`

The CLI catalog, architecture test map, and master docs index were regenerated
from their canonical generators and pass their drift gates.

## Exact validation results

```text
dotnet build Ashfall.csproj --no-restore --verbosity:minimal       PASS
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --no-restore PASS
  10,828 passed; 0 failed; 0 skipped

godot --headless --path . -- --evolving-world-selftest              PASS
godot --headless --path . -- --world-playtest-selftest              PASS
  30 days; 61 migrations; 1 degradation transition; 0 dead seeds
godot --headless --path . -- --expedition-selftest                  PASS 40/40
godot --headless --path . -- --expedition-playtest-selftest         PASS 13/13
godot --headless --path . -- --economy-selftest                     PASS 11/11
godot --headless --path . -- --medical-selftest                     PASS 15/15
godot --headless --path . -- --survivors-selftest                   PASS
godot --headless --path . -- --runtime-scale-selftest               PASS 6/6
godot --headless --path . -- --data-integrity-selftest               PASS 299/299
godot --headless --path . -- --bridge-selftest                      PASS

python3 scripts/ci/run-gates.py --tier fast                       PASS 47/47
```

## Deferred findings and ownership

These are deliberately recorded rather than hidden behind green focused
tests:

1. Vehicle balance was characterized, but no universal multi-route dominance
   claim or data rebalance was made. Owner: Expedition balance follow-up; use
   the fixed-seed multi-route simulator before editing `vehicles.json`.
2. Plain `MarketSystem` explanation covers its actual factors. A unified
   caravan/holdfast explanation and preview contract still belongs in those
   trade-session authorities. Owner: Economy trade-affordance follow-up.
3. Existing thermal/insulation physics and retrofit actions remain in
   `ShelterThermalSystem`; this pass only closes the real air-filter maintenance
   path and does not create a second component ledger. Owner: Shelter thermal
   maintenance follow-up if the existing retrofit contract needs expansion.
4. The combined four-domain 30-day campaign matrix was not added as a new
   harness; the domain-specific deterministic proofs and existing campaign
   gates are green. Owner: integration soak-test follow-up.

No known validation failure is being suppressed. No ballistics authority,
weapon-condition authority, diagnosis/research authority, or volatile trade
text surface was changed by these workstreams.
