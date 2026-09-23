# PLAN-HOTFIX-DRILL-99 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-RELEASE-OPS-20`](../EXPANSION_PROGRAM_WAVE2_2026-09-21/PLAN-RELEASE-OPS-20.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|

## 2. Source inventory (4 files, Core + host)

| File | Lines |
|---|---:|
| `ReleaseVersion.cs` | 127 |
| `host:UI/BlackMarketSnapshotFixture.cs` | 82 |
| `host:UI/EconomyMarketSnapshotFixture.cs` | 29 |
| `host:UI/ShelterDecorSnapshotFixture.cs` | 110 |

## 3b. Release tooling

| Script | Lines |
|---|---:|
| `scripts/ci/release-gate.sh` | 102 |
| `scripts/release/generate_changelog.py` | 171 |
| `scripts/release/hotfix.sh` | 130 |
| `scripts/release/prepare-release.sh` | 292 |


## 3. Data bindings

No catalog name-matched; verify paths/loaders before claiming a data dependency.

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupReleaseHDScaffold` / `SaveReleaseHDScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Release/HD99ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class HD99ScaffoldTests
{
    [Fact] public void HFD99A_TODO() { /* drill script (scratch environment; hard guard: refuses to run on the default bra */ }
    [Fact] public void HFD99B_TODO() { /* rollback rehearsal + verification checklist. */ }
    [Fact] public void HFD99C_TODO() { /* save-compat matrix run (uses Plan 87 fixtures). */ }
    [Fact] public void HFD99D_TODO() { /* timing capture + report template. */ }
    [Fact] public void HFD99E_TODO() { /* findings log + handoff to C2[21] owner. */ }
    [Fact] public void HD99_AuthorityConformance_TODO() { /* pattern parity with PLAN-RELEASE-OPS-20 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Release/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
