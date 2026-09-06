# SHELTER EMP & MEDICAL POWER — INTEGRATION PLAN (G4–G5)

**Source evidence:** `docs/forensics/SHELTER_CASCADE_SEAMS_FORENSIC_REPORT.md` (G4/G5/G6 findings)
**Predecessor wave:** `SHELTER_GRID_CATALOG_SEAL` (G1–G3) — implemented; catalog load, room-ID
vocabulary, and `--power-grid-catalog-selftest` are now in place and are dependencies of this plan.
**Scope:** G4 (EMP/orbital → power-grid surge coupling) and G5 (water-treatment + medical
ward/quarantine power dependency). **G6 (FailureEffectId consumers) stays out of scope** —
the failure-effect vocabulary remains decorative; this wave wires real state instead.
**Plan class:** Integration wave — new Core commands on existing authorities, host adapters only.

---

# 1. Objective

An EMP storm or orbital impact must deterministically trip shelter breakers and drain
battery reserves (persisted, repairable). A clinic power outage must pause scheduled
procedures and block new ones; a quarantine-ventilation outage must reduce isolation
quality; a water-plant outage must pause treatment batches. All through existing
authorities — no new systems, no new save sections.

# 2. Current Reality (verified 2026-09-06)

| Fact | Evidence |
|---|---|
| `WeatherSystem.OnWeatherChanged(WeatherKind)` event exists; `EMPStorm` is a `WeatherKind` | `Assets/Ashfall.Core/World/WeatherSystem.cs:69,176-181` |
| `OrbitalHarrowTelemetrySystem` emits `OrbitalImpactReport.PowerGridDisruption` (0–100) via `OnImpactDetailed`; sole subscriber is a HostCli selftest | `OrbitalHarrowTelemetrySystem.cs:62,204,251`; `HostCli.DynamicWorld.cs:119` |
| Orbital telemetry is campaign-constructed at `Main.FlagshipInstitutions.cs:87` (`EnsureOrbitalHarrowTelemetry`) | `src/Main.FlagshipInstitutions.cs` |
| `PowerGridSystem` has trip primitives (`MarkTripped(roomId, day)`, `ClearTripped`, breaker state, battery) — no surge entry point | `PowerGridSystem.cs:115-122` |
| Trips already persist (`TrippedRooms` in `PowerGridState`/`PowerGridSave`) | `PowerGridSave.cs` |
| `PowerDistributionSubgridSystem` has a full fuse/surge model (`is_fuse_blown`, `ApplyRoomLoad`, capacitor buffer, oil degradation) but **zero campaign callers** — itself PORTED_NOT_WIRED | `PowerDistributionSubgridSystem.cs:132-165`; grep: no `ApplyRoomLoad` callers in src |
| `WaterTreatmentSystem.TickDay(int day)` advances the active batch unconditionally; `StartTreatment` has no power gate | `WaterTreatmentSystem.cs:579-602,244` |
| Water treatment is ticked at `Main.ExpandedShelterSystems.cs:329` (`_waterTreatment?.TickDay(day)`) | src |
| `MedicalPipelineCoordinator.AdvanceScheduled(float hours, int currentDay)` — "Called ONLY by the campaign day owner"; procedures are hour-based; costs consumed at completion | `MedicalPipelineCoordinator.cs:519+` |
| Called at `Main.CampaignOwners.cs:401` with hardcoded `24f` | src |
| Player procedure start is `MedicalPipelineCoordinator.ExecuteTreatment(...)` (`MainOperationResult` + `ReasonCode` pattern) | `MedicalPipelineCoordinator.cs:331` |
| `DiseaseQuarantineCoordinator.TickDaily` computes per-patient isolation `quality`, applies `containment.EfficacyBonus`, clamp 0.10–1.0; constructor takes optional delegates (`_tryConsumeItem`, `_containmentProvider`) | `DiseaseQuarantineCoordinator.cs:244-295,32-46` |
| `room_ward_quarantine` is a canonical room ID (`shelter_rooms.json`) but absent from `power_grid.json` | data |
| `room_clinic` is in the catalog (120 W, critical, `fx_clinic_off`) | `power_grid.json` |
| Established power-snapshot pattern: `BuildAgricultureEnvironment` reads `IsRoomPowered` into a per-day snapshot | `Main.Plans162_165.cs:101-113` |
| Established blocked-result pattern: `ActionResult.Blocked("power_unavailable", "sump.centrifuge_no_power")` | `SumpFloodingSystem.cs:240-241` |

