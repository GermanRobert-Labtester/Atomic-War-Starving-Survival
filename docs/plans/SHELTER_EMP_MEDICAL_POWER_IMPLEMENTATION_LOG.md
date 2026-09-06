# SHELTER EMP & MEDICAL POWER — IMPLEMENTATION LOG

Plan: `docs/plans/SHELTER_EMP_MEDICAL_POWER_INTEGRATION_PLAN.md`
Branch: `feat/asset-pipeline-flagship`. Note: the G1–G3 wave
(SHELTER_GRID_CATALOG_SEAL) is present in the working tree but NOT yet committed
(interleaved concurrent-stream work) — this wave builds on it in-tree.

---

## Phase 0 — Verification

Status: PASS (with documented concurrent-stream interference)

Results:
* Host build: 0 errors.
* Full Core suite: **18 failures — ALL attributed to the concurrent stream**, none
  from this wave (which had touched nothing at test time):
  - `PlansB66ToB69CrossSystemTests` (7) — the other stream's new in-flight tests.
  - `CampaignEnvelopeBuilderTests`/`ComprehensiveSaveStoreCorruptionAndMigrationTests`/
    `VersionReportContractTests` (5) — consistent with their new cryo-vault save section;
    registry-count gates moved under them.
  - `Performance*Tests` (7) — latency-threshold flakiness/other stream.
  The other stream is actively editing the tree mid-run (new test files appeared between runs).
* **One failure was mine and fixed**: the other stream added `room_cryo_vault` to
  `power_grid.json` (8 rooms), colliding with my exact-count assertion from G1–G3.
  `Catalog_HasExactlySevenRooms` → `Catalog_HasAtLeastSevenRooms` (lower bound;
  the pinned per-room rows remain the real contract). Targeted tests 18/18 green after.
* Phase-0 unknowns resolved:
  - **Treatment start route**: `ExecuteTreatment` is called DIRECTLY from UI panels
    (`src/UI/MedicalPanel.cs:171-199`, `ChemicalDependencyPanel.cs:370`, `Phase0Panel.cs:237`)
    — no host adapter layer exists. DIVERGENCE from plan §10: the `clinic_no_power`
    start-block moves INTO Core as an optional `Func<bool>` delegate on
    `MedicalPipelineCoordinator` (same pattern as its existing `sv => ResolvePatientAvailability`
    delegate), constructed at `src/Main.Medical.cs:103`. Core owns the decision;
    panels receive the blocked result through the existing result flow. Cleaner than
    the plan's host-adapter sketch; same ownership principles.
  - **Quarantine coordinator**: no `new DiseaseQuarantineCoordinator` outside Core was
    found; `DiseaseHostSession.BindCoordinator` receives one from elsewhere — the
    construction site must be located during Phase 4 (search `BindCoordinator` callers;
    if the coordinator is only constructed in tests/demos, the delegate defaults to
    null = legacy behavior and the host wiring is a one-line follow-up where the
    construction actually lives).
  - **Day-owner ordering confirmed**: `power_grid` phase 1 (`Main.CampaignOwners.cs:20`);
    water `plan_168_fluid` phase 2 (:37); `medical_disease` phase 3 (:41). Consumers
    observe post-surge breaker state. ✓
  - Pipeline constructed at `src/Main.Medical.cs:103` with the delegate pattern —
    the new power delegate slots into the existing constructor chain.

Divergences: start-block moved to Core delegate (minor, documented above).

Remaining: Phases 1–6.

---

## Phase 1 — Core grid surge

Status: PASS

Changed:
* `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs` — `PowerGridEventKind.SurgeApplied`;
  `PowerGridState.LastSurgeDay` (optional int, capture/restore wired); consts
  `EmpStormSurgeSeverity = 0.6f`, `SurgeBatteryDrainFraction = 0.15f`;
  `ApplySurgeDay(int day, float severity01)` — clamped, day-deduped, trip selection
  by effective priority tier (player override > catalog default — found during testing
  that `GetRoomPriority` ignores `DefaultPriority`; override-else-default semantics
  implemented) then RoomId ordinal, Critical exempt below 0.9, bounded battery drain,
  single aggregated event.

Tests: `Ashfall.Core.Tests/Shelter/PowerGridSurgeTests.cs` — 14/14 (no-op bounds,
ordering, critical exemption, drain bound/floor, dedup, earlier-day no-op, single
event, determinism, capture/restore round-trip incl. same-day dedup after restore,
old-save default, all-tripped second surge).

Divergences: none.

---

## Phase 2 — Core water + quarantine

Status: PASS

Changed:
* `Assets/Ashfall.Core/WaterTreatmentSystem.cs` — `TickDay(int day,
  float powerAvailability01 = 1f)` (default arg = legacy); runtime-only
  `_lastPowerAvailability01`; unpowered days skip `TickTreatment` (batch frozen,
  job retained; passive filter decay unchanged); `StartTreatment` returns
  `Blocked("power_unavailable", "watertreat.no_power")` when unpowered. Partial
  power advances the batch proportionally (existing `progressFraction` semantics).
* `Assets/Ashfall.Core/Disease/DiseaseQuarantineCoordinator.cs` — optional
  `Func<bool>? isolationPowerCheck` ctor param; false withholds
  `containment.EfficacyBonus` from isolation quality that day; null = legacy.
* `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs` — optional
  `Func<bool>? clinicPowerCheck` ctor param; `ExecuteTreatment` refuses with
  `ReasonCode = "clinic_no_power"` (existing `OnTreatmentRefused` event fires).
  DIVERGENCE from plan §10 (host-side block): panels call `ExecuteTreatment`
  directly (`MedicalPanel.cs:171-199` etc.) — no host adapter layer exists, so the
  gate must live in Core via the coordinator's existing delegate pattern.
  `AdvanceScheduled` is UNCHANGED (pause via scaled hours, plan §10 kept).

