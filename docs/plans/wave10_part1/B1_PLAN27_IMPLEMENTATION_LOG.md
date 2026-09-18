# B1 / Plan 27 — Tests That Mean It: Fidelity, Coverage, and Runtime Evidence (Implementation Log)

**Task:** Wave 10 Part 1 — B1 (C1[6] / Plan 27)\
**Status:** COMPLETED\
**Date:** 2026-09-17\
**Authorized by:** user ("Allright eexetute that next plan!, i authorise!")\

---

## 1. Plan Overview & Mandatory Order

Per `C-integration-plans/C1_planintegration[6].md` and `Next-steps-plans/shipped_to_chat/Plan_27_Tests_That_Mean_It_Fidelity_Coverage_Journeys.md`:
- **Order:** Task 27A (Fidelity) → Task 27B (Coverage & Risk Gates) → Task 27C (Runtime Evidence & Journeys).
- **Core Principle:** Test the game the player actually runs, measure the risk-bearing behavior that matters, and prove integration by observed state transitions — not presence, class names, route registration, or synthetic demo fixtures.

---

## 2. Phase Log

### Task 27A — Fixture Fidelity & Shipped Authority
- [x] 27A.1 Inventory test construction seams and implicit fixture fallbacks.
- [x] 27A.2 Make fixture creation explicit in `InventoryHostSession` (`CreateForFixture`, `SeedCatalogForTest`, authority-backed `Create(dataDir)` default).
- [x] 27A.3 Migrate production selftests and UI tests from implicit demo construction to authority-backed `Create(dataDir)`.
- [x] 27A.4 Document `docs/testing/FIXTURE_POLICY.md`.
- [x] 27A.5 Create shared authority-backed `CampaignFixture` (`Ashfall.Core.Tests/Fixtures/CampaignFixture.cs`).
- [x] 27A.6 Add `DataAuthorityFidelityTests` verifying items.json fields against behavioral models (passing 4/4).
- [x] 27A.7 Add source-scan gate `NoFreshCampaignSystemGateTests` banning fresh campaign-owned system instantiation in host selftests without explicit whitelisting (passing 2/2).
- [x] 27A.8 Create golden save fixtures (`artifacts/golden_saves/` early, mid, late campaign states + manifest).
- [x] 27A.9 Codify determinism digest fixtures (`GoldenSaveFixtureTests`, passing 6/6).

### Task 27B — Coverage, Save Fidelity, Determinism & Assertion Strength
- [x] 27B.1 Check/configure Coverlet package in test project (`coverlet.collector` v6.0.2 via CPM in `Directory.Packages.props`).
- [x] 27B.2 Publish baseline metrics in `docs/testing/COVERAGE.md`.
- [x] 27B.3 Stateful system scan & round-trip coverage gate (`SaveStateRoundTripCoverageGateTests`, passing 2/2; 100% active stateful Core systems covered).
- [x] 27B.4 Campaign-day owner determinism coverage gate (`CampaignDayOwnerDeterminismGateTests`, passing 2/2; 100% registered day owners covered).
- [x] 27B.5 High-risk assertion strength repairs (replacing Level 0/1 presence checks with Level 3/4 state checks).
- [x] 27B.6 Coverage gate script (`scripts/ci/coverage-gate.sh`) registered in `docs/ci/CI_GATE_MANIFEST.json` as gate 48.

### Task 27C — Runtime Evidence & Real Campaign Journeys
- [x] 27C.1 Real boot content utilization verification (`godot --headless --path . -- --content-utilization-selftest` passed with 1,563 runtime events, 38 queried catalogs, 169 selected definitions, 170 consumed definitions; CI Content Utilization Gate & Deep-Chain Gate PASS).
- [x] 27C.2 Five named campaign journeys codified & documented in `docs/testing/JOURNEYS.md`:
  1. Journey 1: First-Hour Continuity Journey (`--real-campaign-journey-selftest`, `--day1-selftest`, `EndToEndPlayerJourneyTests.cs`).
  2. Journey 2: Ending & Epilogue Matrix Journey (`Plan19SessionContinuityJourneyTests.cs`).
  3. Journey 3: State Persistence & Reload Parity Journey (`--real-campaign-journey-selftest`, `GoldenSaveFixtureTests.cs`).
  4. Journey 4: Keyboard Navigation & Accessibility Journey (`--ui-accessibility-selftest`).
  5. Journey 5: Runtime Panel Authority Identity Journey (`--panel-bind-lifecycle-selftest`, `PlayerSurfaceLivenessGateTests.cs`).
- [x] 27C.3 Runtime panel authority identity validation (`--panel-bind-lifecycle-selftest` 17/17 PASS, `PlayerSurfaceLivenessGateTests` PASS, `PlayerSurfaceBindingPurityGateTests` PASS).
- [x] 27C.4 Full verification sweep:
  - Unit tests: 100/100 UI tests PASS, 16/16 Plan 27 fidelity & coverage tests PASS.
  - Headless Godot runs: `--data-integrity-selftest` PASS, `--bridge-selftest` PASS, `--day1-selftest` PASS (32/32), `--real-campaign-journey-selftest` PASS, `--ui-accessibility-selftest` PASS (5/5), `--panel-bind-lifecycle-selftest` PASS (17/17).
  - Fast CI Gates: `bash scripts/ci/verify-fast.sh` executed — **ALL 48 GATES PASSED CLEANLY**.
