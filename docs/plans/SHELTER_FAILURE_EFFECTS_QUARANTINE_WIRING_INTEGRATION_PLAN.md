# SHELTER FAILURE EFFECTS & QUARANTINE WIRING — INTEGRATION PLAN (G6 + QUARANTINE CONSTRUCTION)

**Source evidence:** `docs/forensics/SHELTER_CASCADE_SEAMS_FORENSIC_REPORT.md` (G6);
**Post-G4–G5 recon** (this session — several original G6 gaps no longer exist).
**Predecessors:** SHELTER_GRID_CATALOG_SEAL (G1–G3), SHELTER_EMP_MEDICAL_POWER (G4–G5) — both in-tree.
**Plan class:** Small sealing wave — one new Core seam, one missing campaign construction.

---

# 1. Objective

1. Give every authored `fx_*` failure-effect ID either a live state consumer or a
   formally registered disposition — closing G6.
2. Construct and bind `DiseaseQuarantineCoordinator` in the campaign (it currently
   exists only in tests), activating the G5 quarantine power delegate.

# 2. Current Reality (verified this session)

| Fact | Evidence |
|---|---|
| 6 of 8 `fx_*` IDs gained real consumers via G1–G5 + prior work: `fx_clinic_off` (G5 procedure gate), `fx_water_pressure_drop` (G1 fluid power), `fx_grow_lights_off` (Plan 162 lighting), `fx_foundry_standstill` (foundry gate), `fx_workshop_offline` (workshop gate), `fx_quarantine_ventilation_off` (G5 delegate, pending construction below) | this session's greps |
| `fx_filtration_off` has NO consumer: `StartingLevelSystem.TickDay` degrades air filtration unconditionally — no power input | `Assets/Ashfall.Core/StartingLevel/StartingLevelSystem.cs:206-232` |
| `fx_lighting_dim`'s effect (dimmed lighting) is already consumed by `ShelterScheduleSystem`'s brownout path (halved lighting demand) | `ShelterScheduleSystem.cs:186-191` |
| Air filtration tick site: `Main.CampaignOwners.cs:175` (`_m._startingLevel.TickDay()` — no args) | src |
| `DiseaseQuarantineCoordinator` is constructed only in tests; `BindCoordinator` has zero external callers; `DiseaseHostSession.TickDaily` already calls `Coordinator?.TickDaily(day)` and is ticked at `Main.CampaignOwners.cs:456` | `src/Disease/DiseaseHostSession.cs:138-148`, `Main.CampaignOwners.cs:456` |
| Campaign wiring points exist: ward at `Main.Medical.cs:294` (`_medicalWard`, isolation bed `bed_isolation`), disease engine `_expansions.Disease` (`SetupDisease`, `Main.Medical.cs:390-400`), roster in `Main.DutyRoster.cs` | src |
| Coordinator ctor: `(ward, diseaseSystem, dutyRoster?, tryConsumeItem?, containmentProvider?, isolationPowerCheck?)` — all optional after the first two | `DiseaseQuarantineCoordinator.cs:41-47` |
| `ContainmentCapability.FromResearch(Func<string,bool>)` is the established research-projection factory | `ContainmentCapability.cs:34` |

# 3. Required Delta

**Existing:** authored `fx_*` vocabulary inert; quarantine coordinator unbound in campaign
(`DiseaseHostSession.Coordinator == null` → `TickDaily` is a no-op passthrough).

**Requested:** air filtration responds to power loss; every `fx_*` ID has a named,
test-verified consumer; the quarantine coordinator ticks in the campaign with the
G5 power delegate, inventory consumption, and research containment.

**Delta:** one optional Core parameter (air filtration), one construction + bind block
(host), one verification test (fx registry), journal/doc updates. No new systems.

# 4. Evidence

§2. All claims verified by direct source reads this session.

# 5. Existing Extension Seams

- `StartingLevelSystem.TickDay(bool, WeatherKind)` — optional-parameter extension (G5 pattern).
- `DiseaseHostSession.BindCoordinator` — the ready-made binding point.
- `DiseaseQuarantineCoordinator` ctor delegates — `tryConsumeItem`, `containmentProvider`, `isolationPowerCheck` all defined.
- `ContainmentCapability.FromResearch` — research projection.
- `--power-grid-catalog-selftest` — extend for fx coverage.

# 6. Proposed Architecture

```
room_air_filtration breaker ──▶ StartingLevelSystem.TickDay(day-duty, weather, power01)
                                     │ unpowered ⇒ scrubbing offline:
                                     │ filter degrades faster, quality formula loses
                                     │ the powered offset
room_ward_quarantine breaker ──▶ DiseaseQuarantineCoordinator (NEW campaign construction)
   + inventory + research      │      constructed in Main.SetupDisease()
                                ▼
                     DiseaseHostSession.BindCoordinator → TickDaily (already ticked)
```

No dispatcher framework: each `fx_*` maps to the existing owner that consumes the
state (breaker → system). The "registry" is a test-pinned mapping, not new runtime code.

# 7. Ownership Matrix