# 3. Required Delta

**Existing:** EMP/orbital events fire with zero grid effect; water treatment, procedure
advance, and quarantine quality are power-inviolate.

**Requested:** grid surge response (trips + battery drain, persisted, repairable);
water batch pause/block when the water plant is unpowered; procedure pause when the
clinic is unpowered + blocked starts; reduced isolation quality when quarantine
ventilation is unpowered.

**Delta:** one new Core command + one new persisted scalar on the grid; two Core
signature extensions with defaults; one optional delegate on the quarantine
coordinator; host adapters wiring events and passing power snapshots. No new systems.

# 4. Evidence

§2. All claims verified by direct source reads this session; the G1–G3 wave already
proved the room-ID/load/fallback mechanics this plan builds on.

# 5. Existing Extension Seams

- `PowerGridSystem.MarkTripped/ClearTripped` + `OnPowerChanged` — trip application and UI feedback already exist.
- `PowerGridSave`/`PowerGridState` capture/restore — optional-field tolerance is the repo's tested migration policy.
- `WeatherSystem.OnWeatherChanged` — typed weather observation.
- `OnImpactDetailed` — typed orbital report; `PowerGridDisruption` is a dead value awaiting a consumer.
- `WaterTreatmentSystem.TickDay` / `StartTreatment` — batch model with `ActionResult` reason codes.
- `MedicalPipelineCoordinator.AdvanceScheduled(hours)` — hours are the natural power-fraction knob.
- `DiseaseQuarantineCoordinator` optional-delegate constructor pattern (`_tryConsumeItem`).
- `BuildAgricultureEnvironment` — the per-day power snapshot pattern to copy.

# 6. Proposed Architecture

```
EMPStorm onset (WeatherSystem.OnWeatherChanged) ──┐
                                                  ├─▶ PowerGridSystem.ApplySurgeDay(day, severity01)
OrbitalImpactDetailed.PowerGridDisruption ────────┘        │ trips (existing TrippedRooms) + battery drain
                                                           │ (existing events; LastSurgeDay dedup)
WaterTreatmentSystem.TickDay(day, power01) ◀── room_water_pump breaker
MedicalPipelineCoordinator ◀── 24h × clinicPower (room_clinic breaker)   [host day owner]
ExecuteTreatment blocked ◀──────────────── clinic breaker (host command adapter, reason clinic_no_power)
DiseaseQuarantineCoordinator.TickDaily ◀── isolationPowerCheck delegate (room_ward_quarantine breaker)
```

Phase ordering inside a day (documented contract): weather/orbital surge applies when
the event fires (PowerGridDayOwner owns the grid tick; surge events may arrive before
or after it — trips are idempotent per room and battery drain is bounded, so ordering
cannot double-apply). Water/medical/quarantine consumers read breaker state during
their own day-owner phases, after the grid tick (PowerGridDayOwner precedes
GreenhouseFoundryDayOwner and the medical owner in `Main.CampaignOwners.cs`).

# 7. Ownership Matrix

| Concern | Owner |
|---|---|
| Surge math, trip selection, battery drain, dedup | `PowerGridSystem` (Core) |
| EMP storm severity constant | Core const (see §11 — catalog move deferred) |
| Orbital disruption value | `OrbitalHarrowTelemetrySystem` (existing, unchanged) |
| Event→command adapters | Host (`src/Main.*`), never UI |
| Water batch pause semantics | `WaterTreatmentSystem` (Core) |
| Procedure pause semantics | Host day owner scaling `AdvanceScheduled` hours |
| Procedure start block | Host command adapter (reason code), Core unchanged |
| Isolation quality modifier | `DiseaseQuarantineCoordinator` via injected delegate |
| Room catalog | `power_grid.json` |

# 8. Data Flow

