# Plan 12 — Regression Matrix

Maps every Plan 12 deliverable to its test coverage, verification gate, and regression risk.

## Test Coverage Map

### Task 12A — Generational Society

| Deliverable | Test Class | Test Count | Status |
|-------------|-----------|------------|--------|
| Cohort maturation (one-way, persistent) | `Plan12AGenerationTests` | 3 | ✅ PASS |
| Apprenticeship skill grants (canonical skill only) | `Plan12AGenerationTests` | 1 | ✅ PASS |
| 13 events authored in events.json | `Plan12AGenerationTests` | 1 | ✅ PASS |
| 8 condition flags closed set | `Plan12AGenerationTests` | 1 | ✅ PASS |
| 20 questlines authored in questline_master.json | `Plan12AGenerationTests` | 1 | ✅ PASS |
| 26 narrative hooks closed set | `Plan12AGenerationTests` | 1 | ✅ PASS |

### Task 12B — Friction & Ration-Conflict

| Deliverable | Test Class | Test Count | Status |
|-------------|-----------|------------|--------|
| 4 belief sets in ConflictGroups | `Plan12BFrictionTests` | 1 | ✅ PASS |
| Belief pairs reciprocal (AreInConflict symmetric) | `Plan12BFrictionTests` | 1 | ✅ PASS |
| No self-conflict | `Plan12BFrictionTests` | 1 | ✅ PASS |
| Sleep penalty on conflict tick | `Plan12BFrictionTests` | 1 | ✅ PASS |
| Synergy on matching beliefs | `Plan12BFrictionTests` | 1 | ✅ PASS |
| Graffiti catalog (≥10 postings, schema valid) | `Plan12BFrictionTests` | 1 | ✅ PASS |
| 20 events authored in events.json | `Plan12BFrictionTests` | 1 | ✅ PASS |
| All events have choices + effects | `Plan12BFrictionTests` | 1 | ✅ PASS |
| 42 world flags closed set | `Plan12BFrictionTests` | 1 | ✅ PASS |
| Graffiti triggers resolve to event-set flags | `Plan12BFrictionTests` | 1 | ✅ PASS |

### Task 12C — Shelter Decor

| Deliverable | Test Class | Test Count | Status |
|-------------|-----------|------------|--------|
| Assign/remove/list operations | `Plan12CDecorTests` | 6 | ✅ PASS |
| Memorial plaque metadata round-trip | `Plan12CDecorTests` | 4 | ✅ PASS |
| Morale delta calculation | `Plan12CDecorTests` | 3 | ✅ PASS |
| Save section registry | `Plan12CDecorTests` | 2 | ✅ PASS |
| 12 decor items in items.json | `Plan12CDecorTests` | 3 | ✅ PASS |
| Player surface manifest | `Plan12CDecorTests` | 1 | ✅ PASS |
| Capture/restore isolation | `Plan12CDecorTests` | 2 | ✅ PASS |

### Task 12D — Cross-System Continuity

| Deliverable | Test Class | Test Count | Status |
|-------------|-----------|------------|--------|
| Cross-hook authority | `Plan12DCrossSystemContinuityTests` | TBD | Agent creating |
| Chronology guards | `Plan12DCrossSystemContinuityTests` | TBD | Agent creating |
| Participant validity | `Plan12DCrossSystemContinuityTests` | TBD | Agent creating |
| Pending-state persistence | `Plan12DCrossSystemContinuityTests` | TBD | Agent creating |

### Task 12E — Balance Simulation

| Deliverable | Test Class | Test Count | Status |
|-------------|-----------|------------|--------|
| Deterministic simulations | `Plan12EBalanceSimulationTests` | TBD | Agent creating |
| Frequency bounds | `Plan12EBalanceSimulationTests` | TBD | Agent creating |
| Morale balance | `Plan12EBalanceSimulationTests` | TBD | Agent creating |
| Social failure recovery | `Plan12EBalanceSimulationTests` | TBD | Agent creating |
| Save/load stability | `Plan12EBalanceSimulationTests` | TBD | Agent creating |

## Verification Gates

| Gate | Command | Expected | Status |
|------|---------|----------|--------|
| Build | `dotnet build Ashfall.Core.Tests` | 0 errors, 0 warnings | ✅ PASS |
| Plan 12 tests | `dotnet test --filter Plan12` | All pass | ✅ 39/39 PASS |
| Data integrity | `godot --headless --path . -- --data-integrity-selftest` | 0 errors | Pending |
| Bridge selftest | `godot --headless --path . -- --bridge-selftest` | Exit 0 | Pending |
| Decor selftest | `godot --headless --path . -- --shelter-decor-selftest` | Pass | Pending |
| Full test suite | `dotnet test Ashfall.Core.Tests` | All pass | Pending |

## Regression Risks

| Risk | Impact | Mitigation | Test Coverage |
|------|--------|------------|---------------|
| Duplicate survivor lifecycle state | Severe save/continuity bugs | Authority map enforced; no new age/maturation counter | CohortSystem tests |
| Skill rewards bypass progression | UI/balance/save disagreement | All education routes through SkillProgressionSystem | Apprenticeship test |
| Social event spam | Shelter loop becomes tedious | Cooldowns, crisis suppression, frequency budget | 12E simulation tests |
| Ideology scope explosion | Accidental second politics game | 4 lightweight belief sets; reuse friction mechanics | 12B belief tests |
| Decor becomes mandatory optimization | Undermines survival balance | Small local additive effects, hard caps | 12E morale balance tests |
| Decor duplicates inventory | Economy exploit/save corruption | Explicit item-instance transaction contract | 12C assign/remove tests |
| Plaques duplicate death authority | Contradictory memorial identity | Reference authoritative memorial provenance | 12C memorial tests |
| Old saves acquire fabricated history | Continuity corruption | Empty defaults; no imagined guardianships/decor | 12D pending-state tests |
| Event flags become shadow state | Narrative/system disagreement | Flags describe content; systems own facts | 12A/12B flag tests |
| UI leaks rule ownership | Core/headless parity breaks | Core validation APIs; Godot presents only | 12C save round-trip tests |
