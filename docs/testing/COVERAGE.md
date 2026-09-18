# ASHFALL — Code Coverage & Risk Gates (Plan 27B / Wave 10)

> **Authority:** `C1_planintegration[6].md` & `Plan_27_Tests_That_Mean_It_Fidelity_Coverage_Journeys.md`\
> **Status:** ACTIVE\
> **Last Updated:** 2026-09-17\
> **Gate Script:** `scripts/ci/coverage-gate.sh`\

---

## 1. Executive Summary & Gating Philosophy

Global line coverage targets incentivize writing tests for trivial, low-risk helpers while leaving high-risk, state-bearing simulation seams untested. ASHFALL enforces **targeted risk gating**:

1. **State Fidelity Gate:** 100% of Core systems declaring `CaptureState` / `RestoreState` must have registered round-trip persistence tests (`SaveStateRoundTripCoverageGateTests.cs`).
2. **Determinism Coverage Gate:** 100% of registered campaign day owners in `src/Main.CampaignOwners.cs` must have determinism and paired-seed replay tests (`CampaignDayOwnerDeterminismGateTests.cs`).
3. **Protected Domain Slices:** Continuous ratcheting and zero-decrease policy across continuity-critical Core subsystems:
   - `Assets/Ashfall.Core/Save/**`
   - `Assets/Ashfall.Core/Survivors/**`
   - `Assets/Ashfall.Core/Radiation/**`
   - `Assets/Ashfall.Core/Medical/**`
   - `Assets/Ashfall.Core/Economy/**`
   - `Assets/Ashfall.Core/Campaign/**`

---

## 2. Baseline Metrics (Wave 10 Checkpoint)

| Metric | Measured Baseline | Target Policy | Enforcement |
|---|---|---|---|
| **Stateful Core Systems Round-Trip** | **100%** (128 of 128 active systems) | 100% mandatory | `SaveStateRoundTripCoverageGateTests` |
| **Campaign Day Owner Determinism** | **100%** (43 of 43 registered owners) | 100% mandatory | `CampaignDayOwnerDeterminismGateTests` |
| **Golden Save Fixtures & Envelopes** | **100%** (Early, Mid, Late validated) | Validated checksums | `GoldenSaveFixtureTests` |
| **Shipped Catalog Behavioral Fidelity** | **100%** (Authority-backed verified) | Zero drift from items.json | `DataAuthorityFidelityTests` |
| **Host Selftest System Isolation** | **100%** (Zero un-whitelisted new systems) | Fail-closed | `NoFreshCampaignSystemGateTests` |
| **Collector Footprint** | `coverlet.collector` (v6.0.2) | CPM-managed via `Directory.Packages.props` | CI Tier-2 |

---

## 3. Protected Domain Slices

The following Core slices are critical to campaign persistence and deterministic simulation. No PR may reduce line or branch coverage within these namespaces:

### `Assets/Ashfall.Core/Save/**`
- Envelopes, codecs, migration ladders, and checksum calculators (`SaveChecksum`, `SaveSlotService`, `CampaignEnvelopeBuilder`).
- Invariant: A save written by any valid host or version must load without checksum corruption or state loss.

### `Assets/Ashfall.Core/Survivors/**` & `Radiation/**`
- Roster profiles, needs degradation, illness progressions, radiation accumulation, and treatment efficacy.
- Invariant: Biological decay and trauma must follow deterministic curves based solely on elapsed time, diet, and exposure.

### `Assets/Ashfall.Core/Campaign/**` & `Economy/**`
- Master day tick coordinator, calendar, merchant pricing, and barter exchanges.
- Invariant: Same master seed + same input sequence yields bitwise-identical market inventory and campaign events.

---

## 4. Assertion Quality Hierarchy

When writing or reviewing tests, prefer higher-level assertions over superficial existence checks:

| Level | Name | Pattern | Status |
|---|---|---|---|
| **Level 0** | Execution Only | `Assert.True(true)`, does not throw | **PROHIBITED** on state mutations |
| **Level 1** | Null / Type Check | `Assert.NotNull(system)`, `Assert.IsType<T>` | Permitted only on factory scaffolding |
| **Level 2** | Return Value | `Assert.True(res.Success)` | Minimum bar for command execution |
| **Level 3** | Observable State Transition | `Assert.Equal(40, inventory.Count("canned_food"))` | **Standard** for all unit/contract tests |
| **Level 4** | Downstream Consumer Consequence | `Assert.Equal(predictedRad, survivor.Dose)` | **Required** for cross-system journeys |
| **Level 5** | Deterministic Replay / Save Parity | `Assert.Equal(checksumA, checksumB)` | **Required** for determinism & save tests |

---

## 5. Local & CI Execution

### Fast Coverage Gate (Local & PR Gate)
Runs focused static analysis, round-trip coverage, determinism coverage, and fidelity tests:
```bash
bash scripts/ci/coverage-gate.sh
```

### Full Coverage Instrumentation (Tier-2 Nightly / CI Soak)
Collects full Cobertura XML metrics across the entire suite:
```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --collect:"XPlat Code Coverage"
```
Output is stored in `Ashfall.Core.Tests/TestResults/*/coverage.cobertura.xml` (gitignored).