1. `WeatherSystem` transitions to `EMPStorm` → host adapter → `ApplySurgeDay(day, EMP_STORM_SEVERITY)`.
2. Orbital impact resolves → `OnImpactDetailed` → host adapter → `ApplySurgeDay(day, disruption/100f)`.
3. `ApplySurgeDay` dedups on `LastSurgeDay`, trips selected rooms, drains battery, emits `PowerGridEvent(SurgeApplied)` → panel/audio/status refresh (existing listeners).
4. Next water tick reads `room_water_pump` breaker → batch advances or pauses.
5. Medical day owner reads `room_clinic` breaker → scales procedure hours.
6. Player treatment request → host adapter checks `room_clinic` → blocked with `clinic_no_power` or proceeds to `ExecuteTreatment`.
7. Quarantine `TickDaily` invokes `isolationPowerCheck` → EfficacyBonus applied or withheld.
8. All above persist through existing sections; trips via `TrippedRooms`, dedup via `LastSurgeDay`.

# 9. State Model

| State | Owner | Change |
|---|---|---|
| `PowerGridState.LastSurgeDay` (int, default 0) | Core grid | NEW optional field; old saves restore as 0; captured/restored in `Capture()`/`RestoreInto()`; NOT a schema bump (optional-field tolerance, per repo migration policy) |
| Tripped rooms | existing | unchanged shape |
| Water batch (`isProcessing`, progress) | existing | pause is emergent — no new fields |
| Procedure rows (remaining hours) | existing | pause is emergent — hours simply don't advance |
| Isolation quality | per-day recomputed | no persistence change |

Invariants after restore: `LastSurgeDay ≤ SimDay`; no negative battery; trips pruned to valid room IDs (existing `NormalizeAndValidate`).

# 10. API / Contracts

| Contract | Kind | Notes |
|---|---|---|
| `PowerGridSystem.ApplySurgeDay(int day, float severity01)` | NEW Core command | `severity01` clamped 0–1; `≤0` no-op; dedup: no-op if `day == LastSurgeDay`; on apply: trip selection (§13), battery drain `floor(BatteryCapacityWh × 0.15 × severity01)`, set `LastSurgeDay`, emit `PowerGridEventKind.SurgeApplied` |
| `PowerGridEventKind.SurgeApplied` | NEW enum member | Existing event channel; hosts already listen |
| `WaterTreatmentSystem.TickDay(int day, float powerAvailability01 = 1f)` | EXTENDED (default arg) | `power01 ≤ 0` → skip `TickTreatment` advance (batch paused); contamination/filter decay unchanged (passive, not power-dependent) |
| `WaterTreatmentSystem.StartTreatment(...)` | EXTENDED | Returns `ActionResult.Blocked("power_unavailable", "watertreat.no_power")` when `power01 ≤ 0`; stores last known power for the check |
| `DiseaseQuarantineCoordinator` ctor param `Func<bool>? isolationPowerCheck = null` | EXTENDED | Null/true → EfficacyBonus applies (old behavior); false → bonus withheld that day |
| `MedicalPipelineCoordinator` | UNCHANGED | Pause via scaled hours; start-block is host-side |

# 11. Data Changes

**`Assets/StreamingAssets/Data/power_grid.json` (MODIFY — additive)**
- Add `room_ward_quarantine` (id `room_ward_quarantine`, display "Quarantine Ward",
  draw 90, priority `critical`, failure_effect_id `fx_quarantine_ventilation_off`).
  Rationale: canonical room ID (`shelter_rooms.json`); ventilation is life-safety →
  critical tier sheds last. Balance parameter — implementer may adjust the draw.

**EMP severity (deliberate constant, not catalog)**
- `PowerGridSystem.EmpStormSurgeSeverity = 0.6f` const for the weather path; the
  orbital path is inherently data-driven (`PowerGridDisruption`). Moving severity
  into a hazards catalog requires a new file + scanner registration — deferred (§22).
  **Update `PowerGridCatalogTests` room count 7 → 8 and pin the new room.**

# 12. Save/Load

- Grid: only `LastSurgeDay` added (optional int). Old saves (missing field) restore
  with 0 → "no surge yet" → next event applies. Round-trip test added. No envelope
  version bump; `SaveSectionRegistry` section name unchanged (`power_grid`).
- Water/medical/quarantine: no new persisted state; pause/emergent behavior re-derives
  from breaker state on the next tick.