| Concern | Owner |
|---|---|
| Air-filtration power math | `StartingLevelSystem` (Core) |
| Quarantine containment math | `DiseaseQuarantineCoordinator` (Core, unchanged) |
| Coordinator construction/lifetime | `Main.SetupDisease()` (host composition root) |
| Grid reads for both | Host delegates over `PowerGridSystem` (never UI) |
| fx→consumer mapping verification | `Ashfall.Core.Tests` |

# 8. Data Flow

Quarantine: `SetupDisease` → construct coordinator (ward, engine, roster, inventory
consumption delegate, research containment, quarantine power delegate) →
`BindCoordinator` → existing `MedicalDiseaseDayOwner` (phase 3) ticks it after the
grid (phase 1) → isolation quality reflects ventilation state → `DiseaseSystem`
transmission uses it (existing `GetIsolationQuality` wiring in the ctor).

Air: `StartingLevelRationsDayOwner` (phase 2) → `TickDay(duty, weather, power01)` →
degradation/quality per §10 — after the grid tick (phase 1).

# 9. State Model

**No new persisted state.** Air-filtration power is a per-day runtime input; the
quarantine coordinator adds no state beyond its existing dictionaries. Save sections
untouched.

# 10. API / Contracts

| Contract | Kind | Notes |
|---|---|---|
| `StartingLevelSystem.TickDay(bool isFilterDutyAssigned, WeatherKind outdoorWeather, float powerAvailability01 = 1f)` | EXTENDED | Default arg preserves both existing overloads/callers. `power01 ≤ 0` = scrubbing offline: filter degradation doubled (matches hazard-weather magnitude — **balance parameter**), and the quality formula drops the powered offset (see below) |
| `DiseaseQuarantineCoordinator` ctor | UNCHANGED | All params exist |
| `DiseaseHostSession.BindCoordinator` | UNCHANGED | Already implemented |

**Air-filtration power semantics (exact contract):**
- Powered (`power01 > 0`): unchanged formulas (legacy behavior preserved).
- Unpowered: `baseDegrade += 4.0f` (stacking with hazard weather) AND
  `airQualityPercent` clamps toward `airFilterHealthPercent * 0.9` (the +10 powered
  offset drops to 0 — the stack stops pushing clean air). Implementer documents the
  resulting curve in the XML summary; exact magnitudes are balance parameters.
- Duty-roster maintenance still halves degradation when powered only.

# 11. Data Changes

None. All 8 `fx_*` IDs already exist in `power_grid.json`. **No catalog edits.**

# 12. Save/Load

No format changes. Air-filtration state persists as today (`StartingLevel` save);
quarantine quality is per-day recomputed. Old saves unaffected (default arg = legacy).

# 13. Determinism

All new inputs are deterministic grid reads inside fixed day-owner phases. No RNG.
Quarantine `TickDaily` ordering is unchanged; the delegate is a pure grid read.

# 14. System / Event Wiring

- `OnDailyBurdenProcessed` (quarantine) already exists — isolation quality surfaces
  through the existing disease flow; no new events.
- Air filtration: existing warning/directive log lines cover player feedback
  (`airHazardWarning` at <50%); the power-off state is communicated via the existing
  Power Grid panel (`room_air_filtration` breaker) — same "actionable reason" model
  as G5.

# 15. Godot Integration

- `Main.SetupDisease()` gains the construction + bind block (details §17).
- No new panels/scenes. The Starting Level shelter panel already shows air quality.

# 16. Narrative / Content Integration

None required. (Optional follow-up, NOT this wave: an isolation-quality journal line.)

# 17. Composition Detail — quarantine construction

In `Main.SetupDisease()` (after `_disease` is constructed and the medical ward +
roster exist), construct and bind:

```csharp
// SHELTER_EMP_MEDICAL_POWER follow-up: campaign quarantine containment.
var quarantineCoord = new DiseaseQuarantineCoordinator(
    _medicalWard,
    engine,                      // DiseaseSystem (_expansions.Disease)
    dutyRoster: <Main's DutyRosterSystem instance>,     // Phase 0: identify instance
    tryConsumeItem: (item, qty) => _inventory.Inventory.Remove(item, qty).IsSuccess,
    containmentProvider: () => ContainmentCapability.FromResearch(
        k => <Main's knowledge authority has k>),       // Phase 0: identify check
    isolationPowerCheck: () => _powerGrid?.System == null
        || _powerGrid.System.IsRoomPowered("room_ward_quarantine"));
_disease.BindCoordinator(quarantineCoord);
```

**Phase-0 unknowns to resolve before writing this block:**
1. The exact roster instance Main uses for ward-external reservations (`Main.DutyRoster.cs`).
2. The knowledge-check delegate (`DiagnosisKnowledgeStore` vs a flag/knowledge ledger).
3. Whether `tryConsumeItem` should use `IPlayerInventoryPort` (`_inventory.Inventory.Remove`
   return shape) — mirror the working test fixture's boolean contract.

# 18. Test Strategy

