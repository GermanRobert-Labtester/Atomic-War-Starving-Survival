# PLAN-RADIATION-BACKGROUND-TRUTH-189 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-DOSIMETER-CALIBRATION-TRUTH-204`](../EXPANSION_PROGRAM_WAVE15_2026-09-21/PLAN-DOSIMETER-CALIBRATION-TRUTH-204.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `DCT-204A` | calibration state + drift table. |
| `DCT-204B` | reading-band tests (paired sample distributions). |
| `DCT-204C` | recalibration path consuming a standard + time. |

## 2. Source inventory (13 files, Core + host)

| File | Lines |
|---|---:|
| `Radiation/Dosimeter.cs` | 20 |
| `Radiation/DosimeterCalibrationSystem.cs` | 373 |
| `Radiation/ExposureBreakdown.cs` | 110 |
| `Radiation/ExposureEnvironment.cs` | 339 |
| `Radiation/LowBackgroundLeadEngine.cs` | 531 |
| `Radiation/RadiationEconomyBridge.cs` | 357 |
| `Radiation/RadiationPhaseProgression.cs` | 533 |
| `Radiation/RadiationSocialBridge.cs` | 350 |
| `Radiation/RadiationSystem.cs` | 486 |
| `host:Host/LowBackgroundMetrologyHostSession.cs` | 118 |
| `host:Host/LowBackgroundMetrologySaveStore.cs` | 32 |
| `host:Main.LowBackgroundMetrology.cs` | 166 |
| `host:UI/LowBackgroundLeadPanel.cs` | 270 |

## 3. Data bindings

No catalog name-matched; verify paths/loaders before claiming a data dependency.

## 4. Host attachment

- Candidate host partials: `Main.LowBackgroundMetrology.cs`
- Proposed method names: `SetupRadiationRBScaffold` / `SaveRadiationRBScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Radiation/RB189ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class RB189ScaffoldTests
{
    [Fact] public void RBT189A_TODO() { /* shielding curve + material table. */ }
    [Fact] public void RBT189B_TODO() { /* measurement error-floor tests (paired samples). */ }
    [Fact] public void RBT189C_TODO() { /* dose-coupling audit (no foreign writes). */ }
    [Fact] public void RBT189D_TODO() { /* contaminated-shielding detection + handling fixtures. */ }
    [Fact] public void RBT189E_TODO() { /* save round-trip; no re-roll on load. */ }
    [Fact] public void RB189_AuthorityConformance_TODO() { /* pattern parity with PLAN-DOSIMETER-CALIBRATION-TRUTH-204 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Radiation/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