# 13. Determinism — surge trip selection

Given `severity01` and the room list (catalog order):
1. Candidate set = rooms NOT tripped, priority tier descending (Low → Standard → Critical; tier = `(int)priority`, lower trips first).
2. Within a tier: RoomId ordinal ascending.
3. Trip count = `floor(severity01 × candidateCount)`; Critical rooms only enter the candidate set when `severity01 ≥ 0.9f`.
4. Battery drain as in §10 (floor at 0).
5. Emit one aggregated event; `LastSurgeDay` set even when zero trips (event consumed).

Same seed + same state → identical trip set (unit-tested; ties impossible due to ordinal tiebreak).

# 14. System / Event Wiring

| Event | Producer | Consumer (new) |
|---|---|---|
| `OnWeatherChanged(EMPStorm)` | `WeatherSystem` | Host adapter → `ApplySurgeDay(day, 0.6f)` |
| `OnImpactDetailed(report)` | `OrbitalHarrowTelemetrySystem` | Host adapter → `ApplySurgeDay(day, report.PowerGridDisruption / 100f)` |
| `OnPowerChanged(SurgeApplied)` | `PowerGridSystem` | Existing panel/status/audio listeners (no new work) |

Host adapter lives with the grid wiring (`Main.World.cs` or `Main.FlagshipInstitutions.cs`
— implementer places both subscriptions beside existing event plumbing). Idempotency:
`ApplySurgeDay` dedups on `LastSurgeDay`; a storm lasting multiple days re-trips only
if the implementer passes distinct days — weather onset subscription fires once per
transition, so multi-day storms apply once (documented).

# 15. Godot Integration

- Wire the two adapters; extend `--power-grid-catalog-selftest` (Phase 5).
- PowerGridPanel: tripped breakers already render with reset affordance — surge trips appear there with no UI change; the `SurgeApplied` event text surfaces via existing event/status feed.
- Water panel: existing processing state display now truthfully pauses; blocked `StartTreatment` returns the standard `LastEvent` reason (`watertreat.no_power`) — satisfies the §13 flagship "actionable reason" standard.
- Medical UI: procedure rows simply stop advancing; the treatment action returns `clinic_no_power` when blocked.
- No new panels, no scene changes.

# 16. Narrative / Content Integration

None. (EMP narrative text already exists — `weather_emp_storm_radio_blackout`; this
wave makes the machine state match the fiction. Journal hooks are G6 territory.)

# 17. Failure Modes