**Core**
- `StartingLevelSystem` power: unpowered tick degrades faster than powered (same
  duty/weather); quality offset drops when unpowered; default-arg legacy unchanged;
  hazard-weather stacking documented by test.
- **fx registry gate (G6 closure):** a test pinning all 8 authored `fx_*` IDs from
  `power_grid.json` to their named consumer member (grep-backed or literal map), so a
  new failure-effect cannot ship without a consumer — the G6 bug class made structural.
- Quarantine: already covered by `WaterAndQuarantinePowerTests` (8/8) — extend with one
  test proving `tryConsumeItem` depletion lowers quality across days (fixture exists).

**Headless**
- Extend `--power-grid-catalog-selftest`: assert all 8 `fx_*` IDs appear in the loaded
  catalog's rooms (completes the vocabulary loop).

# 19. Dependency-Ordered Phases

**Phase 0 — Verification.** Baseline gates; resolve the three §17 unknowns; confirm
`StartingLevelRationsDayOwner` phase (2) runs after `power_grid` (1) — verified in
G4–G5 recon, re-confirm. *Must not touch: quarantine math, subgrid, fluids.*

**Phase 1 — Core air-filtration seam.** `TickDay` optional param + power semantics +
unit tests. *Gate: targeted green; legacy callers compile unchanged.*

**Phase 2 — Host: air power + quarantine construction.** Pass `room_air_filtration`
power at `Main.CampaignOwners.cs:175`; add the `SetupDisease` construction block.
*Gate: host build 0/0.*

**Phase 3 — fx registry test + selftest extension.** *Gate: targeted green.*

**Phase 4 — Full gates.** AGENTS.md checklist; journal.

# 20. File Impact Map

| File | Action | Reason | Risk |
|---|---|---|---|
| `Assets/Ashfall.Core/StartingLevel/StartingLevelSystem.cs` | MODIFY | Power param + offline semantics | LOW |
| `src/Main.CampaignOwners.cs` | MODIFY (1 line) | Pass air power | LOW |
| `src/Main.Medical.cs` | MODIFY | Coordinator construction + bind | MEDIUM (composition root; Phase-0 unknowns) |
| `Ashfall.Core.Tests/Shelter/StartingLevelPowerTests.cs` (or co-located) | CREATE | Air seam tests | LOW |
| `Ashfall.Core.Tests/Shelter/PowerGridCatalogTests.cs` | MODIFY | fx registry gate | LOW |
| `src/Host/HostCli.PanelTests.cs` | MODIFY | fx completeness in selftest | LOW |
| Everything else | READ ONLY | — | — |

# 21. Risks

| Risk | Mitigation |
|---|---|
| Air-quality curve change harms existing campaigns | Power-off is the delta; powered play is byte-identical (default arg); magnitudes are parameters |
| Coordinator construction changes disease pacing (it currently no-ops) | The coordinator only assigns isolation (quarantine command existed via UI?) — Phase 0 must confirm `ExecuteAssignIsolation` callers; if the UI already quarantines through another path, wiring must not double-quarantine |
| Roster/knowledge wiring mistakes | Phase-0 identification + mirror the proven test fixture |
| fx registry test brittleness | Literal map in test (id → consumer member name), not reflection |

# 22. Out of Scope

- Any new failure effects or catalog entries.
- Corridor-lighting morale mechanics (no such consumer exists; `fx_lighting_dim` is
  dispositioned to the schedule brownout path).
- Subgrid wiring; `OrbitalHarrow` changes; EMP severity cataloging.
- Any change to the medical pipeline, fluids, greenhouse.

# 23. Rollback Strategy

Single-phase revert per change; default args and a construction block that can be
commented out restore exact legacy behavior. No save-format impact.

# 24. Definition of Done

- [ ] Air filtration responds to `room_air_filtration` breaker state (deterministic, legacy-preserving).
- [ ] All 8 `fx_*` IDs pass the registry test with named consumers.
- [ ] Quarantine coordinator constructed, bound, ticked in campaign; power delegate live.
- [ ] Selftest extended; all AGENTS.md gates green.

# 25. Implementation Handoff

**MUST PRESERVE** — Core authorities unchanged beyond the one optional parameter;
legacy default-arg behavior; existing quarantine math; day-owner ordering.
**MUST ADD** — air power seam; construction block; fx registry test; selftest lines.
**MUST NOT DO** — no dispatcher framework, no new events/state, no UI changes, no
subgrid work, no catalog edits.
**VERIFY WITH** — the standard seven-gate matrix (core tests, host build,
data-integrity, power-grid-catalog selftest, bridge, scene-lint).
**FIRST SAFE IMPLEMENTATION STEP** — Phase 0: resolve the three §17 composition
unknowns and re-confirm the baseline (the concurrent stream was mid-flight in these
same files during the last two waves).

**Suggested next prompt after accepting this plan:**
"Execute the SHELTER_FAILURE_EFFECTS_QUARANTINE_WIRING plan via ashfall-implement, phase by phase, journaling to docs/plans/SHELTER_FAILURE_EFFECTS_QUARANTINE_WIRING_IMPLEMENTATION_LOG.md."