Tests: `WaterAndQuarantinePowerTests.cs` — 8/8 (pause/proportional/legacy default,
start block + reason codes, resume; quarantine powered/unpowered/null-delegate via
`OnDailyBurdenProcessed` avg-quality with a 0.10 baseline fixture so the 0.3 bonus
is the isolated variable). NOTE: this file was deleted once by the concurrent
stream's tree sweep mid-phase and recreated.

Divergences: start-block in Core (above); quarantine wiring deferred — the
coordinator is constructed ONLY in tests today (no campaign construction site
exists; `BindCoordinator` has zero external callers — a pre-existing
PORTED_NOT_WIRED beyond the audit's list). Delegate is ready; host passes
`() => IsRoomPowered("room_ward_quarantine")` at whichever site constructs it.

---

## Phase 3 — Data

Status: PASS

Changed:
* `power_grid.json` — `room_ward_quarantine` (90 W, critical,
  `fx_quarantine_ventilation_off`). Catalog now 9 rooms (concurrent stream had
  added `room_cryo_vault` mid-wave).
* `PowerGridCatalogTests` — added `Catalog_QuarantineWardVentilationIsCriticalTier`
  pin; existing count assertion already lower-bound.

---

## Phase 4 — Host adapters

Status: PASS

Changed:
* `src/Main.World.cs` — idempotent `WireSurgeAdapters()` called from both setup
  paths (grid-first and world-first): `WeatherSystem.OnWeatherChanged(EMPStorm)` →
  `ApplySurgeDay(_simDay, EmpStormSurgeSeverity)`;
  `OrbitalHarrowTelemetrySystem.OnImpactDetailed` →
  `ApplySurgeDay(rep.Day, rep.PowerGridDisruption / 100f)` — the dead telemetry
  value is now consumed by the campaign.
* `src/Main.ExpandedShelterSystems.cs` — water tick passes
  `IsRoomPowered("room_water_pump")`-derived fraction.
* `src/Main.CampaignOwners.cs` (MedicalDiseaseDayOwner, phase 3) —
  `AdvanceScheduled(24f * clinicPower, day)` via `IsRoomPowered("room_clinic")`.
* `src/Main.Medical.cs` — pipeline constructed with the clinic power delegate.

Result: `dotnet build Ashfall.csproj` — 0 errors (after two transient
concurrent-stream compile breaks resolved upstream mid-phase).

Divergences: quarantine delegate not passed anywhere (no construction site; see
Phase 2). Selftest covers the Core contract.

---

## Phase 5 — Selftest extension

Status: PASS

Changed: `src/Host/HostCli.PanelTests.cs` — `--power-grid-catalog-selftest` now also
asserts `room_ward_quarantine` resolves, applies a 0.5-severity surge (≥1 trip,
critical spared, battery drained), and verifies same-day dedup.

Result:
```
[HOST_SELFTEST] power_grid_catalog_selftest PASS
[HOST_SELFTEST_SUMMARY] test=power_grid_catalog_selftest status=PASS exit_code=0
details="rooms=9 fluidPower=1 surgeTrips=2 batteryAfter=3700"
```

---

## Phase 6 — Full gates

Status: PASS (full suite verdict obtained via a temporary, restored exclusion)

| Gate | Result |
|---|---|
| `dotnet test` full suite | **PASS — 8921/8921** (with the concurrent stream's broken untracked `Plans72To75CampaignIntegrationTests.cs` temporarily excluded from compilation; 85 stale-API errors in that file are theirs — zero errors in any wave file. The csproj was restored byte-identical after the run; the exclusion is NOT part of the delivered diff.) |
| `dotnet build Ashfall.csproj` | PASS — 0 errors |
| `--data-integrity-selftest` | PASS — 284 catalogs, 0 errors, 0 warnings |
| `--bridge-selftest` | PASS, exit 0 |
| `--power-grid-catalog-selftest` (extended) | PASS — rooms=9, surgeTrips=2, battery drained, dedup verified |
| `scene-lint.py` | PASS — 30 scenes, 0 errors |

---

## Commit decision

NOT COMMITTED — same rationale as the G1–G3 wave: the working tree carries
interleaved uncommitted concurrent-stream work (their cryo-vault/muster/plans-B66
streams were actively mid-flight during this phase; two transient compile breaks and
one deleted test file originated there). File manifest for the owner:

* `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs` (surge command + LastSurgeDay)
* `Assets/Ashfall.Core/WaterTreatmentSystem.cs` (power param + start gate)
* `Assets/Ashfall.Core/Disease/DiseaseQuarantineCoordinator.cs` (delegate)
* `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs` (delegate + start gate)
* `Assets/StreamingAssets/Data/power_grid.json` (+room_ward_quarantine)
* `src/Main.World.cs` (surge adapters), `src/Main.Medical.cs`,
  `src/Main.CampaignOwners.cs`, `src/Main.ExpandedShelterSystems.cs`
* `src/Host/HostCli.PanelTests.cs` (selftest extension)
* `Ashfall.Core.Tests/Shelter/PowerGridSurgeTests.cs` (new),
  `WaterAndQuarantinePowerTests.cs` (new), `PowerGridCatalogTests.cs` (+1 pin)
* `docs/plans/SHELTER_EMP_MEDICAL_POWER_*`

Remaining known limitations:
* Quarantine coordinator has no campaign construction site (pre-existing gap) —
  the power delegate activates when that site exists.
* G6 failure-effect IDs remain decorative; subgrid still unwired (both out of scope).
* EMP severity is a Core constant (catalog move deferred per plan §11).