| Case | Expected behavior |
|---|---|
| Orbital impact with healthy armor (disruption 0) | `ApplySurgeDay` no-op (`severity ≤ 0`), `LastSurgeDay` NOT set |
| Two surge sources same day (EMP storm + impact) | Second call no-ops on `LastSurgeDay` dedup — one bounded consequence pass per day (flagship §1.5) |
| All rooms already tripped | Trip count = 0; battery drain still applies; no error |
| severity ≥ 0.9 | Critical rooms eligible; at least the clinic breaker can trip — documented as the "grid devastated but recoverable" bound |
| Old save, `LastSurgeDay` absent | Restores 0; first surge applies normally |
| Water batch mid-progress during outage | Batch pauses (`isProcessing` retained, progress frozen); resumes when powered |
| Player clicks StartTreatment during outage | `ActionResult.Blocked("power_unavailable", "watertreat.no_power")` — actionable reason |
| Procedure mid-surgery during clinic outage | Remaining hours frozen; completes after power returns; no reroll, no cost anomaly (costs consume at completion) |
| Patient dies during paused procedure | Existing `AdvanceScheduled` failure path (cancel + full release) — unchanged |
| Quarantine delegate throws / null | Null → legacy behavior; host passes a pure grid read that cannot throw |
| Grid session absent (`System == null` host guards) | Adapters null-guard; consumers default to powered (matches `BuildAgricultureEnvironment`'s `?.` pattern) |

# 18. Test Strategy

**Core unit tests**
- `PowerGridSystem surge`: severity 0 no-op; dedup same day; trip ordering (Low before Standard before Critical; ordinal within tier); floor counts; Critical exempt below 0.9; battery drain bounded and floored; events emitted once; determinism (same seed/state → same trip set); `LastSurgeDay` capture/restore round-trip; old-save restore (field absent → 0).
- `WaterTreatmentSystem`: unpowered tick pauses batch (progress frozen, `isProcessing` retained); powered tick advances (default arg keeps legacy callers); `StartTreatment` blocked unpowered with reason codes; contamination decay still applies during outage (not power-dependent).
- `DiseaseQuarantineCoordinator`: power off → EfficacyBonus withheld, clamp still enforced; delegate null → legacy; power on → legacy.

**Integration**
- Grid tick → water tick ordering (grid day owner precedes consumers) — consumer sees post-surge breaker state.
- `AdvanceScheduled(0, day)` during outage leaves procedure rows untouched; resume completes and consumes costs exactly once.

**Headless**
- Extend `--power-grid-catalog-selftest`: assert `room_ward_quarantine` resolves; simulate `ApplySurgeDay` and verify trips + `fluidPower`/water derivation react (G1 guard extended).

# 19. Dependency-Ordered Phases

**Phase 0 — Verification.** Confirm baseline gates green (Core suite, host build,
data-integrity, `--power-grid-catalog-selftest`). Locate the exact host command
adapter for `ExecuteTreatment` (MedicalDetailPanel action route) and the quarantine
coordinator construction site (`src/Disease/DiseaseHostSession.cs`). Confirm
`PowerGridDayOwner` ordering precedes water/medical owners in `Main.CampaignOwners.cs`.
*Must not touch: subgrid system, greenhouse, fluids.*

**Phase 1 — Core grid surge.** `ApplySurgeDay` + `SurgeApplied` + `LastSurgeDay`
(+ capture/restore) in `PowerGridSystem`/`PowerGridState`. Unit tests (§18).
*Gate: targeted tests green; Core builds.*

**Phase 2 — Core water + quarantine.** `TickDay` power param, `StartTreatment` gate,
coordinator delegate + quality modifier. Unit tests. *Gate: targeted green; legacy
callers unaffected (default args).*

**Phase 3 — Data.** `room_ward_quarantine` in `power_grid.json`; update
`PowerGridCatalogTests` (8 rooms, pinned row). *Gate: catalog tests + data-integrity green.*

**Phase 4 — Host adapters.** Weather + orbital subscriptions → `ApplySurgeDay`;
water tick passes grid power; medical day owner scales hours; treatment-start
adapter returns `clinic_no_power`; quarantine session passes the delegate.
*Gate: host build 0/0.*

**Phase 5 — Selftest extension.** Surge scenario in `--power-grid-catalog-selftest`.
*Gate: verb PASS.*

**Phase 6 — Full gates.** AGENTS.md checklist + scene lint + full suite.

# 20. File Impact Map

| File | Action | Reason | Risk |
|---|---|---|---|
| `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs` | MODIFY | `ApplySurgeDay`, `SurgeApplied`, `LastSurgeDay` | MEDIUM (core authority — small, tested surface) |
| `Assets/Ashfall.Core/WaterTreatmentSystem.cs` | MODIFY | Power param + start gate | LOW |
| `Assets/Ashfall.Core/Disease/DiseaseQuarantineCoordinator.cs` | MODIFY | Optional delegate + bonus gate | LOW |
| `Assets/StreamingAssets/Data/power_grid.json` | MODIFY | `room_ward_quarantine` | LOW |
| `src/Main.World.cs` or `Main.FlagshipInstitutions.cs` | MODIFY | Surge event adapters | LOW |
| `src/Main.ExpandedShelterSystems.cs` | MODIFY (1–2 lines) | Water power snapshot | LOW |
| `src/Main.CampaignOwners.cs` | MODIFY (1 line) | Scale `AdvanceScheduled` hours | LOW |
| Host treatment-start adapter (located Phase 0) | MODIFY | `clinic_no_power` block | LOW |
| `src/Disease/DiseaseHostSession.cs` | MODIFY | Pass delegate | LOW |
| `Ashfall.Core.Tests/*` (3 new/extended files) | CREATE/MODIFY | §18 coverage | LOW |
| `src/Host/HostCli.PanelTests.cs` | MODIFY | Selftest extension | LOW |
| `PowerDistributionSubgridSystem.cs`, fluids, greenhouse, `MedicalPipelineCoordinator.cs` | READ ONLY | Explicitly untouched | — |

# 21. Risks

| Risk | Mitigation |
|---|---|
| Surge trips annoy players in existing campaigns (new failure source) | Bounded by severity; critical rooms exempt below 0.9; trips are the existing, resettable breaker mechanic with visible UI |
| Double-apply from storm + impact same day | `LastSurgeDay` dedup (tested) |
| Pause semantics leak into cost/consumption logic | Costs consume at completion (existing); pause only freezes hours — verified by integration test |
| `TickDay` signature change breaks other callers | Default argument preserves source compatibility; compile gate catches any reflection/direct misuse |
| Quarantine quality drop reads as a bug | Blocked/paused reasons surface via existing event feed; flagship §13 reason strings included |
| Severity constants feel wrong in play | Constants isolated (`EmpStormSurgeSeverity`, drain fraction) for a later balance pass |

# 22. Out of Scope

- G6 `FailureEffectId` consumers (fx_* IDs remain authored-inert; real state is wired instead).
- Subgrid campaign wiring (`ApplyRoomLoad` is currently uncalled) — the fuse/surge
  node model is the natural home for per-room EMP damage in a future wave; this plan
  couples at `PowerGridSystem` level only.
- EMP severity catalog file; per-cultivar/room EMP susceptibility fields.
- Radio/blackout side effects of EMP (already narrative-driven).
- Greenhouse (`BuildAgricultureEnvironment`) and fluid power seams (already live from G1–G3).

# 23. Rollback Strategy

- Phase-per-commit; each phase independently revertible.
- `LastSurgeDay` is additive and inert if adapters are reverted — old saves stay valid in both directions.
- Water/quarantine changes are default-argument/delegate additions: reverting host wiring restores legacy behavior exactly.

# 24. Definition of Done

- [ ] EMPStorm onset and orbital impacts each apply at most one deterministic, persisted surge per day (trips + battery drain), deduped, repairable via the existing breaker UI.
- [ ] Water treatment batch pauses and StartTreatment blocks with `watertreat.no_power` when `room_water_pump` is unpowered.
- [ ] Scheduled procedures freeze during clinic outages and complete exactly once after power returns; new treatments blocked with `clinic_no_power`.
- [ ] Quarantine isolation quality loses the containment bonus while `room_ward_quarantine` is unpowered.
- [ ] All §18 tests pass; full AGENTS.md gate matrix green including extended selftest.
- [ ] No changes to `MedicalPipelineCoordinator`, subgrid, fluids, greenhouse.

# 25. Implementation Handoff

**MUST PRESERVE**
- `PowerGridSystem` as the single surge authority; no second trip/battery model (subgrid stays unwired and untouched).
- Default-argument/delegate backward compatibility for all Core signature extensions.
- Existing day-owner ordering: grid tick precedes water/medical consumers.
- Cost-at-completion semantics of the medical pipeline.

**MUST ADD**
- `ApplySurgeDay` + `SurgeApplied` + `LastSurgeDay` (with round-trip + old-save tests).
- Water power param/gate; quarantine delegate; host adapters; `room_ward_quarantine` catalog row; extended selftest.

**MUST NOT DO**
- Do not wire or modify `PowerDistributionSubgridSystem` (future wave).
- Do not add new save sections, UI panels, or narrative hooks.
- Do not let panels compute power state — command adapters only.
- Do not apply surge severity > authored values or unbounded battery drain.

**VERIFY WITH**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --power-grid-catalog-selftest
godot --headless --path . -- --bridge-selftest
python3 scripts/ci/scene-lint.py
```

**FIRST SAFE IMPLEMENTATION STEP**
Phase 0: confirm baseline gates and pin down the two Phase-0 unknowns (treatment-start
adapter route; quarantine coordinator construction arguments) before touching Core.

**Suggested next prompt after accepting this plan:**
"Execute the SHELTER_EMP_MEDICAL_POWER plan via ashfall-implement, phase by phase, journaling to docs/plans/SHELTER_EMP_MEDICAL_POWER_IMPLEMENTATION_LOG.md."
