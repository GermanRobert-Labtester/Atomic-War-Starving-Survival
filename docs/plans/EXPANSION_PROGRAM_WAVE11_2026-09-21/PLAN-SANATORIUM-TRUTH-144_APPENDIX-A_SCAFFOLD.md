# PLAN-SANATORIUM-TRUTH-144 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-MENTAL-HEALTH-THERAPY-64`](../EXPANSION_PROGRAM_WAVE6_2026-09-21/PLAN-MENTAL-HEALTH-THERAPY-64.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `MH-64A` | strain model: continuous strain with visible tells; no instant crisis from one event. |
| `MH-64B` | counselling: skill-gated sessions with a time cost; effect bounded and diminishing. |
| `MH-64C` | trauma care: targeted work on a memory (phantom/guilt/bereavement) with progress and relapse risk. |

## 2. Source inventory (3 files, Core + host)

| File | Lines |
|---|---:|
| `Sanatorium/PsychologicalSanatoriumSystem.cs` | 453 |
| `Sanatorium/PsychologicalTherapyCatalog.cs` | 150 |
| `host:Host/PsychologicalSanatoriumSaveStore.cs` | 26 |

## 3. Data bindings

No catalog name-matched; verify paths/loaders before claiming a data dependency.

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupSanatoriumSNScaffold` / `SaveSanatoriumSNScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Sanatorium/SN144ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class SN144ScaffoldTests
{
    [Fact] public void SNT144A_TODO() { /* admission criteria table + event. */ }
    [Fact] public void SNT144B_TODO() { /* therapy plan matching + per-day progress test. */ }
    [Fact] public void SNT144C_TODO() { /* progress writes through Plan 64's owner (no private score proof). */ }
    [Fact] public void SNT144D_TODO() { /* discharge states + consequence routing tests. */ }
    [Fact] public void SNT144E_TODO() { /* coverage integration test with Plan 101 (no double count). */ }
    [Fact] public void SN144_AuthorityConformance_TODO() { /* pattern parity with PLAN-MENTAL-HEALTH-THERAPY-64 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Sanatorium/` (create the region if absent)
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
